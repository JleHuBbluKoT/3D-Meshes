using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockySpaceshipEngines : BlockyComponentInteractive
{
    
    public GameObject spaceShip;
    public Vector3 orientation;
    public float force;
    public float maxspeedModifier;

    // Start is called before the first frame update

    public void OnDelete()
    {

        if (this.myListRepresentation != null) { this.myListRepresentation.DeleteSelf();}
        this.spaceShip.GetComponent<SpaceshipMovement>().RemoveEngine(this);
    }

    public void SetVariables(GameObject _spaceship)
    {
        this.spaceShip = _spaceship;
        this.spaceShip.GetComponent<SpaceshipMovement>().AddEngine(this);
    }
    
    public void UpdateValues()
    {
        if (this.myListRepresentation != null)
        {
            this.myListRepresentation.UpdateValues(this);
        }
    }

    public override void ComponentAction()
    {
        ActivateThrust();
    }

    public void ActivateThrust()
    {
        Debug.Log("trust");
        spaceShip.GetComponent<BlockySpaceship>().rb.AddForce(orientation * force, ForceMode.Force);
    }

}
