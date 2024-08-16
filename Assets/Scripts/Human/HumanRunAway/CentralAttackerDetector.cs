using System.Collections.Generic;
using UnityEngine;

public class AttackerDetector : CentralObjectDetector
{
    [SerializeField] private float detectionRangeForZombies = 18f;
    [SerializeField] private LayerMask zombieLayer;
    private List<Transform> detectedZombies = new List<Transform>();

    void Start()
    {
        DetectionRange = 18f;
    }

    protected override void OnHumanDetected(IHuman human)
    {
        // Optionally handle human detection if needed
    }

    protected override void OnAttackerDetected(IShoot shoot)
    {
        // Detect zombies
        DetectZombies();

        // Attack each detected zombie with available shooters
        foreach (Transform zombie in detectedZombies)
        {
            // For each attacker, attack the detected zombie
            shoot.Attack(zombie.GetComponent<Collider>());
        }
    }

    private void DetectZombies()
    {
        detectedZombies.Clear();
        Collider[] hitCollidersForZombies = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRangeForZombies, hitCollidersForZombies, zombieLayer);
        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = hitCollidersForZombies[i];
            if (collider != null)
            {
                Transform zombieTransform = collider.transform;
                if (!detectedZombies.Contains(zombieTransform))
                {
                    detectedZombies.Add(zombieTransform);
                }
            }
        }
    }
}
