using System;
using UnityEngine;
public class AkainuAttack : MonoBehaviour
{
    [SerializeField] [Range(0f, 100f)] private float ability1Cooldown;
    [SerializeField] private float arriveAnimationTime = 6f;
    [SerializeField] private float ability1AnimationTime;
    private bool ability1Attack = false;
    private float currentCooldown;
    private float currentAbility1AnimationTime = 0f;

    [SerializeField] private Animator _animator;

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
            _animator.SetBool("PunchingGround", true);
        }
    }

    private void FixedUpdate()
    {
        // If on cooldown reduce counter
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        
        // Ability 1 Animation
        if (currentAbility1AnimationTime > 0)
        {
            currentAbility1AnimationTime -= Time.deltaTime;
        }
        if (ability1Attack && currentAbility1AnimationTime <= 0)
        {
            _animator.SetBool("PunchingGround", false);
            ability1Attack = false;
            currentCooldown = ability1Cooldown;
        }
    }
}
