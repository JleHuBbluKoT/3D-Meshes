using System;
using UnityEngine;

public struct Vertex {
    public bool beginning;
    public Vector3 position;
    public Color color;
    public Vector3 normal;
    public Vector4 tangent;
    public Vector2 uv0;
    public Vector2 uv2;
    public Vector4 uv3;
    public Vector4 uv4;
    public void Flip()
        {
            normal *= -1f;
            tangent *= -1f;
        }

        public Vertex(Vector3 _position)
        {
        beginning = false;
        position = _position;
        color = Color.black;
        normal = new Vector3(0, 0, 0);
        tangent = new Vector4(0, 0, 0, 0);
        uv0 = Vector2.zero;
        uv2 = Vector2.zero;
        uv3 = Vector4.zero;
        uv4 = Vector4.zero;

    }

    public override string ToString()
    {
        return $"XYZ: [{this.position}] Color: {this.color} Normal {normal} tangent {tangent}";
    }


}



