using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button[] levelButtons;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        GameManagerOnGameStateChanged(GameManager.Instance.State); // Check the initial state

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
    }

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
        SceneManager.LoadScene(levelIndex);
        GameManager.Instance.ActualGamePlay();
    }

    public void GoBack()
    {
        GameManager.Instance.UpdateGameStates(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }
}
