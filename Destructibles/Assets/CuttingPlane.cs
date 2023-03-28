using UnityEngine;
using System.Collections.Generic;

public class CuttingPlane
{
    static float epsilon = 0.00001f;
    public Vector3 normal;
    public float w;

    enum Type
    {
        SamePlane = 0,
        Front = 1,
        Back = 2,
        Intersects = 3
    };

    public CuttingPlane(Vector3 a, Vector3 b, Vector3 c)
    {
        normal = Vector3.Cross(b - a, c - a);  //нормаль относительно а
        w = Vector3.Dot(normal, a); //сила вектора
    }


    public bool Valid()
    {
        return normal.magnitude > 0f;
    }


    public Vertex Mix( Vertex x, Vertex y, float weight)
    {
        float i = 1f - weight;
        Vertex v = new Vertex();
        v.position = x.position * i + y.position * weight;
        v.color = x.color * i + y.color * weight;
        v.normal = x.normal * i + y.normal * weight;
        v.tangent = x.tangent * i + y.tangent * weight;

        return v;
    }

    public void Flip() { normal *= -1f;      w *= -1f; }

    public void SplitPolygon(Polygon polygon, List<Polygon> coplanarFront, List<Polygon> coplanarBack, List<Polygon> front, List<Polygon> back)
    {
        

        Type polygonType = 0;
        List<Type> types = new List<Type>();

        for (int i = 0; i < polygon.vertices.Count; i++)
        {
            float t = Vector3.Dot(this.normal, polygon.vertices[i].position) - this.w;
            Type type = (t < -epsilon) ? Type.Back : ((t > epsilon) ? Type.Front : Type.SamePlane);
            polygonType |= type;
            types.Add(type);
        }

        switch (polygonType)
        {
            case Type.SamePlane:
                {
                    if (Vector3.Dot(this.normal, polygon.plane.normal) > 0) {
                        coplanarFront.Add(polygon);
                    }
                    else {
                        coplanarBack.Add(polygon);
                    }
                        
                }
                break;

            case Type.Front: {  front.Add(polygon);
                } break;

            case Type.Back: { back.Add(polygon);
                } break;

            case Type.Intersects:
                { 
                    List<Vertex> f = new List<Vertex>();
                    List<Vertex> b = new List<Vertex>();

                    for (int i = 0; i < polygon.vertices.Count; i++)  {
                        int j = (i + 1) % polygon.vertices.Count;

                        Type ti = types[i], tj = types[j];

                        Vertex vi = polygon.vertices[i], vj = polygon.vertices[j];


                        if (ti != Type.Back) {
                            f.Add(vi);
                        }


                        if (ti != Type.Front)   {
                            b.Add(vi);
                        }

                        if ((ti | tj) == Type.Intersects)
                        {

                            float t = (this.w - Vector3.Dot(this.normal, vi.position)) / Vector3.Dot(this.normal, vj.position - vi.position);
                            Vertex v = this.Mix(vi, vj, t);

                            f.Add(v);
                            b.Add(v);
                        }
                    }

                    // —обираем треугольники из точек на разных сторонах плоскости
                    if (f.Count >= 3)
                    {
                        front.Add(new Polygon(f)); //ƒобавл€ем новый полигон из точек спереди полигона
                    }

                    if (b.Count >= 3)
                    {
                        back.Add(new Polygon(b)); //и новый полигон из точек сзади 
                    }
                }
                break;      
        }   // End switch(polygonType)
    }

    public List<Polygon> SplitPolygonList (Polygon polygon)
    {
        List<Polygon> ReturnList = new List<Polygon>();
        Type polygonType = 0;
        List<Type> types = new List<Type>();

        for (int i = 0; i < polygon.vertices.Count; i++)
        {
            float t = Vector3.Dot(this.normal, polygon.vertices[i].position) - this.w;
            Type type = (t < -epsilon) ? Type.Back : ((t > epsilon) ? Type.Front : Type.SamePlane);
            polygonType |= type;
            types.Add(type);
        }

        switch (polygonType)
        {
            case Type.SamePlane:
                //ReturnList.Add(polygon);
                break;

            case Type.Front:
                break;

            case Type.Back:
                break;

            case Type.Intersects:
                {
                    List<Vertex> f = new List<Vertex>();
                    List<Vertex> b = new List<Vertex>();

                    for (int i = 0; i < polygon.vertices.Count; i++)
                    {
                        int j = (i + 1) % polygon.vertices.Count;

                        Type ti = types[i], tj = types[j];

                        Vertex vi = polygon.vertices[i], vj = polygon.vertices[j];


                        if (ti != Type.Back)
                        {
                            f.Add(vi);
                        }


                        if (ti != Type.Front)
                        {
                            b.Add(vi);
                        }

                        if ((ti | tj) == Type.Intersects)
                        {

                            float t = (this.w - Vector3.Dot(this.normal, vi.position)) / Vector3.Dot(this.normal, vj.position - vi.position);
                            Vertex v = this.Mix(vi, vj, t);

                            f.Add(v);
                            b.Add(v);
                        }
                    }

                    // —обираем треугольники из точек на разных сторонах плоскости
                    if (f.Count >= 3)
                    {
                        ReturnList.AddRange(new Polygon(f).BreakApart()); //ƒобавл€ем новый полигон из точек спереди полигона
                    }

                    if (b.Count >= 3)
                    {
                        ReturnList.AddRange(new Polygon(b).BreakApart()); //и новый полигон из точек сзади 
                    }
                }
                break;
        }   // End switch(polygonType)

        Debug.Log(ReturnList.Count);
        return ReturnList;
    }


}
