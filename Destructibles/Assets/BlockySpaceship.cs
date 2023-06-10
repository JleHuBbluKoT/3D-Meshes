using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockySpaceship : MonoBehaviour
{
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
    public PersistenceManager saveManager;

    public LayerMask connected;
    public LayerMask disconnected;
    public Rigidbody rb;
    public SpaceshipMovement spaceshipMover;


    public BlockyPartsLibrary Library;
    public Vector3Int dimensions = new Vector3Int(15, 10, 15);
    public GameObject[,,] componentMatrix = new GameObject[15, 10, 15];
    Vector3 offset;

    public List<GameObject> allComponents = new List<GameObject>();
    public List<GameObject> invalidComponents = new List<GameObject>();
    private BlockyComponent coreComponent;

    public ConfigurableScroller scroller;
    public SpaceshipGenerator spaceshipGenerator;

    private void Start()
    {
        offset = dimensions / 2;
        ResetSpaceship();
    }
    private void Update()
    {

    }

    public void WipeSpaceship()
    {
        if (coreComponent != null)
        {
            coreComponent.CanDelete = true;
        }
        //Debug.Log(allComponents.Count);
        //Debug.Log(invalidComponents.Count);
        int failsafe = 0;
        while (allComponents.Count > 0 && failsafe < 100)
        {
            failsafe++;
            DeleteDetail(allComponents[0]);
        }
        while (invalidComponents.Count > 0 && failsafe < 100)
        {
            failsafe++;
            DeleteDetail(invalidComponents[0]);
        }
        if (coreComponent != null)
        {
            DeleteDetail(coreComponent.gameObject);
        }

        //Debug.Log("wipe out");
    }

    public void ResetSpaceship()
    {
        WipeSpaceship();
        coreComponent = AddBigDetail(Library.cockpit, 8, 3, 8).GetComponent<BlockyComponent>();
        UpdateConnections();
    }

    public void ResetAndRandomize()
    {
        WipeSpaceship();
        spaceshipGenerator.WaveFunctionCollapse();
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
        //component.GetComponent<MeshRenderer>().color
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
                        //Debug.Log("something here!");
                        //Debug.Log(componentMatrix[x,y,z]);
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    public GameObject AddBigDetail(GameObject prefab, int x, int y, int z)
    {
        BlockyComponent comp = prefab.GetComponent<BlockyComponent>();
        Vector3Int possibleLocation = naiveVacantSpaceSearch(comp, x, y ,z);
        //Debug.Log(x + " " + y + " " + z); 
        //Debug.Log(possibleLocation);
        //Debug.Log(comp.dimensions);

        if (CanPlaceBig(possibleLocation.x, possibleLocation.y, possibleLocation.z, possibleLocation.x + comp.dimensions.x, possibleLocation.y + comp.dimensions.y, possibleLocation.z + comp.dimensions.z))
        {
            GameObject component = Instantiate(prefab);
            
            component.transform.parent = this.gameObject.transform;            
            component.transform.position = possibleLocation - offset + ((Vector3)comp.dimensions) / 2f + this.gameObject.transform.position;
            component.GetComponent<BlockyComponent>().OnCreation(this, possibleLocation.x, possibleLocation.y, possibleLocation.z);
            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = component;
            }
            allComponents.Add(component);
            UpdateConnections();
            return component;
        }
        return null;
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
/* Debug.Log(x + " " + y + " " + z); Debug.Log(sx + " " + sy + " " + sz);  Debug.Log(comp.dimensions.x + " " + comp.dimensions.y + " " + comp.dimensions.z); Debug.Log((sx - x)
 * + " " + (sy - y) + " " + (sz - z)); Debug.Log((sx + comp.dimensions.x - x) + " " + (sx + comp.dimensions.y - y) + " " + (sx + comp.dimensions.z - z));*/
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
            this.allComponents.Remove(comp);
            this.invalidComponents.Remove(comp);

            UpdateConnections();
            comp.GetComponent<BlockyComponent>().OnDelete();
            Destroy(comp);
        }
    }
    public void MoveComponent(GameObject comp, Vector3Int direction)
    {
        invalidComponents.Remove(comp);
        BlockyComponent component = comp.GetComponent<BlockyComponent>();
        DeleteDetailFootprint(comp);
        Vector3Int pos = component.positionInArray + direction;
        if (CanPlaceBig(pos.x, pos.y, pos.z, pos.x + component.dimensions.x, pos.y + component.dimensions.y, pos.z + component.dimensions.z))
        {
            comp.transform.position = pos - offset + ((Vector3)component.dimensions) / 2f + this.gameObject.transform.position;
            component.GetComponent<BlockyComponent>().SetVariables(pos.x, pos.y, pos.z);

            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = comp;
            }
            //allComponents.Add(comp);
        }
        else
        {
            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = comp;
            }
        }
        SelectDetail(comp);
        UpdateConnections();
    }
    public void AndRotate(GameObject comp, int x, int y, int z)
    {
        BlockyComponent component = comp.GetComponent<BlockyComponent>();
        DeleteDetailFootprint(comp);
        Vector3Int pos = component.positionInArray;

        component.SetConfigurations(x,y,z);

        Vector3Int validPosition = naiveVacantSpaceSearch(component, pos.x, pos.y, pos.z);
        if (CanPlaceBig(validPosition.x, validPosition.y, validPosition.z, validPosition.x + component.dimensions.x, validPosition.y + component.dimensions.y, validPosition.z + component.dimensions.z))
        {
            //Debug.Log(component.dimensions);
            //Debug.Log(component.gameObject.transform.rotation.eulerAngles);
            //Debug.Log(validPosition);
            comp.transform.position = validPosition - offset + ((Vector3)component.dimensions) / 2f + this.gameObject.transform.position;
            component.GetComponent<BlockyComponent>().SetVariables(validPosition.x, validPosition.y, validPosition.z);

            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = comp;
            }
            SelectDetail(comp);
        }
        else
        {
            component.SetConfigurations(-x, -y, -z);
            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = comp;
            }
        }
        UpdateConnections();
    }
    public GameObject AddBigRotatedDetail(GameObject prefab, int x, int y, int z, int rx, int ry, int rz)
    {
        GameObject component = Instantiate(prefab);
        BlockyComponent comp = component.GetComponent<BlockyComponent>();
        comp.dimensions = comp.RotateSeveralTimesDimenstions(rx, ry, rz);

        Vector3Int possibleLocation = naiveVacantSpaceSearch(comp, x, y ,z);
        //Debug.Log(possibleLocation);
        //Debug.Log(comp.dimensions);
        if (CanPlaceBig(possibleLocation.x, possibleLocation.y, possibleLocation.z, possibleLocation.x + comp.dimensions.x, possibleLocation.y + comp.dimensions.y, possibleLocation.z + comp.dimensions.z))
        {
            component.transform.parent = this.gameObject.transform;
            component.transform.position = possibleLocation - offset + ((Vector3)comp.dimensions) / 2f + this.gameObject.transform.position;
            component.GetComponent<BlockyComponent>().OnCreation(this, possibleLocation.x, possibleLocation.y, possibleLocation.z);

            foreach (var item in component.GetComponent<BlockyComponent>().DesiredSpace())
            {
                componentMatrix[item.x, item.y, item.z] = component;
            }
            allComponents.Add(component);
            UpdateConnections();
            return component;
        }
        else
        {
            Destroy(component);
            return null;
        }
        
    }


    public void UpdateConnections()
    {
        if (coreComponent == null) {  return;  }
        //Debug.Log("Updating components");
        int failsafe = 0;
        List<GameObject> affectedComponents = new List<GameObject>();
        List<GameObject> connectedComponents = new List<GameObject>();
        //Debug.Log(coreComponent);
        connectedComponents.Add(coreComponent.gameObject);
        
        while (connectedComponents.Count > 0 && failsafe < 1000)
        {
            failsafe++;
            //Debug.Log(failsafe);
            List<Vector3Int> neigh = connectedComponents[0].GetComponent<BlockyComponent>().Neighbours();
            for (int i = 0; i < neigh.Count; i++)
            {
                GameObject compo = this.componentMatrix[neigh[i].x, neigh[i].y, neigh[i].z];
                if (compo != null && !affectedComponents.Contains(compo))
                {
                    //Debug.Log("Connection!");
                    ConnectDetail(compo);
                    connectedComponents.Add(compo);
                }
            }
            affectedComponents.Add(connectedComponents[0]);
            connectedComponents.RemoveAt(0);
        }
        List<GameObject> list = new List<GameObject>(allComponents);
        invalidComponents = list.Except(affectedComponents).ToList();

        //allComponents = allComponents.Except(invalidComponents).ToList();

        for (int i = 0; i < invalidComponents.Count; i++)
        {
            DisconnectDetail(invalidComponents[i]);
        }
        //invalidComponents.Add();
        
    }
    public void SelectDetail(GameObject comp) {
        comp.GetComponent<Outline>().enabled = true;
        comp.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
        comp.GetComponent<Outline>().OutlineColor = Color.green;
    }
    public void DeselectDetail(GameObject comp) {
        comp.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
        CheckConnection(comp);
        if (comp.GetComponent<BlockyComponent>().Connected)
        {
            comp.GetComponent<Outline>().enabled = false;
        }
    }
    public void CheckConnection(GameObject comp){
        if (comp.GetComponent<BlockyComponent>().ConnectionCheck()) {
            ConnectDetail(comp);
        }
        else{
            DisconnectDetail(comp);
        }
    }
    public void ConnectDetail(GameObject comp) {
        comp.GetComponent<BlockyComponent>().Connected = true;
    }
    public void DisconnectDetail(GameObject comp) {
        if (comp.GetComponent<BlockyComponent>().Foundation) {
            return;
        }
        comp.GetComponent<BlockyComponent>().Connected = false;
        comp.GetComponent<Outline>().enabled = true;
        comp.GetComponent<Outline>().OutlineColor = Color.red;
    }
    public void DeleteDetailFootprint(GameObject comp)
    {
        List<Vector3Int> space = comp.GetComponent<BlockyComponent>().DesiredSpace();
        foreach (var item in space)
        {
            componentMatrix[item.x, item.y, item.z] = null;
        }
    }

    public void SetRealWorldCoordinates(GameObject comp, int x, int y, int z) {
        comp.transform.position = new Vector3(x, y, z) - offset + ((Vector3)comp.GetComponent<BlockyComponent>().dimensions) / 2f + this.gameObject.transform.position;
    }
    public Vector3 centerPoint(int x, int y, int z) {
        Vector3 center = new Vector3(x - offset.x, y - offset.y, z - offset.z) + this.gameObject.transform.position + new Vector3(0.5f, 0.5f, 0.5f);
        return center;
    }
    public List<Vector3Int> Neighbours()
    {
        List<Vector3Int> neigh = new List<Vector3Int> {Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back};
        return neigh;
    }
    public List<BlockyComponentInteractive> GetEngines()
    {
        List<BlockyComponentInteractive> list = new List<BlockyComponentInteractive>();
        list.AddRange(this.spaceshipMover.engines);
        return list;
    }

    public void LoadSavefile(SpaceshipSavefile savefile)
    {
        WipeSpaceship();
        /*SpaceshipConfiguration conf = savefile.listConfigurations[0];

        Debug.Log(conf.listGameobjectType.Count);
        for (int i = 0; i < conf.listGameobjectType.Count; i++)
        {
            Debug.Log(conf.listGameobjectType[i]);
        }

        for (int i = 0; i < conf.listGameobjectType.Count; i++)
        {
            GameObject cmp = AddBigRotatedDetail(Library.GetPrefabFromNumber(conf.listGameobjectType[i]),
                conf.listPosition[i].x, conf.listPosition[i].y, conf.listPosition[i].z,
                conf.listRotation[i].x, conf.listRotation[i].y, conf.listRotation[i].z);
            //Debug.Log(cmp);
            BlockyComponent component = cmp.GetComponent<BlockyComponent>();
            //Debug.Log(component.Foundation);
            if (component.Foundation)
            {
                Debug.Log("hello");
                coreComponent = cmp.GetComponent<BlockyComponent>();
                coreComponent.Foundation = true;
            } 
        }*/

        UpdateConnections();
    }

    public void SaveSavefile(ref SpaceshipSavefile savefile)
    {
        List<GameObject> list = new List<GameObject>(allComponents);
        //list.AddRange(invalidComponents);
        list.Add(coreComponent.gameObject);
        list = list.Distinct().ToList();
        Debug.Log(list.Count);
        savefile.BreakGameobjects(list);
    }



}

/* This code is no longer needed
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
}*/
