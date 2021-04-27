using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerPrefsManager
    {
        private const String DamageToEnemyTroops = "NumberOfTimesHitEnemyTroop";
        private const String DamageFromEnemyTroops = "NumberOfTimesGotHitByEnemyTroop";
        private const String LevelHasBoss = "DoesLevelHaveBoss";
        private const String DamageToEnemyBosses = "NumberOfTimesHitEnemyBoss";
        private const String DamageFromEnemyBosses = "NumberOfTimesGotHitByEnemyBoss";
        private const String CurrentLevel = "WhatIsTheCurrentLevel";
        private const String BossListForLevel = "WhatBossesToSpawnForThisLevel";

        public void ResetPrefs(bool doesLevelHaveBoss)
        {
            PlayerPrefs.SetInt(DamageToEnemyTroops, 0);
            PlayerPrefs.SetInt(DamageFromEnemyTroops, 0);
            PlayerPrefs.SetInt(LevelHasBoss, doesLevelHaveBoss ? 1 : 0);
            if (doesLevelHaveBoss)
            {
                PlayerPrefs.SetInt(DamageToEnemyBosses, 0);
                PlayerPrefs.SetInt(DamageFromEnemyBosses, 0);
            }
            else
            {
                PlayerPrefs.DeleteKey(DamageToEnemyBosses);
                PlayerPrefs.DeleteKey(DamageFromEnemyBosses);
            }
        }

        public void ResetCurrentLevel()
        {
            PlayerPrefs.SetInt(CurrentLevel, 0);
        }
        
        public void IncrementDamageToEnemyTroops()
        {
            int currentNumber;
            // If not there
            if (PlayerPrefs.HasKey(DamageToEnemyTroops))
            {
                currentNumber = PlayerPrefs.GetInt(DamageToEnemyTroops);
            }
            else
            {
                currentNumber = 0;
            }
            
            // Store new value
            PlayerPrefs.SetInt(DamageToEnemyTroops, currentNumber + 1);
            PlayerPrefs.Save();
        }
        
        public void IncrementDamageFromEnemyTroops()
        {
            int currentNumber;
            // If not there
            if (PlayerPrefs.HasKey(DamageFromEnemyTroops))
            {
                currentNumber = PlayerPrefs.GetInt(DamageFromEnemyTroops);
            }
            else
            {
                currentNumber = 0;
            }
            
            // Store new value
            PlayerPrefs.SetInt(DamageFromEnemyTroops, currentNumber + 1);
            PlayerPrefs.Save();
        }
        
        public void IncrementDamageToEnemyBosses()
        {
            int currentNumber;
            // If not there
            if (PlayerPrefs.HasKey(DamageToEnemyBosses))
            {
                currentNumber = PlayerPrefs.GetInt(DamageToEnemyBosses);
            }
            else
            {
                currentNumber = 0;
            }
            
            // Store new value
            PlayerPrefs.SetInt(DamageToEnemyBosses, currentNumber + 1);
            PlayerPrefs.Save();
        }
        
        public void IncrementDamageFromEnemyBosses()
        {
            int currentNumber;
            // If not there
            if (PlayerPrefs.HasKey(DamageFromEnemyBosses))
            {
                currentNumber = PlayerPrefs.GetInt(DamageFromEnemyBosses);
            }
            else
            {
                currentNumber = 0;
            }
            
            // Store new value
            PlayerPrefs.SetInt(DamageFromEnemyBosses, currentNumber + 1);
            PlayerPrefs.Save();
        }

        public void IncrementCurrentLevel()
        {
            int currentNumber;
            // If not there
            if (PlayerPrefs.HasKey(CurrentLevel))
            {
                currentNumber = PlayerPrefs.GetInt(CurrentLevel);
            }
            else
            {
                currentNumber = 0;
            }
            
            // Store new value
            PlayerPrefs.SetInt(CurrentLevel, currentNumber + 1);
            PlayerPrefs.Save();
        }

        public void SetBossesForThisLevel(int[] bossIndexList)
        {
            // Initialise the string
            String listInStringForm = "";
            for (int i = 0; i < bossIndexList.Length; i++)
            {
                listInStringForm += bossIndexList[i].ToString();
                // if not last index, then add semicolon
                if (i != bossIndexList.Length - 1)
                {
                    listInStringForm += ",";
                }
            }
            
            PlayerPrefs.SetString(BossListForLevel, listInStringForm);
            PlayerPrefs.Save();
        }

        public int GetDamageToEnemyTroops()
        {
            if (!PlayerPrefs.HasKey(DamageToEnemyTroops)) return -1;
            return PlayerPrefs.GetInt(DamageToEnemyTroops);
        }
        
        public int GetDamageFromEnemyTroops()
        {
            if (!PlayerPrefs.HasKey(DamageFromEnemyTroops)) return -1;
            return PlayerPrefs.GetInt(DamageFromEnemyTroops);
        }
        
        public bool GetDoesLevelHaveBoss()
        {
            if (!PlayerPrefs.HasKey(LevelHasBoss)) return false;
            return PlayerPrefs.GetInt(LevelHasBoss) == 1;
        }
        
        public int GetDamageToEnemyBosses()
        {
            if (!PlayerPrefs.HasKey(DamageToEnemyBosses)) return -1;
            return PlayerPrefs.GetInt(DamageToEnemyBosses);
        }
        
        public int GetDamageFromEnemyBosses()
        {
            if (!PlayerPrefs.HasKey(DamageFromEnemyBosses)) return -1;
            return PlayerPrefs.GetInt(DamageFromEnemyBosses);
        }

        public int GetCurrentLevel()
        {
            if (!PlayerPrefs.HasKey(CurrentLevel)) return 0;
            return PlayerPrefs.GetInt(CurrentLevel);
        }

        public int[] GetBossListForLevel()
        {
            if (!PlayerPrefs.HasKey(BossListForLevel)) return new int[]{};
            String listInStringForm = PlayerPrefs.GetString(BossListForLevel);
            String[] indexes = listInStringForm.Split(',');
            List<int> indexList = new List<int>();
            foreach (String index in indexes)
            {
                indexList.Add(Int32.Parse(index));
            }

            return indexList.ToArray();
        }
    }
}