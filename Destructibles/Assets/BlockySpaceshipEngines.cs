using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockySpaceshipEngines : BlockyComponentInteractive
{
    public float force;
    public float maxspeedModifier;

    // Start is called before the first frame update

    public override void DetailDelete()
    {

        if (this.myListRepresentation != null) { this.myListRepresentation.DeleteSelf();}
        this.spaceShip.spaceshipMover.RemoveEngine(this);
    }

    public override void DetailVariables()
    {
        this.spaceShip.spaceshipMover.AddEngine(this);
    }
    
    public override void DetailUpdate()
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
        spaceShip.rb.AddForce(orientation * force, ForceMode.Force);
    }

}
