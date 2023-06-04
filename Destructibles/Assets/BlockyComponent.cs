using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockyComponent : MonoBehaviour
{
    public Vector3Int dimensions;
    public Vector3Int positionInArray;
    public BlockySpaceship spaceShip;
    public bool Foundation;
    public bool CanDelete;

    public bool Connected;
    // Start is called before the first frame update
    void Start()
    {
        if (this.Foundation)  {  Connected = true; }
        Connected = ConnectionCheck();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool ConnectionCheck()
    {
        if (this.Foundation == true) return true;
        List<Vector3Int> neigh = Neighbours();
        for (int i = 0; i < neigh.Count; i++)
        {
            //Debug.Log(i);
            if (spaceShip.componentMatrix[neigh[i].x, neigh[i].y, neigh[i].z] != null && spaceShip.componentMatrix[neigh[i].x, neigh[i].y, neigh[i].z].GetComponent<BlockyComponent>().Connected)
            {
                return true;
            }
        }
        return false;
    }

    public void SetVariables(BlockySpaceship _spaceship, int x, int y, int z)
    {
        spaceShip = _spaceship;
        positionInArray = new Vector3Int(x,y,z);

        BlockySpaceshipEngines engines;
        if (this.TryGetComponent<BlockySpaceshipEngines>(out engines))
        {
            engines.spaceShip = this.spaceShip.gameObject;
        }
    }

    public List<Vector3Int> DesiredSpace()
    {
        List<Vector3Int> list = new List<Vector3Int>();
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    list.Add(new Vector3Int(x,y,z) + this.positionInArray);
                }
            }
        }
        return list;
    }

    public List<Vector3Int> Neighbours()
    {
        HashSet<Vector3Int> hashset = new HashSet<Vector3Int>();
        List<Vector3Int> mySpaces = DesiredSpace();
        for (int i = 0; i < mySpaces.Count; i++) {
            if (mySpaces[i].y + 1 < spaceShip.dimensions.y) {
                hashset.Add(mySpaces[i] + Vector3Int.up);
            }
            if (mySpaces[i].y - 1 >= 0)  {
                hashset.Add(mySpaces[i] + Vector3Int.down);
            }
            if (mySpaces[i].x + 1 < spaceShip.dimensions.x)  {
                hashset.Add(mySpaces[i] + Vector3Int.right);
            }
            if (mySpaces[i].x - 1 >= 0) {
                hashset.Add(mySpaces[i] + Vector3Int.left);
            }
            if (mySpaces[i].z + 1 < spaceShip.dimensions.z) {
                hashset.Add(mySpaces[i] + Vector3Int.forward);
            }
            if (mySpaces[i].z - 1 >= 0) {
                hashset.Add(mySpaces[i] + Vector3Int.back);
            }
        }
        hashset.ExceptWith(mySpaces);
        mySpaces.Clear();
        mySpaces.AddRange(hashset);
        return mySpaces;
    }
    public List<Quaternion> validRotations()
    {
        List<Quaternion> list = new List<Quaternion>();
        list.Add(Quaternion.Euler(90, 0, 0));
        list.Add(Quaternion.Euler(-90, 0, 0));
        list.Add(Quaternion.Euler(0, 90, 0));
        list.Add(Quaternion.Euler(0, -90, 0));
        list.Add(Quaternion.Euler(0, 0, 90));
        list.Add(Quaternion.Euler(0, 0, -90));
        return list;
    }
    public Vector3Int SetConfigurations(int x, int y, int z)
    {
        if (!validRotations().Contains(Quaternion.Euler(x, y, z))) return this.dimensions;
        Vector3 tmp = Quaternion.Euler(x, y, z) * this.dimensions;
        this.dimensions = sillyQuaternion(x,y,z);
        this.transform.rotation = this.transform.rotation * Quaternion.Euler(x, y, z);
        Debug.Log(dimensions);
        return this.dimensions;
    }
    public Vector3Int sillyQuaternion(int x, int y, int z)
    {
        if (x != 0)
        {
            return new Vector3Int(dimensions.x, dimensions.z, dimensions.y);
        }
        if (y != 0)
        {
            return new Vector3Int(dimensions.z, dimensions.y, dimensions.x);
        }
        if (z != 0)
        {
            return new Vector3Int(dimensions.y, dimensions.x, dimensions.z);
        }
        return Vector3Int.zero;
    }
}
