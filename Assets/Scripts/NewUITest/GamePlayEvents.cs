using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePlayEvents
{
    public static event Action OnMainMenu;
    public static event Action OnLevelMenu;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerWin;

    public static void TriggerMainMenu()
    {
        OnMainMenu?.Invoke();
    }

    public static void TriggerLevelMenu()
    {
        OnLevelMenu?.Invoke();
    }

    public static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }
}
