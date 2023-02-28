using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MollerThromblore
{
    static float epsilon = 0.00001f; //нужно его куда-то сложить чтобы везде не писать
    public static bool rayIntersectsTriangle(Vector3 origin, Vector3 direction, Polygon polygon)
    {
        Vector3 vertex0 = polygon.vertices[0].position;
        Vector3 vertex1 = polygon.vertices[1].position;
        Vector3 vertex2 = polygon.vertices[2].position;

        Vector3 edge1 = new Vector3(); // v1, v0
        Vector3 edge2 = new Vector3(); // v2, v0
        Vector3 h = new Vector3();
        Vector3 s = new Vector3();
        Vector3 q = new Vector3();
        double a, f, u, v ;

        edge1 = vertex1 - vertex0;
        edge2 = vertex2 - vertex0;

        h = Vector3.Cross(origin, edge2);
        a = Vector3.Dot(h, origin);

        // ѕј–јЋЋ≈Ћ≈Ќ “–≈”√ќЋ№Ќ» ”
        if (a > -epsilon && a < epsilon) { return false; }

        f = 1.0f / a;
        s = origin - vertex0;
        u = f * Vector3.Dot(s, h);
        if (u < 0.0f || u > 1.0f) { return false;  }
        // —ћ≈–“№


        q = Vector3.Cross(s, edge1);
        v = f * Vector3.Dot(direction, q);
        if (v < 0.0f || u + v > 1.0f)  {  return false; }
        // —ћ≈–“№

        double t = f * Vector3.Dot(edge2, q);

        if (t > epsilon) { return true;  } // Ћуч пересек треугольник
        else { return false; } // Ћини€ пересекла треугольник, но луч не пересек


        
    }


}
