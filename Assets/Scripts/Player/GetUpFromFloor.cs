using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUpFromFloor : MonoBehaviour {

    PlayerMovement playerMovement;
    int platformLayer;

	void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        platformLayer = LayerMask.GetMask("Platform");
	}
	
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit floorOnFront;
        Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
        if (Physics.Raycast(ray, out floorOnFront, 2, platformLayer))
        {
            GetUp();
        }
	}

    void GetUp()
    {
        playerMovement.Jump();
    }
}
