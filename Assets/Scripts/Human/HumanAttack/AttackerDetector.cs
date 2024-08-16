// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AttackerDetector : MonoBehaviour
// {
//     [Header("Human Vars")]
//     [SerializeField] private float detectionRangeForAttackers = 10f;
//     [SerializeField] private List<Transform> detectedAttackers = new List<Transform>();
//     [SerializeField] private LayerMask attackerLayer;
//     [SerializeField] private Collider[] hitCollidersForHumans = new Collider[20];

//     [Header("Zombie Vars")]
//     [SerializeField] private List<Transform> detectedZombies = new List<Transform>();
//     [SerializeField] private float detectionRangeForZombies = 18f;
//     [SerializeField] private LayerMask zombieLayer;
//     [SerializeField] private Collider[] hitCollidersForZombies = new Collider[200];

//     void Start()
//     {
//         StartCoroutine(DelayedAttackerDetection(1f));
//     }

//     private IEnumerator DelayedAttackerDetection(float delay)
//     {
//         yield return new WaitForSeconds(delay); // Wait for the specified delay
//         DetectAttackers(); // Call the detection method after the delay
//     }

//     public void HandleDetection()
//     {
//         DetectZombies();

//         // Ensure all attackers shoot, even if the central attacker dies
//         foreach (Transform attackerTransform in detectedAttackers)
//         {
//             if (attackerTransform == null) continue; // Skip if the attacker is no longer valid

//             IShoot shooter = attackerTransform.GetComponent<IShoot>();
//             if (shooter != null && detectedZombies.Count > 0)
//             {
//                 int zombieIndex = 0;
//                 foreach (Transform zombieTransform in detectedZombies)
//                 {
//                     // Ensure the zombieIndex wraps around if there are more attackers than zombies
//                     zombieIndex %= detectedZombies.Count;
//                     Transform targetZombie = detectedZombies[zombieIndex];
//                     zombieIndex++;

//                     // Rotate the attacker to face the zombie
//                     Vector3 directionToZombie = (targetZombie.position - attackerTransform.position).normalized;
//                     Quaternion lookRotation = Quaternion.LookRotation(directionToZombie);
//                     attackerTransform.rotation = Quaternion.Slerp(attackerTransform.rotation, lookRotation, Time.deltaTime * 5f);

//                     // Attack the zombie
//                     shooter.Attack(targetZombie.GetComponent<Collider>());
//                 }
//             }
//             else
//             {
//                 Debug.LogWarning($"No IShoot component found on attacker {attackerTransform.name}, or no zombies detected.");
//             }
//         }
//     }

//     void DetectAttackers()
//     {
//         detectedAttackers.Clear();

//         // Use a HashSet to track unique attackers
//         HashSet<Transform> uniqueAttackers = new HashSet<Transform>();

//         int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRangeForAttackers, hitCollidersForHumans, attackerLayer);
//         for (int i = 0; i < numColliders; i++)
//         {
//             Collider collider = hitCollidersForHumans[i];
//             if (collider != null)
//             {
//                 Transform attackerTransform = collider.transform;

//                 if (uniqueAttackers.Add(attackerTransform)) // Adds to HashSet if not already present
//                 {
//                     detectedAttackers.Add(attackerTransform); // Add to list
//                 }
//             }
//         }
//     }

//     void DetectZombies()
//     {
//         detectedZombies.Clear();

//         // Use a HashSet to track unique zombies
//         HashSet<Transform> uniqueZombies = new HashSet<Transform>();

//         int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRangeForZombies, hitCollidersForZombies, zombieLayer);
//         for (int i = 0; i < numColliders; i++)
//         {
//             Collider collider = hitCollidersForZombies[i];
//             if (collider != null)
//             {
//                 Transform zombieTransform = collider.transform;

//                 if (uniqueZombies.Add(zombieTransform)) // Adds to HashSet if not already present
//                 {
//                     detectedZombies.Add(zombieTransform); // Add to list
//                 }
//             }
//         }
//     }

//     public void LookAtTarget(Vector3 targetPosition)
//     {
//         // Implementation of looking at the target
//         Vector3 direction = (targetPosition - transform.position).normalized;
//         Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
//         transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust the speed if necessary
//     }   

//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, detectionRangeForAttackers);
//     }
// }
