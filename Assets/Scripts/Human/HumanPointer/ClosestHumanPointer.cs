using UnityEngine;
using System.Collections;

public class ClosestHumanPointer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private GameObject humanPointer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float initialDelay = 1f; // Delay before starting to check for humans

    private Transform safePointTransform;
    private bool hasSafePoint = false;

    private void Start()
    {
        StartCoroutine(StartCheckingForTargetsAfterDelay());
    }

    private IEnumerator StartCheckingForTargetsAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            UpdatePointer();
            yield return null; // Wait until the next frame
        }
    }

    private void UpdatePointer()
    {
        if (hasSafePoint && safePointTransform != null)
        {
            // Point at the safe point
            RotateArrowTowards(safePointTransform);
        }
        else
        {
            // Point at the closest human
            Transform closestHuman = FindClosestHuman();
            if (closestHuman != null)
            {
                RotateArrowTowards(closestHuman);
                if (humanPointer != null && !humanPointer.activeSelf)
                {
                    humanPointer.SetActive(true);
                }
            }
            else
            {
                if (humanPointer != null && humanPointer.activeSelf)
                {
                    humanPointer.SetActive(false);
                }
            }
        }
    }

    private Transform FindClosestHuman()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        Transform closestHuman = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject human in humans)
        {
            float distanceToHuman = Vector3.Distance(playerTransform.position, human.transform.position);
            if (distanceToHuman < closestDistance)
            {
                closestDistance = distanceToHuman;
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

        Vector3 direction = target.position - playerTransform.position;
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(playerTransform.position + direction);
        Vector2 directionOnScreen = new Vector2(screenPoint.x, screenPoint.y) - new Vector2(Screen.width / 2, Screen.height / 2);

        float angle = Mathf.Atan2(directionOnScreen.y, directionOnScreen.x) * Mathf.Rad2Deg;
        arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Call this method when a safe point is set
    public void SetSafePoint(Transform safePoint)
    {
        safePointTransform = safePoint;
        hasSafePoint = true;
    }

    // Call this method if you want to clear the safe point
    public void ClearSafePoint()
    {
        safePointTransform = null;
        hasSafePoint = false;
    }
}
