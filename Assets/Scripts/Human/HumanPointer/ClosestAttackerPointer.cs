using UnityEngine;

public class ClosestAttackerPointer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private GameObject attackerPointer;
    [SerializeField] private string attackerLayerName = "CentralAttacker";
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main camera is not assigned or not found.");
        }

        if (attackerPointer != null)
        {
            attackerPointer.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (playerTransform == null || arrowRectTransform == null)
        {
            Debug.LogError("playerTransform or arrowRectTransform is not assigned.");
            return;
        }

        Transform closestAttacker = FindClosestAttacker();
        if (closestAttacker != null)
        {
            RotateArrowTowards(closestAttacker);
            if (attackerPointer != null && !attackerPointer.activeSelf)
            {
                attackerPointer.gameObject.SetActive(true);
            }
        }
        else
        {
            if (attackerPointer != null && attackerPointer.activeSelf)
            {
                attackerPointer.gameObject.SetActive(false);
            }
        }
    }

    Transform FindClosestAttacker()
    {
        int attackerLayer = LayerMask.NameToLayer(attackerLayerName);
        GameObject[] attackers = FindObjectsOfType<GameObject>();
        Transform closestAttacker = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject attacker in attackers)
        {
            if (attacker.layer == attackerLayer)
            {
                float distanceToAttacker = Vector3.Distance(playerTransform.position, attacker.transform.position);
                if (distanceToAttacker < closestDistance)
                {
                    closestDistance = distanceToAttacker;
                    closestAttacker = attacker.transform;
                }
            }
        }

        return closestAttacker;
    }

    void RotateArrowTowards(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Target is null.");
            return;
        }

        Vector3 direction = target.position - playerTransform.position;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is not assigned or not found.");
            return;
        }

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(playerTransform.position + direction);
        Vector2 directionOnScreen = new Vector2(screenPoint.x, screenPoint.y) - new Vector2(Screen.width / 2, Screen.height / 2);
        
        // Invert the y-axis for correct arrow pointing
        //directionOnScreen.y = -directionOnScreen.y;

        float angle = Mathf.Atan2(directionOnScreen.y, directionOnScreen.x) * Mathf.Rad2Deg;
        arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}