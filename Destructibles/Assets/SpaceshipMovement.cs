using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpeedControl()
    {
        Vector3 flatVe = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVe.magnitude > moveSpeed)
        {
            Vector3 limiredVe = flatVe.normalized * moveSpeed;
            rb.velocity = new Vector3(limiredVe.x, rb.velocity.y, limiredVe.z);
        }
    }
}
