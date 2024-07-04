using System.Collections.Generic;
using UnityEngine;

public class CircleContainer : MonoBehaviour
{
    private float circleRadius = 5f; // Radius of the circle
    private float minZombieFollowDistance = 1f; // Minimum distance for a zombie to follow the circle
    private GameObject player; // Reference to the player object
    public List<GameObject> zombiesInCircle = new List<GameObject>(); // List to store zombies in the circle

    private List<Vector3> zombieRelativePositions = new List<Vector3>(); // List to store assigned positions for zombies

    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position; // Move the circle container with the player
        }
        
        // Update positions of zombies in the circle
        for (int i = 0; i < zombiesInCircle.Count; i++)
        {
            GameObject zombie = zombiesInCircle[i];
            if (zombie != null)
            {
                // Ensure we have a position assigned for this zombie
                if (zombieRelativePositions.Count <= i)
                {
                    // Assign a position within the circle radius
                    Vector3 offset = Random.insideUnitCircle.normalized * Random.Range(minZombieFollowDistance, circleRadius);
                    Vector3 spawnPosition = new Vector3(offset.x, 0f, offset.y); // Adjust y-axis as needed

                    // Raycast to find ground position
                    Vector3 groundPosition = FindGroundPosition(spawnPosition);

                    // Set zombie's position on the ground
                    zombie.transform.position = groundPosition;

                    // Store assigned position
                    zombieRelativePositions.Add(groundPosition);
                }
                else
                {
                    // Set the zombie's position if already assigned
                    zombie.transform.position = transform.position + zombieRelativePositions[i];
                }
            }
        }
    }

    // public void AddZombie(GameObject zombie)
    // {
    //     if (!zombiesInCircle.Contains(zombie))
    //     {
    //         zombiesInCircle.Add(zombie);
    //     }
    // }

    // public void RemoveZombie(GameObject zombie)
    // {
    //     zombiesInCircle.Remove(zombie);
    //     // Clear the assigned position for the removed zombie
    //     int index = zombiesInCircle.IndexOf(zombie);
    //     if (index != -1 && index < zombieRelativePositions.Count)
    //     {
    //         zombieRelativePositions.RemoveAt(index);
    //     }
    // }

    Vector3 FindGroundPosition(Vector3 originalPosition)
    {
        Vector3 rayStart = originalPosition + Vector3.up * 10f; // Start the raycast from above the object
        Ray ray = new Ray(rayStart, Vector3.down); // Cast the ray downwards

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point; // Return the point where the ray hit the ground
        }

        return originalPosition; // If no ground was found, return the original position
    }
}