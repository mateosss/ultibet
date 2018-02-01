using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float speed = 4f;
    public float speedModifier = 1f; // Multiplies speed by this
    public float jump = 30f;
    public float jumpAttackAirPause = 0.2f;

    [Header("Long Path Bonus (BPS = Bonus per second)")]
    public float isLongPathFrom = 15f;
    public float cooldownBPS = 0.1f;
    public float rangeBPS = 0.5f;
    public float overdriveBPS = 0.06f; // 0.1 is approx one lap to the 20x20 meters level

    [Header("Extra")]
    public float nodeContactThreshold = 0.5f;
    public float maxTimeForWallCollision = 0.5f; // After this amount of seconds the movement starts to check for wall collisions, it is useful when you felt really close to a wall and want to get up.
    public GameObject pathNodeDisplay;
    public int maxJumps = 2;

    // Overdrive

    [Header("Overdrive")]
    public float maxOverdriveDistance = 100f;
    public GameObject overdriveButton;
    public float dashDuration = 0.2f;
    public bool overdriving = false;
    public float OverdriveCharge { get; private set; }
    public float overdriveDistance = 0f; // Current overdrive distance traveled
    bool endOverdrive = false;
    float dashTimer = 0f;

    // Current Path Stats

    public float PathDistance { get; private set; }
    public int PathStep { get; private set; }
    public int PathKilled { get; private set; }
    public int CurrentJump { get; private set; }
    public int JumpState { get; private set; }

    bool onGround = false;
    bool running = false;
    List<GameObject> path = new List<GameObject>();
    GetUpFromFloor getUpFromFloor;
    bool pauseJump = false;
    bool shouldJump = false;
    Vector3 targetLocation;
    float maxWallCollisionTimer = 0f;

    // General
    Transform player;
    Rigidbody rb;
    PlayerAnimation playerAnimation;
    PlayerAttack playerAttack;
    Camera cam;
    const float camRayLength = 100f;
    float defaultY;
    int nodeLayer;
    int platformLayer;
   
    // Long Term Stats
    public float DistanceTravelled { get; private set; }
    public int NodesTravelled { get; private set; }
    public int EnemiesKilled { get; private set; }

    void Awake()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAttack = GetComponent<PlayerAttack>();
        cam = Camera.main;
        nodeLayer = LayerMask.GetMask("Node");
        platformLayer = LayerMask.GetMask("Platform");
        getUpFromFloor = GetComponent<GetUpFromFloor>();
        getUpFromFloor.enabled = false;

        CurrentJump = 0;
        DistanceTravelled = 0f;
        NodesTravelled = 0;
        EnemiesKilled = 0;
        PathDistance = 0f;
        PathStep = 0;
        PathKilled = 0;
        OverdriveCharge = 0f;
    }

    private void Start()
    {
        defaultY = player.position.y;
        ResetPath();
        StartCoroutine(BPS());
    }

    void Update()
    {
        if (!pauseJump && !overdriving && Input.GetButton("Fire1") && !running) // Input: Add node to path
        {
            GameObject node = NodeTouchedByMouse();
            if (node)
            {
                AddToPath(node.transform.position);
            }
        }

        if (overdriving && Input.GetButtonDown("Fire1") && !running) // Overdrive input
        {
            GameObject node = NodeTouchedByMouse();
            if (node)
            {
                overdriveDistance += Vector3.Distance(node.transform.position, path[path.Count - 1].transform.position);
                if (overdriveDistance < maxOverdriveDistance)
                {
                    AddToPath(node.transform.position);
                }
                else
                {
                    endOverdrive = true;
                    StartPath();
                }
                
            }
        }

        if (!pauseJump && !overdriving && Input.GetButtonUp("Fire1") && !running) // Start path
        {
            if (path.Count > 1) // Path stroke has started
            {
                StartPath();
            }
            else
            {
                if (TouchingLeftSide())
                {
                    if (CurrentJump < maxJumps || onGround)
                    {
                        Jump();
                    }
                }
                else
                {
                    if (playerAttack.CooldownTimer > playerAttack.cooldown)
                    {
                        playerAttack.Attack();
                    }    
                }
            }
        }
        
        if (running && !overdriving && (shouldJump || (Input.GetButtonDown("Fire1") && TouchingLeftSide() || Input.GetButtonDown("Jump"))) && (CurrentJump < maxJumps || onGround)) // Jump
        {
            if (pauseJump)
            {
                shouldJump = true;
            }
            else
            {
                Jump();
            }
            
        }
        else if (CurrentJump > 0)
        {
            CheckJumpFinish();
        }

        if (!overdriving && !running && OverdriveCharge >= 1) // Activate overdrive
        {
            SetOverdrive(true);
        }

        if (overdriving && !running && endOverdrive)
        {
            SetOverdrive(false);
        }
    }

    private void FixedUpdate()
    {
        if (DistanceFromTop(targetLocation, player.position) > nodeContactThreshold) // Move, change node or stop running
        {
            if (overdriving)
            {
                DashMove();
            }
            else if (!pauseJump)
            {
                Move(Time.deltaTime);
            }
        }
        else if (PathStep <= path.Count && running)
        {
            StartPath();
            if (overdriving)
            {
                dashTimer = 0f;
            }
        }
        else
        {
            running = false;
        }
    }

    IEnumerator BPS() // Long path bonuses
    {
        bool withBonus = false;
        float initialCooldown = playerAttack.cooldown;
        float initialRange = playerAttack.range;
        while (true)
        {
            if (!overdriving && running && PathDistance > isLongPathFrom)
            {
                if (!withBonus)
                {
                    withBonus = true;
                }
                if (playerAttack.cooldown > 0) // Cooldown
                {
                    playerAttack.cooldown -= cooldownBPS * Time.deltaTime;
                }
                playerAttack.SetRange(playerAttack.range + rangeBPS * Time.deltaTime); // Range
                if (OverdriveCharge < 1f) // Overdrive
                {
                    OverdriveCharge += overdriveBPS * Time.deltaTime;
                }
                else
                {
                    OverdriveCharge = 1f;
                }
            }
            else if (withBonus)
            {
                withBonus = false;
                playerAttack.cooldown = initialCooldown;
                playerAttack.SetRange(initialRange);
            }
            yield return null;
        }
    }

    void SetOverdrive(bool active)
    {
        overdriving = active;
        overdriveButton.SetActive(active);
        if (!active)
        {
            OverdriveCharge = 0f;
            overdriveDistance = 0f;
            endOverdrive = false;
        }
    }

    void Move(float dt)
    {
        float move = speed * dt * speedModifier;
        player.Translate(Vector3.forward * move);
        PathDistance += move;
        DistanceTravelled += move;
    }

    void DashMove()
    {
        Vector3 target = GetWithDefaultY(targetLocation);
        if (dashTimer == 0)
        {
            if (PathStep <= path.Count) playerAnimation.Dash();
        }
        dashTimer += Time.deltaTime;

        Vector3 move = (target - player.position) * (dashTimer / dashDuration);
        player.position += move;

        PathDistance += move.magnitude;
        DistanceTravelled += move.magnitude;
        overdriveDistance += move.magnitude;
    }

    public void Jump()
    {
        shouldJump = false;
        rb.velocity = Vector3.up * jump;
        playerAnimation.Jump();
        CurrentJump++;
    }

    void CheckJumpFinish()
    {
        if (IsOnPlatform()) // Jump finished
        {
            CurrentJump = 0;
        }
    }

    // Path section

    public void StartPath()
    {
        if (!running && !overdriving)
        {
            playerAnimation.Run();
        }

        if (onGround)
        {
            getUpFromFloor.enabled = true;
        }

        if (overdriving)
        {
            playerAttack.SetContinuousAttack(true);
        }

        if (PathStep == 0)
        {
            PathStep++;
            running = true;
        }
        else if (PathStep < path.Count) // Change to new node in the same path
        {
            targetLocation = GetWithDefaultY(path[PathStep].transform.position);
            targetLocation.y = player.position.y;
            player.LookAt(targetLocation);
            PathStep++;
            NodesTravelled++;
        }
        else // Stop path
        {
            ResetPath();
        }
    }

    void ResetPath()
    {
        path.ForEach(Destroy);
        path.Clear();
        targetLocation = GetWithDefaultY(player.position);
        //targetLocation -= new Vector3(0, 0.5f, 0);
        targetLocation.y -= 0.5f;
        AddToPath(targetLocation);
        PathDistance = 0f;
        PathStep = 0;
        PathKilled = 0;
        running = false;
        maxWallCollisionTimer = 0f;
        playerAttack.SetContinuousAttack(false);
        playerAnimation.Still();
    }

    void AddToPath(Vector3 point)
    {
        point.y += 0.1f;
        if (path.Count > 0 && point == path[path.Count - 1].transform.position) return;

        GameObject pathNode = Instantiate(pathNodeDisplay, point, pathNodeDisplay.GetComponent<Transform>().rotation);
        if (path.Count > 0)
        {
            LineRenderer lr = path[path.Count - 1].GetComponent<LineRenderer>();
            Vector3 start = path[path.Count - 1].transform.position;
            start.y -= 0.05f;
            lr.SetPosition(0, start);
            point.y -= 0.1f;
            lr.SetPosition(1, point);
        }
        path.Add(pathNode);
    }

    void SetSpeedModifier(float to)
    {
        speedModifier = to;
    }

    public void OutOfFloor()
    {
        onGround = false;
        getUpFromFloor.enabled = false;
    }

    public void Fell()
    {
        onGround = true;
        ResetPath();
        CurrentJump = 0;
    }

    public bool IsOnGround()
    {
        return onGround;
    }

    public bool IsRunning()
    {
        return running;
    }

    public bool IsJumping()
    {
        return CurrentJump > 0;
    }

    public void PauseJump()
    {
        //StartCoroutine(JumpAttack());
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        pauseJump = true;
        Invoke("RestoreJump", jumpAttackAirPause);
    }
    
    public void RestoreJump()
    {
        rb.useGravity = true;
        pauseJump = false;
    }

    IEnumerator JumpAttack()
    {
        Vector3 velocity = rb.velocity;
        Vector3 angularVelocity = rb.angularVelocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        pauseJump = true;
        yield return new WaitForSeconds(jumpAttackAirPause);

        rb.velocity = velocity;
        rb.angularVelocity = angularVelocity;
        rb.useGravity = true;
        pauseJump = false;
    }

    public void AddEnemyKilled()
    {
        if (running)
        {
            PathKilled++;
        }
        EnemiesKilled++;
    }

    Vector3 GetWithDefaultY(Vector3 t)
    {
        return new Vector3(t.x, defaultY, t.z);
    }

    Vector3 MouseToPoint()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, nodeLayer);
        return floorHit.point;
    }

    GameObject NodeTouchedByMouse()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, nodeLayer);
        if (!floorHit.collider) return null;
        return floorHit.collider.gameObject;
    }

    bool IsOnPlatform()
    {
        if (rb.velocity.y > 0)
        {
            return false;
        }
        Ray topdown = new Ray(player.position, Vector3.down);
        RaycastHit platformHit;
        Physics.Raycast(topdown, out platformHit, player.localScale.z, platformLayer);
        return platformHit.collider != null;
    }

    bool IsOnWall()
    {
        Ray front = new Ray(player.position, player.forward);
        RaycastHit wallHit;
        Physics.Raycast(front, out wallHit, player.localScale.z, platformLayer);
        //Debug.DrawLine(player.position, player.position + player.forward * 5, Color.green);
        return wallHit.collider != null;
    }

    float DistanceFromTop(Vector3 p0, Vector3 p1)
    {
        return Vector2.Distance(new Vector2(p0.x, p0.z), new Vector2(p1.x, p1.z));
    }

    bool TouchingLeftSide()
    {
        return Input.mousePosition.x < Screen.width / 2;
    }

    public bool IsDrawingPath()
    {
        return !running && path.Count > 1;
    }
}
