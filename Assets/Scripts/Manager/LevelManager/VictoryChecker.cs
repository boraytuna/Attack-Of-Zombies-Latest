using System;
using TMPro;
using UnityEngine;

public abstract class VictoryChecker : MonoBehaviour
{
    [SerializeField] public int requiredZombieCount; // Number of zombies required to achieve victory
    [SerializeField] protected ZombieCounter zombieCounter; // Reference to the ZombieCounter script
    [SerializeField] public int levelNumber; // Current level number

    [Header("Victory Checker")]
    [SerializeField] protected TextMeshProUGUI victoryMessage; // Reference to the victory message TextMeshProUGUI


    protected bool victoryAchieved = false; // Flag to track if victory has been achieved

    protected virtual void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        zombieCounter = FindObjectOfType<ZombieCounter>();
    }

    protected virtual void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    protected virtual void Update()
    {
        CheckVictoryCondition();
    }

    protected virtual void GameManagerOnGameStateChanged(GameState state)
    {
        UIManager.Instance.victoryPanel.SetActive(state == GameState.Victory);
    }

    protected abstract void CheckVictoryCondition();

    protected void AchieveVictory()
    {
        if (victoryAchieved) return;

        victoryAchieved = true;
        UnlockNextLevel();
        UIManager.Instance.OnVictory();
        GamePlayEvents.TriggerPlayerWin();
        DisplayVictoryMesseage();
    }

    protected abstract void DisplayVictoryMesseage();

    protected void UnlockNextLevel()
    {
        int highestLevelCompleted = PlayerPrefs.GetInt("HighestLevelCompleted", 0);
        Debug.Log("Current HighestLevelCompleted: " + highestLevelCompleted);

        if (levelNumber > highestLevelCompleted)
        {
            PlayerPrefs.SetInt("HighestLevelCompleted", levelNumber);
            PlayerPrefs.Save();
            Debug.Log("New HighestLevelCompleted: " + levelNumber);
        }
    }
}
