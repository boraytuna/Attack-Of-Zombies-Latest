using UnityEngine;

public class ZombieCounter : MonoBehaviour
{
    public static ZombieCounter Instance {get; private set;}
    private int zombieCount = 1;

    void Awake()
    {
        Instance = this;
    }

    public int GetZombieCount()
    {
        return zombieCount;
    }

    public void IncrementZombieCount()
    {
        zombieCount++;
        //Debug.Log("Zombie Count: " + zombieCount);
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
