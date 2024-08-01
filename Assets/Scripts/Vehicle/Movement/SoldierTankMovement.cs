using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTankMovement : ArmoredVehicleMovement
{
    protected override void PlayMoveSound()
    {
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager is null. Cannot play sound.");
            return;
        }

        if (!isSoundPlaying)
        {
            audioManager.Play("SoldierTank");
            Debug.Log("Playing SoldierTank move sound");
            isSoundPlaying = true;
        }
    }

    protected override void StopMoveSound()
    {
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager is null. Cannot stop sound.");
            return;
        }

        if (isSoundPlaying)
        {
            audioManager.Stop("SoldierTank");
            Debug.Log("Stopping SoldierTank move sound");
            isSoundPlaying = false;
        }
    }
}
