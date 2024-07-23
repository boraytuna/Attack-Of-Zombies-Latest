using System;
using UnityEngine;

public static class GamePlayEvents
{
    public static event Action OnMainMenu;
    public static event Action OnLevelMenu;
    public static event Action OnBoostUsed;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerWin;
    public static event Action onPlayerEnterSafePoint;

    public static void TriggerMainMenu()
    {
        OnMainMenu?.Invoke();
    }

    public static void TriggerLevelMenu()
    {
        OnLevelMenu?.Invoke();
    }

    public static void TriggerBoostUsed()
    {
        OnBoostUsed?.Invoke();
    }

    public static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void TriggerPlayerReachedSafePoint()
    {
        onPlayerEnterSafePoint?.Invoke();
    }
}
