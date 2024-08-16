using UnityEngine;

public class CentralHumanDetector : CentralObjectDetector
{
    void Start()
    {
        DetectionRange = 20f;
    }
    
    protected override void OnHumanDetected(IHuman human)
    {
        // Custom logic for when a human is detected
        //Debug.Log("Human detected in custom range.");
        human.HandleDetection();
    }

    protected override void OnAttackerDetected(IShoot shoot)
    {
        // Optionally override this if necessary
    }
}
