using System.Collections.Generic;
using UnityEngine;

public class VictoryChecker : MonoBehaviour
{
    private List<GameObject> humans;
    private float initialDelay = 5.0f; // Initial delay before first check
    private float checkInterval = 4.0f; // Interval to check for new humans
    private float nextCheckTime;

    void Start()
    {
        humans = new List<GameObject>(GameObject.FindGameObjectsWithTag("Human"));
        nextCheckTime = Time.time + initialDelay;
    }

    void Update()
    {
        // Periodically check for newly instantiated humans after the initial delay
        if (Time.time >= nextCheckTime)
        {
            UpdateHumanList();
            nextCheckTime = Time.time + checkInterval;
        }

        // Remove any null entries (destroyed objects) from the list
        humans.RemoveAll(item => item == null);

        // Check if the list is empty
        if (humans.Count == 0)
        {
            // Notify the UIManager of the victory
            UIManager.Instance.OnVictory();
        }
    }

    private void UpdateHumanList()
    {
        GameObject[] allHumans = GameObject.FindGameObjectsWithTag("Human");

        // Add new humans to the list
        foreach (GameObject human in allHumans)
        {
            if (!humans.Contains(human))
            {
                humans.Add(human);
            }
        }
    }
}
