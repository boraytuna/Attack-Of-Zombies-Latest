// using System.Collections;
// using UnityEngine;

// public class CollectibleManager : MonoBehaviour
// {
//     public void Collect(CollectibleType type)
//     {
//         switch (type)
//         {
//             case CollectibleType.Star:
//                 StartCoroutine(StarCollect());
//                 break;
//             case CollectibleType.ExtraPoints:
//                 // Handle extra points logic
//                 break;
//             case CollectibleType.HealthBooster:
//                 // Handle health boost logic
//                 break;
//             case CollectibleType.SpeedBooster:
//                 // Handle speed boost logic
//                 break;
//         }
//     }

//     private IEnumerator StarCollect()
//     {
//         // Make zombies invincible for a short period
//         Debug.Log("Zombies are now invincible!");
//         yield return new WaitForSeconds(5); // Example duration
//         Debug.Log("Zombies are no longer invincible.");
//     }
// }
