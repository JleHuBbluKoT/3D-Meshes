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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetVariables(BlockySpaceship _spaceship, int x, int y, int z)
    {
        spaceShip = _spaceship;
        positionInArray = new Vector3Int(x,y,z);
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
        for (int i = 0; i < mySpaces.Count; i++)
        {
            hashset.Add(mySpaces[i] + Vector3Int.up);
            hashset.Add(mySpaces[i] + Vector3Int.down);
            hashset.Add(mySpaces[i] + Vector3Int.right);
            hashset.Add(mySpaces[i] + Vector3Int.left);
            hashset.Add(mySpaces[i] + Vector3Int.forward);
            hashset.Add(mySpaces[i] + Vector3Int.back);
        }
        hashset.ExceptWith(mySpaces);
        mySpaces.Clear();
        mySpaces.AddRange(hashset);
        return mySpaces;
    }
}
