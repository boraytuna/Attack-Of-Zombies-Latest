using UnityEngine;

public class HumanToZombie : TurnToZombie, ITurnable
{
    private HumanSpawner humanSpawner; // Reference to the human spawner script
    public bool isCentralHuman; // Indicate if this is a central human

    protected override void Start()
    {
        base.Start();

        // Find the HumanSpawner by tag
        humanSpawner = GameObject.FindWithTag("HumanManager")?.GetComponent<HumanSpawner>();

        // Determine if this is a central human by checking its layer
        isCentralHuman = IsCentral();

        if (humanSpawner == null)
        {
            Debug.LogError("HumanSpawner not found. Please ensure there is an object with the 'HumanSpawner' tag and the HumanSpawner component attached.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleOnTriggerEnter(other, isCentralHuman, () => humanSpawner?.OnCentralHumanKilled(this.gameObject));
    }

    public bool IsCentral()
    {
        // Check if the human object is on the CentralHuman layer
        int centralHumanLayer = LayerMask.NameToLayer("CentralHuman");
        return gameObject.layer == centralHumanLayer;
    }
}
