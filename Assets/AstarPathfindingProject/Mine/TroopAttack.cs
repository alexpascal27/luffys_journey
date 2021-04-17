using System;
using Pathfinding;
using UnityEngine;

namespace Mine
{
    public class TroopAttack : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private const float AttackAnimationTime = 1f;
        private float remainingAnimationTime = 0f;
        private const float SwordReleaseTime = 0.5f;
        private float remainingSwordReleaseTime = 0f;
        
        public bool attacking = false;

        [SerializeField] private BoxCollider2D swordBoxCollider2D;
        private Rigidbody2D _rb;
        
        // AI Scripts
        private Seeker _seeker;
        private AIPath _aiPath;
        private AIDestinationSetter _aiDestinationSetter;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            //_seeker = GetComponent<Seeker>();
            _aiPath = GetComponent<AIPath>();
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        }

        public void Attack()
        {
            //Debug.Log("called the attack function");
            if (!attacking)
            {
                // Disable AI Scripts
                //_seeker.enabled = false;
                //_aiPath.enabled = false;
                //_aiDestinationSetter.enabled = false;
            
                // Freeze the X and Y of the troop
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
                // Start the timer
                remainingAnimationTime = AttackAnimationTime;
                remainingSwordReleaseTime = SwordReleaseTime;
                animator.SetBool("Attacking", true);
                attacking = true;
            }
        }

        private void Update()
        {
            // If Sword is released
           if (attacking && remainingSwordReleaseTime <= 0)
           {
               swordBoxCollider2D.enabled = true;
           }
           // If finished attacking
            if (attacking && remainingAnimationTime <= 0)
            {
                animator.SetBool("Attacking", false);
                attacking = false;
                swordBoxCollider2D.enabled = false;
                
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                // Enable AI scripts
                //_seeker.enabled = true;
                //_aiPath.enabled = true;
                //_aiDestinationSetter.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (remainingAnimationTime >= 0) remainingAnimationTime -= Time.deltaTime;
            if (remainingSwordReleaseTime >= 0) remainingSwordReleaseTime -= Time.deltaTime;
        }
    }
}