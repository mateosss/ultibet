using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRestartOnFell : MonoBehaviour {

    public Transform restartFrom;

    PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update () {
        if (player.IsOnGround()) {
            player.transform.position = restartFrom.position;
            player.ResetPath();
        }
	}
}
