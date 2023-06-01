using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshRenderer))]
public class AsteroidChunk : MonoBehaviour
{
    public MeshFilter myMesh;
    public MeshRenderer myRenderer;
    public MeshCollider myMeshCollider;

    public Vector3 worldPosition;
    public Vector3Int asteroidPosition;
    public Asteroid parent;

    public AsteroidChunk(List<Polygon> _polygons, Vector3Int _asteroidPosition, Asteroid _parent)
    {
        this.asteroidPosition = _asteroidPosition;
        this.parent = _parent;
        Debug.Log(asteroidPosition);
    }

    public void AsteroidChunkSetData(List<Polygon> _polygons, Vector3Int _asteroidPosition, Asteroid _parent)
    {
        this.transform.position = Vector3.zero;
        this.asteroidPosition = _asteroidPosition;
        this.parent = _parent;

        /*
        this.myMesh = this.GetComponent<MeshFilter>();
        this.myRenderer = this.GetComponent<MeshRenderer>();
        this.myMeshCollider = this.GetComponent<MeshCollider>();
        */
        //this.FillGaps();

        SetMesh(_polygons);
        InitialMeshTweaks();
        //Debug.Log(asteroidPosition);
    }

    public void SetMesh(List<Polygon> _polygons)
    {
        this.myMesh.sharedMesh = BSPNode.ReturnMesh(_polygons, Vector3.zero);
        this.myMeshCollider.sharedMesh = this.myMesh.sharedMesh;
    }

    public void FillGaps(List<Polygon> _polygons)
    {
        Mesh cube = new SphereToAsteroid().Cube();
        new SphereToAsteroid().Move(cube, this.asteroidPosition);
        List<Polygon> cubePolys = BSPNode.ModelToPolygons(cube);
        foreach (var poly in cubePolys)
        {
            poly.material = parent.innerMaterial;
        }
        List<Polygon> finalMesh = BSPNode.OneSidedIntersect(_polygons, cubePolys);
        this.myMesh.sharedMesh = BSPNode.ReturnMesh(finalMesh, Vector3.zero);
        this.myMeshCollider.sharedMesh = myMesh.sharedMesh;
    }
    public void InitialMeshTweaks()
    {
        int count = myMesh.mesh.vertices.Length;
        List<Vector2> uv0 = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            uv0.Add(new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
        }
        myMesh.mesh.SetUVs(0, uv0);
        myMesh.mesh.SetUVs(2, uv0);

        myMesh.mesh.SetTriangles(myMesh.mesh.triangles, 0);


        this.myRenderer.sharedMaterials = new Material[] { this.parent.outerMaterial,  this.parent.innerMaterial };

        this.myMesh.mesh.RecalculateNormals();
        this.myMesh.mesh.RecalculateTangents();
    }


    public void DigMesh(List<Polygon> diggyHole, Vector3 position)
    {
        foreach (var poly in diggyHole)
        {
            poly.material = parent.innerMaterial;
        }
        List<Polygon> finalMesh = BSPNode.Substract( BSPNode.ModelToPolygons( this.myMesh.mesh), diggyHole);

        this.myMesh.mesh = BSPNode.ReturnMesh(finalMesh, Vector3.zero);
        this.myMesh.mesh.RecalculateNormals();
        this.myMesh.mesh.RecalculateTangents();

        this.myMeshCollider.sharedMesh = this.myMesh.mesh;
    }




}
