using System;
using UnityEngine;

namespace Aokiji
{
    public class BossHealthManager : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] scriptsToDisableOnDeath;
        private BoxCollider2D _boxCollider2D;

        private Vector2 _deathAnimationDestinationPoint;
        private float movementSpeed = 0f;
        
        private float _health = 100f;
        private float healAmount = 30f;
        
        private Animator _animator;
        [SerializeField] private String leaveAnimationName;
        [SerializeField] private float leaveAnimationTime;
        private bool _leaving = false;
        private Vector3 _bottomLeft;

        [Range(0f, 100f)][SerializeField] private float healthDecreaseAmount;
        [SerializeField] private GameObject healthBarFilling;

        [SerializeField] private bool spawnMeatWhenDead = true;
        [SerializeField] private GameObject meatPrefab;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
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
                    CalculateMovementSpeed();
                }
            }
            else
            {
                // if death animation finishes, die
                if (IsPositionOutsideOfScreen((Vector2)gameObject.transform.position))
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
        
        private bool IsPositionOutsideOfScreen(Vector3 position)
        {
            // If position on right of screen or on left of screen or above or below
            return position.x > -_bottomLeft.x || position.x < _bottomLeft.x || position.y > -_bottomLeft.y ||
                   position.y < _bottomLeft.y;
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

        private void CalculateMovementSpeed()
        {
            float distance = ((Vector2) gameObject.transform.position - _deathAnimationDestinationPoint).magnitude;
            // Speed = distance/time
            movementSpeed = distance / leaveAnimationTime;
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
        
        public void HealBoss()
        {
            if(_health>=99) return;
            float amount = healAmount;
            if (healAmount + _health > 100f)
            {
                amount = 100 - _health;
            }
            // Increase health
            _health += amount;
            // Reduce health filling UI
            ChangeHealthBarFilling(amount);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject collisionGameObject = other.gameObject;
            if (collisionGameObject.CompareTag("Meat"))
            {
                HealBoss();
                Destroy(collisionGameObject);
            }
        }
    }
    
}