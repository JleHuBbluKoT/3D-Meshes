using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator {

    public List<Polygon> StepOne()
    {
        List<Polygon> polygons = new List<Polygon>();
        

        List<Polygon> core = new List<Polygon>();
        core = AsteroidGEneratorVolumes.GenerateCuboidBSPCompatible(8,8,8, new Vector3Int(0,0,0));

        List<Vector3Int> points = new List<Vector3Int>();
        for (int i = 0; i < 15; i++)
        {
            Vector3 pos = core[Random.Range(0, core.Count)].vertices[Random.Range(0,3)].position;
            points.Add(new Vector3Int((int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y), (int)Mathf.Round(pos.z)));
        }
        foreach (var point in points)
        {
            core = BSPNode.Substract(core, RandomVolume(2, 2, 2, 0.0f, point));
        }
        

        /*
        foreach (var point in points)
        {
            core = BSPNode.Union(core, RandomVolume(6, 6, 6, 0.5f, point));
        }

        points = GenerateRandomPoints(15, 0, 0, 0, 8, 8, 8, new Vector3Int(0, 0, 0));
        foreach (var point in points)
        {
            core = BSPNode.Union(core, RandomVolume(2, 2, 2, 0.5f, point));
        }

        points.Clear();
        */


        return core;
    }



    public List<Polygon> RandomVolume(int x, int y, int z, float deviation, Vector3Int origin)
    {
        x = (int)Random.Range(Mathf.Round(x - x * deviation), Mathf.Round(x + x * deviation));
        y = (int)Random.Range(Mathf.Round(y - y * deviation), Mathf.Round(y + y * deviation));
        z = (int)Random.Range(Mathf.Round(z - z * deviation), Mathf.Round(z + z * deviation));
        return AsteroidGEneratorVolumes.GenerateCuboidBSPCompatible(x, y, z, origin);
    }


    public List<Vector3Int> GenerateRandomPoints(int amount, int x1, int y1, int z1, int x2, int y2, int z2, Vector3Int origin)
    {
        List<Vector3Int> points = new List<Vector3Int>();
        for (int i = 0; i < amount; i++) {
            int x = (int)Mathf.Round( Random.Range(x1, x2));
            int y = (int)Mathf.Round( Random.Range(y1, y2));
            int z = (int)Mathf.Round( Random.Range(z1, z2));
            points.Add( new Vector3Int(x,y,z) + origin);
        }
        return points;
    }

}




/*
public void Connect(AsteroidNode vert, int length, int height, int width)
{
    Debug.Log(vert.position + " " + vert.number);
    if (vert.position.x < length && vert.minusX != null) //&& matrix[vert.position.x + 1, vert.position.y, vert.position.z] != null
    {
        vert.plusX = matrix[vert.position.x + 1, vert.position.y, vert.position.z];
    }
    if(vert.position.y < height) // && matrix[vert.position.x, vert.position.y + 1, vert.position.z] != null
    {
        vert.plusX = matrix[vert.position.x, vert.position.y + 1, vert.position.z];
    }
    if (vert.position.z < width) // && matrix[vert.position.x, vert.position.y, vert.position.z + 1] != null
    {
        vert.plusX = matrix[vert.position.x, vert.position.y, vert.position.z + 1];
    }
}*/