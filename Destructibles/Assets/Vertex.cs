using System;
using UnityEngine;

public struct Vertex {
        public Vector3 position;
        public Color color;
        public Vector3 normal;
        public Vector4 tangent;

        public void Flip()
        {
            normal *= -1f;
            tangent *= -1f;
        }

        public Vertex(Vector3 _position)
        {

        position = _position;
        color = Color.black;
        normal = new Vector3(0, 0, 0);
        tangent = new Vector4(0, 0, 0, 0);

        }
    }



