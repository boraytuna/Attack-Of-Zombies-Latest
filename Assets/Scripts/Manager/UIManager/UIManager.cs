using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas; // Reference to the main canvas
    [SerializeField] private GameObject deathPanel; // Reference to the death panel
    [SerializeField] private GameObject pausePanel; // Reference to the pause panel
    [SerializeField] private GameObject pauseButton; // Reference to the pause button 
    [SerializeField] public GameObject humanPointer; // Reference to the human pointer

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

    public void Start()
    {
        // Find and assign references dynamically
        canvas = GetComponentInChildren<Canvas>(); // Find the Canvas component in children
        if (canvas == null)
        {
            Debug.LogError("Canvas component not found in children of UIManager.");
            return;
        }

        // Assuming DeathPanel, PausePanel, PauseButton are direct children of the canvas
        deathPanel = canvas.transform.Find("DeathScreen").gameObject;
        pausePanel = canvas.transform.Find("PauseScreen").gameObject;
        pauseButton = canvas.transform.Find("PauseButton").gameObject;
        humanPointer = canvas.transform.Find("HumanPointer").gameObject;

        // Disable panels initially
        deathPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        // Check for ESC key press to toggle pause panel
        if (Input.GetKeyDown(KeyCode.Escape) && !deathPanel.activeSelf)
        {
            TogglePausePanel();
        }
    }

    // Method to activate the canvas
    public void LoadCanvas()
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas reference is null in UIManager.");
        }

        if (humanPointer != null)
        {
            humanPointer.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas reference is null in UIManager.");
        }
    }

    // Method to activate the pause panel and hide the pause button
    public void ActivatePausePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            pauseButton.SetActive(false); // Hide the pause button when the game is paused
        }
        else
        {
            Debug.LogError("Pause panel reference is null in UIManager.");
        }
    }

    // Method to deactivate the pause panel and show the pause button
    public void DeactivatePausePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            pauseButton.SetActive(true); // Show the pause button when the game is resumed
        }
        else
        {
            Debug.LogError("Pause panel reference is null in UIManager.");
        }
    }

    // Method to activate the death panel
    public void OnPlayerDeath()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Death panel reference is null in UIManager.");
        }
    }

    // Method to deactivate death panel
    public void DeactivateDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Death panel reference is null in UIManager.");
        }
    }
    // Method to toggle the pause panel and game state
    private void TogglePausePanel()
    {
        if (pausePanel != null)
        {
            bool isPaused = pausePanel.activeSelf;
            pausePanel.SetActive(!isPaused);

            if (!isPaused)
            {
                // Pause the game
                GameManager.Instance.PauseAllGameActions();
                if (pauseButton != null)
                {
                    pauseButton.SetActive(false); // Hide the pause button when the game is paused
                }
                else
                {
                    Debug.LogError("Pause button reference is null in UIManager.");
                }
            }
            else
            {
                // Resume the game
                GameManager.Instance.ResumeAllGameActions();
                if (pauseButton != null)
                {
                    pauseButton.SetActive(true); // Show the pause button when the game is resumed
                }
                else
                {
                    Debug.LogError("Pause button reference is null in UIManager.");
                }
            }
        }
        else
        {
            Debug.LogError("Pause panel reference is null in UIManager.");
        }
    }

    public void HumanPointerActivate()
    {
        humanPointer.gameObject.SetActive(true);
    }

    public void HumanPointerDeactivate()
    {
        humanPointer.gameObject.SetActive(false);
    }
}
