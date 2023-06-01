using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float naturalDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;

    /*
    [Header("Interactions")]
    public float interactionCooldown;
    public float interactionRange;
    */
    public bool readyToInteract;
    [Header("Ground check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("keys")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode useTool = KeyCode.Mouse0;

    public Transform orientation;
    //public Transform directionOfView;
    //public Transform camPosition;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    /*
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        //readyToInteract = true;
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ToolTarget()
    {
        List<Polygon> hole = new List<Polygon>();
        Vector3 direction = directionOfView.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(camPosition.position, direction, out hit, interactionRange))
        {
            Mesh cube = new SphereToAsteroid().Cube();
            new SphereToAsteroid().Resize(cube, new Vector3(0.4f, 0.4f, 0.4f));
            new SphereToAsteroid().Move(cube, hit.point - new Vector3(0.2f, 0.2f,0.2f));
            List<Polygon> cubePolys = BSPNode.ModelToPolygons(cube);

            GameObject James = hit.collider.gameObject;
            //James.transform.position = James.transform.position + Vector3.up;

            James.GetComponent<AsteroidChunk>().DigMesh(cubePolys, hit.point);
            Debug.Log("here i am");
            Debug.DrawLine(hit.point, camPosition.position, Color.red, 30f);
        }
    }

    private void MovePlayer()
    {
        
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = naturalDrag;
        else
            rb.drag = 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SpeedControl()
    {
        Vector3 flatVe = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVe.magnitude > moveSpeed)
        {
            Vector3 limiredVe = flatVe.normalized * moveSpeed;
            rb.velocity = new Vector3( limiredVe.x, rb.velocity.y, limiredVe.z);
        }
    }*/
}
