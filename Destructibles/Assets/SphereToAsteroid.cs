using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SphereToAsteroid : MonoBehaviour
{ // yet another futile attempt to produce something that resembles an asteroid
    public MeshFilter sphere;

    void Start()
    {
        List<Vector3> points = new List<Vector3>();

        int failSafe = 0;
        while (points.Count < 13 & failSafe < 100)
        {
            failSafe++;
            float x = Random.Range(-0.6f, 0.6f);
            float y = Random.Range(-0.6f, 0.6f);
            float z = Random.Range(-0.6f, 0.6f);
            if (Distance(new Vector3(x,y,z), Vector3.zero) > 0.2f)
            {
                points.Add(new Vector3(x,y,z));
            }
            
        }


        List<Vector3> vertices = new List<Vector3>();
        sphere.mesh.GetVertices(vertices);

        for (int i = 0; i < vertices.Count; i++)
        {
            float minDist = points.Min(point => Distance( point, vertices[i] ) );
            vertices[i] = vertices[i] * (1 + minDist * 3f);
        }

        sphere.mesh.SetVertices(vertices);
        sphere.mesh.RecalculateTangents();
        sphere.mesh.RecalculateNormals();
    }

    

    float Distance(Vector3 one, Vector3 two) {
        return Mathf.Pow(Mathf.Pow(one.x - two.x, 2f) + Mathf.Pow(one.y - two.y, 2f) + Mathf.Pow(one.z - two.z, 2f), 0.5f);
    }

}
