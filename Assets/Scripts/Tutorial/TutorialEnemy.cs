using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : EnemyMovement {
    int step = 0;
    public Transform[] floorTiles;

    void Awake()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (floorTiles.Length > 0 && nav.remainingDistance <= nav.stoppingDistance)
        {
            nav.SetDestination(floorTiles[step].position);
            step = (step + 1) % floorTiles.Length;
        }
    }

    public override void Stop()
    {
        nav.isStopped = true;
        enabled = false;
    }

    public void Continue()
    {
        nav.isStopped = false;
        enabled = true;
    }
}
