using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private int initialCompletedLevelIndex;
    [SerializeField] private TextMeshProUGUI healthBoosterText; // UI Text for health boosters count
    [SerializeField] private TextMeshProUGUI speedBoosterText; // UI Text for speed boosters count
    [SerializeField] private TextMeshProUGUI starCollectibleText; // UI Text for star collectibles count
    private AudioManager audioManager;

    private void Start()
    {
        // Uncomment the following line to clear PlayerPrefs for testing purposes
        //PlayerPrefs.DeleteAll();
        audioManager =  FindObjectOfType<AudioManager>();

        FindUIReferences();

        menuPanel.gameObject.SetActive(true);

        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        GameManagerOnGameStateChanged(GameManager.Instance.State); // Check the initial state

        // Initialize PlayerPrefs 
        SetPlayerPrefsForLevel();

        ShowButtonTexts(false);
    }

    private void FindUIReferences()
    {
         if (healthBoosterText == null)
        {
            GameObject healthBoosterTextObject = GameObject.Find("HealthBoosterText");
            if (healthBoosterTextObject != null)
            {
                healthBoosterText = healthBoosterTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("HealthBoosterText GameObject not found in the scene.");
            }
        }

        if (speedBoosterText == null)
        {
            GameObject speedBoosterTextObject = GameObject.Find("SpeedBoosterText");
            if (speedBoosterTextObject != null)
            {
                speedBoosterText = speedBoosterTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("SpeedBoosterText GameObject not found in the scene.");
            }
        }

        if (starCollectibleText == null)
        {
            GameObject starCollectibleTextObject = GameObject.Find("StarCollectibleText");
            if (starCollectibleTextObject != null)
            {
                starCollectibleText = starCollectibleTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("StarCollectibleText GameObject not found in the scene.");
            }
        }
    }

    private void ShowButtonTexts(bool show)
    {
        if (healthBoosterText != null)
        {
            healthBoosterText.enabled = show;
        }
        else
        {
            Debug.Log("Text is null");
        }

        if (starCollectibleText != null)
        {
            starCollectibleText.enabled = show;
        }
        else
        {
            Debug.Log("Text is null");
        }

        if (speedBoosterText != null)
        {
            speedBoosterText.enabled = show;
        }
        else
        {
            Debug.Log("Text is null");
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        menuPanel.SetActive(state == GameState.MainMenu);
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
