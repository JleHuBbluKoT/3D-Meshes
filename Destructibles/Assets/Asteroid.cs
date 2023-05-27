using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Asteroid", menuName = "Asteroid")]
public class Asteroid : ScriptableObject
{
    public GameObject[,,] cuboids;
    public Vector3Int AsteroidDimensions;
    public Vector3Int DisplacementPoint;
    public Vector3Int SecondPoint;
    public GameObject asteroidChunkPrefab;
    public Material outerMaterial;
    public Material innerMaterial;

    public List<List<Polygon>> SplitAsteroid(Mesh mesh, float chunkSize)
    {
        List<Polygon> meshPolygons = BSPNode.ModelToPolygons(mesh);

        float minx; float miny; float minz; float maxx; float maxy; float maxz;
        getCubicDimensions(mesh, out minx, out miny, out minz, out maxx, out maxy, out maxz);

        int startx; int starty; int startz; int endx; int endy; int endz;
        startx = (int)Mathf.Floor(minx / chunkSize);
        starty = (int)Mathf.Floor(miny / chunkSize);
        startz = (int)Mathf.Floor(minz / chunkSize);

        endx = (int)Mathf.Ceil(maxx / chunkSize);
        endy = (int)Mathf.Ceil(maxy / chunkSize);
        endz = (int)Mathf.Ceil(maxz / chunkSize);

        Debug.Log(startx + " " + starty + " " + startz + " " + endx + " " + endy + " " + endz);

        this.DisplacementPoint = new Vector3Int(startx, starty, startz);
        this.AsteroidDimensions = new Vector3Int(endx - startx + 1, endy - starty + 1, endz - startz + 1);
        this.cuboids = new GameObject[AsteroidDimensions.x, AsteroidDimensions.y, AsteroidDimensions.z];


        List<List<Polygon>> Xcut = BSPCutterX(BSPNode.ModelToPolygons(mesh),  startx, endx, starty, endy, startz, endz,  chunkSize);
        return Xcut;
    }

    public List<List<Polygon>> BSPCutterX(List<Polygon> mesh, int startx, int endx, int starty, int endy, int startz, int endz, float chunkSize/* List<Polygon> divider*/)
    {
        List<List<Polygon>> halves = new List<List<Polygon>>();


        if ((endx - startx - 1) == 0) // If difference between start and end is 1, returns the slice with edges applied
        {
            //Debug.Log(startx + " " + endx);
            /*
            Debug.Log(startx + " " + endx  + " " + mesh.Count + "F");

            Polygon divider1 = new Polygon( new Vector3(start * chunkSize, starty, startz), new Vector3(start * chunkSize, endy, startz), new Vector3(start * chunkSize, starty, endz));
            Polygon divider2 = new Polygon(new Vector3(start * chunkSize, endy, endz), new Vector3(start * chunkSize, starty, endz), new Vector3(start * chunkSize, endy, startz));

            Polygon divider3 = new Polygon(new Vector3(end * chunkSize, starty, startz), new Vector3(end * chunkSize, endy, startz), new Vector3(end * chunkSize , starty, endz));
            Polygon divider4 = new Polygon(new Vector3(end * chunkSize, endy, endz), new Vector3(end * chunkSize, starty, endz), new Vector3(end * chunkSize , endy, startz));
            divider1.Flip(); divider2.Flip(); //divider3.Flip(); divider4.Flip();


            mesh = BSPNode.Intersect(mesh, new List<Polygon> { divider1, divider2, divider3, divider4 });*/
            halves.AddRange(BSPCutterY(mesh, startx, endx, starty, endy, startz, endz, chunkSize));

            //halves.Add(mesh);
            return halves;
        }
        List<Polygon> coplanar = new List<Polygon>(); // junk
        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();

        int middlex = Mathf.RoundToInt(endx - (endx - startx) / 2);
        //Debug.Log(middle + " " + start + " " + end);
        //Debug.Log(chunkSize);

        Polygon divider = new Polygon(new Vector3(middlex * chunkSize, -5, -5), new Vector3(middlex * chunkSize, 5, -5), new Vector3(middlex * chunkSize, -5, 5));

        for (int i = 0; i < mesh.Count; i++)
            divider.plane.SplitPolygon(mesh[i], coplanar, coplanar, listFront, listBack);
        //Debug.Log(start + " " + end + "|||" + listFront.Count + " " + listBack.Count);
        //halves.AddRange(BSPCutterX(listFront, middle, end, starty, endy, startz, endz, chunkSize));
        if (listFront.Count != 0)
        {
            halves.AddRange(BSPCutterX(listFront, middlex, endx, starty, endy, startz, endz, chunkSize));
        }
        // Back
        if (listBack.Count != 0)
        {
            halves.AddRange(BSPCutterX(listBack, startx, middlex, starty, endy, startz, endz, chunkSize));
        }
        return halves;
    }

