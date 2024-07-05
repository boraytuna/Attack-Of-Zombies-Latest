using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : MonoBehaviour
{
    public static GamePauser Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GamePauser
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

    // Method to pause the game
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    // Method to resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
