using Luffy;
using UnityEngine;

namespace Troop
{
    public class SwordAttack : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject colliderGameObject = other.gameObject;
            Debug.Log("Sword collided with: " + colliderGameObject.tag);
            if (colliderGameObject.CompareTag("Luffy"))
            {
                LuffyHealthManager luffyHealthManager = colliderGameObject.GetComponent<LuffyHealthManager>();
                luffyHealthManager.DamageLuffy(false);
            }
        }
    }
}