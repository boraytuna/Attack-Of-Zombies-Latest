// using System;
// using UnityEngine;

// public class UIManager : MonoBehaviour
// {
//     [SerializeField] private Canvas canvas; // Reference to the Canvas
//     [SerializeField] private string mainMenuPanelPath = "UIElements/MainMenuPanel"; 
//     [SerializeField] private string levelMenuPanelPath = "UIElements/LevelMenuPanel";
//     private GameObject mainMenuPanelInstance;
//     private GameObject levelMenuPanelInstance;

//     private void Awake()
//     {
//         DontDestroyOnLoad(this.gameObject);
//         FindCanvas();
//         DontDestroyOnLoad(canvas.gameObject);
//         FindAndShowMainMenuPanel();
//         FindLevelMenuPanel();
//     }



//     private void FindCanvas()
//     {
//         canvas = FindObjectOfType<Canvas>();
//     }

//     private void FindAndShowMainMenuPanel()
//     {
//         var mainMenuPanelPrefab = Resources.Load<GameObject>(mainMenuPanelPath);
//         if (mainMenuPanelPrefab == null)
//         {
//             Debug.LogError("Main menu panel prefab not found at path: " + mainMenuPanelPath);
//             return;
//         }

//         mainMenuPanelInstance = Instantiate(mainMenuPanelPrefab, canvas.transform); // Instantiate as a child of the canvas
//         mainMenuPanelInstance.SetActive(true);
//         GamePlayEvents.TriggerMainMenu();
//     }

//     private void FindLevelMenuPanel()
//     {
//         var levelMenuPanelPrefab = Resources.Load<GameObject>(levelMenuPanelPath);
//         if (levelMenuPanelPath == null)
//         {
//             Debug.LogError("Level menu panel prefab not found at path: " + mainMenuPanelPath);
//             return;
//         }

//         mainMenuPanelInstance = Instantiate(levelMenuPanelPrefab, canvas.transform); // Instantiate as a child of the canvas
//         mainMenuPanelInstance.SetActive(false);
//     }
// }
