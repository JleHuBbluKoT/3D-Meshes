using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCutter : MonoBehaviour
{
    public GameObject meshA;
    public GameObject meshB;

    Mesh mesh1;
    Material[] materials1;
    Transform transform1;

    Mesh mesh2;
    Material[] materials2;
    Transform transform2;

    void Start()
    {
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.localScale = Vector3.one * 1.3f;

        //StartCoroutine(waiter());
        // Тут будет булево CSG
       


    }

    IEnumerator waiter()
    {
        
        yield return new WaitForSecondsRealtime(2);
        /*
        Model result = CSG.Cutout(meshA, meshB);

        var composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        meshA.transform.position = new Vector3(10, 10, 10);
        meshB.transform.position = new Vector3(10, 10, 10);
        */
    }

}