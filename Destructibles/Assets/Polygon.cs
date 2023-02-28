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

        public void Flip()
        {
            vertices.Reverse();
            for (int i = 0; i < vertices.Count; i++)
                vertices[i].Flip();
            plane.Flip();
        }

        public override string ToString()
        {
            return $"[{vertices.Count}] {plane.normal}";
        }
    }

