using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
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
        canvas.SetActive(state == GameState.LevelMenu);
    }

    public void OnLevel1()
    {
        SceneManager.LoadScene(2);
        GameManager.Instance.ActualGamePlay();
    }

    public void OnLevel2()
    {
        SceneManager.LoadScene(3);
        GameManager.Instance.ActualGamePlay();
    }

    public void OnLevel3()
    {
        SceneManager.LoadScene(4);
        GameManager.Instance.ActualGamePlay();
    }

    public void GoBack()
    {
        GameManager.Instance.UpdateGameStates(GameState.MainMenu);
        SceneManager.LoadScene(0);
    }
}
