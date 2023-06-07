using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockyComponent : MonoBehaviour
{
    public Outline myOutline;
    public Vector3Int dimensions;
    public Vector3Int myRotation;
    public Vector3Int positionInArray;
    public BlockySpaceship spaceShip;
    public BlockySpaceshipEngines engines;
    public bool Foundation;
    public bool CanDelete;

    public bool Connected;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Outline>().enabled = false;
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
    public void OnCreation(BlockySpaceship _spaceship, int x, int y, int z)
    {
        this.spaceShip = _spaceship;
        positionInArray = new Vector3Int(x, y, z);
        if (this.engines != null)
        {
            engines.SetVariables(this.spaceShip.gameObject);
            spaceShip.scroller.AddListElement(engines);
        }
    }

    public void SetVariables(int x, int y, int z)
    {
        positionInArray = new Vector3Int(x,y,z);

        if (this.engines != null)
        {
            engines.UpdateValues();
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

    public void OnDelete()
    {
        if (this.engines != null)
        {
            engines.OnDelete();
        }
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
        myRotation = new Vector3Int((x + myRotation.x) % 360, (y + myRotation.y) % 360, (z + myRotation.z ) % 360);
        if (!validRotations().Contains(Quaternion.Euler(x, y, z))) return this.dimensions;
        this.dimensions = sillyQuaternion(x,y,z);

        this.gameObject.transform.RotateAround(Vector3Int.zero, new Vector3Int(x,y,z), 90);   
        
        if (this.TryGetComponent<BlockySpaceshipEngines>(out engines))
        {
            engines.orientation = dumbQuaternion(engines.orientation, x, y, z);
        }

        return this.dimensions;
    }

    public Vector3Int RotateSeveralTimesDimenstions(int x, int y, int z)
    {
        int failsafe = 0;
        Vector3Int dims = this.dimensions;
        x = x % 360;
        y = y % 360;
        z = z % 360;
        while (x > 0 && failsafe < 20)
        {
            failsafe++;
            dims = sillyQuaternion(dims, x, 0, 0);
            this.gameObject.transform.RotateAround(Vector3Int.zero, new Vector3Int(1, 0, 0), 90);
            if (this.TryGetComponent<BlockySpaceshipEngines>(out engines))
            {
                engines.orientation = dumbQuaternion(engines.orientation, x, 0, 0);
            }
            x -= 90;
        }
        while (y > 0 && failsafe < 20)
        {
            failsafe++;
            dims = sillyQuaternion(dims, 0, y, 0);
            this.gameObject.transform.RotateAround(Vector3Int.zero, new Vector3Int(0, 1, 0), 90);
            if (this.TryGetComponent<BlockySpaceshipEngines>(out engines))
            {
                engines.orientation = dumbQuaternion(engines.orientation, 0, y, 0);
            }
            y -= 90;
        }
        while (z > 0 && failsafe < 20)
        {
            failsafe++;
            dims = sillyQuaternion(dims, 0, 0, z);
            this.gameObject.transform.RotateAround(Vector3Int.zero, new Vector3Int(0, 0, 1), 90);
            if (this.TryGetComponent<BlockySpaceshipEngines>(out engines))
            {
                engines.orientation = dumbQuaternion(engines.orientation, 0, 0, z);
            }
            z -= 90;
        }
        Debug.Log(failsafe);
        this.dimensions = dims;
        return dims;
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
    public Vector3Int sillyQuaternion(Vector3Int dims, int x, int y, int z)
    {
        if (x != 0)
        {
            return new Vector3Int(dims.x, dims.z, dims.y);
        }
        if (y != 0)
        {
            return new Vector3Int(dims.z, dims.y, dims.x);
        }
        if (z != 0)
        {
            return new Vector3Int(dims.y, dims.x, dims.z);
        }
        return Vector3Int.zero;
    }

    public Vector3 dumbQuaternion(Vector3 value, int x, int y, int z)
    {
        // Head hurts but it works
        if (x != 0)
        {
            return new Vector3(value.x, value.y < 0 ? value.z : -value.z, value.z < 0 ? -value.y : value.y);
        }
        if (y != 0)
        {
            return new Vector3(value.x < 0 ? -value.z : value.z, value.y, value.z < 0 ? value.x : -value.x);
        }
        if (z != 0)
        {
            return new Vector3(value.x < 0 ? value.y : -value.y, value.y < 0 ? -value.x : value.x, value.z);
        }
        return Vector3.zero;
    }
}
