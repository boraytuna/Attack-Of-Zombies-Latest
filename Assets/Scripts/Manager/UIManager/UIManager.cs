using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas; // Reference to the main canvas
    [SerializeField] private GameObject deathPanel; // Reference to the death panel
    [SerializeField] private GameObject pausePanel; // Reference to the pause panel
    [SerializeField] private GameObject pauseButton; // Reference to the pause button 
    [SerializeField] public GameObject humanPointer; // Reference to the human pointer
    [SerializeField] public GameObject joyStick; // Reference to the joyStick object
    [SerializeField] private GameObject victoryPanel; // Reference to the victory panel

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;

        // Enable pause button and humanpointer and joystick
        canvas.gameObject.SetActive(true); 
        pauseButton.gameObject.SetActive(true);
        joyStick.gameObject.SetActive(true);
        humanPointer.gameObject.SetActive(true);

        // Disable panels initially
        deathPanel.SetActive(false);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        
    }

    // Method to toggle the pause game
    public void PauseGame()
    {
        if (pausePanel != null)
        {
            bool isPaused = pausePanel.activeSelf;
            pausePanel.SetActive(!isPaused);

            if (!isPaused)
            {
                // Pause the game
                GameManager.Instance.OpenPauseState();
                Time.timeScale = 0;
                if (pauseButton != null)
                {
                    pauseButton.SetActive(false); // Hide the pause button when the game is paused
                }
                else
                {
                    Debug.LogError("Pause button reference is null in UIManager.");
                }
            }
        }
    }

    // Method to resume the game after its paused
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            Time.timeScale = 1;
            GameManager.Instance.ActualGamePlay();
            pausePanel.SetActive(false);
            pauseButton.SetActive(true); // Show the pause button when the game is resumed
        }
        else
        {
            Debug.LogError("Pause panel reference is null in UIManager.");
        }
    }

    // Method to go back to levels scene
    public void OnLevelsButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.GoToLevelMenu();
        SceneManager.LoadScene(1);
    }

    // Method to go back to main menu scene
    public void OnMainMenuButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.BackToMainMenu();
        SceneManager.LoadScene(0);
    }

    // Method to restart the current level, after dying or on player's purpose
    public void OnRestartButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.ActualGamePlay();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to move to the next level
    public void OnNextLevelButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.ActualGamePlay();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Method to open the death panel, freeze the time and notify the game manager when the player dies
    public void OnPlayerDeath()
    {
        Time.timeScale = 0;
        GameManager.Instance.WhenPlayerDies();
        deathPanel.SetActive(true); 
    }

    // Method to open the victory panel, freeze the time and change the game state
    public void OnVictory()
    {
        Time.timeScale = 0;
        GameManager.Instance.WhenPlayerWins();
        pauseButton.SetActive(false);
        victoryPanel.SetActive(true);
    }

}