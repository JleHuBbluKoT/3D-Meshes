using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SphereToAsteroid
{ // yet another futile attempt to produce something that resembles an asteroid
    //public MeshFilter sphere;

    public Mesh Cube()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3> { 
            new Vector3(0, 0, 0), new Vector3(1, 0, 0), 
            new Vector3(1, 1, 0), new Vector3(0, 1, 0), 
            new Vector3(0, 0, 1), new Vector3(1, 0, 1), 
            new Vector3(1, 1, 1), new Vector3(0, 1, 1) 
        };

        List<int> triangles = new List<int>
        {
            2,1,0, 3,2,0,
            6,5,1, 2,6,1,
            6,7,4, 5,6,4,
            4,7,3, 0,4,3,
            7,6,2, 3,7,2,
            5,4,0, 1,5,0
        };
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        return mesh;
    }

    public Mesh icosahedron(int subdivisions)
    {
        Mesh mesh = new Mesh();
        float phi = (1.0f + Mathf.Sqrt(5.0f)) * 0.5f;
        float a = 1.0f;
        float b = 1.0f / phi;

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add( new Vector3(0, b, -a) );
        vertices.Add( new Vector3(b, a, 0));
        vertices.Add( new Vector3(-b, a, 0));
        vertices.Add(new Vector3(0, b, a));

        vertices.Add(new Vector3(0, -b, a));
        vertices.Add(new Vector3(-a, 0, b));
        vertices.Add(new Vector3(0, -b, -a));
        vertices.Add(new Vector3(a, 0, -b));

        vertices.Add(new Vector3(a, 0, b));
        vertices.Add(new Vector3(-a, 0, -b));
        vertices.Add(new Vector3(b, -a, 0));
        vertices.Add(new Vector3(-b, -a, 0));

        mesh.SetVertices(vertices);
        

        List<int> triangles1 = new List<int> { 2, 1, 0, 1, 2, 3, 5, 4, 3, 4, 8, 3};
        List<int> triangles2 = new List<int> { 7, 6, 0, 6, 9, 0, 11, 10, 4, 10, 11, 6 };
        List<int> triangles3 = new List<int> { 9, 5, 2, 5, 9, 11, 8, 7, 1, 7, 8, 10 };
        List<int> triangles4 = new List<int> { 2, 5, 3, 8, 1, 3, 9, 2, 0, 1, 7, 0 };
        List<int> triangles5 = new List<int> { 11, 9, 6, 7, 10, 6, 5, 11, 4, 10, 8, 4 };
        triangles1.AddRange(triangles2);
        triangles1.AddRange(triangles3);
        triangles1.AddRange(triangles4);
        triangles1.AddRange(triangles5);
        mesh.SetTriangles(triangles1, 0);
        SubdivideTriangleToFour(mesh, subdivisions);
        projectUnitToSphere(mesh);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }



    public void projectUnitToSphere(Mesh inputMesh) {
        List<Vector3> vertices = new List<Vector3>();
        inputMesh.GetVertices(vertices);
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vertices[i].normalized / 2;
        }
        inputMesh.SetVertices(vertices);
    }   

    public void SubdivideTriangleToFour(Mesh inputmesh, int depth)
    {
        if (depth == 0)
        {
            return;
        }
        depth--;
        if (depth > 0) {
            SubdivideTriangleToFour(inputmesh, depth);
        }

        List<Vector3> vertices = new List<Vector3>(); inputmesh.GetVertices(vertices);
        List<int> triangles = new List<int>(); inputmesh.GetTriangles(triangles, 0);
        List<Vector3> astVert = new List<Vector3>();
        List<AstroPoly> astPoly = new List<AstroPoly>();

        for (int i = 0; i < triangles.Count; i += 3)
        {
            Vector3 vert1 = vertices[ triangles[i]];
            Vector3 vert3 = vertices[triangles[i + 1]];
            Vector3 vert5 = vertices[triangles[i + 2]];

            Vector3 vert2 = Halfpoint(vert1, vert3);
            Vector3 vert4 = Halfpoint(vert3, vert5);
            Vector3 vert6 = Halfpoint(vert1, vert5);

            astPoly.Add( new AstroPoly(vert1, vert2, vert6) );
            astPoly.Add(new AstroPoly(vert2, vert3, vert4));
            astPoly.Add(new AstroPoly(vert2, vert4, vert6));
            astPoly.Add(new AstroPoly(vert6, vert4, vert5));

        }

        foreach (var poly in astPoly)
        {
            astVert.Add(poly.vertices[0]);
            astVert.Add(poly.vertices[1]);
            astVert.Add(poly.vertices[2]);
        }

        astVert = astVert.Distinct().ToList();
        List<int> newTriangles = new List<int>();

        foreach (var poly in astPoly)
        {
            newTriangles.Add(astVert.IndexOf(poly.vertices[0]));
            newTriangles.Add(astVert.IndexOf(poly.vertices[1]));
            newTriangles.Add(astVert.IndexOf(poly.vertices[2]));
        }

        inputmesh.SetVertices(astVert);
        inputmesh.SetTriangles(newTriangles, 0);

        List<Vector2> uv0 = new List<Vector2>();
        //List<Vector3> uv2 = new List<Vector3>();

        foreach (var vert in astVert)
        {
            // THIS IS THE DUMBEST SOLUTION BUT IT WORKS
            uv0.Add(new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
            //uv2.Add( (new Vector3(1 + vert.x, 1+  vert.y,0)) * 0.5f   );
        }

        inputmesh.SetUVs(0, uv0);


    }

    public Vector3 Halfpoint(Vector3 a, Vector3 b)
    {
        return (a + b) / 2;
    }

    public void Resize(Mesh mesh, Vector3 scale)
    {
        List<Vector3> points = new List<Vector3>(mesh.vertices);
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new Vector3( points[i].x * scale.x, points[i].y * scale.y, points[i].z * scale.z );
        }
        mesh.SetVertices(points);
    }

    public void Move(Mesh mesh, Vector3 scale)
    {
        List<Vector3> points = new List<Vector3>(mesh.vertices);
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new Vector3(points[i].x + scale.x, points[i].y + scale.y, points[i].z + scale.z);
        }
        mesh.SetVertices(points);
    }


    public void DistortSphere(Mesh sphere, float vx, float vy, float vz, int numberOfPoints = 10, float intensity = 1f)
    {

        List<Vector3> points = new List<Vector3>();

        int failSafe = 0;
        while (points.Count < numberOfPoints & failSafe < 100)
        {
            failSafe++;
            float x = Random.Range(-vx, vx);
            float y = Random.Range(-vy, vy);
            float z = Random.Range(-vz, vz);
            if (Distance(new Vector3(x,y,z), Vector3.zero) > 0.2f)
            {
                points.Add(new Vector3(x,y,z));
            }
            
        }


        List<Vector3> vertices = new List<Vector3>();
        sphere.GetVertices(vertices);

        for (int i = 0; i < vertices.Count; i++)
        {
            float minDist = points.Min(point => Distance( point, vertices[i] ) );
            vertices[i] = vertices[i] * (1 + minDist * intensity);
        }

        sphere.SetVertices(vertices);
        sphere.RecalculateTangents();
        sphere.RecalculateNormals();
    }

    

    float Distance(Vector3 one, Vector3 two) {
        return Mathf.Pow(Mathf.Pow(one.x - two.x, 2f) + Mathf.Pow(one.y - two.y, 2f) + Mathf.Pow(one.z - two.z, 2f), 0.5f);
    }

}
class AstroPoly {
    public Vector3[] vertices = new Vector3[3];
    public AstroPoly(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        vertices[0] = v1;
        vertices[1] = v2;
        vertices[2] = v3;
    }
}