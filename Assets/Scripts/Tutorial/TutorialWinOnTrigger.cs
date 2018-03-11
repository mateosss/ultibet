using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWinOnTrigger : MonoBehaviour {

    TutorialManager tutorial;
    BoxCollider player;

    private void Awake()
    {
        tutorial = GameObject.Find("GameManager").GetComponent<TutorialManager>();
        player = GameObject.FindWithTag("Player").GetComponents<BoxCollider>()[0]; //Player, not attack collider
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player) tutorial.WinTutorial();
    }
}
