using System;
using Troop;
using UnityEngine;

namespace Luffy
{
    public class FistAttack : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject colliderGameObject = other.gameObject;
            Debug.Log("colliding with: " + colliderGameObject.tag);
            if (colliderGameObject.CompareTag("Troop"))
            {
                Debug.Log("Damaging the troop");
                TroopHealthManager troopHealthManager = colliderGameObject.GetComponent<TroopHealthManager>();
                troopHealthManager.DamageTroop();
            }
        }
    }
}