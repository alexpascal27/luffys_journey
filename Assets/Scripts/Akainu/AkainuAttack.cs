using System;
using UnityEngine;
public class AkainuAttack : MonoBehaviour
{
    [SerializeField] [Range(0f, 100f)] private float ability1Cooldown;
    [SerializeField] private float arriveAnimationTime = 6f;
    [SerializeField] private float ability1AnimationTime;
    [SerializeField] private Vector3 ability1Spawn;
    private bool ability1Attack = false;
    private float currentCooldown;
    private float currentAbility1AnimationTime = 0f;

    [SerializeField] private float periodBetweenWarningAndAflame;
    private float remainingWait = 0f;
    private bool waiting = false;
    [SerializeField] private float warningAnimationPeriod;
    private float remainingWarningAnimationPeriod = 0f;
    private bool warningAnimating = false;
    [SerializeField] private float cellAflameAnimationPeriod;
    private bool aflameAnimating = false;
    private float remainingCellAflameAnimationPeriod = 0f;
    

    [SerializeField] private Animator _akainuAnimator;
    [SerializeField] private Animator _ability1Animator;

    private void Start()
    {
        currentCooldown = arriveAnimationTime;
    }

    private void Update()
    {
        // If not attacking and not on cooldown, initiate attack
        if (!ability1Attack && currentCooldown <= 0)
        {
            ability1Attack = true;
            currentAbility1AnimationTime = ability1AnimationTime;
            _akainuAnimator.SetBool("PunchingGround", true);
        }
        // Warning finished
        if (warningAnimating && remainingWarningAnimationPeriod <= 0)
        {
            warningAnimating = false;
            _ability1Animator.SetBool("Warning" , false);

            waiting = true;
            remainingWait = periodBetweenWarningAndAflame;
        }
        // Waiting between warning and flame finished
        if (waiting && remainingWait <= 0)
        {
            waiting = false;

            aflameAnimating = true;
            _ability1Animator.SetBool("Flame" , true);
            remainingCellAflameAnimationPeriod = cellAflameAnimationPeriod;
        }
        // Finished cell blazing
        if (aflameAnimating && remainingCellAflameAnimationPeriod <= 0)
        {
            aflameAnimating = false;
            _ability1Animator.SetBool("Flame" , false);
            
            currentCooldown = ability1Cooldown;
            _akainuAnimator.SetBool("PunchingGround", false);
            ability1Attack = false;
        }
    }

    private void FixedUpdate()
    {
        // If on cooldown reduce counter
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (warningAnimating)
        {
            remainingWarningAnimationPeriod -= Time.deltaTime;
        }

        if (aflameAnimating)
        {
            remainingCellAflameAnimationPeriod -= Time.deltaTime;
        }

        if (waiting)
        {
            remainingWait -= Time.deltaTime;
        }
        
        // Ability 1 Animation
        if (currentAbility1AnimationTime > 0)
        {
            currentAbility1AnimationTime -= Time.deltaTime;
        }
        if (ability1Attack && currentAbility1AnimationTime <= 0)
        {
            _akainuAnimator.SetBool("PunchingGround", false);
            ability1Attack = false;
            currentCooldown = ability1Cooldown;
            
            // Show warning
            remainingWarningAnimationPeriod = warningAnimationPeriod;
            warningAnimating = true;
            _ability1Animator.SetBool("Warning" , true);
        }
    }
}
