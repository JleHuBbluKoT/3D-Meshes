using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MollerThromblore
{
    static float epsilon = 0.00001f; //����� ��� ����-�� ������� ����� ����� �� ������
    public static bool rayIntersectsTriangle(Vector3 origin, Vector3 direction, Polygon polygon)
    {
        Vector3 vertex0 = polygon.vertices[0].position;
        Vector3 vertex1 = polygon.vertices[1].position;
        Vector3 vertex2 = polygon.vertices[2].position;

        Vector3 edge1 = vertex1 - vertex0;
        Vector3 edge2 = vertex2 - vertex0;
        Vector3 h = new Vector3();
        Vector3 s = new Vector3();
        Vector3 q = new Vector3();
        double a, Det, barU, barV ;


        h = Vector3.Cross(direction, edge2);
        a = Vector3.Dot(edge1, h); //������

        // ���������� ������������ - DROP
        if (a > -epsilon && a < epsilon) { Debug.Log("����������"); return false; }
        
        
        Det = 1.0f / a; 
        s = origin - vertex0;
        barU = Det * Vector3.Dot(s, h);
        if (barU < 0.0f || barU > 1.0f) { Debug.Log("������ 1"); return false; } // ��� ������������



        q = Vector3.Cross(s, edge1);
        barV = Det * Vector3.Dot(direction, q);
        if (barV < 0.0f || barU + barV > 1.0f) { Debug.Log("������ 2"); return false; } // ��� ������������


        double t = Det * Vector3.Dot(edge2, q); // ���������� �� ������ ���� �� ���������

        Debug.Log(t);
        if (t > epsilon) {
            Vector3 result = MixVertex(new Vertex(origin), new Vertex(direction), (float)t  ).position;
            Debug.Log(result);
            return true;
        } // ��� ������� �����������
        else { return false; } // ����� ��������� �����������, �� ��� �� �������


        
    }


    public static Vertex MixVertex(Vertex x, Vertex y, float weight)
    {
        float i = 1f - weight;
        Vertex v = new Vertex();
        v.position = x.position * i + y.position * weight;
        v.color = x.color * i + y.color * weight;
        v.normal = x.normal * i + y.normal * weight;
        v.tangent = x.tangent * i + y.tangent * weight;

        return v;
    }

}
