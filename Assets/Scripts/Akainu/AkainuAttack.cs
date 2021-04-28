using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AkainuAttack : MonoBehaviour
{
    // General animation stuff
    [SerializeField] private float arriveAnimationTime = 6f;
    private Animator _akainuAnimator;
    
    private float _currentCooldown;
    private float _currentAnimationTime = 0f;

    // Ability 1
    [SerializeField] [Range(0f, 100f)] private float ability1Cooldown;
    private const float Ability1AnimationTime = 1.5f;
    
    [SerializeField] private GameObject cellAflamePrefab;
    private bool _ability1Attack = false;
    [SerializeField] private int numberOfCellsPerSpawn;
    
    // Ability 2
    [SerializeField] [Range(0f, 100f)] private float ability2Cooldown;
    [SerializeField] [Range(0f, 100f)] private float ability2CooldownOffset;
    private const float Ability2AnimationTime = 2.625f;
    private bool _ability2Attack = false;
    
    [SerializeField] private GameObject lavaProjectilePrefab;
    [SerializeField] private float projectileSpawnAreaHeight; 
    private float _projectileSpawnAreaWidth;
    private Vector3 _topLeft;
    [SerializeField] private int numberOfProjectilesPerSpawn;

    private void Start()
    {
        _currentCooldown = arriveAnimationTime;
        _akainuAnimator = GetComponent<Animator>();
        
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        _projectileSpawnAreaWidth = -(2 * bottomLeft.x);
        _topLeft = new Vector3(bottomLeft.x, -bottomLeft.y);
    }

    private void Update()
    {
        // If not attacking and not on cooldown, initiate an attack
        if (!_ability1Attack && !_ability2Attack && _currentCooldown <= 0)
        {
            // twice the chance to get ability 1 compared to ability2
            bool choseAbility1 = Random.Range(0, 2) == 0;
            if (choseAbility1)
            {
                _ability1Attack = true;
                _currentAnimationTime = Ability1AnimationTime;
                _akainuAnimator.SetBool("PunchingGround", true);
            }
            else
            {
                _ability2Attack = true;
                _currentAnimationTime = Ability2AnimationTime;
                _akainuAnimator.SetBool("PunchingSky", true);
            }
        }
        
        // If finished playing ability 1 attack animation
        if (_ability1Attack && _currentAnimationTime <= 0)
        {
            _akainuAnimator.SetBool("PunchingGround", false);
            _ability1Attack = false;
            _currentCooldown = ability1Cooldown;

            CellAflame cellAflame = cellAflamePrefab.GetComponent<CellAflame>();
            _currentCooldown += cellAflame.warningAnimationPeriod + cellAflame.periodBetweenWarningAndAflame +
                               cellAflame.cellAflameAnimationPeriod;

            // Show warning
            for (int i = 0; i < numberOfCellsPerSpawn; i++)
            {
                GameObject cellGameObject = cellAflamePrefab;
                cellGameObject.transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-1f, 3f), 0);
                Instantiate(cellGameObject);
            }
            
        }
        
        // If finished playing ability 2 attack animation
        if (_ability2Attack && _currentAnimationTime <= 0)
        {
            _akainuAnimator.SetBool("PunchingSky", false);
            _ability2Attack = false;
            
            _currentCooldown = ability2Cooldown;
            _currentCooldown += ability2CooldownOffset;

            // Spawn Projectiles
            SpawnLavaProjectiles();
        }
    }

    private void SpawnLavaProjectiles()
    {
        for (int i = 0; i < numberOfProjectilesPerSpawn; i++)
        {
            // Randomise the point within the spawn area
            Vector3 spawnPoint = new Vector3(Random.Range(0, _projectileSpawnAreaWidth),
                Random.Range(0, projectileSpawnAreaHeight));
            // Form screen point
            spawnPoint += _topLeft;
            // Spawn
            GameObject projectile = lavaProjectilePrefab;
            projectile.transform.position = spawnPoint;
            Instantiate(projectile);
        }
    }

    private void FixedUpdate()
    {
        // If on cooldown reduce counter
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }

        // Ability 1 Animation
        if (_currentAnimationTime > 0)
        {
            _currentAnimationTime -= Time.deltaTime;
        }
    }
}
