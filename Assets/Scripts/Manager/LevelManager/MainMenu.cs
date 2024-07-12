using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        GameManagerOnGameStateChanged(GameManager.Instance.State); // Check the initial state
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        canvas.SetActive(state == GameState.MainMenu);
    }

    public void OnPlayButton()
    {
        GameManager.Instance.GoToLevelMenu();
        SceneManager.LoadScene(1);
    }
}
