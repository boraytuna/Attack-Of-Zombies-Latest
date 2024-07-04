using UnityEngine;

public interface ITurnable 
{
    bool IsCentral(); // Determines if the object is a central entity
    Vector3 FindGroundPosition(Vector3 originalPosition); // Finds the ground position based on the original position
}
