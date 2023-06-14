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
    public Light myLight;

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
            float theta = Mathf.PI * (Mathf.Lerp(-80, 80, p) + directions[orientation] ) / 180f;
            float phi = Mathf.PI * Mathf.Lerp(-80, 80, y) / 180f;
            Vector3 vectorOne = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(phi), Mathf.Cos(theta) * Mathf.Cos(phi));
            finalAngle = vectorOne;
        }
        this.myCamera.transform.LookAt(this.transform.position + finalAngle);
        this.myLight.transform.rotation = myCamera.transform.rotation;
        //Debug.DrawLine(myCamera.transform.position, myCamera.transform.position + finalAngle, color: Color.red, 10f);
    }

    public override void ComponentAction() {
        throw new System.NotImplementedException();
    }

    public Dictionary<Vector3, int> directions = new Dictionary<Vector3, int>
    {
        {Vector3.forward, 0},
        {Vector3.back, 180},
        {Vector3.right, 90 },
        {Vector3.left, 270 },
        {Vector3.up, 0 },
        {Vector3.down, 0 },
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