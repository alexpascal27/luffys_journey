using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class BountyCalculation : MonoBehaviour
    {
        // Stats UI elements
        [SerializeField] private TMP_Text damageToEnemyTroopsText;
        [SerializeField] private TMP_Text damageFromEnemyTroopsText;
        [SerializeField] private TMP_Text damageToEnemyBossesText;
        [SerializeField] private TMP_Text damageFromEnemyBossesText;
        [SerializeField] private TMP_Text totalText;
        [SerializeField] private TMP_Text bountyPosterText;
        
        // Showing stats
        [SerializeField] private bool doesLevelHaveBoss;
        private List<int> _statIntList;
        private int[] _intScaleList = new[] {5000, -1000, 15000, -3000};
        private PlayerPrefsManager _playerPrefsManager;
        private List<TMP_Text> _statsTextList;
        private int _numberOfStats;
        private int _currentStatIndex = 0;
        
        // Animation stuff
        [SerializeField] private float timeBetweenStatsReveal;
        [SerializeField] private float timeBetweenBossReveal;
        private bool _waitingForStatReveal = true;
        private bool _waitingForBossReveal = false;
        private float _remainingWaitTime;
        
        // UI elements to show on boss show
        [SerializeField] private GameObject[] elementsToHide;
        // UI elements to hide on boss show
        [SerializeField] private GameObject[] elementsToShow;
        
        // Variables to help pick boss/bosses
        private int _aokijiLevel1Threshold = 250000;
        // first is threshold to spawn akainu and aokiji, second is akainu and kizaru, third is akainu, aokiji and kizaru
        private int[] _level2BossThreshold = new[] {500000, 600000, 750000, 1000000};
        
        // UI stuff to output change in Boss
        private String[] _bossNames = new[] {"Aokiji", "Kizaru", "Akainu"};
        private String[] _bossQuotes = new[] {"Get Ready to freeze!", "You might be fast, but you ain't faster than light.", "He’s the son of the legendary Dragon?"};
        [SerializeField] private Sprite[] bossIcons;
        [SerializeField] private TMP_Text bossNameText;
        [SerializeField] private GameObject bossIconImage;
        [SerializeField] private TMP_Text bossQuoteText;

        private void Start()
        {
            _remainingWaitTime = timeBetweenStatsReveal;
            _playerPrefsManager = new PlayerPrefsManager();
            
            _numberOfStats = _playerPrefsManager.GetDoesLevelHaveBoss() ? 6 : 4;
            _statsTextList = new List<TMP_Text>();
            _statIntList = new List<int>();
            TMP_Text[] staticListText = new[]
            {
                damageToEnemyTroopsText, damageFromEnemyTroopsText, damageToEnemyBossesText, damageFromEnemyBossesText,
                totalText, bountyPosterText
            };
            for (int i = 0; i < 6; i++)
            {
                _statsTextList.Add(staticListText[i]);
                if (_numberOfStats == 4 && i == 1) i += 2;
            }

            InitStats();
            _playerPrefsManager.ResetPrefs(doesLevelHaveBoss);
        }

        private void InitStats()
        {
            _statIntList.Add(_playerPrefsManager.GetDamageToEnemyTroops() * _intScaleList[0]);
            _statIntList.Add( _playerPrefsManager.GetDamageFromEnemyTroops() * _intScaleList[1]);
            if (_playerPrefsManager.GetDoesLevelHaveBoss())
            {
                _statIntList.Add(_playerPrefsManager.GetDamageToEnemyBosses() * _intScaleList[2]);
                _statIntList.Add(_playerPrefsManager.GetDamageFromEnemyBosses() * _intScaleList[3]);
            }

            int total = 0;
            for (int i = 0; i < _statIntList.Count; i++)
            {
                total += _statIntList[i];
            }

            _statIntList.Add(total);
            _statIntList.Add(total);
        }

        private void Update()
        {
            // If finished waiting for a stat reveal reveal the next one
            if (_waitingForStatReveal && _remainingWaitTime <= 0)
            {
                // Last stat
                if (_currentStatIndex >= _numberOfStats-1)
                {
                    _waitingForStatReveal = false;
                    _waitingForBossReveal = true;
                    _remainingWaitTime = timeBetweenBossReveal;
                }
                // Still have stats to show
                else
                {
                    _remainingWaitTime = timeBetweenStatsReveal;
                }
                
                // Show current stat
                int statInt = _statIntList[_currentStatIndex];
                TMP_Text statText = _statsTextList[_currentStatIndex];
                statText.SetText(statInt.ToString());
                _currentStatIndex++;
            }
            
            // If finished Boss reveal
            if (_waitingForBossReveal && _remainingWaitTime <= 0)
            {
                ShowBoss();
                _waitingForBossReveal = false;
            }
        }

        private void ShowBoss()
        {
            // hide stuff
            foreach (GameObject gameObjectToHide in elementsToHide)
            {
                gameObjectToHide.SetActive(false);
            }
            
            // show stuff
            foreach (GameObject gameObjectToShow in elementsToShow)
            {
                gameObjectToShow.SetActive(true);
            }

            int bountyTotal = _statIntList[_statIntList.Count - 1];

            // Decide boss
            int[] bossesPicked = DecideBoss(bountyTotal);
            // Output boss on UI
            bossNameText.SetText(ConstructBossName(bossesPicked));
            bossIconImage.GetComponent<Image>().sprite = bossIcons[bossesPicked[0]];
            bossQuoteText.SetText(_bossQuotes[bossesPicked[0]]);
        }

        private int[] DecideBoss(int bountyTotal)
        {
            // Get level
            int level = _playerPrefsManager.GetCurrentLevel();
            // Coming from level 1
            if (level == 0)
            {
                // if bounty above aokiji threshold - spawn kizaru
                if (bountyTotal >= _aokijiLevel1Threshold)
                {
                    return new[] {1};
                }
                // otherwise spawn aokiji
                else
                {
                    return new[] {0};
                }
            }
            // coming from level 2
            else
            {
                // if spawn all 3
                if (bountyTotal >= _level2BossThreshold[3])
                {
                    return new[] {2, 1, 0};
                }
                // spawn kizaru and akainu
                else if (bountyTotal >= _level2BossThreshold[2])
                {
                    return new[] {2, 1};
                }
                // spawn aokiji and akainu
                else if(bountyTotal >= _level2BossThreshold[1])
                {
                    return new[] {2, 0};
                }
                // spawn only akainu
                else
                {
                    return new[] {2};
                }
            }
        }

        private String ConstructBossName(int[] bossesPicked)
        {
            switch (bossesPicked.Length)
            {
                case(1):
                    return _bossNames[bossesPicked[0]];
                
                case(2):
                    return _bossNames[bossesPicked[0]] + " and " + _bossNames[bossesPicked[1]];
                
                case(3):
                    return _bossNames[bossesPicked[0]] + ", " + _bossNames[bossesPicked[1]] + " and " + _bossNames[bossesPicked[2]];
            }

            return "";
        }

        private void FixedUpdate()
        {
            if (_remainingWaitTime > 0) _remainingWaitTime -= Time.deltaTime;
        }
    }
}