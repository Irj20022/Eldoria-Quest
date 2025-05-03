using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private FadeCanvasController fadeController;

    public void StartGame()
    {
        
        GameObject existingPlayer = GameObject.FindWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }

        
        Time.timeScale = 1f;

        
        SceneManager.LoadScene("Town");
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting...");
    }
}
