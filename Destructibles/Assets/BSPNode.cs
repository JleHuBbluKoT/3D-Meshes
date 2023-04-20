using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPNode
{
    public List<Polygon> Polys;

    public BSPNode front;
    public BSPNode back;

    public CuttingPlane plane;


    public BSPNode()
    {
        front = null;
        back = null;
    }

    public static List<Polygon> ModelToPolygons(GameObject thing)
    { // ��� ������������� �������� ������� � ������ ���������
        List<Polygon> finalPolys = new List<Polygon>();
        int[] GATriangles = thing.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] GAVertices = thing.GetComponent<MeshFilter>().mesh.vertices;
        Vector3 GAposition = thing.transform.position;
        for (int i = 0; i < thing.GetComponent<MeshFilter>().mesh.triangles.Length / 3; i++)
        {
            Vector3 item1 = GAVertices[GATriangles[i * 3 + 0]] + GAposition;
            Vector3 item2 = GAVertices[GATriangles[i * 3 + 1]] + GAposition;
            Vector3 item3 = GAVertices[GATriangles[i * 3 + 2]] + GAposition;
            //Debug.Log(item1 + " | " + item2 + " | " + item3);
            finalPolys.Add(new Polygon(item1, item2, item3));
        }
        return finalPolys;
    }
    public BSPNode(GameObject thing )
    { // ���������� ������ � ����
        this.Build(ModelToPolygons(thing), "start", 0);
    }
    public BSPNode(List<Polygon> _list)
    {
        this.Build(_list, "start", 0);
    }
    public BSPNode(List<Polygon> list, CuttingPlane plane, BSPNode front, BSPNode back) {
        this.Polys = list;
        this.plane = plane;
        this.front = front;
        this.back = back;
    }
    // �������, ���������� ������� �������� ���� � �����.
    // ������ ������� ����� ������ ��� �������� �� ��� ������: ������� � �����.
    // ��� �� ������������, � ������ ����� ���� �������� ������ �� �����. 
    // ����� � ��������� ���� ������ ������ ������, ������� ����� ������� �� ���
    // ����� ���������� ���-�� ����� BSP ������

    public void Build(List<Polygon> listPoly, string dir, int depth)
    {
        //Debug.Log(dir + " " + depth);
        if (listPoly.Count < 1 || depth > 100)  return; // �������� �� �� ��� ��� ������� ��� ������ � �� �� ��� ������� ����������

        if (Polys == null)
        {
            Polys = new List<Polygon>();
        }
        
        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();

        // �������� ����� ������ ������� � ������, �� ����� ����������� �������, �� � ���� �� ����� ������
        if (this.plane == null || !(this.plane.normal.magnitude > 0))
        {
            this.plane = new CuttingPlane(listPoly[0].plane.normal, listPoly[0].plane.w);
        }

        for (int i = 0; i < listPoly.Count; i++)
            this.plane.SplitPolygon(listPoly[i], Polys, Polys, listFront, listBack); // ������ ��� Polys ���������� �������� � ���� �� ���������
        //Debug.Log(listFront.Count + " " + listBack.Count);

        // ��� �������� ������� ��������� ������� ������������� ������� ������ ���� �� �����
        // ��� �������� ������� ��������� �� ���� ���������� ����� ������ ������� ��� ������
        if (listFront.Count > 0)  {
            front = new BSPNode();
            front.Build(listFront, "front", depth+1);
        }

        if (listBack.Count > 0) {
            back = new BSPNode();
            back.Build(listBack, "back", depth + 1);
        }

    }

    // ��������� ��� ������ ���� ���� ������ �������� ������ ��������
    public List<Polygon> DeleteInsides(List<Polygon> otherPoly, string dir, int depth)
    {
        Debug.Log(dir + " " + depth);
        if (this.plane == null || !(this.plane.normal.magnitude > 0)) {
            return otherPoly;
        }

        // ��������� ����� �������� �� ���� ����
        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();
        for (int i = 0; i < otherPoly.Count; i++)
            this.plane.SplitPolygon(otherPoly[i], listFront, listBack, listFront, listBack);
        // ������� ��������� � ���� front � back, ���������� ������ DeleteInsides
        Debug.Log(listFront.Count + " " + listBack.Count);
        // �����
        if (this.front != null)  {
            listFront = this.front.DeleteInsides(listFront, "front", depth + 1);
        }

        // ���. 
        if (this.back != null)   {
            listBack = this.back.DeleteInsides(listBack, "back", depth + 1);
        }
        else { // ���� � �������� �������� ���� ������ ����, ������ ��� �������� ��������� ������ � ��������. 
            listBack.Clear();
        }

        listFront.AddRange(listBack);

        // ������� �������� �� ����������� ������ ����� ������
        return listFront;
    }



}


// ������� � ������ ��������� ��� ������ ��� �������� 1
// ����� � ������ �������� �������� �������� 2 ��������� �������� �� ����� ��� ������
// ����� ���� ����������� ��������� ��� ��� ��������� ��� ��� ���������� ��������� �������
