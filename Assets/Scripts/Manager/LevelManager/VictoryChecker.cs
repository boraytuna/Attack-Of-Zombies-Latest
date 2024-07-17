using System;
using UnityEngine;

public class VictoryChecker : MonoBehaviour
{
    [SerializeField] public int requiredZombieCount; // Number of zombies required to achieve victory
    [SerializeField] private ZombieCounter zombieCounter; // Reference to the ZombieCounter script

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

    private void CheckVictoryCondition()
    {
        if (victoryAchieved) return;

        int currentZombieCount = zombieCounter.GetZombieCount();

        if (currentZombieCount >= requiredZombieCount)
        {
            victoryAchieved = true;
            UIManager.Instance.OnVictory();
        }
    }
}
