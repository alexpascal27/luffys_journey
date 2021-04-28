using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MeatBehaviour : MonoBehaviour
    {
        [SerializeField] private float expiryTime;

        private void Update()
        {
            if (expiryTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (expiryTime > 0) expiryTime -= Time.deltaTime;
        }
    }
}