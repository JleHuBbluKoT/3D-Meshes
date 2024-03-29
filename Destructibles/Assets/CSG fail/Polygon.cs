using UnityEngine;
using System.Collections.Generic;

public sealed class Polygon
{
    public List<Vertex> vertices;
    public CuttingPlane plane;
    public Material material;

    public Polygon(List<Vertex> list, Material mat)
    {
        vertices = list;
        plane = new CuttingPlane(list[0].position, list[1].position, list[2].position);
        material = mat;
    }

    public Polygon(List<Vector3> list)
    {
        vertices = new List<Vertex>();
        for (int i = 0; i < list.Count; i += 1)
        {
            this.vertices.Add(new Vertex(list[i]));
        }
        plane = new CuttingPlane(list[0], list[1], list[2]);
    }


    public Polygon(Vector3 v1, Vector3 v2, Vector3 v3)
    {
    vertices = new List<Vertex>{ new Vertex(v1), new Vertex(v2), new Vertex(v3) };
    plane = new CuttingPlane(vertices[0].position, vertices[1].position, vertices[2].position);
    }

    public Polygon(Vertex v1, Vertex v2, Vertex v3, Material mat = null)
    {
        List<Vertex> H = new List<Vertex>();
        H.Add(v1);
        H.Add(v2);
        H.Add(v3);
        vertices = H;
        plane = new CuttingPlane(v1.position, v2.position, v3.position);
        material = mat;
    }


    public void Flip()
    {
        vertices.Reverse();
        for (int i = 0; i < vertices.Count; i++)
            vertices[i].Flip();
        plane.Flip();
    }

    public override string ToString()
{
    string vert = "";
    foreach (var vertice in this.vertices)
    {
        vert = vert + vertice.position.ToString();
    }
    return $"[{vertices.Count}] Vertices: {vert}";
}
// ��������� ������� �� ����� ���� ����� �� ����������� ����������� || ���� ��� ����������� �� ������ ���������� ���� � ������
// �������� � ������ ���� ��� ������� �������� ��������
public List<Polygon> BreakApart() 
{
    List<Polygon> smallPoly = new List<Polygon>();

    for (int i = 2; i < this.vertices.Count; i++)
    {
        List<Vertex> H = new List<Vertex>();
        H.Add(vertices[0]);
        H.Add(vertices[i - 1]);
        H.Add(vertices[i]);
        smallPoly.Add(new Polygon(H, this.material));
    }

    return smallPoly;
}


}

