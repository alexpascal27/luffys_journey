using System;
using System.Linq;
using UnityEngine;

namespace Aokiji
{
    public class AokijiAbility1 : MonoBehaviour
    {
        // Cooldown
        [SerializeField] private float abilityCooldown = 5f;
        private bool _abilityOnCooldown = true;
        private float _remainingCooldownPeriod = 0f;
        
        // Ability 
        private const float CellRadius = 0.5f;
        [SerializeField] private float distanceBetweenCells = 0.1f;

        [SerializeField] private GameObject frozenCellPrefab;
        private BoxCollider2D _boxCollider2D;
        
        // In order [North, East, South, West]
        private Vector3[] _nextPositionInDirection;
        private bool[] _canSpawnInDirection;
        private bool _spawning = false;

        private void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            // No longer on cooldown
            if (_abilityOnCooldown && _remainingCooldownPeriod <= 0)
            {
                _remainingCooldownPeriod = abilityCooldown;
                InitArrays();
                _spawning = true;
            }

            // If in process in spawning
            if (_spawning)
            {
                // If can spawn in a direction
                if (_canSpawnInDirection.Any(x => x))
                {
                    // Spawn next along axis
                    SpawnNextCell();
                }
                // Can no longer spawn so set ability on cooldown
                else
                {
                    _spawning = false;
                    _abilityOnCooldown = true;
                    _remainingCooldownPeriod = abilityCooldown;
                }
                
            }
        }

        private void InitArrays()
        {
            // Get width of box collider
            Vector3 boxColliderDimensions = _boxCollider2D.bounds.size;
            
            // Get position
            Vector2 currentPosition = gameObject.transform.position;
            
            // Initialise the can spawn in direction array
            _canSpawnInDirection = new []{true, true, true, true};
            
            // Initialise the next position array
            Vector3 north = currentPosition + Vector2.up * (CellRadius * 2 + boxColliderDimensions.y/2);
            Vector3 east = currentPosition + Vector2.right * (CellRadius * 2 + boxColliderDimensions.x/2);
            Vector3 south = currentPosition + Vector2.down * (CellRadius * 2 + boxColliderDimensions.y/2);
            Vector3 west = currentPosition + Vector2.left * (CellRadius * 2 + boxColliderDimensions.x/2);
            _nextPositionInDirection = new[] {north, east, south, west};
        }

        private void PrintArrays()
        {
            Debug.Log("CanSpawnInDirection: " );
            foreach(var direction in _canSpawnInDirection)
            {
                Debug.Log(direction);
            }
            
            Debug.Log("NextPositionInDirection: " );
            foreach(var direction in _nextPositionInDirection)
            {
                Debug.Log(direction);
            }
        }

        private void SpawnNextCell()
        {
            Vector2[] direction = new[] {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
            // For all axes
            for (int i = 0; i < 4; i++)
            {
                // If can spawn in that direction
                if (_canSpawnInDirection[i])
                {
                    // Spawn at currect location
                    frozenCellPrefab.transform.position = _nextPositionInDirection[i];
                    Instantiate(frozenCellPrefab);
                    
                    // Get next location
                    Vector3 newPosition = (Vector2)_nextPositionInDirection[i] + direction[i] * (2 * CellRadius + distanceBetweenCells);
                    if (IsPositionOutsideOfScreen(newPosition))
                    {
                        _canSpawnInDirection[i] = false;
                    }
                    else
                    {
                        _nextPositionInDirection[i] = newPosition;
                    }
                }
            }
            
            //PrintArrays();
        }

        private bool IsPositionOutsideOfScreen(Vector3 position)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            // If position on right of screen or on left of screen or above or below
            return position.x > -bottomLeft.x || position.x < bottomLeft.x || position.y > -bottomLeft.y ||
                   position.y < bottomLeft.y;
        }

        private void FixedUpdate()
        {
            if (_remainingCooldownPeriod > 0) _remainingCooldownPeriod -= Time.deltaTime;
        }
    }
}