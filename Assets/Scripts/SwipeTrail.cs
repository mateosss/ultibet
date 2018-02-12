using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrail : MonoBehaviour {

    Plane plane;


    void Start() {
        plane = new Plane(Camera.main.transform.forward * -1, transform.position);
	}
	
	void Update () {

        if (Input.GetButton("Fire1"))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;
            plane.Raycast(camRay, out enter);
            transform.position = camRay.GetPoint(enter);
        }
	}
}
