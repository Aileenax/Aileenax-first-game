using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float xSpawnRange = 19.0f;
    private float zTop = 7.0f;
    private float zBottom = -16.0f;
    private float y = 0.56f;
    private float spawnDelay = 2.0f;
    private float enemySpawnTime = 3.0f;
    private float powerupSpawnTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, enemySpawnTime);
        InvokeRepeating("SpawnPowerup", spawnDelay, powerupSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        float randomZ = Random.Range(zBottom, zTop);
        Vector3 spawnPos = new Vector3(randomX, y, randomZ);

        Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);
    }

    void SpawnPowerup()
    {
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        float randomZ = Random.Range(zBottom, zTop);
        Vector3 spawnPos = new Vector3(randomX, y, randomZ);

        Instantiate(powerupPrefab, spawnPos, powerupPrefab.transform.rotation);
    }
}
