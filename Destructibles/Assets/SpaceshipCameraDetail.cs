using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCameraDetail : BlockyComponentInteractive
{
    public RenderTexture prefabRenderTexture;
    public Transform cameraVisual;
    public Vector2 PitchDegree;
    public Vector2 YawDegree;
    // public GameObject MyListRepresentation
    public GameObject associatedComponent;
    public RenderTexture myRenderTexture;
    public Camera myCamera;

    public override void DetailDelete() {
        if (this.myListRepresentation != null) { this.myListRepresentation.DeleteSelf(); }
        this.spaceShip.spaceshipCameras.RemoveCamera(this);
    }

    public override void DetailVariables() {
        myCamera.transform.rotation.SetLookRotation(this.orientation);

        this.myRenderTexture = new RenderTexture(prefabRenderTexture);
        Debug.Log(this.myRenderTexture.isPowerOfTwo);
        this.myRenderTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
        this.spaceShip.spaceshipCameras.AddCamera(this);
        myCamera.targetTexture = myRenderTexture;
    }

    public override void DetailUpdate()
    {
        if (this.myListRepresentation != null) {
            this.myListRepresentation.UpdateValues(this);
        }
    }

    public void SetRotation(float p, float y)
    {
        Vector3 rotationVector = orientation;
        Vector3 finalAngle;
        if (orientation.y != 0)
        {
            float theta = Mathf.PI * Mathf.Lerp(-180, 180, p) / 180f;
            float phi = Mathf.PI * Mathf.Lerp(-20, 85, y) * orientation.y / 180f;
            Vector3 pointToLookAt = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(phi), Mathf.Cos(theta) * Mathf.Cos(phi));
            finalAngle = pointToLookAt;
        }
        else
        {
            Vector3 vectorOne = directions[orientation][0] * Mathf.Lerp(-10, 10, p);
            Vector3 vectorTwo = Vector3.up * Mathf.Lerp(-10, 10, y);
            finalAngle = rotationVector + vectorOne + vectorTwo;
        }
        this.myCamera.transform.LookAt(this.transform.position + finalAngle);
        //Debug.DrawLine(myCamera.transform.position, myCamera.transform.position + finalAngle, color: Color.red, 10f);
    }

    public override void ComponentAction() {
        throw new System.NotImplementedException();
    }

    public Dictionary<Vector3, Vector3[]> directions = new Dictionary<Vector3, Vector3[]>
    {
        {Vector3.forward, new Vector3[]{Vector3.right, Vector3.up } },
        {Vector3.back, new Vector3[]{Vector3.left, Vector3.up } },
        {Vector3.right, new Vector3[]{Vector3.back, Vector3.up } },
        {Vector3.left, new Vector3[]{Vector3.forward, Vector3.up } },
        {Vector3.up, new Vector3[]{Vector3.forward, Vector3.up } },
        {Vector3.down, new Vector3[]{Vector3.forward, Vector3.up } },
    };
}

/*
Vector3 rotationVector = orientation;
        Vector3 vectorOne = Quaternion.Euler(90, 90, 0) * rotationVector;
        Vector3 vectorTwo = Vector3.Cross(vectorOne, rotationVector);

        Debug.Log(rotationVector);
        Debug.Log(vectorOne);
        Debug.Log(vectorTwo);
        Vector3 virtualP = vectorOne * Mathf.Lerp(-10, 10, p);
        Vector3 virtualY = vectorTwo * Mathf.Lerp(-10, 10, y);
        Vector3 finalAngle = rotationVector + virtualP + virtualY;
        Debug.Log(virtualP);
        Debug.Log(virtualY);
        Debug.Log(finalAngle);

        Debug.DrawLine(myCamera.transform.position, myCamera.transform.position + finalAngle, color: Color.red, 10f);*/