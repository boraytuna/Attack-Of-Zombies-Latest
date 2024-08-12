// using UnityEngine;
// using System.Collections;

// public class ClosestHumanPointer : MonoBehaviour
// {
//     [SerializeField] private Transform playerTransform;
//     [SerializeField] private RectTransform arrowRectTransform;
//     [SerializeField] private GameObject humanPointer;
//     [SerializeField] private Camera mainCamera;
//     [SerializeField] private float initialDelay = 1f; // Delay before starting to check for humans

//     private Transform safePointTransform;
//     private bool hasSafePoint = false;

//     private void Start()
//     {
//         StartCoroutine(StartCheckingForTargetsAfterDelay());
//     }

//     private IEnumerator StartCheckingForTargetsAfterDelay()
//     {
//         yield return new WaitForSeconds(initialDelay);
//         while (true)
//         {
//             UpdatePointer();
//             yield return null; // Wait until the next frame
//         }
//     }

//     private void UpdatePointer()
//     {
//         if (hasSafePoint && safePointTransform != null)
//         {
//             // Point at the safe point
//             RotateArrowTowards(safePointTransform);
//         }
//         else
//         {
//             // Point at the closest human
//             Transform closestHuman = FindClosestHuman();
//             if (closestHuman != null)
//             {
//                 RotateArrowTowards(closestHuman);
//             }
//         }
//     }

//     private Transform FindClosestHuman()
//     {
//         GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
//         Transform closestHuman = null;
//         float closestDistance = Mathf.Infinity;

//         foreach (GameObject human in humans)
//         {
//             float distanceToHuman = Vector3.Distance(playerTransform.position, human.transform.position);
//             if (distanceToHuman < closestDistance)
//             {
//                 closestDistance = distanceToHuman;
//                 closestHuman = human.transform;
//             }
//         }

//         return closestHuman;
//     }

//     private void RotateArrowTowards(Transform target)
//     {
//         if (target == null)
//         {
//             Debug.LogError("Target is null.");
//             return;
//         }

//         Vector3 direction = (target.position - playerTransform.position).normalized;
//         Vector3 screenPoint = mainCamera.WorldToScreenPoint(playerTransform.position + direction * 10f);
//         Vector2 directionOnScreen = new Vector2(screenPoint.x, screenPoint.y) - new Vector2(Screen.width / 2, Screen.height / 2);

//         float angle = Mathf.Atan2(directionOnScreen.y, directionOnScreen.x) * Mathf.Rad2Deg;
//         arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
//     }

//     // Call this method when a safe point is set
//     public void SetSafePoint(Transform safePoint)
//     {
//         safePointTransform = safePoint;
//         hasSafePoint = true;
//         UpdatePointer(); // Update pointer immediately after setting the safe point
//     }

//     // Call this method if you want to clear the safe point
//     public void ClearSafePoint()
//     {
//         safePointTransform = null;
//         hasSafePoint = false;
//     }
// }
using UnityEngine;
using System.Collections;

public class ClosestHumanPointer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private GameObject humanPointer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float initialDelay = 1f; // Delay before starting to check for humans
    [SerializeField] private float checkInterval = 0.1f; // Interval for checking closest human
    [SerializeField] private float distanceThreshold = 1f; // Distance threshold for re-checking closest human

    private Transform safePointTransform;
    private bool hasSafePoint = false;
    private Transform closestHuman;
    private Vector3 lastPlayerPosition;

    private void Start()
    {
        lastPlayerPosition = playerTransform.position;
        StartCoroutine(StartCheckingForTargetsAfterDelay());
    }

    private IEnumerator StartCheckingForTargetsAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        InvokeRepeating(nameof(CheckClosestHuman), 0f, checkInterval); // Check closest human at intervals
    }

    private void CheckClosestHuman()
    {
        if (!hasSafePoint)
        {
            if (closestHuman == null || Vector3.Distance(playerTransform.position, lastPlayerPosition) > distanceThreshold)
            {
                closestHuman = FindClosestHuman();
                lastPlayerPosition = playerTransform.position; // Update last player position
                humanPointer.SetActive(closestHuman != null); // Disable pointer if no humans found
            }
        }
    }

    private void Update()
    {
        UpdatePointer();
    }

    private void UpdatePointer()
    {
        if (hasSafePoint && safePointTransform != null)
        {
            RotateArrowTowards(safePointTransform);
        }
        else if (closestHuman != null)
        {
            RotateArrowTowards(closestHuman);
        }
    }

    private Transform FindClosestHuman()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        Transform closestHuman = null;
        float closestDistanceSqr = Mathf.Infinity;

        Vector3 playerPosition = playerTransform.position; // Cache player's position

        foreach (GameObject human in humans)
        {
            Vector3 directionToHuman = human.transform.position - playerPosition;
            float distanceSqrToHuman = directionToHuman.sqrMagnitude;

            if (distanceSqrToHuman < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToHuman;
                closestHuman = human.transform;
            }
        }

        return closestHuman;
    }

    private void RotateArrowTowards(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Target is null.");
            return;
        }

        Vector3 direction = (target.position - playerTransform.position).normalized;
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(playerTransform.position + direction * 10f);
        Vector2 directionOnScreen = new Vector2(screenPoint.x, screenPoint.y) - new Vector2(Screen.width / 2, Screen.height / 2);

        float angle = Mathf.Atan2(directionOnScreen.y, directionOnScreen.x) * Mathf.Rad2Deg;
        arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void SetSafePoint(Transform safePoint)
    {
        safePointTransform = safePoint;
        hasSafePoint = true;
        UpdatePointer(); // Update pointer immediately after setting the safe point
    }

    public void ClearSafePoint()
    {
        safePointTransform = null;
        hasSafePoint = false;
    }
}
