using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrail : MonoBehaviour {

    Plane plane;

    void Start() {
        plane = new Plane(Camera.main.transform.forward * -1, transform.position);
	}
	
	void Update () {

        if (IsPointerMoving())
        {
            Ray camRay = Camera.main.ScreenPointToRay(PointerPosition());
            float enter;
            plane.Raycast(camRay, out enter);
            transform.position = camRay.GetPoint(enter);
        }
	}

    bool IsPointerMoving()
    {
        if (Input.touchCount > 0) return Input.GetTouch(0).phase == TouchPhase.Moved;
        else return Input.GetButton("Fire1");
    }

    Vector2 PointerPosition()
    {
        if (Input.touchCount > 0) return Input.GetTouch(0).position;
        else return Input.mousePosition;
    }
}
