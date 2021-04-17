using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Luffy
{
    public class LuffyHealthManager : MonoBehaviour
    {
        public static float health = 100f;

        [SerializeField] private float decreaseHealthEveryXSeconds;
        private float currentDecreaseTimer;
        [Range(0f, 100f)][SerializeField] private float healthDecreaseAmount;

        [SerializeField] private GameObject healthBarFilling;

        private void Start()
        {
            currentDecreaseTimer = decreaseHealthEveryXSeconds;
            health = 100f;
        }

        private void Update()
        {
            // If die
            if (health <= 0)
            {
                SceneManager.LoadScene(5);
            }
            // If time to decrease health
            if (currentDecreaseTimer <= 0)
            {
                // Decrease health
                health -= healthDecreaseAmount;
                // Reduce health filling UI
                ChangeHealthBarFilling(-healthDecreaseAmount);
                // Reset timer
                currentDecreaseTimer = decreaseHealthEveryXSeconds;
            }
        }

        private void FixedUpdate()
        {
            // Decrease current decrease timer at regular intervals
            if (currentDecreaseTimer > 0)
            {
                currentDecreaseTimer -= Time.deltaTime;
            }
        }

        private void ChangeHealthBarFilling(float change)
        {
            float scaleChange = change * 0.0042f;
            float positionChange = change * 0.00212f;
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
    }
}