// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;
// using UnityEngine.UI;
// public class SceneLoader : MonoBehaviour
// {
//     public static SceneLoader Instance { get; private set; }
//     public GameObject loadingScreen;
//     public Slider loadingBar;

//     // private void Awake()
//     // {
//     //     // Singleton pattern to ensure only one instance of GameManager
//     //     if (Instance == null)
//     //     {
//     //         Instance = this;
//     //         DontDestroyOnLoad(gameObject);
//     //     }
//     //     else
//     //     {
//     //         Destroy(gameObject);
//     //     }
//     // }

//     public void LoadScene(int sceneIndex)
//     {
//        StartCoroutine(LoadSceneAsynchronously(sceneIndex));
//     }

//     public void LoadLevels(int sceneIndex)
//     {
//         StartCoroutine(LoadLevelsAsynchronously(sceneIndex));
//     }

//     public void RestartLevel()
//     {
//         StartCoroutine(ReLoadLevelAsynchronously());
//     }

//     public void LoadNextLevel()
//     {
//         StartCoroutine(LoadNextLevelAsynchronously());
//     }

//     IEnumerator LoadSceneAsynchronously(int sceneIndex)
//     {
//         AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
//         loadingScreen.SetActive(true);
//         while(!operation.isDone)
//         {
//             loadingBar.value = operation.progress;
//             yield return null;
//         }
//         loadingScreen.SetActive(false);
//     }

//     IEnumerator LoadLevelsAsynchronously(int sceneIndex)
//     {
//         AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
//         loadingScreen.SetActive(true);
//         while(!operation.isDone)
//         {
//             loadingBar.value = operation.progress;
//             yield return null;
//         }
//         loadingScreen.SetActive(false);
//     }

//     IEnumerator LoadNextLevelAsynchronously()
//     {
//         AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
//         loadingScreen.SetActive(true);
//         while(!operation.isDone)
//         {
//             loadingBar.value = operation.progress;
//             yield return null;
//         }
//         loadingScreen.SetActive(false);
//     }

//     IEnumerator ReLoadLevelAsynchronously()
//     {
//         AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
//         loadingScreen.SetActive(true);
//         while(!operation.isDone)
//         {
//             loadingBar.value = operation.progress;
//             yield return null;
//         }
//         loadingScreen.SetActive(false);
//     }

// }
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System;

public class SceneLoader : MonoBehaviour
{
    public static event Action OnSceneLoaded; // Event to notify when the scene is loaded
    public static SceneLoader Instance { get; private set; }
    public GameObject loadingScreen;
    public Slider loadingBar;

    private ObjectiveCountdownManager objectiveCountdownManager;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SceneLoader
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

    public void LoadLevelScene(int sceneIndex)
    {
        StartCoroutine(LoadLevelsAsynchronously(sceneIndex));
    }

    public void LoadMainMenuScene(int sceneIndex)
    {
        StartCoroutine(LoadMainMenuScenesAsynchronously(sceneIndex));
    }

    public void LoadLevelMenuScene(int sceneIndex)
    {
        StartCoroutine(LoadLevelMenuScenesAsynchronously(sceneIndex));
    }

    public void RestartLevel()
    {
        StartCoroutine(ReLoadLevelAsynchronously());
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelAsynchronously());
    }

    IEnumerator LoadMainMenuScenesAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        GameManager.Instance.BackToMainMenu();
    }

    IEnumerator LoadLevelMenuScenesAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        GameManager.Instance.GoToLevelMenu();
    }

    IEnumerator LoadLevelsAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        OnSceneLoaded?.Invoke();     
    }

    IEnumerator LoadNextLevelAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        OnSceneLoaded?.Invoke();
    }

    IEnumerator ReLoadLevelAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        OnSceneLoaded?.Invoke();
    }

}
