using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class BasicBossMovement : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private bool checkHorizontally = true;
        [SerializeField] private bool checkVertically = true;

        [SerializeField] private float raycastDistance = 10f;
        [SerializeField] private LayerMask environmentLayerMask;
        
        private Vector2 _direction;
        [SerializeField] private float secondsInDirection = 5f;
        private float _remainingSecondsInDirection = 0f;
        private Rigidbody2D rb;
        [SerializeField] private float movementSpeed = 1f;
        private bool _facingRight = true;
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
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
            directions = directions.OrderBy(c => random.Next()).ToArray();
            

            float biggestDistance = -Mathf.Infinity;
            int directionIndex = 0;
            
            for (int i = 0; i < directions.Length; i++)
            {
                // Shoot a raycast
                RaycastHit2D raycastHit2D =
                    Physics2D.Raycast(gameObject.transform.position, directions[i], raycastDistance, environmentLayerMask);
                Debug.DrawRay(gameObject.transform.position, directions[i] * raycastDistance);

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
        
        private void PrintArray(Vector2[] array)
        {
            Debug.Log("PrintingVector2Array: " );
            foreach(var element in array)
            {
                Debug.Log(element);
            }
        }

        private void Update()
        {
            ManageFlip();
            _animator.SetFloat("MovementSpeed", GetFloatVelocity(rb.velocity));
            if (_remainingSecondsInDirection <= 0)
            {
                SetDirectionOfMovement();
                _remainingSecondsInDirection =  UnityEngine.Random.Range(0, secondsInDirection);
            }
        }

        private float GetFloatVelocity(Vector3 velocity)
        {
            if (velocity.x != 0) return Mathf.Abs(velocity.x);
            if (velocity.y != 0) return Mathf.Abs(velocity.y);
            return 0f;
        }

        private void ManageFlip()
        {
            // If facing right but moving left or facing left but moving right then FLIP
            if ((_facingRight && rb.velocity.x < 0) || (!_facingRight && rb.velocity.x > 0))
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
                _facingRight = !_facingRight;
            }
        }

        private void FixedUpdate()
        {
            if (_remainingSecondsInDirection > 0) _remainingSecondsInDirection -= Time.deltaTime;

            rb.velocity = _direction * (movementSpeed * Time.fixedDeltaTime);
        }

        public void DisableMovement()
        {
            rb.velocity = Vector2.zero;
            _animator.SetFloat("MovementSpeed", 0f);
        }
    }
}