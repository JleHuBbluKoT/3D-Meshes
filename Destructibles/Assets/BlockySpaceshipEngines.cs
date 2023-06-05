using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockySpaceshipEngines : MonoBehaviour
{
    public GameObject spaceShip;
    public Vector3 orientation;
    public float force;
    public float maxspeedModifier;
    // Start is called before the first frame update

    public void OnDelete()
    {
        this.spaceShip.GetComponent<SpaceshipMovement>().RemoveEngine(this);
    }

    public void SetVariables(GameObject _spaceship)
    {
        this.spaceShip = _spaceship;
        this.spaceShip.GetComponent<SpaceshipMovement>().AddEngine(this);
    }
    public void ActivateThrust()
    {
        Debug.Log("trust");
        spaceShip.GetComponent<BlockySpaceship>().rb.AddForce(orientation * force, ForceMode.Force);
    }
}
