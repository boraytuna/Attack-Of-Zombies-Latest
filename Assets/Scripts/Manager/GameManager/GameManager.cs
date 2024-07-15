using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameStates(GameState.MainMenu);
    }

    public void UpdateGameStates(GameState newState)
    {
        Debug.Log($"GameState changing from {State} to {newState}");
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenuActions();
                break;
            case GameState.LevelMenu:
                HandleLevelMenuActions();
                break;
            case GameState.ActualGamePlay:
                HandleGamePlayActions();
                break;
            case GameState.PauseState:
                HandlePauseMenuActions();
                break;
            case GameState.Victory:
                HandleVictoryActions();
                break;
            case GameState.Lose:
                HandleLoseActions();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
        //Debug.Log($"GameState changed to {newState}");
    }

    private void HandleMainMenuActions()
    {
        // Logic to handle main menu actions
    }

    private void HandleLevelMenuActions()
    {
        // Logic to handle level menu actions
    }

    private void HandleGamePlayActions()
    {
        // Logic to handle actual gameplay actions
    }

    private void HandlePauseMenuActions()
    {
        // Logic to handle pause menu actions
    }

    private void HandleVictoryActions()
    {
        // Logic to handle victory actions
    }

    private void HandleLoseActions()
    {
        // Logic to handle lose actions
    }

    public void BackToMainMenu()
    {
        UpdateGameStates(GameState.MainMenu);
    }

    public void GoToLevelMenu()
    {
        UpdateGameStates(GameState.LevelMenu);
    }

    public void ActualGamePlay()
    {
        UpdateGameStates(GameState.ActualGamePlay);
    }

    public void OpenPauseState()
    {
        UpdateGameStates(GameState.PauseState);
    }

    public void WhenPlayerWins()
    {
        UpdateGameStates(GameState.Victory);
    }

    public void WhenPlayerDies()
    {
        UpdateGameStates(GameState.Lose);
    }
}

public enum GameState
{
    MainMenu,
    LevelMenu,
    ActualGamePlay,
    PauseState,
    Victory,
    Lose,
}
