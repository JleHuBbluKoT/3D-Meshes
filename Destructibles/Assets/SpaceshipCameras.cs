using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCameras : MonoBehaviour
{
    public BlockySpaceship spaceship;
    public List<SpaceshipCameraDetail> cameras = new List<SpaceshipCameraDetail>();

    
    public void RemoveCamera(SpaceshipCameraDetail camera)
    {
        Debug.Log("kill");
        cameras.Remove(camera);
    }
    public void AddCamera(SpaceshipCameraDetail camera)
    {
        Debug.Log("new camera");
        cameras.Add(camera);
    }


}
