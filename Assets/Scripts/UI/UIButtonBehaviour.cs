using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonBehaviour : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private bool doesLevelHaveBoss = false;

    [SerializeField] private GameObject[] bossesPrefabs;

    private void Start()
    {
        PlayerPrefsManager playerPrefsManager = new PlayerPrefsManager();
        
        // Reset Preferences
        // first level reset current level
        if (levelNumber ==  0)
        {
            playerPrefsManager.ResetCurrentLevel();
        }
        // if level
        if (levelNumber >= 0)
        {
            playerPrefsManager.ResetPrefs(doesLevelHaveBoss);   
        }
        
        // Destroy bosses that we do not need
        int[] bossIndexList = playerPrefsManager.GetBossListForLevel();
        // For each boss that we need to spawn
        for(int i = 0; i < bossesPrefabs.Length; i++) 
        {
            GameObject bossPrefab = bossesPrefabs[i];
            // if not in boss that we need spawned, then destroy
            if (!bossIndexList.Contains(i))
            {
                Destroy(bossPrefab);
                bossesPrefabs[i] = null;
            }
        }
    }

    public void OnQuitButtonClick()
    {
        Debug.Log("You want to quit!");        
    }

    public void OnStartButtonClick()
    {
        // Load level 1
        SceneManager.LoadScene(0);
    }

    public void OnCreditsButtonClick()
    {
        // Load credits screen
        Debug.Log("You want credits!");
    }

    public void OnBackToMainMenuButtonClick()
    {
        // load main screen
        SceneManager.LoadScene(3);
    }

    public void OnNextLevelButtonClick()
    {
        PlayerPrefsManager playerPrefsManager = new PlayerPrefsManager();
        
        // Get current level
        int currentLevel = playerPrefsManager.GetCurrentLevel();
        
        // Increment current level
        playerPrefsManager.IncrementCurrentLevel();
        
        // Change scene
        SceneManager.LoadScene(currentLevel + 1);
    }
}