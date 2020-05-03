using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject powerupPrefab;

    private float _xSpawnRange = 19.0f;
    private float _zTop = 7.0f;
    private float _zBottom = -16.0f;
    private float _y = 4.5f;
    private float _spawnDelay = 2.0f;
    private float _powerupSpawnTime;
    private float _enemySpawnTime;
    private string _gameDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        ChangeSpawnAccordingToDifficulty();

        InvokeRepeating("SpawnEnemy", _spawnDelay, _enemySpawnTime);
        InvokeRepeating("SpawnPowerup", _spawnDelay, _powerupSpawnTime);
    }

    private void ChangeSpawnAccordingToDifficulty()
    {
        _gameDifficulty = GameDifficulty.Instance.Difficulty;

        if (_gameDifficulty == "Easy")
        {
            _enemySpawnTime = 5.0f;
            _powerupSpawnTime = 5.0f;
        }
        else if (_gameDifficulty == "Medium")
        {
            _enemySpawnTime = 3.0f;
            _powerupSpawnTime = 7.0f;
        }
        else if (_gameDifficulty == "Hard")
        {
            _enemySpawnTime = 1.0f;
            _powerupSpawnTime = 10.0f;
        }
    }

    private void SpawnEnemy()
    {
        float randomX = UnityEngine.Random.Range(-_xSpawnRange, _xSpawnRange);
        float randomZ = UnityEngine.Random.Range(_zBottom, _zTop);
        Vector3 spawnPos = new Vector3(randomX, _y, randomZ);

        GameObject pooledEnemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");

        if (pooledEnemy != null)
        {
            pooledEnemy.SetActive(true);
            pooledEnemy.transform.position = spawnPos;
            pooledEnemy.transform.rotation = enemyPrefab.transform.rotation;
        }
    }

    private void SpawnPowerup()
    {
        float randomX = UnityEngine.Random.Range(-_xSpawnRange, _xSpawnRange);
        float randomZ = UnityEngine.Random.Range(_zBottom, _zTop);
        Vector3 spawnPos = new Vector3(randomX, _y, randomZ);

        GameObject pooledPowerup = ObjectPooler.SharedInstance.GetPooledObject("Powerup");

        if (pooledPowerup != null)
        {
            pooledPowerup.SetActive(true);
            pooledPowerup.transform.position = spawnPos;
            pooledPowerup.transform.rotation = powerupPrefab.transform.rotation;
        }
    }
}
