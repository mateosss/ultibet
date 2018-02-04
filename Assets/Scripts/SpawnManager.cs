using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public int wave = 1;
    public float spawnTime = 1f;
    public float timeBetweenWaves = 5f;
    public GameObject enemy;
    public Text waveText;

    int currentWaveSpawnedEnemies = 0;
    int currentWaveKilledEnemies = 0;

    List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        foreach (Transform child in transform) spawnPoints.Add(child);
    }

    void Start()
    {
        StartCoroutine(ManageSpawn());
    }

    IEnumerator ManageSpawn()
    {
        while (true)
        {
            if (currentWaveSpawnedEnemies < wave)
            {
                Spawn();
                currentWaveSpawnedEnemies++;
                yield return new WaitForSeconds(spawnTime);
            }
            else if (currentWaveKilledEnemies >= currentWaveSpawnedEnemies)
            {
                wave += 3;
                waveText.text = "WAVE " + wave;
                currentWaveSpawnedEnemies = 0;
                currentWaveKilledEnemies = 0;
                yield return new WaitForSeconds(timeBetweenWaves);
            } else
            {
                yield return null;
            }
        }
    }

    void Spawn()
    {
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(enemy, randomSpawn.position, randomSpawn.rotation);
    }

    public void EnemyDied()
    {
        currentWaveKilledEnemies++;
    }

}
