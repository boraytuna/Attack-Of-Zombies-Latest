// using System;
// using UnityEngine;

// public class ZombieManager : MonoBehaviour
// {
//     [SerializeField] private ZombieCounter zombieCounter; // Manages the zombie count

//     public void Start()
//     {
//         // Attempt to find components if they are not assigned in the Inspector
//         if (zombieCounter == null)
//         {
//             zombieCounter = GetComponent<ZombieCounter>();
//         }

//         // Initialize components if they are found
//         if (zombieCounter != null)
//         {
//             zombieCounter.GetZombieCount();
//         }
//         else
//         {
//             Debug.LogError("Zombie Counter is still null.");
//         }
//     }

//     public int GetZombieCount()
//     {
//         return zombieCounter != null ? zombieCounter.GetZombieCount() : 0;
//     }
// }
