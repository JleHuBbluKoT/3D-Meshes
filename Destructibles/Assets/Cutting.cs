using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutting : MonoBehaviour
{
    public GameObject TheThingie;
    public GameObject Pointer;
    // Start is called before the first frame update
    void Start()
    {
        Thingy();
    }

    static float epsilon = 0.00001f;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Thingy()
    {
 
        GameObject meshA = TheThingie;
        meshA.transform.position = new Vector3(0,0,0);

        //GameObject meshB = new GameObject();

        /*
        Mesh TriA = new Mesh();
        Vector3[] verticesA = new Vector3[] {
        new Vector3(0, 0.5f, 1),
        new Vector3(1, 0.5f, 0),
        new Vector3(1, 0.5f, 1),
        };
        TriA.vertices = verticesA;
        TriA.triangles = new int[] { 0,2,1 };
        meshA.AddComponent<MeshFilter>();
        meshA.AddComponent<MeshRenderer>();
        meshA.GetComponent<MeshFilter>().mesh = TriA;


        Mesh TriB = new Mesh();
        Vector3[] verticesB = new Vector3[] {
        new Vector3(1, 0, 1),
        new Vector3(1, 1, 1),
        new Vector3(0, 1, 0),
        };
        TriB.vertices = verticesB;
        TriB.triangles = new int[] { 0, 1, 2 };
        meshB.AddComponent<MeshFilter>();
        meshB.AddComponent<MeshRenderer>();
        meshB.GetComponent<MeshFilter>().mesh = TriB;
        meshA.transform.position = new Vector3(1,1,1);
        meshB.transform.position = new Vector3(1, 1, 1);*/

        Vector3 offset = new Vector3(0,0,0);
        Vector3 vertex1 = new Vector3(1, 0.5f, 1) + offset;
        Vector3 vertex2 = new Vector3(1, 0.5f, 0) + offset;
        Vector3 vertex3 = new Vector3(0, 0.5f, 1) + offset;

        CuttingPlane middlePlane = new CuttingPlane(vertex1, vertex2, vertex3);
        Vector3 pA = new Vector3(0, 0, 0);
        Vector3 pB = new Vector3(0, -2, 0);

        //RENDER POLYGON
        Mesh TriA = new Mesh(); Vector3[] verticesA = new Vector3[] {vertex1, vertex2, vertex3};
        TriA.vertices = verticesA; TriA.triangles = new int[] { 0, 1, 2 };
        //meshA.AddComponent<MeshFilter>();  meshA.AddComponent<MeshRenderer>(); 
        meshA.GetComponent<MeshFilter>().mesh = TriA;
        Instantiate<GameObject>(meshA);



        CutLine(pA, pB, middlePlane); 

    }

    public Vector3 CutLine(Vector3 vertexA, Vector3 vertexB, CuttingPlane plane)
    {
        

        float t1 = Vector3.Dot(plane.normal, vertexA) - plane.w;
        int type1 = (t1 < -epsilon) ? 2 : ((t1 > epsilon) ? 1 : 0);
        float t2 = Vector3.Dot(plane.normal, vertexB) - plane.w; 
        int type2 = (t2 < -epsilon) ? 2 : ((t2 > epsilon) ? 1 : 0);
        int Ftype = type1 | type2;
        Debug.Log(t1 + " " + t2 + " " + type1 + " " + type2 + " " + Ftype);

        float t = (plane.w - Vector3.Dot(plane.normal, vertexA)) / Vector3.Dot(plane.normal, vertexB - vertexA);
        Debug.Log(t);


        Vector3 newVertex = new Vector3();
        newVertex = vertexA * (1f - t) + vertexB * t;
        Debug.Log("( " + newVertex.x + " | " + newVertex.y + " | " + newVertex.z + " )");
        //Debug.Log(newVertex.z);

        GameObject obj = Instantiate<GameObject>(Pointer);
        obj.transform.position = newVertex;
        obj.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        return newVertex;
    }


    //Moller–Trumbore intersection algorithm
    public bool IsOnTheInside(Vector3 point)
    {




        return false;
    }

    


}


