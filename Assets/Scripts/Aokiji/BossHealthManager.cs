using System;
using UnityEngine;

namespace Aokiji
{
    public class BossHealthManager : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] scriptsToDisableOnDeath;
        private BoxCollider2D _boxCollider2D;

        private Vector2 _deathAnimationDestinationPoint;
        [SerializeField] private float movementSpeed;
        
        private float _health = 100f;

        private Animator _animator;
        [SerializeField] private String leaveAnimationName;
        private bool _leaving = false;

        [Range(0f, 100f)][SerializeField] private float healthDecreaseAmount;
        [SerializeField] private GameObject healthBarFilling;

        [SerializeField] private bool spawnMeatWhenDead = true;
        [SerializeField] private GameObject meatPrefab;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (!_leaving)
            {
                // If die
                if (_health <= 0)
                {
                    _animator.SetBool(leaveAnimationName, true);
                    _leaving= true;
                    DisableScripts();
                    CalculateEndDestination();
                }
            }
            else
            {
                // if death animation finishes, die
                if ((Vector2)gameObject.transform.position == _deathAnimationDestinationPoint)
                {
                    if (spawnMeatWhenDead)
                    {
                        meatPrefab.transform.position = gameObject.transform.position;
                        Instantiate(meatPrefab);
                    }
                    
                    Destroy(gameObject);
                }
            }
        }

        private void CalculateEndDestination()
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            // Check if closer to bottom left, bottom middle, or bottom right
            Vector3 destinationPoint = bottomLeft;
            float smallestDistance = Mathf.Infinity;
            
            Vector3[] possibleDestinations = new[]
                {new Vector3(bottomLeft.x-2f, bottomLeft.y-2f), new Vector3(0, bottomLeft.y-2f), new Vector3(-bottomLeft.x+2f, bottomLeft.y+2f)};
            foreach (Vector3 destination in possibleDestinations)
            {
                float distance = (gameObject.transform.position - destination).magnitude;
                if (distance < smallestDistance)
                {
                    destinationPoint = destination;
                    smallestDistance = distance;
                }
            }

            _deathAnimationDestinationPoint = destinationPoint;
        }

        private void DisableScripts()
        {
            _boxCollider2D.enabled = false;
            foreach (MonoBehaviour script in scriptsToDisableOnDeath)
            {
                script.enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (_leaving)
            {
                float step = movementSpeed * Time.deltaTime;
                transform.position =
                    Vector2.MoveTowards(gameObject.transform.position, _deathAnimationDestinationPoint, step);
            }
        }

        private void ChangeHealthBarFilling(float change)
        {
            float scaleChange = change * 0.00475f;
            float positionChange = change * 0.0027f;
            // Change scale
            healthBarFilling = ChangeScale(healthBarFilling, new Vector3(scaleChange, 0));
            // Move appropriately
            healthBarFilling.transform.position += new Vector3(positionChange, 0);
        }
        
        private GameObject ChangeScale(GameObject go, Vector3 change)
        {
            Transform parentTransform = go.transform.parent;
            go.transform.parent = null;
            Vector3 localScale = go.transform.localScale;
            localScale += change;
            go.transform.localScale = localScale;
            go.transform.parent = parentTransform;

            return go;
        }

        public void DamageBoss()
        {
            // Decrease health
            _health -= healthDecreaseAmount;
            // Reduce health filling UI
            ChangeHealthBarFilling(-healthDecreaseAmount);
        }
    }
    
}