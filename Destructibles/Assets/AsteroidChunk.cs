using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshRenderer))]
public class AsteroidChunk : MonoBehaviour
{
    public List<Polygon> polygons;
    public MeshFilter myMesh;
    public MeshRenderer myRenderer;

    public Vector3 worldPosition;
    public Vector3Int asteroidPosition;
    public Asteroid parent;

    public AsteroidChunk(List<Polygon> _polygons, Vector3Int _asteroidPosition, Asteroid _parent)
    {
        this.polygons = _polygons;
        this.asteroidPosition = _asteroidPosition;
        this.parent = _parent;
        Debug.Log(asteroidPosition);
    }

    public void AsteroidChunkSetData(List<Polygon> _polygons, Vector3Int _asteroidPosition, Asteroid _parent)
    {
        this.transform.position = Vector3.zero;
        this.polygons = _polygons;
        this.asteroidPosition = _asteroidPosition;
        this.parent = _parent;
        this.myMesh = this.GetComponent<MeshFilter>();
        this.myRenderer = this.GetComponent<MeshRenderer>();

        this.FillGaps();

        InitialMeshTweaks();
        //Debug.Log(asteroidPosition);
    }

    public void SetMesh()
    {
        this.myMesh.mesh = BSPNode.ReturnMesh(this.polygons, Vector3.zero);
    }

    public void FillGaps()
    {
        Mesh cube = new SphereToAsteroid().Cube();
        new SphereToAsteroid().Move(cube, this.asteroidPosition);
        List<Polygon> cubePolys = BSPNode.ModelToPolygons(cube);
        foreach (var poly in cubePolys)
        {
            poly.material = parent.innerMaterial;
        }
        List<Polygon> finalMesh = BSPNode.Intersect(this.polygons, cubePolys);
        this.myMesh.mesh = BSPNode.ReturnMesh(finalMesh, Vector3.zero);
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

        this.myRenderer.sharedMaterials = new Material[] { this.parent.outerMaterial,  this.parent.innerMaterial };

        this.myMesh.mesh.RecalculateNormals();
        this.myMesh.mesh.RecalculateTangents();
    }

}
