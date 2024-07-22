using System;
using UnityEngine;

public class VictoryChecker : MonoBehaviour
{
    [SerializeField] public int requiredZombieCount; // Number of zombies required to achieve victory
    [SerializeField] private ZombieCounter zombieCounter; // Reference to the ZombieCounter script
    [SerializeField] private int levelNumber; // Current level number

    private bool victoryAchieved = false; // Flag to track if victory has been achieved

    void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        zombieCounter = FindObjectOfType<ZombieCounter>();
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void Update()
    {
        CheckVictoryCondition();
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        UIManager.Instance.victoryPanel.SetActive(state == GameState.Victory);
    }

    // Checks if the player completed the current level its in
    private void CheckVictoryCondition()
    {
        if (victoryAchieved) return;

        int currentZombieCount = zombieCounter.GetZombieCount();
        //Debug.Log("Current Zombie Count: " + currentZombieCount + ", Required: " + requiredZombieCount);

        if (currentZombieCount >= requiredZombieCount)
        {
            victoryAchieved = true;
            UnlockNextLevel();
            UIManager.Instance.OnVictory();
        }
    }

    // Unlocks the next level for player if the previous is completed
    private void UnlockNextLevel()
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
