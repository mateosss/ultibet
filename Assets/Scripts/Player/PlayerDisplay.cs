using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour {

    public GameObject displayObject;

    [Header("Particles")]
    public ParticleSystem jumpParticles;
    public ParticleSystem swordParticlesOnAttack;
    public ParticleSystem floorParticlesOnAttack;

    Animator anim;
    Transform floorAttackParticlesTransform;
    PlayerAttack playerAttack;

    void Awake () {
        anim = displayObject.GetComponent<Animator>();
        floorAttackParticlesTransform = floorParticlesOnAttack.gameObject.transform;
        playerAttack = GetComponent<PlayerAttack>();
	}

    public void Jump()
    {
        jumpParticles.Play();
        anim.SetTrigger("Jump");
    }

    public void JumpLand()
    {
        anim.SetTrigger("JumpLand");
    }

    public void Attack()
    {
        floorAttackParticlesTransform.position = new Vector3(gameObject.transform.position.x, floorAttackParticlesTransform.position.y, gameObject.transform.position.z);
        ParticleSystem.ShapeModule shape = floorParticlesOnAttack.shape;
        shape.scale = new Vector3(playerAttack.range, playerAttack.range, 0);
        ParticleSystem.MainModule main = swordParticlesOnAttack.main;
        main.startSize = playerAttack.range;
        floorParticlesOnAttack.Play();
        swordParticlesOnAttack.Play();
        anim.SetTrigger("Attack");
    }

    public void Dash()
    {
        anim.SetTrigger("Dash");
    }

    public void Still()
    {
        anim.SetBool("Running", false);
    }

    public void Run()
    {
        anim.SetBool("Running", true);
    }
}
