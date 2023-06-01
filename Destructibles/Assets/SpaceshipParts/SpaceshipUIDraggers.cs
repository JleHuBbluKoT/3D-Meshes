using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipUIDraggers : MonoBehaviour
{
    public GameObject associatedComponent;
    public Vector3 axis;
    private WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

    private IEnumerator DragUpdate(GameObject UIelement, Camera cam)
    {
        //Plane plane = new Plane(Vector3.zero, Vector3.up, Vector3.right);
        SpaceshipUIDraggers dragger = UIelement.GetComponent<SpaceshipUIDraggers>();

        Vector3 axis = UIelement.GetComponent<SpaceshipUIDraggers>().axis;

        Vector3 originalPosition = dragger.associatedComponent.transform.position;

        while (Input.GetKey(KeyCode.Mouse0))
        {

            Debug.Log(this.gameObject + " " + axis);
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mousePosition);

            Plane newPlane = new Plane( (this.transform.position - cam.transform.position).normalized, this.transform.position);

            float dist;
            newPlane.Raycast(ray, out dist);

            Vector3 getPoint = ray.GetPoint(dist);
            Vector3 demandedPosition =
                new Vector3(originalPosition.x * (1 - axis.x), originalPosition.y * (1 - axis.y), originalPosition.z * (1 - axis.z)) +
                new Vector3(getPoint.x * axis.x, getPoint.y * axis.y, getPoint.z*axis.z);

            dragger.associatedComponent.transform.position = new Vector3(Mathf.Clamp(demandedPosition.x, -10, 10), Mathf.Clamp(demandedPosition.y, -10, 10), Mathf.Clamp(demandedPosition.z, -10, 10));
            yield return fixedUpdate;
        }
    }



    public void StartCoroutineDrag(GameObject UIelement, Camera cam)
    {
        StartCoroutine(DragUpdate(UIelement, cam));
    }

    public void setVariables(GameObject shipComponent, Vector3 _axis)
    {
        associatedComponent = shipComponent;
        axis = _axis;
    }
}
