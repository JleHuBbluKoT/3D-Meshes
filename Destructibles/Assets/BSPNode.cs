using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public BSPNode(GameObject thing )
    { // Превращает объект в ноду
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

    // Функция, рекурсивно делящая полигоны друг о друга.
    // Каждое деление делит вообще все полигоны на две группы: Спереди и Сзади.
    // Они не пересекаются, а значит между ними проверки делать не нужно. 
    // Смысл в проверках есть только внутри группы, которая также делится на две
    // Итого получается что-то вроде BSP дерева
    // Эту функцию можно использовать также для добавления новых полигонов в уже существующее дерево
    public void Build(List<Polygon> listPoly, string dir, int depth)
    {
        //Debug.Log(dir + " " + depth);
        if (listPoly.Count < 1)  return; // Проверка на то что это полигон без ошибок и на то что полигон существует

        if (Polys == null)
        {
            Polys = new List<Polygon>();
        }
        
        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();

        // Выбирает самый первый полигон в списке, не самый эффективный вариант, но с чего то нужно начать
        if (this.plane == null || !(this.plane.normal.magnitude > 0))
        {
            this.plane = new CuttingPlane(listPoly[0].plane.normal, listPoly[0].plane.w);
        }

        for (int i = 0; i < listPoly.Count; i++)
            this.plane.SplitPolygon(listPoly[i], Polys, Polys, listFront, listBack); // первые два Polys возвращают полигоны в этой же плоскости
        //Debug.Log(listFront.Count + " " + listBack.Count);

        // Все полигоны которые оказались спереди отправляютсяс строить дерево друг об друга
        // Все полигоны которые оказались за этой плоскостью также начнут строить БСП дерево
        if (listFront.Count > 0)  {
            front = new BSPNode();
            front.Build(listFront, "front", depth+1);
        }

        if (listBack.Count > 0) {
            back = new BSPNode();
            back.Build(listBack, "back", depth + 1);
        }

    }

    // Используя БСП дерево этой ноды делить полигоны другой модельки
    public List<Polygon> DeleteInsides(List<Polygon> otherPoly, string dir = "start", int depth = 0)
    {
        //Debug.Log(dir + " " + depth);
        if (this.plane == null || !(this.plane.normal.magnitude > 0)) {
            return otherPoly;
        }

        // Разделить чужую модельку от этой ноды
        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();
        for (int i = 0; i < otherPoly.Count; i++)
            this.plane.SplitPolygon(otherPoly[i], listFront, listBack, listFront, listBack);
        // Результ отправить в ноды front и back, рекурсивно вызвав DeleteInsides
        //Debug.Log(listFront.Count + " " + listBack.Count);
        // Перед
        if (this.front != null)  {
            listFront = this.front.DeleteInsides(listFront, "front", depth + 1);
        }

        // Зад. 
        if (this.back != null)   {
            listBack = this.back.DeleteInsides(listBack, "back", depth + 1);
        }
        else { // Если у основной модельки нету задней ноды, значит эти полигоны находятся внутри и удалятся. 
            listBack.Clear();
        }

        listFront.AddRange(listBack);

        // Вернуть полигоны не находящиеся внутри этого дерева
        return listFront;
    }


    // Рекурсивно переворачивает все полигоны
    // Flip() в комбинации с DeletInsides() позволит мне удалять все полигоны ВНЕ модельки, что нужно для некоторых операций
    public void Flip()
    {
        //foreach (var poly in this.Polys)   {  poly.Flip(); }
        this.plane.Flip();

        if (this.front != null)
            this.front.Flip();
        if (this.back != null)
            this.back.Flip();

        BSPNode tmp = this.back;
        this.back = this.front;
        this.front = tmp;
    }

    public List<Polygon> ToPolygons()
    {
        List<Polygon> ret = this.Polys;

        if (this.front != null)
            ret.AddRange(this.front.ToPolygons());
        if (this.back != null)
            ret.AddRange(this.back.ToPolygons());
        return ret;
    }

    public BSPNode CopyNode()
    {
        BSPNode copy = new BSPNode(this.Polys, this.plane, this.front, this.back);
        return copy;
    }

    public enum Operation {
        Union,
        Substract,
        Intersect,
    };
    public static Mesh Interface(Operation operation, GameObject A, GameObject B)
    {
        List<Polygon> tmp = new List<Polygon>();
        switch (operation){
            case Operation.Union:
                tmp = Union(ModelToPolygons(A), ModelToPolygons(B)); break;
            case Operation.Substract:
                tmp = Substract(ModelToPolygons(A), ModelToPolygons(B)); break;
            case Operation.Intersect:
                tmp = Intersect(ModelToPolygons(A), ModelToPolygons(B)); break;
            default:
                break;
        }

        Vector3 origin = A.transform.position;

        return ReturnMesh(tmp, origin);
    }

    public static List<Polygon> Union(List<Polygon> polyA, List<Polygon> polyB)
    {
        BSPNode a = new BSPNode(polyA);
        BSPNode b = new BSPNode(polyB);

        List<Polygon> Half1 =  a.DeleteInsides(polyB);
        List<Polygon> Half2 = b.DeleteInsides(polyA);

        Half1.AddRange(Half2);

        return Half1;
    }

    public static List<Polygon> Substract(List<Polygon> polyA, List<Polygon> polyB)
    {
        BSPNode a = new BSPNode(polyA);
        BSPNode b = new BSPNode(polyB);

        List<Polygon> Half1 = b.DeleteInsides(polyA);

        a.Flip();

        List<Polygon> Half2 = a.DeleteInsides(polyB);

        foreach (var poly in Half2) {  poly.Flip();  }

        Half1.AddRange(Half2);

        return Half1;
    }

    public static List<Polygon> Intersect(List<Polygon> polyA, List<Polygon> polyB)
    {
        BSPNode a = new BSPNode(polyA);
        BSPNode b = new BSPNode(polyB);

        b.Flip();
        List<Polygon> Half1 = b.DeleteInsides(polyA);


        a.Flip();
        List<Polygon> Half2 = a.DeleteInsides(polyB);


        Half1.AddRange(Half2);

        return Half1;
    }


    public static Mesh ReturnMesh(List<Polygon> polys, Vector3 origin)
    {
        Mesh TriA = new Mesh();
        List<Vector3> HashSet = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Polygon> NewOnes = new List<Polygon>();
        for (int i = 0; i < polys.Count; i++)
        {
            if (polys[i].vertices.Count > 3)
            {
                List<Polygon> tmp = polys[i].BreakApart();
                polys[i] = tmp[0];
                tmp.RemoveAt(0);
                NewOnes.AddRange(tmp);
            }
        }
        polys.AddRange(NewOnes);
// Vetices ===============================================================================
        List<Vertex> notUnique = new List<Vertex>();
        foreach (var poly in polys)
        {
            notUnique.AddRange(poly.vertices);
        }
        List<Vertex> UniqueVertices = notUnique.Distinct().ToList();
        List<Vector3> vectorVertice = new List<Vector3>();
        foreach (var vertex in UniqueVertices)
        {
            vectorVertice.Add(vertex.position);
        }
        //Debug.Log(notUnique.Count + " " + UniqueVertices.Count);
// Triangles ===============================================================================
        foreach (var poly in polys)
        {
            //string b = "";
            for (int i = 0; i < poly.vertices.Count; i++)
            {
                int a = UniqueVertices.IndexOf(poly.vertices[i]);
                //b = b + " " + a;
                triangles.Add(a);
            }
            //Debug.Log(b);
        }

        for (int i = 0; i < vectorVertice.Count; i++)
        {
            vectorVertice[i] -= origin;
        } 

        TriA.vertices = vectorVertice.ToArray();
        TriA.triangles = triangles.ToArray();
        
        return TriA;
    }

    public static Vertex[] GetVertices(Mesh mesh, Vector3 origin)
    {
        int vcount = mesh.vertices.Count();
        Vector3[] positions = mesh.vertices;
        Color[] colors = mesh.colors;
        Vector3[] normals = mesh.normals;
        Vector4[] tangents = mesh.tangents;
        Vector2[] uv0s = mesh.uv;
        Vector2[] uv2s = mesh.uv2;
        List<Vector4> uv3s = new List<Vector4>();
        List<Vector4> uv4s = new List<Vector4>();
        mesh.GetUVs(2, uv3s);
        mesh.GetUVs(3, uv4s);
/*Debug.Log(positions.Length);Debug.Log(colors.Length);Debug.Log(normals.Length);Debug.Log(tangents.Length);Debug.Log(uv0s.Length);Debug.Log(uv2s.Length);Debug.Log(uv3s.Count);Debug.Log(uv4s.Count);*/
        Vertex[] v = new Vertex[vcount];

        for (int i = 0; i < v.Length; i++)
        {
            if (positions.Length== vcount)  v[i].position = positions[i] + origin;
            if (colors.Length   == vcount)  v[i].color    = colors[i];
            if (normals.Length  == vcount)  v[i].normal   = normals[i];
            if (tangents.Length == vcount)  v[i].tangent  = tangents[i];
            if (uv0s.Length     == vcount)  v[i].uv0      = uv0s[i];
            if (uv2s.Length     == vcount)  v[i].uv2      = uv2s[i];
            if (uv3s.Count      == vcount)  v[i].uv3      = uv3s[i];
            if (uv4s.Count      == vcount)  v[i].uv4      = uv4s[i];
        }

        return v;
    }

    public static List<Polygon> ModelToPolygons(GameObject thing)
    { // Для трансформации модельки сначала в список полигонов
        List<Polygon> finalPolys = new List<Polygon>();
        int[] GATriangles = thing.GetComponent<MeshFilter>().mesh.triangles;

        Vertex[] vertices = GetVertices(thing.GetComponent<MeshFilter>().mesh, thing.transform.position);

        Material[] Materials = thing.GetComponent<MeshRenderer>().sharedMaterials;
        Vector3 GAposition = thing.transform.position;

        //Debug.Log( vertices.Length);
        for (int i = 0; i < thing.GetComponent<MeshFilter>().mesh.triangles.Length / 3; i++)
        {
            finalPolys.Add(new Polygon(vertices[ GATriangles[i * 3 + 0]], vertices[GATriangles[i * 3 + 1]], vertices[GATriangles[i * 3 + 2]]));
        }
        return finalPolys;
    }

}




// Сначала я должен построить БСП дерево для модельки 1
// Потом я должен сплитить полигоны модельки 2 используя полигоны на нодах БСп дерева
// Может быть потребуется повторить это еще несколько раз для достижения желаемого эффекта
