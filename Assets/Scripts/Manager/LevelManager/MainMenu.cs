using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private int initialCompletedLevelIndex;
    private AudioManager audioManager;

    private void Start()
    {
        // Uncomment the following line to clear PlayerPrefs for testing purposes
        //PlayerPrefs.DeleteAll();
        audioManager =  FindObjectOfType<AudioManager>();

        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        GameManagerOnGameStateChanged(GameManager.Instance.State); // Check the initial state

        // Initialize PlayerPrefs 
        SetPlayerPrefsForLevel();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        canvas.SetActive(state == GameState.MainMenu);
    }

    // Load the level menu on clicking play button
    public void OnPlayButton()
    {
        GameManager.Instance.GoToLevelMenu();
        SceneManager.LoadScene(1);
        audioManager.Play("ButtonClick");
    }

    public void OnQuitButton()
    {
        CollectibleManager.Instance.OnApplicationQuit();
        audioManager.Play("ButtonClick");
        Debug.Log("Closing game");
        Application.Quit();
    }

    // Initialize PlayerPrefs if it's the first time the game is played
    private void SetPlayerPrefsForLevel()
    {
        if (!PlayerPrefs.HasKey("HighestLevelCompleted"))
        {
            PlayerPrefs.SetInt("HighestLevelCompleted", initialCompletedLevelIndex);
            PlayerPrefs.Save();
            Debug.Log("Initialized HighestLevelCompleted to " + initialCompletedLevelIndex);
        }
        else
        {
            Debug.Log("HighestLevelCompleted: " + PlayerPrefs.GetInt("HighestLevelCompleted"));
        }
    }
}
