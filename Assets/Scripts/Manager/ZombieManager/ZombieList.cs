// using System.Collections.Generic;
// using UnityEngine;

// public class ZombieList : MonoBehaviour
// {
//     private Dictionary<GameObject, int> zombieIds = new Dictionary<GameObject, int>();
//     private int nextId = 1;

//     void Start()
//     {
//         zombieIds.Clear();
//         nextId = 1;
//     }

//     public void AddZombie(GameObject zombie)
//     {
//         if (!zombieIds.ContainsKey(zombie))
//         {
//             zombieIds.Add(zombie, nextId);
//             nextId++;
//         }
//     }

//     public void RemoveZombie(GameObject zombie)
//     {
//         if (zombieIds.ContainsKey(zombie))
//         {
//             zombieIds.Remove(zombie);
//         }
//     }
// }
