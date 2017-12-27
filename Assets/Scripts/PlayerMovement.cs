using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 0.1f;
    public float jump = 30f;
    public float range = 0.1f;
    public GameObject pathNodeDisplay;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool isRunning = false;

    float defaultY;
    Transform player;
    Rigidbody rb;
    Camera cam;
    Vector3 targetLocation;
    PlayerAnimation playerAnimation;
    float camRayLength = 100f;
    int floorMask;
    bool onGround = false;
    List<GameObject> path = new List<GameObject>();
    int pathStep = 0;

    void Awake()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        cam = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
    }

    private void Start()
    {
        defaultY = player.position.y;
        ResetPath();
    }

    void Update()
    {

        ManageJump();

        if (Input.GetButtonDown("Fire1") && !isRunning)
        {
            AddToPath(MouseToPoint());
        }

        if (Input.GetButtonDown("Fire2") && !isRunning)
        {
            StartPath();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (DistanceFromTop(targetLocation, player.position) > range)
        {
            Move();
        }
        else if (pathStep <= path.Count && isRunning)
        {
            StartPath();
        }
        else
        {
            isRunning = false;
        }
    }

    void Move()
    {
        player.Translate(Vector3.forward * speed);
    }

    void Jump()
    {
        rb.velocity = Vector3.up * jump;
        playerAnimation.Jump();
    }

    void ManageJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else
        {
            playerAnimation.JumpLand();
        }
    }

    void StartPath()
    {
        if (pathStep < path.Count)
        {
            targetLocation = GetWithDefaultY(path[pathStep].transform.position);
            targetLocation.y = player.position.y;
            player.LookAt(targetLocation);
            pathStep++;
            isRunning = true;
        }
        else
        {
            ResetPath();
        }
    }

    void ResetPath()
    {
        path.ForEach(Destroy);
        path.Clear();
        targetLocation = GetWithDefaultY(player.position);
        AddToPath(targetLocation);
        pathStep = 0;
        isRunning = false;
    }

    void AddToPath(Vector3 point)
    {
        point.y += 0.1f;
        GameObject pathNode = Instantiate(pathNodeDisplay, point, pathNodeDisplay.GetComponent<Transform>().rotation);

        if (path.Count > 0)
        {
            LineRenderer lr = path[path.Count - 1].GetComponent<LineRenderer>();
            lr.SetPosition(0, path[path.Count - 1].transform.position);
            lr.SetPosition(1, point);
        }
        path.Add(pathNode);
    }

    public void Fell()
    {
        onGround = true;
        ResetPath();
    }

    public void OutOfFloor()
    {
        onGround = false;
    }

    Vector3 GetWithDefaultY(Vector3 t)
    {
        return new Vector3(t.x, defaultY, t.z);
    }

    Vector3 MouseToPoint()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, floorMask);
        return floorHit.point;
    }

    float DistanceFromTop(Vector3 p0, Vector3 p1)
    {
        return Vector2.Distance(new Vector2(p0.x, p0.z), new Vector2(p1.x, p1.z));
    }
}
