using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockySpaceship : MonoBehaviour
{
    public BlockyPartsLibrary Library;
    Vector3Int dimensions = new Vector3Int(15, 10, 15);
    GameObject[,,] componentMatrix = new GameObject[15, 10, 15];
    Vector3 offset;

    private void Start()
    {
        offset = dimensions / 2;
        AddBigDetail(Library.cockpit, 8, 3, 8);

        AddBigDetail(Library.floor, 12,5, 3);
    }
    private void Update()
    {

    }

    public void AddDetail(GameObject prefab, int x, int y, int z)
    {
        if (CanPlace(x,y,z))
        {
            GameObject component = Instantiate(prefab);
            componentMatrix[x, y, z] = component;
            component.transform.parent = this.gameObject.transform;
            component.transform.position = new Vector3(x, y, z) - offset + ((Vector3) component.GetComponent<BlockyComponent>().dimensions) / 2f;
            component.GetComponent<BlockyComponent>().SetVariables(this, x, y, z);
        }
    }

    public Vector3Int GetAdjacentSpace(Vector3 point, GameObject component)
    {
        BlockyComponent comp = component.GetComponent<BlockyComponent>();       
        List<Vector3Int> neighBig = comp.Neighbours();
        Vector3Int neigh = neighBig.Select(n => ( Vector3.Angle(centerPoint(n.x, n.y, n.z) - component.transform.position, point - component.transform.position), n) ).Min().n;
        return neigh;
    }

    public void ChangeDetail(GameObject component)
    {
        BlockyComponent comp = component.GetComponent<BlockyComponent>();

    }


    public bool CanPlace(int x, int y, int z)
    {
        if (0 > x && dimensions.x < x)
        {
            return false;
        }
        if (0 > y && dimensions.y < y)
        {
            return false;
        }
        if (0 > z && dimensions.z < z)
        {
            return false;
        }
        return componentMatrix[x, y, z] == null;
    }


    public bool CanPlaceBig(int sx, int sy, int sz, int ex, int ey, int ez)
    {
        //Debug.Log(ex + " " + ey + " " + ez);
        if (0 > sx || dimensions.x < ex)
        {
            return false;
        }
        if (0 > sy || dimensions.y < ey)
        {
            return false;
        }
        if (0 > sz || dimensions.z < ez)
        {
            return false;
        }
        
        for (int x = sx; x < ex; x++)
        {
            for (int y = sy; y < ey; y++)
            {
                for (int z = sz; z < ez; z++)
                {
                    //Debug.Log(componentMatrix[x, y, z]);
                    if (componentMatrix[x, y, z] != null)
                    {
                        
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void AddBigDetail(GameObject prefab, int x, int y, int z)
    {
        BlockyComponent comp = prefab.GetComponent<BlockyComponent>();
        //Debug.Log(x + " " + y + " " + z);

        Vector3Int possibleLocation = naiveVacantSpaceSearch(comp, x, y ,z);
        Debug.Log(x + " " + y + " " + z);
        Debug.Log(possibleLocation);


        if (CanPlaceBig(possibleLocation.x, possibleLocation.y, possibleLocation.z, possibleLocation.x + comp.dimensions.x, possibleLocation.y + comp.dimensions.y, possibleLocation.z + comp.dimensions.z))
        {

            GameObject component = Instantiate(prefab);
            component.transform.parent = this.gameObject.transform;            
            component.transform.position = possibleLocation - offset + ((Vector3)comp.dimensions) / 2f + this.gameObject.transform.position;
            component.GetComponent<BlockyComponent>().SetVariables(this, possibleLocation.x, possibleLocation.y, possibleLocation.z);

            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = component;
            }
        }
    }

    public Vector3Int naiveVacantSpaceSearch(BlockyComponent comp, int sx, int sy, int sz)
    {
        Vector3Int antipoint = new Vector3Int( sx - comp.dimensions.x, sy - comp.dimensions.y, sz - comp.dimensions.z);
        for (int x = 0; x < comp.dimensions.x; x++)
        {
            for (int y = 0; y < comp.dimensions.y; y++)
            {
                for (int z = 0; z < comp.dimensions.z; z++)
                {
                    /*
                    Debug.Log(x + " " + y + " " + z);
                    Debug.Log(sx + " " + sy + " " + sz);
                    Debug.Log(comp.dimensions.x + " " + comp.dimensions.y + " " + comp.dimensions.z);
                    Debug.Log((sx - x) + " " + (sy - y) + " " + (sz - z));
                    Debug.Log((sx + comp.dimensions.x - x) + " " + (sx + comp.dimensions.y - y) + " " + (sx + comp.dimensions.z - z));*/
                    if (CanPlaceBig(sx - x, sy - y, sz - z, sx + comp.dimensions.x - x, sy + comp.dimensions.y - y, sz + comp.dimensions.z - z))
                    {
                        return new Vector3Int(sx - x, sy - y, sz - z);
                    }
                }
            }
        }

        return new Vector3Int(-100, -100, -100);

    }

    public void DeleteDetail(GameObject comp)
    {
        if (comp.GetComponent<BlockyComponent>().CanDelete)
        {
            List<Vector3Int> spaces = comp.GetComponent<BlockyComponent>().DesiredSpace();
            foreach (var space in spaces)
            {
                this.componentMatrix[space.x, space.y, space.z] = null;
            }
            Destroy(comp);
        }
        
    }

    public Vector3 centerPoint(int x, int y, int z)
    {
        Vector3 center = new Vector3(x - offset.x, y - offset.y, z - offset.z) + this.gameObject.transform.position + new Vector3(0.5f, 0.5f, 0.5f);
        return center;
    }

    public List<Vector3Int> Neighbours()
    {
        List<Vector3Int> neigh = new List<Vector3Int> {Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back};
        return neigh;


    }

}
