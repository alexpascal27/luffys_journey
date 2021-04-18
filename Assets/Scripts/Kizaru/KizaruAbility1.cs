using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

namespace Kizaru
{
    public class KizaruAbility1 : MonoBehaviour
    {
        private BasicBossMovement _basicBossMovement;
        private BoxCollider2D _kizaruBodyBoxCollider2D;
        private Rigidbody2D _kizaryRigidBody2d;
        private List<GameObject> _childrenGameObjects;
        
        // Cooldown
        [SerializeField] private float abilityCooldown = 5f;
        private bool _abilityOnCooldown = true;
        private float _remainingCooldownPeriod = 0f;
        
        // Animations
        private Animator _animator;
        // 0 = upwards, 1 =  horizontal, 2 = downwards
        private int _directionIndex = 1;
        private Vector3 _dashLocation;
        private const float DashAnimationTime = 1.175f;
        // 0 = upwards, 1 =  horizontal, 2 = downwards
        private readonly float[] _kickAnimationTimes = {1.125f, 1.25f, 0.625f};
        [SerializeField] private String[] animationNames;
        private bool _dashingTo = false;
        private bool _dashingFrom = false;
        private bool _kicking = false;
        private bool _flippedWhileKicking = false;
        private float _remainingAnimationTime = 0f;
        
        // Dash positional stats
        private Vector2 _positionBeforeDash;
        private GameObject _playerGameObject;
        [SerializeField] private BoxCollider2D[] kickHitboxes;
        private float _dashSpeed = 0f;
        
        // Raycast
        [SerializeField] private float raycastDistance;
        [SerializeField] private LayerMask environmentLayerMask;
        [SerializeField] private Vector2 dashPositionalOffSet;

        private void Start()
        {
            _basicBossMovement = GetComponent<BasicBossMovement>();
            _kizaryRigidBody2d = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _kizaruBodyBoxCollider2D = GetComponent<BoxCollider2D>();
            _childrenGameObjects = new List<GameObject>();
            
            
            _playerGameObject = GameObject.Find("Luffy");
                
            _remainingCooldownPeriod = abilityCooldown;
        }

        private void Update()
        {
            // If stop being on cooldown
            if (_abilityOnCooldown && _remainingCooldownPeriod <= 0)
            {
                _abilityOnCooldown = false;
                
                Dash(true);
                
                _kizaruBodyBoxCollider2D.enabled = false;
                _basicBossMovement.enabled = false;
            }
            
            // If finished dashing to kick
            if (_dashingTo && _remainingAnimationTime <= 0)
            {
                _dashingTo = false;
                
                Kick();
                _animator.SetBool("Dashing", false);
            }
            
            // If finished kicking and need to dash to original position
            if (_kicking && _remainingAnimationTime <= 0)
            {
                _kicking = false;
                _animator.SetBool(animationNames[_directionIndex], false);
                
                Dash(false);
                // Unfreeze movement
                _kizaryRigidBody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (_flippedWhileKicking)
                {
                    _flippedWhileKicking = false;
                    FlipKizaru();
                }
                kickHitboxes[_directionIndex].enabled = false;
            }
            
            // If finished dashing back go on cooldown
            if(_dashingFrom && _remainingAnimationTime <= 0)
            {
                _dashingFrom = false;
                _kizaruBodyBoxCollider2D.enabled = true;
                _animator.SetBool("Dashing", false);

                _abilityOnCooldown = true;
                _remainingCooldownPeriod = abilityCooldown;
                
                _basicBossMovement.enabled = true;
            }
        }

        private void Dash(bool dashingTo)
        {
            if (dashingTo)
            {
                // set position before dash
                _positionBeforeDash = gameObject.transform.position;
                // set dashing location
                SetDashDestination();
                // set dashing to true
                _dashingTo = true;
                _remainingAnimationTime = DashAnimationTime;
                SetDashSpeed();
                // Animation
                _animator.SetBool("Dashing", true);
            }
            else
            {
                _dashLocation = _positionBeforeDash;
                // set dashing to true
                _dashingFrom = true;
                _remainingAnimationTime = DashAnimationTime;
                SetDashSpeed();
                // Animation
                _animator.SetBool("Dashing", true);
            }
        }

        private void SetDashDestination()
        {
            Vector2 playerPosition = _playerGameObject.transform.position;
            Vector2 freestDirection = GetFreestDirection(playerPosition);
            // Set direction index
            if (freestDirection.Equals(Vector2.down)) _directionIndex = 0;
            if (freestDirection.Equals(Vector2.up)) _directionIndex = 2;
            else _directionIndex = 1;
            // Set dash location
            
            _dashLocation = new Vector3(playerPosition.x + freestDirection.x * dashPositionalOffSet.x,
                playerPosition.y + freestDirection.y * dashPositionalOffSet.y);
            Debug.Log("DashLocation: " + _dashLocation);
        }

        private Vector2 GetFreestDirection(Vector2 raycastStartPosition)
        {
            Vector2[] directions = new[] {Vector2.up, Vector2.right, Vector2.down, Vector2.left};
            // Shuffle array
            Random random = new Random();
            directions = directions.OrderBy(c => random.Next()).ToArray();

            float biggestDistance = -Mathf.Infinity;
            int directionIndex = 0;
            
            for (int i = 0; i < directions.Length; i++)
            {
                // Shoot a raycast
                RaycastHit2D raycastHit2D =
                    Physics2D.Raycast(raycastStartPosition, directions[i], raycastDistance, environmentLayerMask);

                // If no collider, pick that direction
                if (raycastHit2D.collider == null)
                {
                    return directions[i];
                }

                // If there is a collider, then find distance to collider and update current 
                float distanceFromBoss = ((Vector2) gameObject.transform.position - raycastHit2D.point).magnitude;
                if (distanceFromBoss > biggestDistance)
                {
                    directionIndex = i;
                    biggestDistance = distanceFromBoss;
                }
            }

           return directions[directionIndex];
        }

        private void SetDashSpeed()
        {
            float distance = (gameObject.transform.position - _dashLocation).magnitude;
            // Speed = distance / time
            _dashSpeed = distance / DashAnimationTime;
        }

        private void Kick()
        {
            _kicking = true;
            _remainingAnimationTime = _kickAnimationTimes[_directionIndex];
            
            // Flip if facing other way
            if (gameObject.transform.position.x < _dashLocation.x)
            {
                FlipKizaru();
                _flippedWhileKicking = true;
            }
            
            // Enable hitbox
            kickHitboxes[_directionIndex].enabled = true;
            _animator.SetBool(animationNames[_directionIndex], true);
            // Movement is frozen
            _kizaryRigidBody2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void FlipKizaru()
        {
            DetachChildren();
            transform.Rotate(0f, 180f, 0f);
            AttackChildren();
        }
        
        private void DetachChildren()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                if (child.CompareTag("UI"))
                {
                    _childrenGameObjects.Add(child);
                    child.transform.parent = null;
                }
            }
        }

        private void AttackChildren()
        {
            for (int i = 0; i < _childrenGameObjects.Count; i++)
            {
                GameObject child = _childrenGameObjects[i];
                child.transform.parent = gameObject.transform;
            }

            _childrenGameObjects = new List<GameObject>();
        }

        private void FixedUpdate()
        {
            if (_remainingAnimationTime > 0) _remainingAnimationTime -= Time.deltaTime;
            if (_remainingCooldownPeriod > 0) _remainingCooldownPeriod -= Time.deltaTime;
            // if we are dashing then move
            if (_dashingFrom || _dashingTo)
            {
                float step = _dashSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(gameObject.transform.position, _dashLocation, step);
            }
        }
    }
}