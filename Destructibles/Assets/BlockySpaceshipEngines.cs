using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockySpaceshipEngines : BlockyComponentInteractive
{
    public float force;
    public float maxspeedModifier;
    public float minForce;
    public float maxForce;
    public float targetForce;
    public CUIEngineSliders associatedSpeedometer;
    private float antimaxforce;
    // Start is called before the first frame update

    public override void DetailDelete() {
        if (this.myListRepresentation != null) { this.myListRepresentation.DeleteSelf();}
        this.spaceShip.spaceshipMover.RemoveEngine(this);
    }

    public override void DetailVariables() {
        antimaxforce = 1 / maxForce;
        this.spaceShip.spaceshipMover.AddEngine(this);
    }
    
    public override void DetailUpdate() {
        if (this.myListRepresentation != null)
        {
            this.myListRepresentation.UpdateValues(this);
        }
    }

    public override void ComponentAction() {
        ActivateThrust();
    }

    public void ActivateThrust() {
        if (associatedSpeedometer != null) {
            associatedSpeedometer.speedometer.sizeDelta = new Vector2(associatedSpeedometer.speedometer.sizeDelta.x, associatedSpeedometer.maxHeight * force * antimaxforce + 0.01f);
            associatedSpeedometer.speedometerImage.color = Color.Lerp(associatedSpeedometer.speedometerColorZero, associatedSpeedometer.speedometerColorOne, 1 * force * antimaxforce);
        }
        force += Mathf.Sign(targetForce - force) * 0.002f;

        spaceShip.rb.AddForce(spaceShip.transform.rotation * orientation * force, ForceMode.Force);
        Debug.Log(spaceShip.transform.rotation * orientation);
    }

}
