using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button[] levelButtons;

    // [SerializeField] private TextMeshProUGUI healthBoosterText; // UI Text for health boosters count
    // [SerializeField] private TextMeshProUGUI speedBoosterText; // UI Text for speed boosters count
    // [SerializeField] private TextMeshProUGUI starCollectibleText; // UI Text for star collectibles count
    private AudioManager audioManager;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        GameManagerOnGameStateChanged(GameManager.Instance.State); // Check the initial state
        audioManager = FindObjectOfType<AudioManager>();
        //FindUIReferences();

        // Ensure PlayerPrefs has a default value set for "HighestLevelCompleted"
        int highestLevelCompleted = PlayerPrefs.GetInt("HighestLevelCompleted", 0);
        Debug.Log("HighestLevelCompleted at LevelMenu Start: " + highestLevelCompleted);

        // Disable buttons for levels not yet unlocked
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > highestLevelCompleted)
            {
                levelButtons[i].interactable = false;
                Debug.Log("Level " + (i + 1) + " is locked.");
            }
            else
            {
                levelButtons[i].interactable = true;
                Debug.Log("Level " + (i + 1) + " is unlocked.");
            }
        }

        //ShowButtonTexts(false); 
    }

    // private void FindUIReferences()
    // {
    //      if (healthBoosterText == null)
    //     {
    //         GameObject healthBoosterTextObject = GameObject.Find("HealthBoosterText");
    //         if (healthBoosterTextObject != null)
    //         {
    //             healthBoosterText = healthBoosterTextObject.GetComponent<TextMeshProUGUI>();
    //         }
    //         else
    //         {
    //             Debug.LogError("HealthBoosterText GameObject not found in the scene.");
    //         }
    //     }

    //     if (speedBoosterText == null)
    //     {
    //         GameObject speedBoosterTextObject = GameObject.Find("SpeedBoosterText");
    //         if (speedBoosterTextObject != null)
    //         {
    //             speedBoosterText = speedBoosterTextObject.GetComponent<TextMeshProUGUI>();
    //         }
    //         else
    //         {
    //             Debug.LogError("SpeedBoosterText GameObject not found in the scene.");
    //         }
    //     }

    //     if (starCollectibleText == null)
    //     {
    //         GameObject starCollectibleTextObject = GameObject.Find("StarCollectibleText");
    //         if (starCollectibleTextObject != null)
    //         {
    //             starCollectibleText = starCollectibleTextObject.GetComponent<TextMeshProUGUI>();
    //         }
    //         else
    //         {
    //             Debug.LogError("StarCollectibleText GameObject not found in the scene.");
    //         }
    //     }
    // }

    // private void ShowButtonTexts(bool show)
    // {
    //     if (healthBoosterText != null)
    //     {
    //         healthBoosterText.enabled = show;
    //     }
    //     else
    //     {
    //         Debug.Log("Text is null");
    //     }

    //     if (starCollectibleText != null)
    //     {
    //         starCollectibleText.enabled = show;
    //     }
    //     else
    //     {
    //         Debug.Log("Text is null");
    //     }

    //     if (speedBoosterText != null)
    //     {
    //         speedBoosterText.enabled = show;
    //     }
    //     else
    //     {
    //         Debug.Log("Text is null");
    //     }
    // }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        canvas.SetActive(state == GameState.LevelMenu);
    }

    public void OnLevelButton(int levelIndex)
    {
        audioManager.Play("ButtonClick");
        SceneManager.LoadScene(levelIndex);
        GameManager.Instance.StartCountdown();
    }

    public void GoBack()
    {
        audioManager.Play("ButtonClick");
        GameManager.Instance.UpdateGameStates(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }
}
