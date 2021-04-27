using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonBehaviour : MonoBehaviour
{
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