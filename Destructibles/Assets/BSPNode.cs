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
    public static Mesh Interface(Operation operation, GameObject A, GameObject B, GameObject Return)
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
        Return.transform.position = origin;

        return ReturnMesh(tmp, origin, Return);
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


    public static Mesh ReturnMesh(List<Polygon> polys, Vector3 origin, GameObject newObject)
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

        List<Vertex>  UniqueVertices =  notUnique.Distinct().ToList();
        List<Vector3> vectorVertice =   new List<Vector3>();
        List<Color>   color =           new List<Color>();
        List<Vector3> normal =          new List<Vector3>();
        List<Vector4> tangent =         new List<Vector4>();
        List<Vector2> uv0 =             new List<Vector2>();
        List<Vector2> uv2 =             new List<Vector2>();
        List<Vector4> uv3 =             new List<Vector4>();
        List<Vector4> uv4 =             new List<Vector4>();

        foreach (var vertex in UniqueVertices)
        {
            vectorVertice.Add(vertex.position - origin);
            color.Add(vertex.color);
            normal.Add(vertex.normal);
            tangent.Add(vertex.tangent);
            uv0.Add(vertex.uv0);
            uv2.Add(vertex.uv2);
            uv3.Add(vertex.uv3);
            uv4.Add(vertex.uv4);
        }

        TriA.vertices = vectorVertice.ToArray();
        TriA.colors = color.ToArray();
        TriA.normals = normal.ToArray();
        TriA.tangents = tangent.ToArray();
        TriA.uv = uv0.ToArray();
        TriA.uv2 = uv2.ToArray();
        TriA.SetUVs(2, uv3);
        TriA.SetUVs(3, uv4);
        // Triangles ===============================================================================
        HashSet<Material> HSmaterials = new HashSet<Material>();
        
        
        //Debug.Log("JOE BIDEN");
        foreach (var poly in polys)
        {
            HSmaterials.Add( poly.material);
            //Debug.Log(poly.material);
        }


        List<Material> Lmaterials = HSmaterials.ToList();
        /* if (Lmaterials.Count > 1) {
            for (int i = 0; i < Lmaterials.Count; i++) {
                for (int j = i + 1; j < Lmaterials.Count; j++)  {
                    Debug.Log(Lmaterials[i] + " " + Lmaterials[j]);
                    Debug.Log(Lmaterials[i] == Lmaterials[j]);
                } }  }*/

        //Lmaterials = Lmaterials.Distinct().ToList();
        // ===============================================================================
        TriA.subMeshCount = Lmaterials.Count;
        List<List<int>> indexes = new List<List<int>>();
        for (int i = 0; i < Lmaterials.Count; i++) { indexes.Add(new List<int>());}

        newObject.GetComponent<MeshRenderer>().materials = Lmaterials.ToArray();

        

        foreach (var poly in polys) {
            int submeshIndex = Lmaterials.IndexOf(poly.material);
            
            for (int i = 0; i < poly.vertices.Count; i++)  {
                int a = UniqueVertices.IndexOf(poly.vertices[i]);
                //b = b + " " + a;
                indexes[submeshIndex].Add(a);
                
            }
        }
        


        for (int i = 0; i < TriA.subMeshCount; i++)   {
            TriA.SetIndices(indexes[i], MeshTopology.Triangles, i);
            //Debug.Log(indexes[i].Count);
        }

        for (int i = 0; i < TriA.subMeshCount; i++)
        {
            List<int> rrr = new List<int>();
            TriA.GetIndices(rrr, i);
            //Debug.Log(rrr.Count);
            //Debug.Log(newObject.GetComponent<MeshRenderer>().materials[i]);
        }

        
        return TriA;
    }



    public static Vertex[] GetVertices(Mesh mesh, Transform origin)
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
            if (positions.Length== vcount)  v[i].position = origin.TransformPoint(positions[i]);
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
        Mesh mesh = thing.GetComponent<MeshFilter>().mesh;

        int[] GATriangles = mesh.triangles;
        Vertex[] vertices = GetVertices(mesh, thing.transform);
        Vector3 GAposition = thing.transform.position;
        Material[] Materials = thing.GetComponent<MeshRenderer>().sharedMaterials;

        List<List<int>> mIndexes = new List<List<int>>();

        // Этот for возвращает список списков, внутренний список содержит треугольники, индекс внутреннего списка соответствует материалу
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            var Indexes = new List<int>();
            mesh.GetIndices(Indexes, i);
            mIndexes.Add(Indexes);
        }

        // ====================================================================================================================
        for (int m = 0; m < mIndexes.Count; m++)
        {
            List<int> triangles = mIndexes[m];
            for (int j = 0; j < triangles.Count; j += 3)
            {
                finalPolys.Add(new Polygon(vertices[triangles[j + 0]], vertices[triangles[j + 1]], vertices[triangles[j + 2]], Materials[m]));
            }
        }
        // for (int i = 0; i < mesh.triangles.Length / 3; i++) {     finalPolys.Add(new Polygon(vertices[ GATriangles[i * 3 + 0]], vertices[GATriangles[i * 3 + 1]], vertices[GATriangles[i * 3 + 2]]));  }
        return finalPolys;
    }

}




// Сначала я должен построить БСП дерево для модельки 1
// Потом я должен сплитить полигоны модельки 2 используя полигоны на нодах БСп дерева
// Может быть потребуется повторить это еще несколько раз для достижения желаемого эффекта
