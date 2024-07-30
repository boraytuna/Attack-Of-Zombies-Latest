using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SafePointVictoryChecker : VictoryChecker
{   
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string safePointPrefabPath; // Path to the safe point prefab in the Resources folder
    [SerializeField] private float searchRadius = 100.0f; // Radius within which to find a random point on the NavMesh
    [SerializeField] private ClosestHumanPointer closestHumanPointer;
    private Vector3 safePoint;
    private bool safePointSet = false;
    private bool safePointReached = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI safePointText;
    [SerializeField] private float textDisplayDuration = 4f; // Duration to display the text


    protected override void Start()
    {
        base.Start();
        GamePlayEvents.onPlayerEnterSafePoint += OnPlayerEnterSafePoint;

        closestHumanPointer = FindObjectOfType<ClosestHumanPointer>();
        DisableUI();
    }

    protected override void OnDestroy()
    {
        GamePlayEvents.onPlayerEnterSafePoint -= OnPlayerEnterSafePoint;
        base.OnDestroy();
    }

    private void OnPlayerEnterSafePoint()
    {
        safePointReached = true;
        CheckVictoryCondition();
    }

    private void SetSafePoint()
    {
        bool foundValidPoint = false;
        int attempts = 0;
        int maxAttempts = 10; // Maximum number of attempts to find a valid point

        while (!foundValidPoint && attempts < maxAttempts)
        {
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection += playerTransform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, NavMesh.AllAreas))
            {
                safePoint = hit.position;
                foundValidPoint = true;
                InstantiateSafePoint();
                break;
            }
            else
            {
                attempts++;
                Debug.LogWarning($"Attempt {attempts} failed to find a valid NavMesh position. Retrying...");
            }
        }

        if (!foundValidPoint)
        {
            Debug.LogError("Failed to find a valid NavMesh position for the safe point after multiple attempts.");
        }
    }

    private void InstantiateSafePoint()
    {
        GameObject safePointPrefab = Resources.Load<GameObject>(safePointPrefabPath);
        if (safePointPrefab != null)
        {
            GameObject safePointObject = Instantiate(safePointPrefab, safePoint, Quaternion.identity);
            if (closestHumanPointer != null)
            {
                closestHumanPointer.SetSafePoint(safePointObject.transform);
            }
        }
        else
        {
            Debug.LogError("Safe Point Prefab not found in Resources.");
        }
    }

    protected override void CheckVictoryCondition()
    {
        int currentZombieCount = zombieCounter.GetZombieCount();

        if (currentZombieCount >= requiredZombieCount)
        {
            if (!safePointSet)
            {
                SetSafePoint();
                Debug.Log("Player reached enough zombies for next step");
                ChangeUI();
                safePointSet = true;
            }
        }

        if (safePointReached)
        {
            AchieveVictory();
        }
    }

    protected override void DisplayVictoryMesseage()
    {
        if (victoryMessage.text != null)
        {
            victoryMessage.text = $"You Reached The Safe Point";
        }
        else
        {
            Debug.LogError("victory text is null");
        }
    }

    private void DisableUI()
    {
        safePointText.gameObject.SetActive(false);
    }

    private void ChangeUI()
    {
        Debug.Log("ChangeUI method called"); // For debugging

        safePointText.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterDelay(textDisplayDuration));
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        safePointText.gameObject.SetActive(false);
    }
}
