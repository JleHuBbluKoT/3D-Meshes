using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    public BlockySpaceship spaceship;
    public Rigidbody rb;
    float defaultMoveSpeed = 1f;
    public float moveSpeed;
    public List<BlockySpaceshipEngines> engines = new List<BlockySpaceshipEngines>();

    public List<BlockySpaceshipEngines> enginesPlusX = new List<BlockySpaceshipEngines>();
    public List<BlockySpaceshipEngines> enginesPlusY = new List<BlockySpaceshipEngines>();
    public List<BlockySpaceshipEngines> enginesPlusZ = new List<BlockySpaceshipEngines>();
    public List<BlockySpaceshipEngines> enginesMinusX = new List<BlockySpaceshipEngines>();
    public List<BlockySpaceshipEngines> enginesMinusY = new List<BlockySpaceshipEngines>();
    public List<BlockySpaceshipEngines> enginesMinusZ = new List<BlockySpaceshipEngines>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveSpaceship();
    }

    public void MoveSpaceship()
    {
        /*if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.Rotate(0, 0.2f, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            gameObject.transform.Rotate(0, -0.2f, 0);
        }*/

        if (Input.GetKey(KeyCode.W))
        {
            foreach (var item in enginesPlusZ)
            {
                item.ActivateThrust();
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            foreach (var item in enginesMinusZ)
            {
                item.ActivateThrust();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            foreach (var item in enginesMinusX)
            {
                item.ActivateThrust();
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            foreach (var item in enginesPlusX)
            {
                item.ActivateThrust();
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var item in enginesPlusY)
            {
                item.ActivateThrust();
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            foreach (var item in enginesMinusY)
            {
                item.ActivateThrust();
            }
        }
    }

    public void RecalculateEngines()
    {
        enginesPlusX.Clear();
        enginesPlusY.Clear();
        enginesPlusZ.Clear();
        enginesMinusX.Clear();
        enginesMinusY.Clear();
        enginesMinusZ.Clear();

        if (engines.Count == 0) return;
        moveSpeed = defaultMoveSpeed;
        for (int i = 0; i < engines.Count; i++)
        {
            moveSpeed += engines[i].maxspeedModifier;
        }
        for (int i = 0; i < engines.Count; i++)  {
            if (engines[i].orientation == Vector3.forward){
                enginesPlusZ.Add(engines[i]);
            }
            else if (engines[i].orientation == Vector3.back) {
                enginesMinusZ.Add(engines[i]);
            }
            else if (engines[i].orientation == Vector3.right) {
                enginesPlusX.Add(engines[i]);
            }
            else if (engines[i].orientation == Vector3.left) {
                enginesMinusX.Add(engines[i]);
            }
            else if (engines[i].orientation == Vector3.up)  {
                enginesPlusY.Add(engines[i]);
            }
            else if (engines[i].orientation == Vector3.down) {
                enginesMinusY.Add(engines[i]);
            }
        }
    }

    public void RemoveEngine(BlockySpaceshipEngines engine)
    {
        Debug.Log("kill");
        engines.Remove(engine);
        RecalculateEngines();
    }
    public void AddEngine(BlockySpaceshipEngines engine)
    {
        //Debug.Log("new engine");
        engines.Add(engine);
        RecalculateEngines();
    }

    private void SpeedControl()
    {
        Vector3 flatVe = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);

        if (flatVe.magnitude > moveSpeed)
        {
            Vector3 limiredVe = flatVe.normalized * moveSpeed;
            rb.velocity = limiredVe;
        }
    }
}
