using UnityEngine;
using UnityEngine.AI;

public class NavMeshBoundaryWalls : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab; // Wall prefab to instantiate
    [SerializeField] private float wallThickness = 1f; // Thickness of the walls
    private NavMeshTriangulation triangulation;

    private GameObject topWall, bottomWall, leftWall, rightWall;

    void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        // Calculate the boundaries
        Vector3 minBounds = GetMinBounds(triangulation.vertices);
        Vector3 maxBounds = GetMaxBounds(triangulation.vertices);

        // Find the OuterWalls object
        Transform outerWallsParent = GameObject.FindGameObjectWithTag("OuterWalls")?.transform;

        if (outerWallsParent != null)
        {
            // Ensure walls are only placed once if the scene reloads
            if (topWall == null && bottomWall == null && leftWall == null && rightWall == null)
            {
                PlaceWalls(minBounds, maxBounds, outerWallsParent);
            }
        }
        else
        {
            Debug.LogError("OuterWalls object with the tag 'OuterWalls' not found.");
        }
    }

    private Vector3 GetMinBounds(Vector3[] vertices)
    {
        Vector3 minBounds = vertices[0];

        foreach (Vector3 vert in vertices)
        {
            minBounds.x = Mathf.Min(minBounds.x, vert.x);
            minBounds.z = Mathf.Min(minBounds.z, vert.z);
        }

        return minBounds;
    }

    private Vector3 GetMaxBounds(Vector3[] vertices)
    {
        Vector3 maxBounds = vertices[0];

        foreach (Vector3 vert in vertices)
        {
            maxBounds.x = Mathf.Max(maxBounds.x, vert.x);
            maxBounds.z = Mathf.Max(maxBounds.z, vert.z);
        }

        return maxBounds;
    }

    private void PlaceWalls(Vector3 minBounds, Vector3 maxBounds, Transform parent)
    {
        float halfWallThickness = wallThickness / 2f;

        // Top Wall
        topWall = Instantiate(wallPrefab, parent);
        topWall.transform.position = new Vector3((minBounds.x + maxBounds.x) / 2f, 0, maxBounds.z + halfWallThickness);
        topWall.transform.localScale = new Vector3(maxBounds.x - minBounds.x + wallThickness, topWall.transform.localScale.y, wallThickness);

        // Bottom Wall
        bottomWall = Instantiate(wallPrefab, parent);
        bottomWall.transform.position = new Vector3((minBounds.x + maxBounds.x) / 2f, 0, minBounds.z - halfWallThickness);
        bottomWall.transform.localScale = new Vector3(maxBounds.x - minBounds.x + wallThickness, bottomWall.transform.localScale.y, wallThickness);

        // Left Wall
        leftWall = Instantiate(wallPrefab, parent);
        leftWall.transform.position = new Vector3(minBounds.x - halfWallThickness, 0, (minBounds.z + maxBounds.z) / 2f);
        leftWall.transform.localScale = new Vector3(wallThickness, leftWall.transform.localScale.y, maxBounds.z - minBounds.z + wallThickness);

        // Right Wall
        rightWall = Instantiate(wallPrefab, parent);
        rightWall.transform.position = new Vector3(maxBounds.x + halfWallThickness, 0, (minBounds.z + maxBounds.z) / 2f);
        rightWall.transform.localScale = new Vector3(wallThickness, rightWall.transform.localScale.y, maxBounds.z - minBounds.z + wallThickness);
    }
}
