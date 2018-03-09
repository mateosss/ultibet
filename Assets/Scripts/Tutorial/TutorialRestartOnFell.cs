using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRestartOnFell : MonoBehaviour {

    public TutorialManager tutorial;

    PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update () {
        if (player.IsOnGround()) {
            player.transform.position = tutorial.StartPosition();
            player.ResetPath();
        }
	}
}
