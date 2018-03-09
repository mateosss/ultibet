using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndPoint : MonoBehaviour {

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) Debug.Log("Finish line");
    }
}
