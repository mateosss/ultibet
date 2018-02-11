using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfMap : MonoBehaviour {

    PlayerMovement player;
    BoxCollider playerCollider;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerCollider = player.GetComponents<BoxCollider>()[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            player.ResetPath();
        }
    }
}