    public List<List<Polygon>> BSPCutterY(List<Polygon> mesh, int startx, int endx, int starty, int endy, int startz, int endz, float chunkSize/* List<Polygon> divider*/)
    {
        List<List<Polygon>> halves = new List<List<Polygon>>();

        if ((endy - starty - 1) == 0 ) // If difference between start and end is 1, returns the slice with edges applied
        {
            //Debug.Log(starty + " " + endy);
            /*
            Polygon divider1 = new Polygon(new Vector3(startx, start * chunkSize, startz), new Vector3(endx, start * chunkSize, startz), new Vector3(startx, start * chunkSize, endz)); // bottom divider, it is here to seal the hole in the mesh
            Polygon divider2 = new Polygon(new Vector3(endx, start * chunkSize, endz), new Vector3(startx, start * chunkSize, endz), new Vector3(endx, start * chunkSize, startz));

            Polygon divider3 = new Polygon(new Vector3(startx, end * chunkSize, startz), new Vector3(endx, end * chunkSize, startz), new Vector3(startx, end * chunkSize, endz)); // top divider, same function
            Polygon divider4 = new Polygon(new Vector3(endx, end * chunkSize, endz), new Vector3(startx, end * chunkSize, endz), new Vector3(endx, end * chunkSize, startz));
            divider3.Flip(); divider4.Flip(); //divider3.Flip(); divider4.Flip();

            
            mesh = BSPNode.Intersect(mesh, new List<Polygon> { divider1, divider2, divider3, divider4 });*/
            //halves.Add(mesh);


            halves.AddRange(BSPCutterZ(mesh, startx, endx, starty, endy, startz, endz, chunkSize));

            return halves;
        }
        List<Polygon> coplanar = new List<Polygon>(); // junk

        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();

        int middley = Mathf.RoundToInt(endy - (endy - starty) / 2);
        //Debug.Log(middle);

        Polygon divider = new Polygon(new Vector3(-5, middley * chunkSize, -5), new Vector3(5, middley * chunkSize, -5), new Vector3(-5, middley * chunkSize, 5));

        for (int i = 0; i < mesh.Count; i++)
            divider.plane.SplitPolygon(mesh[i], coplanar, coplanar, listFront, listBack);


        // Back
        if (listBack.Count != 0)
        {
            halves.AddRange(BSPCutterY(listBack, startx, endx, middley, endy, startz, endz, chunkSize));
        }
        if (listFront.Count != 0)
        {
            halves.AddRange(BSPCutterY(listFront, startx, endx, starty, middley, startz, endz, chunkSize));
        }
        


        // Front
       

        return halves;
    }



