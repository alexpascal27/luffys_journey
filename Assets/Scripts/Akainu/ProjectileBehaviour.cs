using System;
using UnityEngine;

namespace DefaultNamespace.Akainu
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        private Animator _animator;
        private const float PutOutAnimationTime = 0.5f;
        private float _currentAnimationTime = 0;
        private bool _beingPutOut = false;
        
        [SerializeField] private Transform raycastFirePoint;
        [SerializeField] [Range(1f, 10f)] private float raycastDistance;
        private Vector3 _bottomLeft;
        private Rigidbody2D _rb;
        [SerializeField] private float speed;
        private CircleCollider2D _projectileCollider;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _projectileCollider = GetComponent<CircleCollider2D>();
            _bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = Vector2.down * speed;
        }

        private void Update()
        {
            // If finished animation to be put out
            if (_beingPutOut && _currentAnimationTime <= 0)
            {
                Destroy(gameObject);
            }

            // If velocity is low or 0 it means that it has collided and needs to be put out
            if ((_rb.velocity.x != 0f || _rb.velocity.y > -0.5f) && !_beingPutOut)
            {
                // initiate put out animation
                _beingPutOut = true;
                _currentAnimationTime = PutOutAnimationTime;
                _animator.SetBool("PutOut", true);
            }
            
            // Send raycast downwards to check for non-player objects
            RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastFirePoint.position, Vector2.down, raycastDistance);
            // if we hit something
            Collider2D raycastCollider = raycastHit2D.collider;
            // if collider is not null
            if (raycastCollider != null)
            {
                // if non player 
                if (!raycastCollider.gameObject.CompareTag("Luffy"))
                {
                    // ignore collision
                    Physics2D.IgnoreCollision(raycastCollider, _projectileCollider);
                }
            }
            
            // if below screen destoy
            if (IsPositionOutsideOfBottomScreen(gameObject.transform.position))
            {
                Destroy(gameObject);
            }
        }
        
        private bool IsPositionOutsideOfBottomScreen(Vector3 position)
        {
            // If position on right of screen or on left of screen or above or below
            return position.y < _bottomLeft.y;
        }

        private void FixedUpdate()
        {
            if (_currentAnimationTime > 0) _currentAnimationTime -= Time.deltaTime;
        }
    }
}