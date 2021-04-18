using System;
using UnityEngine;

namespace Troop
{
    public class TroopHealthManager : MonoBehaviour
    {
        private float health = 100f;

        private Animator _animator;
        [SerializeField] private String deathAnimationName;
        [SerializeField] private float deathAnimationDuration;
        private bool dying = false;
        private float remainingDeathTimer = 0f;

        [Range(0f, 100f)][SerializeField] private float healthDecreaseAmount;
        [SerializeField] private GameObject healthBarFilling;

        [SerializeField] private bool spawnMeatWhenDead = true;
        [SerializeField] private GameObject meatPrefab;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!dying)
            {
                // If die
                if (health <= 0)
                {
                    _animator.SetBool(deathAnimationName, true);
                    dying = true;
                    remainingDeathTimer = deathAnimationDuration;
                }
            }
            else
            {
                // if death animation finishes, die
                if (remainingDeathTimer <= 0f)
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

        private void FixedUpdate()
        {
            // Decrease current decrease timer at regular intervals
            if (remainingDeathTimer > 0)
            {
                remainingDeathTimer -= Time.deltaTime;
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

        public void DamageTroop()
        {
            // Decrease health
            health -= healthDecreaseAmount;
            // Reduce health filling UI
            ChangeHealthBarFilling(-healthDecreaseAmount);
        }
    }
}