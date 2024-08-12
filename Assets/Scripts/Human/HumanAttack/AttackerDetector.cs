using System.Collections.Generic;
using UnityEngine;

public class AttackerDetector : MonoBehaviour
{
    [SerializeField] private float detectionRange = 15f; // Range to detect attackers
    [SerializeField] private LayerMask attackerLayer; // Layer mask to identify attackers
    [SerializeField] private LayerMask zombieLayer; // Layer mask to identify zombies

    private Collider[] detectedAttackers;
    private List<IAttacker> attackersInRange;
    private Collider[] detectedZombies; // Array to store detected zombies

    [SerializeField] private int maxDetectedAttackers = 20; // Set this value based on your expected number of attackers
    [SerializeField] private int maxDetectedZombies = 20; // Set this value based on your expected number of zombies

    void Start()
    {
        detectedAttackers = new Collider[maxDetectedAttackers];
        attackersInRange = new List<IAttacker>();
        detectedZombies = new Collider[maxDetectedZombies];
    }

    void Update()
    {
        DetectAttackersInRange();
        CommandAttackersToShoot();
    }

    void DetectAttackersInRange()
    {
        int detectedCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, detectedAttackers, attackerLayer);
        attackersInRange.Clear();

        for (int i = 0; i < detectedCount; i++)
        {
            Collider collider = detectedAttackers[i];
            IAttacker attacker = collider.GetComponent<IAttacker>();
            if (attacker != null)
            {
                attackersInRange.Add(attacker);
            }
        }
    }

    void CommandAttackersToShoot()
    {
        foreach (IAttacker attacker in attackersInRange)
        {
            // Retrieve the Transform from the GameObject that implements IAttacker
            Transform attackerTransform = (attacker as Component).transform;
            Collider closestZombie = FindClosestZombie(attackerTransform.position);
            if (closestZombie != null)
            {
                attacker.Attack(closestZombie);
            }
        }
    }

    Collider FindClosestZombie(Vector3 origin)
    {
        int detectedCount = Physics.OverlapSphereNonAlloc(origin, detectionRange, detectedZombies, zombieLayer);
        Collider closestZombie = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < detectedCount; i++)
        {
            Collider zombie = detectedZombies[i];
            float distance = Vector3.Distance(origin, zombie.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestZombie = zombie;
            }
        }

        return closestZombie;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
