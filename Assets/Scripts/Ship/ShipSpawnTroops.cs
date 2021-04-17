using System;
using Pathfinding;
using UnityEngine;

public class ShipSpawnTroops : MonoBehaviour
{
    [SerializeField] private float waitTillSpawn;
    private float remainingWaitTime = 0f;
    [SerializeField] private Vector3[] spawnPositions;
    [SerializeField] private float spawnCooldown;
    private float remainingCooldownTime = 0f;
    [SerializeField] private float animationTime;
    private float remainingAnimationTime = 0f;
    [SerializeField]private Animator _animator;
    [SerializeField]private GameObject troopPrefab;

    private void Update()
    {
        // Off cooldown
        if (remainingCooldownTime <= 0)
        {
            remainingWaitTime = waitTillSpawn;
            remainingCooldownTime = spawnCooldown;
            remainingAnimationTime = animationTime;
            _animator.SetBool("Moving", true);
        }
        // At point which we spawn troops
        if (remainingWaitTime <= 0)
        {
            // Spawn
            Spawn();
            remainingWaitTime = waitTillSpawn + animationTime + remainingCooldownTime;
        }
        // Animation finished
        if (remainingAnimationTime <= 0)
        {
            _animator.SetBool("Moving", false);
            // Reset cooldown
            remainingCooldownTime = spawnCooldown;
            remainingAnimationTime = animationTime + remainingCooldownTime;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            GameObject troopGameObject = troopPrefab;
            // Set Spawn position
            troopGameObject.transform.position = spawnPositions[i];

            // Set player position for pathfinding algorithm
            Instantiate(troopGameObject);
        }
    }
    
    private void FixedUpdate()
    {
        if (remainingWaitTime > 0)
        {
            remainingWaitTime -= Time.deltaTime;
        }

        if (remainingCooldownTime > 0)
        {
            remainingCooldownTime -= Time.deltaTime;
        }

        if (remainingAnimationTime > 0)
        {
            remainingAnimationTime -= Time.deltaTime;
        }
    }
}
