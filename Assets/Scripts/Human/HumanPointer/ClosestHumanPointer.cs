using UnityEngine;

public class ClosestHumanPointer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform arrowRectTransform;
    [SerializeField] private GameObject humanPointer;
    [SerializeField] private string humanLayerName = "CentralHuman";
    [SerializeField] private Camera mainCamera;

    void Awake()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.HumanPointerActivate();
        }   

        mainCamera = Camera.main;
        if(mainCamera == null)
        {
            Debug.Log("main camera is null");
        }

        if (humanPointer != null)
        {
            humanPointer.gameObject.SetActive(true);
        }

        playerTransform = gameObject.GetComponent<Transform>();
        humanPointer = GameObject.Find("HumanPointer");
        if(humanPointer == null)
        {
            Debug.Log("HumanPointer is null");
        }
        arrowRectTransform = GameObject.Find("HumanPointerArrow").GetComponent<RectTransform>();
        if(arrowRectTransform == null)
        {
            Debug.Log("arrow rect is null");
        }
    }

    void Update()
    {
        // if (playerTransform == null || arrowRectTransform == null)
        // {
        //     Debug.LogError("playerTransform or arrowRectTransform is not assigned.");
        //     return;
        // }

        Transform closestHuman = FindClosestHuman();
        if (closestHuman != null)
        {
            RotateArrowTowards(closestHuman);
            if (humanPointer != null && !humanPointer.activeSelf)
            {
                humanPointer.gameObject.SetActive(true);
            }
        }
        else
        {
            if (humanPointer != null && humanPointer.activeSelf)
            {
                humanPointer.gameObject.SetActive(false);
            }
        }
    }

    Transform FindClosestHuman()
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

    void RotateArrowTowards(Transform target)
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
        //directionOnScreen.y = -directionOnScreen.y;

        float angle = Mathf.Atan2(directionOnScreen.y, directionOnScreen.x) * Mathf.Rad2Deg;
        arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
