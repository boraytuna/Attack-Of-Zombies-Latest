using UnityEngine;
using System.Collections;

public class ClosestHumanPointer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private GameObject humanPointer;
    [SerializeField] private string humanLayerName = "Human";
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float initialDelay = 1f; // Delay before starting to check for humans

    private void Start()
    {
        StartCoroutine(StartCheckingForHumansAfterDelay());
    }

    private IEnumerator StartCheckingForHumansAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            UpdateHumanPointer();
            yield return null; // Wait until the next frame
        }
    }

    private void UpdateHumanPointer()
    {
        Transform closestHuman = FindClosestHuman();
        if (closestHuman != null)
        {
            RotateArrowTowards(closestHuman);
        }
        else
        {
            if (humanPointer != null && humanPointer.activeSelf)
            {
                humanPointer.SetActive(false);
            }
        }
    }

    private Transform FindClosestHuman()
    {
        int humanLayer = LayerMask.NameToLayer(humanLayerName);
        GameObject[] humans = FindObjectsOfType<GameObject>();
        Transform closestHuman = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject human in humans)
        {
            if (human.layer == humanLayer)
            {
                float distanceToHuman = Vector3.Distance(playerTransform.position, human.transform.position);
                if (distanceToHuman < closestDistance)
                {
                    closestDistance = distanceToHuman;
                    closestHuman = human.transform;
                }
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

        // Invert the y-axis for correct arrow pointing
        // directionOnScreen.y = -directionOnScreen.y;

        float angle = Mathf.Atan2(directionOnScreen.y, directionOnScreen.x) * Mathf.Rad2Deg;
        arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
