using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockySpaceshipTractorBeam : BlockyComponentInteractive
{
    public int CheckCount;
    public float TractorRange;
    public float TractorAngle;
    public LayerMask mask;

    public override void DetailDelete()
    {
        if (this.myListRepresentation != null) { this.myListRepresentation.DeleteSelf(); }
        //this.spaceShip.spaceshipMover.RemoveEngine(this);
    }

    public override void DetailVariables()
    {
        //this.spaceShip.spaceshipMover.AddEngine(this);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            ComponentAction();
        }
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
        Collider[] hits = Physics.OverlapCapsule(this.transform.position, this.transform.position + orientation * TractorRange, 0.7f, mask );
        //Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; i++) {
            //Debug.Log(hits[i].CompareTag("Magnetic"));
            if (hits[i].CompareTag("Magnetic")) {
                Debug.Log(hits[i]);
                Vector3 hitPos = hits[i].transform.position;
                Vector3 direction = this.transform.position - hitPos;
                hits[i].GetComponent<Rigidbody>().AddForce(direction * 3, ForceMode.Force);

                if (Vector3.Distance(hits[i].transform.position, this.transform.position) < 2)
                {
                    Destroy(hits[i].gameObject);
                }
            }
        }
    }

}



