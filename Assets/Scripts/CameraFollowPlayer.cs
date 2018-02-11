using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    public float rapidness = 0.5f;

    Transform player;
    float offset;
    Vector3 initialPosition;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = player.position.y - transform.position.y;
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = new Vector3(transform.position.x, Mathf.Lerp(initialPosition.y, player.position.y - offset, rapidness), transform.position.z);
	}
}
