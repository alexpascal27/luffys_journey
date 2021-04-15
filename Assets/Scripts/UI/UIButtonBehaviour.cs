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
}