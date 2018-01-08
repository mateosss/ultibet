using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public float spawnTime = 1f;
    public GameObject enemy;

    List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        foreach (Transform child in transform) spawnPoints.Add(child);
    }

    void Start()
    {
        InvokeRepeating("Spawn", 0f, spawnTime);
    }

    void Spawn()
    {
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(enemy, randomSpawn.position, randomSpawn.rotation);
    }

}
