using UnityEngine;
using System.Collections.Generic;

// Changes the alpha level of the buildings to see the player
public class TransparencyManager : MonoBehaviour
{
    public Transform player;
    public Camera mainCamera;
    public float maxDistance = 10f;
    public float alphaValue = 0.4f;
    public float lerpSpeed = 5f;

    private List<TransparencyController> transparencyControllers;
    private List<TransparencyController> previousHitObjects;

    private void Start()
    {
        transparencyControllers = new List<TransparencyController>(FindObjectsOfType<TransparencyController>());
        previousHitObjects = new List<TransparencyController>();
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 direction = (player.position - mainCamera.transform.position).normalized;

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
