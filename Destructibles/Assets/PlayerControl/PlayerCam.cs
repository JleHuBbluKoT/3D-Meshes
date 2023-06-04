using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform playerOrientation;

    float xRotation;
    float yRotation;

    public KeyCode useTool = KeyCode.Mouse0;


    private void Start()
    {
        
        //Debug.Log(Vector3. ( new Vector3Int(3, 1, 3), new Vector3Int(1, 0, 1) ));
        //Debug.Log(Quaternion.Euler(90, 0, 0) * new Vector3Int(3, 1, 3));
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKey(useTool))
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
