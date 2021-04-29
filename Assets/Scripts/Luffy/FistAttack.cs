using System;
using Aokiji;
using DefaultNamespace;
using Troop;
using UnityEngine;

namespace Luffy
{
    public class FistAttack : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;

        private void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject colliderGameObject = other.gameObject;
            if (colliderGameObject.CompareTag("Troop"))
            {
                TroopHealthManager troopHealthManager = colliderGameObject.GetComponent<TroopHealthManager>();
                troopHealthManager.DamageTroop();
                if(_boxCollider2D.enabled)_boxCollider2D.enabled = false;
                
                // Increase number of times luffy hit troops
                PlayerPrefsManager playerPrefsManager = new PlayerPrefsManager();
                playerPrefsManager.IncrementDamageToEnemyTroops();
            }
            else if(colliderGameObject.CompareTag("Boss"))
            {
                BossHealthManager bossHealthManager = colliderGameObject.GetComponent<BossHealthManager>();
                bossHealthManager.DamageBoss();
                if(_boxCollider2D.enabled)_boxCollider2D.enabled = false;
                
                // Increase number of times luffy hit troops
                PlayerPrefsManager playerPrefsManager = new PlayerPrefsManager();
                playerPrefsManager.IncrementDamageToEnemyBosses();
            }
        }
    }
}