using UnityEngine;

public class HumanToZombie : TurnToZombie, ITurnable
{
    private HumanSpawner humanSpawner; // Reference to the human spawner script
    public bool isCentralHuman; // Indicate if this is a central human

    protected override void Start()
    {
        base.Start();

        humanSpawner = FindObjectOfType<HumanSpawner>();
        if (humanSpawner == null)
        {
            Debug.LogError("HumanSpawner not found in the scene.");
        }

        // Determine if this is a central human by checking its layer
        isCentralHuman = IsCentral();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleOnTriggerEnter(other, isCentralHuman, () => humanSpawner.OnCentralHumanKilled(this.gameObject));
    }

    public bool IsCentral()
    {
        // Check if the human object is on the CentralHuman layer
        int centralHumanLayer = LayerMask.NameToLayer("CentralHuman");
        return gameObject.layer == centralHumanLayer;
    }
}
