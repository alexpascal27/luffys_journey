using System;
using DefaultNamespace;
using Luffy;
using UnityEngine;

namespace Troop
{
    public class SwordAttack : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;

        private void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject colliderGameObject = other.gameObject;
            if (colliderGameObject.CompareTag("Luffy"))
            {
                LuffyHealthManager luffyHealthManager = colliderGameObject.GetComponent<LuffyHealthManager>();
                luffyHealthManager.DamageLuffy(false);
                _boxCollider2D.enabled = false;
                
                // Increase number of times luffy got hit
                PlayerPrefsManager playerPrefsManager = new PlayerPrefsManager();
                playerPrefsManager.IncrementDamageFromEnemyTroops();
            }
        }
    }
}