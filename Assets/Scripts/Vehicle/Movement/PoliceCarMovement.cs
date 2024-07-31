using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarMovement : ArmoredVehicleMovement
{   
    private bool isSoundPlaying = false;

    protected override void PlayMoveSound()
    {
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager is null. Cannot play sound.");
            return;
        }

        if (!isSoundPlaying)
        {
            audioManager.Play("PoliceCar");
            Debug.Log("Playing PoliceCar move sound");
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
            audioManager.Stop("PoliceCar");
            Debug.Log("Stopping PoliceCar move sound");
            isSoundPlaying = false;
        }
    }
}
