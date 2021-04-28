using System;
using UnityEngine;

namespace DefaultNamespace.Akainu
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        private Vector3 _bottomLeft;
        private Rigidbody2D _rb;
        [SerializeField] private float speed;

        private void Start()
        {
            _bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = Vector2.down * speed;
        }

        private void Update()
        {
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
    }
}