using System;
using Aokiji;
using Troop;
using UnityEngine;

namespace Luffy
{
    public class FistAttack : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject colliderGameObject = other.gameObject;
            if (colliderGameObject.CompareTag("Troop"))
            {
                TroopHealthManager troopHealthManager = colliderGameObject.GetComponent<TroopHealthManager>();
                troopHealthManager.DamageTroop();
            }
            else if(colliderGameObject.CompareTag("Boss"))
            {
                BossHealthManager bossHealthManager = colliderGameObject.GetComponent<BossHealthManager>();
                bossHealthManager.DamageBoss();
            }
        }
    }
}