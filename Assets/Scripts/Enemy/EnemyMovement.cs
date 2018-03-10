using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    protected UnityEngine.AI.NavMeshAgent nav;
    static Vector3[] floorTiles;

    void Awake()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        Transform tiles = GameObject.Find("FloorTiles").transform;
        floorTiles = new Vector3[tiles.childCount];
        for (int i = 0; i < floorTiles.Length; i++)
        {
            floorTiles[i] = tiles.GetChild(i).position;
        }
    }

    void Update()
    {
        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            nav.SetDestination(floorTiles[Random.Range(0, floorTiles.Length)]);
        }
    }

    public virtual void Stop()
    {
        nav.isStopped = true;
        enabled = false;
    }
}
