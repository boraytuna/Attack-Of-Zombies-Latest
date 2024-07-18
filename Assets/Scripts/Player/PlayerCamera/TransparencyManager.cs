using UnityEngine;
using System.Collections.Generic;

// Changes the alpha level of the buildings to see the player
public class TransparencyManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float alphaValue = 0.4f;

    private List<TransparencyController> transparencyControllers;
    private List<TransparencyController> previousHitObjects;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
        mainCamera = Camera.main;
        transparencyControllers = new List<TransparencyController>(FindObjectsOfType<TransparencyController>());
        previousHitObjects = new List<TransparencyController>();
    }
    private void OnEnable()
    {
        GamePlayEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GamePlayEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        this.enabled = false;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 direction = (playerTransform.position - mainCamera.transform.position).normalized;

        List<TransparencyController> currentHitObjects = new List<TransparencyController>();

        if (Physics.Raycast(mainCamera.transform.position, direction, out hit, maxDistance))
        {
            TransparencyController tc = hit.collider.GetComponent<TransparencyController>();
            if (tc != null)
            {
                tc.SetTargetAlpha(alphaValue);
                currentHitObjects.Add(tc);
            }
        }

        foreach (TransparencyController tc in previousHitObjects)
        {
            if (!currentHitObjects.Contains(tc))
            {
                tc.SetTargetAlpha(1f);
            }
        }

        previousHitObjects = currentHitObjects;
    }
}
