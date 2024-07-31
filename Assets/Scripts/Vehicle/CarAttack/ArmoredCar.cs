using UnityEngine;

public class ArmoredCar : ArmoredVehicle
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Player" or "Zombie"
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            // Use the Attack method to deal damage to the collided object
            Debug.Log("Cop Car attacking");
            Attack(other);

            // Apply push force if the object is a player
            ApplyPushForce(other);
        }
    }

    // Override the Attack method to provide specific functionality
    public override void Attack(Collider targetCollider)
    {
        base.Attack(targetCollider);
        // Additional logic specific to ArmoredCar can be added here if needed
    }
}
