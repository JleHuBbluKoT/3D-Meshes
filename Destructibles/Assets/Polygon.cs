using UnityEngine;
using System.Collections.Generic;

    public sealed class Polygon
    {
        public List<Vertex> vertices;
        public CuttingPlane plane;
        //public Material material;

        public Polygon(List<Vertex> list /* , Material mat*/)
        {
            vertices = list;
            plane = new CuttingPlane(list[0].position, list[1].position, list[2].position);
            //material = mat;
        }

        public Polygon(Vector3 v1, Vector3 v2, Vector3 v3)
        {
        List<Vertex> H = new List<Vertex>();
        H.Add(new Vertex(v1));
        H.Add(new Vertex(v2));
        H.Add(new Vertex(v3));
        vertices = H;
        plane = new CuttingPlane(vertices[0].position, vertices[1].position, vertices[2].position);
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
    // Разбивает полигон из более трех точек на треугольные полигончики || Если уже треугольный то просто возвращает себя в списке
    // Работает с учетом того что полигон является выпуклым
    public List<Polygon> BreakApart() 
    {
        List<Polygon> smallPoly = new List<Polygon>();

        for (int i = 2; i < this.vertices.Count; i++)
        {
            List<Vertex> H = new List<Vertex>();
            H.Add(vertices[0]);
            H.Add(vertices[i - 1]);
            H.Add(vertices[i]);
            smallPoly.Add(new Polygon(H));
        }

        return smallPoly;
    }


    }

