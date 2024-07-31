using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas")]
    [SerializeField] private Canvas canvas; // Reference to the main canvas

    [Header("GamePlay Panels")]
    [SerializeField] private GameObject deathPanel; // Reference to the death panel
    [SerializeField] private GameObject pausePanel; // Reference to the pause panel
    [SerializeField] public GameObject victoryPanel; // Reference to the victory panel
    [SerializeField] public GameObject gamePlayPanel; //Reference to the gameplay panel

    [Header("GamePlay UI")]
    [SerializeField] private GameObject pauseButton; // Reference to the pause button 
    [SerializeField] private GameObject zombieCounter; // Reference to the zombie counter text
    [SerializeField] public GameObject humanPointer; // Reference to the human pointer
    [SerializeField] public GameObject joyStick; // Reference to the joyStick object
    [SerializeField] private Button[] boosterButtons; // Array to reference booster buttons 

    private AudioManager audioManager;

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
        audioManager = FindObjectOfType<AudioManager>();

        // Enable pause button and human pointer and joystick
        canvas.gameObject.SetActive(true); 
        pauseButton.gameObject.SetActive(true);
        humanPointer.gameObject.SetActive(true);
        zombieCounter.gameObject.SetActive(true);
        joyStick.gameObject.SetActive(false); // Initially disabled

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
        Debug.Log($"Game State Changed: {state}");

        if (state == GameState.ActualGamePlay)
        {
            ActivateJoystick();
        }
    }

    private void ActivateJoystick()
    {
        if (joyStick != null)
        {
            joyStick.SetActive(true);
        }
        else
        {
            Debug.LogError("Joystick reference is not assigned.");
        }
    }

    // Method to toggle the pause game
    public void PauseGame()
    {
        if (pausePanel != null)
        {
            bool isPaused = pausePanel.activeSelf;
            pausePanel.SetActive(!isPaused);
            audioManager.Play("ButtonClick");

            if (!isPaused)
            {
                // Pause the game
                GameManager.Instance.OpenPauseState();
                Time.timeScale = 0;
                gamePlayPanel.SetActive(false);
                SetBoosterButtonsActive(false);
            }
        }
    }

    // Method to resume the game after its paused
    public void ResumeGame()
    {   
        if (pausePanel != null)
        {
            audioManager.Play("ButtonClick");
            Time.timeScale = 1;
            GameManager.Instance.ActualGamePlay();
            pausePanel.SetActive(false);
            gamePlayPanel.SetActive(true);
            SetBoosterButtonsActive(true);
        }   
    }

    // Method to go back to levels scene
    public void OnLevelsButton()
    {
        audioManager.Play("ButtonClick");
        Time.timeScale = 1;
        GameManager.Instance.GoToLevelMenu();
        SceneManager.LoadScene(1);
    }

    // Method to go back to main menu scene
    public void OnMainMenuButton()
    {
        audioManager.Play("ButtonClick");
        Time.timeScale = 1;
        GameManager.Instance.BackToMainMenu();
        SceneManager.LoadScene(0);
    }

    // Method to restart the current level, after dying or on player's purpose
    public void OnRestartButton()
    {
        audioManager.Play("ButtonClick");
        Time.timeScale = 1;
        GameManager.Instance.UpdateGameStates(GameState.Countdown);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to move to the next level
    public void OnNextLevelButton()
    {
        audioManager.Play("ButtonClick");
        Time.timeScale = 1;
        GameManager.Instance.UpdateGameStates(GameState.Countdown);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
  
    // Method to open the death panel, freeze the time and notify the game manager when the player dies
    public void OnPlayerDeath()
    {
        audioManager.Play("RoundLose");
        Time.timeScale = 0;
        GameManager.Instance.WhenPlayerDies();
        deathPanel.SetActive(true); 
        gamePlayPanel.SetActive(false);
        SetBoosterButtonsActive(false);
    }

    // Method to open the victory panel, freeze the time and change the game state
    public void OnVictory()
    {
        audioManager.Play("RoundWin");
        Time.timeScale = 0;
        GameManager.Instance.WhenPlayerWins();
        victoryPanel.SetActive(true);
        SetBoosterButtonsActive(false);
        gamePlayPanel.SetActive(false);
    }

    private void SetBoosterButtonsActive(bool isActive)
    {
        foreach (Button button in boosterButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }
}
