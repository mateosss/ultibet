using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public GameObject displayObject;

    Animator anim;

    void Awake () {
        anim = displayObject.GetComponent<Animator>();
	}

    public void Jump()
    {
        anim.SetTrigger("Jump");
    }

    public void JumpLand()
    {
        anim.SetTrigger("JumpLand");
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
