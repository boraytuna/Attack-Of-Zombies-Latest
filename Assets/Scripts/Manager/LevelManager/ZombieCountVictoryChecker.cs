using TMPro;
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

    protected override void DisplayVictoryMesseage()
    {
        if (victoryMessage.text != null)
        {
            victoryMessage.text = $"You Reached " + requiredZombieCount + " Points!";
        }
        else
        {
            Debug.LogError("victory text is null");
        }
    }
}
