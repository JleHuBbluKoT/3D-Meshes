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
    { // Для трансформации модельки сначала в список полигонов
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

    public void Build(List<Polygon> listPoly, string dir, int depth)
    {
        //Debug.Log(dir + " " + depth);
        if (listPoly.Count < 1 || depth > 100)  return; // Проверка на то что это полигон без ошибок и на то что полигон существует

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
    public List<Polygon> DeleteInsides(List<Polygon> otherPoly, string dir, int depth)
    {
        Debug.Log(dir + " " + depth);
        if (this.plane == null || !(this.plane.normal.magnitude > 0)) {
            return otherPoly;
        }

        // Разделить чужую модельку от этой ноды
        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();
        for (int i = 0; i < otherPoly.Count; i++)
            this.plane.SplitPolygon(otherPoly[i], listFront, listBack, listFront, listBack);
        // Результ отправить в ноды front и back, рекурсивно вызвав DeleteInsides
        Debug.Log(listFront.Count + " " + listBack.Count);
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



}


// Сначала я должен построить БСП дерево для модельки 1
// Потом я должен сплитить полигоны модельки 2 используя полигоны на нодах БСп дерева
// Может быть потребуется повторить это еще несколько раз для достижения желаемого эффекта
