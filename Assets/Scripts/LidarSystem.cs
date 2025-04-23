using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LidarSystem : MonoBehaviour
{
    [Header("Lidar Settings")]
    public int totalPointsToDisplay = 5000;  // Maximum number of points in the scene
    public int pointsPerClick = 500;         // How many points are created per click
    public int pointsPerFrame = 50;          // How many points are instantiated per frame
    public float maxTimeToRenderPoints = 1f; // Time before points fade
    public float minSpamButton = 2f;

    [Header("Point Settings")]
    public GameObject pointPrefab;           // Prefab for the points
    public Transform headsetTransform;       // Reference to the headset

    private Queue<GameObject> activePoints = new Queue<GameObject>(); // Pool of active points
    private Queue<GameObject> pointPool = new Queue<GameObject>();    // Object pool
    private int pointsLeftToSpawn = 0;

    
    public static event Action OnClickDetected; // Event to trigger Lidar
    private float lastLidarScan;
    
    

    void Start()
    {
        // Preload points into the pool
        for (int i = 0; i < totalPointsToDisplay; i++)
        {
            GameObject point = Instantiate(pointPrefab);
            point.SetActive(false);
            pointPool.Enqueue(point);
        }

        // Subscribe to click event
        OnClickDetected += StartLidarScan;
    }

    void OnDestroy()
    {
        OnClickDetected -= StartLidarScan;
    }

    private void StartLidarScan()
    {
        if( Time.time - lastLidarScan < minSpamButton)
            return;
        lastLidarScan = Time.time;
        pointsLeftToSpawn = pointsPerClick;
        StartCoroutine(SpawnPointsOverTime());
    }

    private IEnumerator SpawnPointsOverTime()
    {
        while (pointsLeftToSpawn > 0)
        {
            int spawnCount = Mathf.Min(pointsPerFrame, pointsLeftToSpawn);
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnPoint();
            }
            pointsLeftToSpawn -= spawnCount;
            yield return null; // Wait for next frame
        }
    }

    private void SpawnPoint()
    {
        // Get or recycle a point
        GameObject point;
        if (pointPool.Count > 0)
        {
            point = pointPool.Dequeue();
        }
        else
        {
            point = activePoints.Dequeue();
        }

        // Set position based on random raycast from headset
        Vector3 direction = GetRandomDirection();
        if (Physics.Raycast(headsetTransform.position, direction, out RaycastHit hit))
        {
            point.transform.position = hit.point;
        }
        else
        {
            point.transform.position = headsetTransform.position + direction * 10f; // Fallback position
        }

        // Activate point and schedule its removal
        point.SetActive(true);
        activePoints.Enqueue(point);
        StartCoroutine(DeactivatePointAfterTime(point));
    }

    private Vector3 GetRandomDirection()
    {
        // Generates an approximately evenly distributed direction
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
        randomDirection += new Vector3(
            UnityEngine.Random.Range(-0.1f, 0.1f),
            UnityEngine.Random.Range(-0.1f, 0.1f),
            UnityEngine.Random.Range(-0.1f, 0.1f) // Small noise
        );
        return randomDirection.normalized;
    }

    private IEnumerator DeactivatePointAfterTime(GameObject point)
    {
        float randomDelay = UnityEngine.Random.Range(-1f, 1f);
        yield return new WaitForSeconds(maxTimeToRenderPoints + randomDelay);
        point.SetActive(false);
        if( activePoints.Count > 0)
            activePoints.Dequeue();
        pointPool.Enqueue(point);
    }
    
    public static void TriggerClickDetected()
    {
        OnClickDetected?.Invoke();
    }
}
