using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class BasicBossMovement : MonoBehaviour
    {
        [SerializeField] private bool checkHorizontally = true;
        [SerializeField] private bool checkVertically = true;

        [SerializeField] private float raycastDistance = 10f;
        [SerializeField] private LayerMask environmentLayerMask;
        
        private Vector2 _direction;
        [SerializeField] private float secondsInDirection = 5f;
        private float _remainingSecondsInDirection = 0f;
        private Rigidbody2D rb;
        [SerializeField] private float movementSpeed = 1f;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            
            // Calculate the direction
            SetDirectionOfMovement();

            _remainingSecondsInDirection = secondsInDirection;
        }

        private void SetDirectionOfMovement()
        {
            Vector2[] directions = new []{new Vector2()};
            if (checkHorizontally && checkVertically)
            {
                directions = new[] {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
            }
            else if (checkHorizontally)
            {
                directions = new[] {Vector2.right,Vector2.left};
            }
            else if (checkVertically)
            {
                directions = new[] {Vector2.up, Vector2.down};
            }
            
            // Shuffle array
            Random random = new Random();
            directions.OrderBy(c => random.Next());

            float biggestDistance = -Mathf.Infinity;
            int directionIndex = 0;
            
            for (int i = 0; i < directions.Length; i++)
            {
                // Shoot a raycast
                RaycastHit2D raycastHit2D =
                    Physics2D.Raycast(gameObject.transform.position, directions[i], raycastDistance, environmentLayerMask);

                // If no collider, pick that direction
                if (raycastHit2D.collider == null)
                {
                    _direction = directions[i];
                    return;
                }

                // If there is a collider, then find distance to collider and update current 
                float distanceFromBoss = ((Vector2) gameObject.transform.position - raycastHit2D.point).magnitude;
                if (distanceFromBoss > biggestDistance)
                {
                    directionIndex = i;
                    biggestDistance = distanceFromBoss;
                }
            }

            _direction = directions[directionIndex];
        }

        private void Update()
        {
            if (_remainingSecondsInDirection <= 0)
            {
                SetDirectionOfMovement();
                _remainingSecondsInDirection = secondsInDirection;
            }
        }

        private void FixedUpdate()
        {
            if (_remainingSecondsInDirection > 0) _remainingSecondsInDirection -= Time.deltaTime;

            rb.velocity = _direction * (movementSpeed * Time.fixedDeltaTime);
        }
    }
}