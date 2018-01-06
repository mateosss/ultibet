using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [Header("Movement")]
    public float speed = 0.1f;
    public float jump = 30f;

    [Header("Advanced")]
    public float nodeContactThreshold = 0.5f;
    public GameObject pathNodeDisplay;

    // Initialization ad hoc
    const float camRayLength = 100f;
    bool onGround = false;
    bool running = false;
    int pathStep = 0;
    List<GameObject> path = new List<GameObject>();

    // Initialized on awake
    Transform player;
    Rigidbody rb;
    PlayerAnimation playerAnimation;
    Camera cam;
    int touchableLayer;

    // Initialized on start
    float defaultY;
    Vector3 targetLocation;

    void Awake()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        cam = Camera.main;
        touchableLayer = LayerMask.GetMask("Node");
    }

    private void Start()
    {
        defaultY = player.position.y;
        ResetPath();
    }

    void Update()
    {

        if (Input.GetButton("Fire1") && !running) // Add node to path
        {
            GameObject node = NodeTouchedByMouse();
            if (node)
            {
                AddToPath(node.transform.position);
            }
        }

        if (Input.GetButtonDown("Fire2") && !running) // Start path
        {
            StartPath();
        }

        if (Input.GetButtonDown("Jump")) // Jump
        {
            Jump();
        }

        if (DistanceFromTop(targetLocation, player.position) > nodeContactThreshold) // Move, change node or stop running
        {
            Move();
        }
        else if (pathStep <= path.Count && running)
        {
            StartPath();
        }
        else
        {
            running = false;
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

    // Path section

    void StartPath()
    {
        if (pathStep < path.Count)
        {
            targetLocation = GetWithDefaultY(path[pathStep].transform.position);
            targetLocation.y = player.position.y;
            player.LookAt(targetLocation);
            pathStep++;
            running = true;
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
        running = false;
    }

    void AddToPath(Vector3 point)
    {
        point.y += 0.1f;
        if (path.Count > 0 && point == path[path.Count - 1].transform.position) return;

        GameObject pathNode = Instantiate(pathNodeDisplay, point, pathNodeDisplay.GetComponent<Transform>().rotation);
        if (path.Count > 0)
        {
            LineRenderer lr = path[path.Count - 1].GetComponent<LineRenderer>();
            lr.SetPosition(0, path[path.Count - 1].transform.position);
            lr.SetPosition(1, point);
        }
        path.Add(pathNode);
    }

    public void OutOfFloor()
    {
        onGround = false;
    }

    public void Fell()
    {
        onGround = true;
        ResetPath();
    }

    public bool isRunning()
    {
        return running;
    }

    Vector3 GetWithDefaultY(Vector3 t)
    {
        return new Vector3(t.x, defaultY, t.z);
    }

    Vector3 MouseToPoint()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, touchableLayer);
        return floorHit.point;
    }

    GameObject NodeTouchedByMouse()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        Physics.Raycast(camRay, out floorHit, camRayLength, touchableLayer);
        if (!floorHit.collider) return null;
        return floorHit.collider.gameObject;
    }

    float DistanceFromTop(Vector3 p0, Vector3 p1)
    {
        return Vector2.Distance(new Vector2(p0.x, p0.z), new Vector2(p1.x, p1.z));
    }
}
