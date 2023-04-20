using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutting : MonoBehaviour
{
    public GameObject TheThingie;
    public GameObject Pointer;
    public GameObject GA;
    public GameObject GB;
    // Start is called before the first frame update
    void Start()
    {
        //Thingy();
        BSPTreetest();
    }

    static float epsilon = 0.00001f;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BSPTreetest()
    {
        List<Polygon> GBPolys = new List<Polygon>();
        int[] GBTriangles = GB.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] GBVertices = GB.GetComponent<MeshFilter>().mesh.vertices;
        Vector3 GBposition = GB.transform.position;
        for (int i = 0; i < GB.GetComponent<MeshFilter>().mesh.triangles.Length / 3; i++)
        {
            Vector3 item1 = GBVertices[GBTriangles[i * 3 + 0]] + GBposition;
            Vector3 item2 = GBVertices[GBTriangles[i * 3 + 1]] + GBposition;
            Vector3 item3 = GBVertices[GBTriangles[i * 3 + 2]] + GBposition;
            //Debug.Log(item1 + " | " + item2 + " | " + item3);
            GBPolys.Add(new Polygon(item1, item2, item3));
        }
        //Debug.Log(GBPolys.Count);

        BSPNode testCubeA = new BSPNode(GA);

        List<Polygon> NewB =  testCubeA.DeleteInsides(GBPolys, "start", 0);

        List<Polygon> NewOnes = new List<Polygon>();
        for (int i = 0; i < NewB.Count; i++)
        {
            if (NewB[i].vertices.Count > 3)
            {
                List<Polygon>  tmp = NewB[i].BreakApart();
                NewB[i] = tmp[0];
                tmp.RemoveAt(0);
                NewOnes.AddRange(tmp);
            }
        }

        NewB.AddRange(NewOnes);


        foreach (var item in NewB)
        {
            RenderPolyPoly(item);
        }
    }

    public void First3DCheck()
    {
        List<Polygon> GAPolys = new List<Polygon>();
        List<Polygon> GBPolys = new List<Polygon>();

        List<Polygon> NewPolys = new List<Polygon>();

        int[] GATriangles = GA.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] GAVertices = GA.GetComponent<MeshFilter>().mesh.vertices;
        Vector3 GAposition = GA.transform.position;
        for (int i = 0; i < GA.GetComponent<MeshFilter>().mesh.triangles.Length / 3; i++)
        {
            Vector3 item1 = GAVertices[GATriangles[i * 3 + 0]] + GAposition;
            Vector3 item2 = GAVertices[GATriangles[i * 3 + 1]] + GAposition;
            Vector3 item3 = GAVertices[GATriangles[i * 3 + 2]] + GAposition;
            Debug.Log(item1 + " | " + item2 + " | " + item3);
            GAPolys.Add(new Polygon(item1, item2, item3));
        }

        int[] GBTriangles = GB.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] GBVertices = GB.GetComponent<MeshFilter>().mesh.vertices;
        Vector3 GBposition = GB.transform.position;
        for (int i = 0; i < GB.GetComponent<MeshFilter>().mesh.triangles.Length / 3; i++)
        {
            Vector3 item1 = GBVertices[GBTriangles[i * 3 + 0]] + GBposition;
            Vector3 item2 = GBVertices[GBTriangles[i * 3 + 1]] + GBposition;
            Vector3 item3 = GBVertices[GBTriangles[i * 3 + 2]] + GBposition;
            Debug.Log(item1 + " | " + item2 + " | " + item3);
            GBPolys.Add(new Polygon(item1, item2, item3));
        }
        
        for (int a = 0; a < GAPolys.Count; a++)
        {
            for (int b = 0; b < GBPolys.Count; b++)
            {
                NewPolys.AddRange(GAPolys[a].plane.SplitPolygonList(GBPolys[b]) );
              
            }
        }

        foreach (var item in NewPolys)
        {
            RenderPolyPoly(item);
        }

    }

    public void Moller()
    {
        Vector3 offset = new Vector3(0,0,0);
        Vector3 vertex1 = new Vector3(-1, 0, -1) + offset;
        Vector3 vertex2 = new Vector3(0, 3, 1) + offset;
        Vector3 vertex3 = new Vector3(1, 0, 0) + offset;

        Polygon Poly1 = new Polygon(vertex1, vertex2, vertex3);

        Vector3 vertex4 = new Vector3(-1, 1, -1);
        Vector3 vertex5 = new Vector3(0, 1, 1);
        Vector3 vertex6 = new Vector3(1, 1, 0);

        Polygon Poly2 = new Polygon(vertex4, vertex5, vertex6);

        List<Polygon> newOne = new List<Polygon>();
        List<Polygon> newTwo = new List<Polygon>();

        //Poly1.plane.SplitPolygon(Poly2, newOne, newTwo, newOne, newTwo);

        newOne.AddRange( Poly1.plane.SplitPolygonList(Poly2) );


        Debug.Log(newOne.Count);
        for (int i = 0; i < newOne.Count; i++) {
            Debug.Log(newOne[i].ToString());
            if (newOne[i].vertices.Count > 3) {
                newOne.AddRange(newOne[i].BreakApart()); // Splits i element into smaller polygons and returns them
                newOne.RemoveAt(i);
            }
        }


        


        
        foreach (var item in newOne)
        {
            RenderPolyVec(item.vertices[0].position, item.vertices[1].position, item.vertices[2].position);
        }

        //Îòðèñîâêà

        RenderPolyVec(vertex1, vertex2, vertex3);
        RenderPolyVec(vertex4, vertex5, vertex6);


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
        Vector3 vertex1 = new Vector3(-1, 2, -1) + offset;
        Vector3 vertex2 = new Vector3(0, 2, 1) + offset;
        Vector3 vertex3 = new Vector3(1, 2, 0) + offset;


        Polygon NewPoly = new Polygon(vertex1, vertex2, vertex3);

        CuttingPlane middlePlane = new CuttingPlane(vertex1, vertex2, vertex3);
        Vector3 pA = new Vector3(0, 0, 0);
        Vector3 pB = new Vector3(0, 1, 0);

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
        //Debug.Log(t1 + " " + t2 + " " + type1 + " " + type2 + " " + Ftype);

        float t = (plane.w - Vector3.Dot(plane.normal, vertexA)) / Vector3.Dot(plane.normal, vertexB - vertexA);
        //Debug.Log(t);


        Vector3 newVertex = new Vector3();
        newVertex = vertexA * (1f - t) + vertexB * t;
        //Debug.Log("( " + newVertex.x + " | " + newVertex.y + " | " + newVertex.z + " )");
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


    public void RenderPolyVec(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3 )
    {
        GameObject meshA = TheThingie;
        meshA.transform.position = new Vector3(0, 0, 0);
        Mesh TriA = new Mesh(); Vector3[] verticesA = new Vector3[] { vertex1, vertex2, vertex3 };
        TriA.vertices = verticesA; TriA.triangles = new int[] { 0, 1, 2 };
        meshA.GetComponent<MeshFilter>().mesh = TriA;
        Instantiate<GameObject>(meshA);
    }

    public void RenderPolyPoly(Polygon Poly /*, Vector3 Precursor*/)
    {
        GameObject meshA = TheThingie;
        meshA.transform.position = new Vector3(0, 0, 0);
        Mesh TriA = new Mesh(); Vector3[] verticesA = new Vector3[] { Poly.vertices[0].position, Poly.vertices[1].position, Poly.vertices[2].position };
        TriA.vertices = verticesA; TriA.triangles = new int[] { 0, 1, 2 };
        meshA.GetComponent<MeshFilter>().mesh = TriA;
        GameObject thingamagic = Instantiate<GameObject>(meshA);
        //thingamagic.transform.position = Precursor;
    }
}




