using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipComponent : MonoBehaviour
{

    public GameObject UIdragController;
    public GameObject UIdragAxis;

    public GameObject UIcontrols;

    public GameObject AxisX;
    public GameObject AxisY;
    public GameObject AxisZ;

    public Vector3 constrainsAxis;


    public void ShowControls(Camera cam)
    {
        if (UIcontrols == null)
        {
            UIcontrols = Instantiate(UIdragController);
            UIcontrols.transform.SetParent(this.transform);

            Vector3 summon = this.transform.position + Vector3.one - constrainsAxis;
            Vector3 summon2 = this.transform.position - Vector3.one + constrainsAxis;
            if (Vector3.Distance(summon2, cam.transform.position) < Vector3.Distance(summon, cam.transform.position))
            {
                summon = summon2;
            }

            if (constrainsAxis.z == 1)
            {
                AxisX = GameObject.Instantiate(UIdragAxis);
                AxisX.transform.SetParent(UIcontrols.transform);
                AxisX.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                //UIaxis.transform.position = Vector3.zero;

                AxisX.GetComponent<SpaceshipUIDraggers>().setVariables(this.gameObject, new Vector3(0,0,1));
            }
            if (constrainsAxis.x == 1)
            {
                AxisZ = GameObject.Instantiate(UIdragAxis);
                AxisZ.transform.SetParent(UIcontrols.transform);
                AxisZ.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                //UIaxis.transform.position = Vector3.zero;

                AxisZ.GetComponent<SpaceshipUIDraggers>().setVariables(this.gameObject, new Vector3(1, 0, 0));
            }
            if (constrainsAxis.y == 1)
            {
                AxisY = GameObject.Instantiate(UIdragAxis);

                AxisY.transform.SetParent(UIcontrols.transform);
                AxisY.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                //UIaxis.transform.position = Vector3.zero;
                AxisY.GetComponent<SpaceshipUIDraggers>().setVariables(this.gameObject, new Vector3(0, 1, 0));



            }
            UIcontrols.transform.position = summon;


        }
    }



    public void HideControls()
    {
        Destroy(UIcontrols);
        UIcontrols = null;
    }

}
