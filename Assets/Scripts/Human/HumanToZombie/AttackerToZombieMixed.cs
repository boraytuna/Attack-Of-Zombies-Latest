using UnityEngine;

public class AttackerToZombieMixed : TurnToZombie, ITurnable
{
    private MixedAttackerSpawner mixedAttackerSpawner; // Reference to the attacker mixed spawner script
    public bool centralAttacker; // Indicate if this is a central attacker

    protected override void Start()
    {
        base.Start();

        mixedAttackerSpawner = FindObjectOfType<MixedAttackerSpawner>();
        if (mixedAttackerSpawner == null)
        {
            Debug.LogError("AttackerSpawner not found in the scene.");
        }

        // Determine if this is a central attacker by checking its layer
        centralAttacker = isCentralAttacker();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleOnTriggerEnter(other, centralAttacker, () => mixedAttackerSpawner.OnCentralAttackerKilled(this.gameObject));
    }

    private bool isCentralAttacker()
    {
        // Check if the attacker object is on the CentralPolice layer
        int centralAttackerLayer = LayerMask.NameToLayer("CentralAttacker");
        return gameObject.layer == centralAttackerLayer;
    }

    public bool IsCentral()
    {
        return isCentralAttacker();
    }
}
