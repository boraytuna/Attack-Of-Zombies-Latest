// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.AI;

// public class ObjectPooler : MonoBehaviour
// {
//     public static ObjectPooler Instance;

//     [System.Serializable]
//     public class Pool
//     {
//         public string tag;
//         public GameObject prefab;
//         public int size;
//         public Transform parent;
//     }

//     public List<Pool> pools;
//     public Dictionary<string, Queue<GameObject>> poolDictionary;
//     public event Action OnPrefabsLoaded;

//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Debug.LogError("Another instance of ObjectPooler already exists.");
//             Destroy(gameObject);
//             return;
//         }
//         Instance = this;
//     }

//     private void Start()
//     {
//         poolDictionary = new Dictionary<string, Queue<GameObject>>();

//         foreach (Pool pool in pools)
//         {
//             if (pool.prefab == null)
//             {
//                 Debug.LogError($"Prefab for tag {pool.tag} is not assigned.");
//                 continue;
//             }

//             Queue<GameObject> objectPool = new Queue<GameObject>();

//             if (pool.parent == null)
//             {
//                 GameObject parentObject = new GameObject(pool.tag + " Pool");
//                 pool.parent = parentObject.transform;
//             }

//             for (int i = 0; i < pool.size; i++)
//             {
//                 GameObject obj = Instantiate(pool.prefab);
//                 obj.SetActive(false);
//                 obj.transform.parent = pool.parent;
//                 objectPool.Enqueue(obj);
//             }

//             poolDictionary.Add(pool.tag, objectPool);
//         }

//         Debug.Log("Object pooling setup complete.");
//         // Notify subscribers that the pooler is ready
//         OnPrefabsLoaded?.Invoke();
//     }

//     public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
//     {
//         if (!poolDictionary.ContainsKey(tag))
//         {
//             Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
//             return null;
//         }

//         Queue<GameObject> pool = poolDictionary[tag];

//         if (pool.Count == 0)
//         {
//             Debug.LogWarning($"Pool for tag '{tag}' is empty.");
//             return null;
//         }

//         GameObject objectToSpawn = pool.Dequeue();

//         if (objectToSpawn == null)
//         {
//             Debug.LogError($"Dequeued object is null from pool with tag '{tag}'.");
//             return null;
//         }

//         // Ensure the spawn position is on the NavMesh
//         NavMeshHit hit;
//         if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
//         {
//             position = hit.position;
//         }
//         else
//         {
//             Debug.LogError($"Failed to find a valid NavMesh position for spawning at {position}.");
//             pool.Enqueue(objectToSpawn); // Return the object back to the pool
//             return null;
//         }

//         objectToSpawn.SetActive(true);
//         objectToSpawn.transform.position = position;
//         objectToSpawn.transform.rotation = rotation;

//         // Return the object back to the pool after use
//         pool.Enqueue(objectToSpawn);

//         //Debug.Log($"Spawned object with tag '{tag}' at position {position}.");

//         return objectToSpawn;
//     }
// }
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI; 
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public event Action OnPrefabsLoaded;

    private List<Vector3> activePositions = new List<Vector3>(); // To track positions of active objects
    public float minDistanceBetweenObjects = 1.0f; // Minimum allowed distance between objects

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Another instance of ObjectPooler already exists.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            if (pool.prefab == null)
            {
                Debug.LogError($"Prefab for tag {pool.tag} is not assigned.");
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            if (pool.parent == null)
            {
                GameObject parentObject = new GameObject(pool.tag + " Pool");
                pool.parent = parentObject.transform;
            }

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = pool.parent;
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }

        Debug.Log("Object pooling setup complete.");
        OnPrefabsLoaded?.Invoke();
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        // Check if the spawn position is too close to any active object's position
        if (IsPositionOccupied(position))
        {
            Debug.LogWarning($"Cannot spawn object with tag '{tag}' at position {position} because it's too close to another object.");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[tag];

        if (pool.Count == 0)
        {
            Debug.LogWarning($"Pool for tag '{tag}' is empty.");
            return null;
        }

        GameObject objectToSpawn = pool.Dequeue();

        if (objectToSpawn == null)
        {
            Debug.LogError($"Dequeued object is null from pool with tag '{tag}'.");
            return null;
        }

        // Ensure the spawn position is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            Debug.LogError($"Failed to find a valid NavMesh position for spawning at {position}.");
            pool.Enqueue(objectToSpawn); // Return the object back to the pool
            return null;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Add the position to the list of active positions
        activePositions.Add(position);

        // Return the object back to the pool after use
        pool.Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // public void ReturnToPool(GameObject objectToReturn, string tag)
    // {
    //     if (poolDictionary.ContainsKey(tag))
    //     {
    //         objectToReturn.SetActive(false);

    //         // Remove the object's position from the active positions list
    //         activePositions.Remove(objectToReturn.transform.position);

    //         poolDictionary[tag].Enqueue(objectToReturn);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("No pool with tag " + tag + " found to return object.");
    //     }
    // }

    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (var activePosition in activePositions)
        {
            if (Vector3.Distance(position, activePosition) < minDistanceBetweenObjects)
            {
                return true;
            }
        }
        return false;
    }
}