    public List<List<Polygon>> BSPCutterZ(List<Polygon> mesh, int startx, int endx, int starty, int endy, int startz, int endz, float chunkSize/* List<Polygon> divider*/)
    {
        List<List<Polygon>> halves = new List<List<Polygon>>();


        if ((endz - startz - 1) == 0) // If difference between start and end is 1, returns the slice with edges applied
        {
            //Debug.Log(startz + " " + endz);
            /*
            Debug.Log(start + " " + end + " " + mesh.Count + "F");
            Polygon divider1 = new Polygon(new Vector3(startx, starty, start * chunkSize), new Vector3(endx, starty, start * chunkSize), new Vector3(startx, endy, start * chunkSize));
            Polygon divider2 = new Polygon(new Vector3(endx, endy, start * chunkSize), new Vector3(startx, endy, start * chunkSize), new Vector3(endx, starty, start * chunkSize));
            Polygon divider3 = new Polygon(new Vector3(startx, starty, end * chunkSize), new Vector3(endx, starty, end * chunkSize), new Vector3(startx, endy, end * chunkSize));
            Polygon divider4 = new Polygon(new Vector3(endx, endy, end * chunkSize), new Vector3(startx, endy, end * chunkSize), new Vector3(endx, starty, end * chunkSize));
            divider1.Flip(); divider2.Flip(); //divider3.Flip(); divider4.Flip();
            mesh = BSPNode.Intersect(mesh, new List<Polygon> { divider1, divider2, divider3, divider4 });*/
            //this.cuboids[startx - DisplacementPoint.x, starty - DisplacementPoint.y, startz - DisplacementPoint.z] = new AsteroidChunk(mesh, new Vector3Int(startx,starty,startz), this);

            this.cuboids[startx - DisplacementPoint.x, starty - DisplacementPoint.y, startz - DisplacementPoint.z] = Instantiate(asteroidChunkPrefab);
            this.cuboids[startx - DisplacementPoint.x, starty - DisplacementPoint.y, startz - DisplacementPoint.z].GetComponent<AsteroidChunk>().AsteroidChunkSetData(mesh, new Vector3Int(startx, starty, startz), this);
            halves.Add(mesh);
            return halves;
        }
        List<Polygon> coplanar = new List<Polygon>(); // junk

        List<Polygon> listFront = new List<Polygon>();
        List<Polygon> listBack = new List<Polygon>();

        int middlez = Mathf.RoundToInt(endz - (endz - startz) / 2);
        //Debug.Log(middle + " " + start + " " + end);
        //Debug.Log(chunkSize);

        Polygon divider = new Polygon(new Vector3(-5, -5, middlez * chunkSize), new Vector3(5, -5, middlez * chunkSize), new Vector3(-5, 5, middlez * chunkSize));

        for (int i = 0; i < mesh.Count; i++)
            divider.plane.SplitPolygon(mesh[i], coplanar, coplanar, listFront, listBack);

        //Debug.Log(start + " " + end + "|||" + listFront.Count + " " + listBack.Count);
        //halves.AddRange(BSPCutterX(listFront, middle, end, starty, endy, startz, endz, chunkSize));
        if (listFront.Count != 0)
        {
            halves.AddRange(BSPCutterZ(listFront, startx, endx, starty, endy, middlez, endz, chunkSize));
        }

        // Back
        if (listBack.Count != 0)
        {
            halves.AddRange(BSPCutterZ(listBack, startx, endx, starty, endy, startz, middlez, chunkSize));
        }


        return halves;
    }







    public void getCubicDimensions(Mesh mesh, out float minx, out float miny, out float minz, out float maxx, out float maxy, out float maxz)
    {
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        minx = vertices[0].x;
        miny = vertices[0].y;
        minz = vertices[0].z;
        maxx = vertices[0].x;
        maxy = vertices[0].y;
        maxz = vertices[0].z;
        for (int i = 1; i < vertices.Count; i++)
        {
            minx = Mathf.Min(minx, vertices[i].x);
            miny = Mathf.Min(miny, vertices[i].y);
            minz = Mathf.Min(minz, vertices[i].z);
            maxx = Mathf.Max(maxx, vertices[i].x);
            maxy = Mathf.Max(maxy, vertices[i].y);
            maxz = Mathf.Max(maxz, vertices[i].z);
        }
    }
}
