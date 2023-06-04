using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockySpaceshipEngines : MonoBehaviour
{
    public GameObject spaceShip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.U) )
        {
            ActivateThrust();
        }
    }
    public void SetVariables(GameObject _spaceship)
    {
        this.spaceShip = _spaceship;
    }
    public void ActivateThrust()
    {
        spaceShip.GetComponent<BlockySpaceship>().rb.AddForce(Vector3.up, ForceMode.Force);
    }
}
