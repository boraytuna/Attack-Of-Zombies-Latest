using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePlayEvents
{
    public static event Action OnPlayerDeath;

    public static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}
