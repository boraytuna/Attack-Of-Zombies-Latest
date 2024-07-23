using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveCountdownManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI countdownText; 

    private GameObject gamePlayPanel;
    private GameObject objectiveTextObject;
    private GameObject countdownTextObject;

    [Header("Settings")]
    [SerializeField] private VictoryChecker victoryChecker;
    [SerializeField] private float countdownTime = 3f;

    private void Start()
    {   
        FindUIReferencesAndVictoryChecker(); // Find UI components and script reference

        gamePlayPanel.SetActive(false);

        GameManager.OnGameStateChanged += OnGameStateChanged;

        StartObjectiveCountdown();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {

    }

    private void StartObjectiveCountdown()
    {
        Debug.Log("State is countdown");
        objectiveText.text = $"Capture {victoryChecker.requiredZombieCount} Humans";
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        if (countdownText == null || objectivePanel == null)
        {
            Debug.LogError("Countdown Text or Objective Panel is not assigned.");
            yield break; // Exit the coroutine if essential references are missing
        }

        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            countdownText.text = Mathf.Ceil(remainingTime).ToString();
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);

        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }
        if (gamePlayPanel != null)
        {
            gamePlayPanel.SetActive(true);
        }

        GameManager.Instance.UpdateGameStates(GameState.ActualGamePlay);
    }

    private void FindUIReferencesAndVictoryChecker()
    {
        objectivePanel = GameObject.Find("ObjectivePanel");
        if(objectivePanel == null)
        {
            Debug.LogError("Panel is null");
        }

        objectiveTextObject = GameObject.Find("CaptureHumans");
        if(objectiveTextObject == null)
        {
            Debug.LogError("Panel is null");
        }
        else
        {
            objectiveText = objectiveTextObject.GetComponent<TextMeshProUGUI>();
        }

        countdownTextObject = GameObject.Find("Countdown");
        if(countdownTextObject == null)
        {
            Debug.LogError("Panel is null");
        }
        else
        {
            countdownText = countdownTextObject.GetComponent<TextMeshProUGUI>();
        }

        gamePlayPanel = GameObject.Find("GameplayPanel");
        if (gamePlayPanel == null)
        {
            Debug.LogError("GameplayPanel is null");
        }

        victoryChecker = FindObjectOfType<VictoryChecker>();
        if(victoryChecker == null)
        {
            
        }

        // Find the victory checker dynamically
        victoryChecker = FindObjectOfType<VictoryChecker>();
        if (victoryChecker == null)
        {
            Debug.LogError("No VictoryChecker found in the scene.");
        }

    }
}
