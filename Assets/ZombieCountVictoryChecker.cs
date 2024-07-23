using UnityEngine;

public class ZombieCountVictoryChecker : VictoryChecker
{
    protected override void CheckVictoryCondition()
    {
        int currentZombieCount = zombieCounter.GetZombieCount();

        if (currentZombieCount >= requiredZombieCount)
        {
            AchieveVictory();
        }
    }
}
