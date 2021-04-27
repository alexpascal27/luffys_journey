using System;
using UnityEngine;

namespace DefaultNamespace.Akainu
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        private Vector3 _bottomLeft;
        private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private float minAngle;
        [SerializeField] private float maxAngle;

        private void Start()
        {
            _bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = Vector2.down * speed;
        }

        private void Update()
        {
            if (IsPositionOutsideOfScreen(gameObject.transform.position))
            {
                Destroy(gameObject);
            }
        }
        
        private bool IsPositionOutsideOfScreen(Vector3 position)
        {
            // If position on right of screen or on left of screen or above or below
            return position.x > -_bottomLeft.x || position.x < _bottomLeft.x || position.y > -_bottomLeft.y ||
                   position.y < _bottomLeft.y;
        }
    }
}