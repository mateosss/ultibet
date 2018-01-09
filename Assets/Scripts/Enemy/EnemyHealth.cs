using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = GameObject.Find("Spawns").GetComponent<SpawnManager>();
    }

    public void Death()
    {
        Destroy(gameObject);
        spawnManager.EnemyDied();
    }
}
