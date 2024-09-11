using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform spawnPoint;
    public List<Transform> waypoints;
    public List<EnemyType> enemyTypes;
    public int enemiesPerWave = 10;
    public float spawnInterval = 1f;
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private int enemiesKilled = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
{
    while (true)
    {
        currentWave++;
        enemiesSpawned = 0;
        enemiesKilled = 0;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }

        // Wait until all enemies are killed before starting the next wave
        while (enemiesKilled < enemiesSpawned)
        {
            yield return null;
        }

        Debug.Log("Wave completed. Calling NextWave.");
        MyGameManager.Instance.NextWave();

        yield return new WaitForSeconds(timeBetweenWaves);
    }
}

    void SpawnEnemy()
    {
        GameObject enemyPrefab = GetEnemyPrefabForCurrentWave();
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.waypoints = waypoints.ToArray(); // Convert List<Transform> to Transform[]
            enemyScript.OnEnemyKilled += HandleEnemyKilled;
        }
        else
        {
            Debug.LogError("Enemy component not found on the instantiated enemy.");
        }
    }

    GameObject GetEnemyPrefabForCurrentWave()
    {
        List<GameObject> availablePrefabs = new List<GameObject>();
        foreach (EnemyType enemyType in enemyTypes)
        {
            if (currentWave >= enemyType.minWave)
            {
                availablePrefabs.Add(enemyType.enemyPrefab);
            }
        }

        // Increase the probability of spawning the last enemy type in the list
        if (currentWave >= enemyTypes[enemyTypes.Count - 1].minWave)
        {
            int additionalEntries = currentWave - enemyTypes[enemyTypes.Count - 1].minWave + 1;
            for (int i = 0; i < additionalEntries; i++)
            {
                availablePrefabs.Add(enemyTypes[enemyTypes.Count - 1].enemyPrefab);
            }
        }

        if (availablePrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePrefabs.Count);
            return availablePrefabs[randomIndex];
        }
        else
        {
            return enemyTypes[0].enemyPrefab;
        }
    }

    public void HandleEnemyKilled()
    {
        enemiesKilled++;
    }
}

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public int minWave;
}