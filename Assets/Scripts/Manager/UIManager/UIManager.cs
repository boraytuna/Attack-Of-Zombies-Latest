using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas; // Reference to the main canvas
    [SerializeField] private GameObject deathPanel; // Reference to the death panel
    [SerializeField] private GameObject pausePanel; // Reference to the pause panel
    [SerializeField] private GameObject pauseButton; // Reference to the pause button 

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of UIManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Check for ESC key press to toggle pause panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausePanel();
        }
    }

    // Method to activate the canvas
    public void LoadCanvas()
    {
        canvas.gameObject.SetActive(true);
    }

    // Method to activate the pause panel and hide the pause button
    public void ActivatePausePanel()
    {
        pausePanel.gameObject.SetActive(true);
        pauseButton.SetActive(false); // Hide the pause button when the game is paused
    }

    // Method to deactivate the pause panel and show the pause button
    public void DeactivatePausePanel()
    {
        pausePanel.gameObject.SetActive(false);
        pauseButton.SetActive(true); // Show the pause button when the game is resumed
    }

    // Method to activate the death panel
    public void OnPlayerDeath()
    {
        deathPanel.gameObject.SetActive(true);
    }

    // Method to toggle the pause panel and game state
    private void TogglePausePanel()
    {
        bool isPaused = pausePanel.activeSelf;
        pausePanel.SetActive(!isPaused);

        if (!isPaused)
        {
            // Pause the game
            GameManager.Instance.PauseAllGameActions();
            pauseButton.SetActive(false); // Hide the pause button when the game is paused
        }
        else
        {
            // Resume the game
            GameManager.Instance.ResumeAllGameActions();
            pauseButton.SetActive(true); // Show the pause button when the game is resumed
        }
    }
}
