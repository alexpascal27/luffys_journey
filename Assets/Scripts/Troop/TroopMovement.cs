using System;
using Pathfinding;
using UnityEngine;

namespace Troop
{
    public class TroopMovement : MonoBehaviour
    {
        private GameObject playerGameObject;
        private AIDestinationSetter aiDestinationSetter;
        
        
        private void Start()
        {
            playerGameObject = GameObject.Find("Luffy");
            Transform playerTransform = playerGameObject.transform;
            
            aiDestinationSetter = GetComponent<AIDestinationSetter>();
            aiDestinationSetter.target = playerTransform;
        }

        private void Update()
        {
            aiDestinationSetter.target = playerGameObject.transform;
        }
    }
}