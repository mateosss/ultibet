using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    public float impactTime = 0.2f; // The time after the attack during which it destroys enemies
    public float cooldown = 1f; // Cooldown between attacks
    public float range = 4f;
    public Animator healLineDisplayAnimator;
    public Button attackButton;

    PlayerDisplay playerDisplay;
    PlayerMovement playerMovement;
    BoxCollider attackCollider;
    RandomSound playerSound;
    float impactTimer = 0f;
    public float CooldownTimer { get; private set; }
    int groundLayer;

    void Awake () {
        playerDisplay = GetComponent<PlayerDisplay>();
        playerMovement = GetComponent<PlayerMovement>();
        attackCollider = GetComponents<BoxCollider>()[1];
        playerSound = GetComponent<RandomSound>();
        groundLayer = LayerMask.GetMask("Ground");

        CooldownTimer = cooldown;

        SetRange(range);
    }

    void Update()
    {
        CooldownTimer += Time.deltaTime;
        
        if (impactTimer >= impactTime) // attackCollider time of presence
        {
            impactTimer = 0f;
            attackCollider.enabled = false;
        }

        if (attackCollider.enabled)
        {
            impactTimer += Time.deltaTime;
        }

        if (CooldownTimer > cooldown && !attackButton.interactable)
        {
            attackButton.interactable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (impactTimer > 0)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.Death();
                playerMovement.AddEnemyKilled();
            }
        }
    }

    public void AttackInput()
    {
        if (CooldownTimer > cooldown)
        {
            Attack();
            attackButton.interactable = false;
        }
    }

    public void Attack()
    {
        playerSound.Play();
        CooldownTimer = 0f;
        playerDisplay.Attack();
        healLineDisplayAnimator.SetTrigger("Attack");

        if (playerMovement.CurrentJump > 0 && playerMovement.CurrentJump < playerMovement.maxJumps) playerMovement.PauseJump();

        attackCollider.enabled = true;

        FloorTileController floorTile = GetTileBelow().GetComponent<FloorTileController>();
        if (floorTile != null) floorTile.Heal();
    }

    public GameObject GetTileBelow()
    {
        Ray topdown = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit tileHit;
        Physics.Raycast(topdown, out tileHit, 100f, groundLayer);
        return tileHit.collider.gameObject;
    }

    public void SetContinuousAttack(bool value)
    {
        attackCollider.enabled = value;
    }

    public void SetRange(float newRange)
    {
        range = newRange;
        attackCollider.size = new Vector3(range, 10f, range);
    }

}
