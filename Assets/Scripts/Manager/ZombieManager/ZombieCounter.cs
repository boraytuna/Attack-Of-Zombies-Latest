// using UnityEngine;

// public class ZombieCounter : MonoBehaviour
// {
//     private int zombieCount = 1;

//     public void Initialize()
//     {
//         GetZombieCount();
//     }

//     public int GetZombieCount()
//     {
//         return zombieCount;
//     }

//     public void IncrementZombieCount()
//     {
//         zombieCount++;
//         Debug.Log("Zombie Count: " + zombieCount);
//     }

//     public void DecrementZombieCount()
//     {
//         zombieCount--;
//         Debug.Log("Zombie Count");
//     }
// }
using UnityEngine;

public class ZombieCounter : MonoBehaviour
{
    private int zombieCount = 1;

    public void Initialize()
    {
        GetZombieCount();
    }

    public int GetZombieCount()
    {
        return zombieCount;
    }

    public void IncrementZombieCount()
    {
        zombieCount++;
        Debug.Log("Zombie Count: " + zombieCount);
    }

    public void DecrementZombieCount()
    {
        if (zombieCount > 0)
        {
            zombieCount--;
            Debug.Log("Zombie Count: " + zombieCount);
        }
        else
        {
            Debug.LogWarning("Zombie Count is already at zero or negative");
        }
    }
}
