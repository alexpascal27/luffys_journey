using System;
using Pathfinding;
using UnityEngine;

namespace Troop
{
    public class TroopMovement : MonoBehaviour
    {
        private bool _facingLeft = true;
        private GameObject playerGameObject;
        private AIDestinationSetter aiDestinationSetter;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            playerGameObject = GameObject.Find("Luffy");
            Transform playerTransform = playerGameObject.transform;
            
            aiDestinationSetter = GetComponent<AIDestinationSetter>();
            aiDestinationSetter.target = playerTransform;
            
            GetComponent<Animator>().SetBool("Running", true);

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            Transform playerTransform = playerGameObject.transform;
            aiDestinationSetter.target = playerTransform;
            
            // Flip troop according to player position
            // If player is to the left of the troop and troop not facing left or If player is to the right of the troop and troop facing left
            if ((playerTransform.position.x <= gameObject.transform.position.x && !_facingLeft) || (playerTransform.position.x > gameObject.transform.position.x && _facingLeft))
            {
                _facingLeft = !_facingLeft;
                
                spriteRenderer.flipX = !spriteRenderer.flipX;
                
                int childCount = transform.childCount;
                for(int i = 0; i < childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (!child.gameObject.CompareTag("UI"))
                    {
                        child.Rotate(0f, 180f, 0f);
                    }
                }
            }
        }
    }
}