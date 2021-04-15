using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AkainuAttack : MonoBehaviour
{
    [SerializeField] [Range(0f, 100f)] private float ability1Cooldown;
    [SerializeField] private float arriveAnimationTime = 6f;
    [SerializeField] private float ability1AnimationTime;
    [SerializeField] private GameObject cellAflamePrefab;
    private bool ability1Attack = false;
    private float currentCooldown;
    private float currentAbility1AnimationTime = 0f;


    [SerializeField] private Animator _akainuAnimator;
    

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
            _akainuAnimator.SetBool("PunchingGround", false);
            ability1Attack = false;
            currentCooldown = ability1Cooldown;

            CellAflame cellAflame = cellAflamePrefab.GetComponent<CellAflame>();
            currentCooldown += cellAflame.warningAnimationPeriod + cellAflame.periodBetweenWarningAndAflame +
                               cellAflame.cellAflameAnimationPeriod;

            // Show warning
            GameObject cellGameObject = cellAflamePrefab;
            cellGameObject.transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-1f, 3f), 0);
            Instantiate(cellGameObject);
        }
    }
}
