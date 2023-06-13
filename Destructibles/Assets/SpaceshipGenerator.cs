using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpaceshipGenerator : MonoBehaviour
{
    public BlockySpaceship spaceship;
    public BlockyPartsLibrary Library;

    public List<wavefunction>[,,] matrix = new List<wavefunction>[15, 10, 7];

    public Dictionary<wavefunction, BlockyPartsLibrary.detailType> dictionary = new Dictionary<wavefunction, BlockyPartsLibrary.detailType> {
        { wavefunction.Nothing, BlockyPartsLibrary.detailType.Wall },
        { wavefunction.Wall, BlockyPartsLibrary.detailType.Wall },
        { wavefunction.Floor, BlockyPartsLibrary.detailType.FloorBig },
        { wavefunction.Generator, BlockyPartsLibrary.detailType.Generator },
        { wavefunction.EngineForward, BlockyPartsLibrary.detailType.Engine },
        { wavefunction.EngineBack, BlockyPartsLibrary.detailType.Engine },
        { wavefunction.EngineUp, BlockyPartsLibrary.detailType.Engine },
        { wavefunction.EngineDown, BlockyPartsLibrary.detailType.Engine },
        { wavefunction.EngineLeft, BlockyPartsLibrary.detailType.Engine },
    };
    Dictionary<wavefunction, int> weights = new Dictionary<wavefunction, int> {
        { wavefunction.Nothing, 4 },
        { wavefunction.Wall, 10 },
        { wavefunction.Floor, 2 },
        { wavefunction.Generator, 4 },
        { wavefunction.EngineForward, 2 },
        { wavefunction.EngineBack, 2 },
        { wavefunction.EngineUp, 2 },
        { wavefunction.EngineDown, 2 },
        { wavefunction.EngineLeft, 2 },
    };

    public enum wavefunction
    {
        Nothing, Wall, Floor, Generator, EngineForward, EngineBack, EngineUp, EngineDown, EngineLeft, Cockpit
    }

    public List<wavefunction> thisTags = new List<wavefunction> {
wavefunction.Nothing, wavefunction.Wall, wavefunction.Floor, wavefunction.Generator, wavefunction.EngineForward, wavefunction.EngineBack, wavefunction.EngineLeft, wavefunction.EngineUp, wavefunction.EngineDown};

    HashSet<Vector3Int> affectedCells = new HashSet<Vector3Int>();
    List<Vector3Int> lowEntropyCells = new List<Vector3Int>();
    List<GameObject> fakeWall = new List<GameObject>();
    public void WaveFunctionCollapse()
    {
        affectedCells.Clear();
        lowEntropyCells.Clear();
        fakeWall.Clear();

        for (int x = 0; x < 15; x++) {
            for (int y = 0; y < 10; y++) {
                for (int z = 0; z < 7; z++) {

                    matrix[x, y, z] = new List<wavefunction>(thisTags);
        }   }   }
        GameObject core = spaceship.AddBigDetail(Library.cockpit, 6,4,6);
        spaceship.coreComponent = core.GetComponent<BlockyComponent>();
        for (int i = 6; i < 9; i++)  {
            for (int j = 4; j < 7; j++)  {
                matrix[i, j, 6] = new List<wavefunction> { wavefunction.Cockpit };
                affectedCells.Add(new Vector3Int(i,j,6));
        }   }
        
        for (int i = 0; i < 15; i += 3)   {
            for (int j = 0; j < 10; j += 3) {
                GameObject gameObject = spaceship.AddBigRotatedDetail(Library.GetPrefabFromNumber(3), i, j, 7, 90, 0, 0);
                if (gameObject != null)  {
                    fakeWall.Add(gameObject);
        }   }   }
        // Barrier to stop blocks from running to the other side
        for (int i = 0; i < 15; i++){
            GameObject gameObject = spaceship.AddBigDetail(Library.GetPrefabFromNumber(2), i, 9, 7);
            if (gameObject != null) {
                fakeWall.Add(gameObject);
            }
            gameObject = spaceship.AddBigDetail(Library.GetPrefabFromNumber(2), i, 3, 7);
            if (gameObject != null) {
                fakeWall.Add(gameObject);
        }   }
        foreach (var item in core.GetComponent<BlockyComponent>().Neighbours())
        {
            if (WithinBounderies(item.x, item.y, item.z))
            {
                lowEntropyCells.Add(item);
                if (item.y < 5)
                {
                    matrix[item.x, item.y, item.z].Remove(wavefunction.Nothing);
                }
            }
        }

        int failsafe = 0;
        while (failsafe < 50) { failsafe++;
            lowEntropyCells = lowEntropyCells.Except(affectedCells).ToList();
            int ints = lowEntropyCells.Select(n => matrix[n.x, n.y, n.z].Count).Min();
            List<Vector3Int> lowestEnt = lowEntropyCells.Where(n => matrix[n.x,n.y,n.z].Count == ints).ToList();

            Vector3Int coordinate = lowestEnt[ Random.Range(0,lowestEnt.Count)];
            wavefunction component = matrix[coordinate.x, coordinate.y, coordinate.z][RandomWeighted(matrix[coordinate.x, coordinate.y, coordinate.z])];

            PlaceDetailGetFootprint(component, coordinate.x, coordinate.y, coordinate.z);
        }

        //Debug.Log(fakeWall.Count);
        failsafe = 0;
        while (fakeWall.Count > 0 && failsafe < 150) { failsafe++;
            GameObject tmp = fakeWall[0]; fakeWall.RemoveAt(0);
            spaceship.DeleteDetail(tmp);
        }
        spaceship.UpdateConnections();

        List<GameObject> components = new List<GameObject>(spaceship.allComponents);
        foreach (var component in components)
        {
            BlockyComponent cmp = component.GetComponent<BlockyComponent>();
            Vector3Int coords = new Vector3Int(cmp.positionInArray.x, cmp.positionInArray.y, 7 + (7 - cmp.positionInArray.z) - cmp.dimensions.z + 1);
            spaceship.AddBigRotatedDetail(component, coords.x, coords.y, coords.z, 0, 0, 0);
        }

    }

    public void PlaceDetailGetFootprint(wavefunction component, int x, int y, int z)
    {
        //Debug.Log(component);
        BlockyComponent comp;
        switch (component)
        {
            case wavefunction.EngineBack:
                comp = Library.GetPrefabFromNumber(dictionary[component]).GetComponent<BlockyComponent>();break;
            case wavefunction.EngineForward:
                comp = Library.GetPrefabFromNumber(dictionary[component]).GetComponent<BlockyComponent>();  break;
            case wavefunction.EngineLeft:
                comp = Library.GetPrefabFromNumber(dictionary[component]).GetComponent<BlockyComponent>(); break;
            case wavefunction.EngineDown:
                comp = Library.GetPrefabFromNumber(dictionary[component]).GetComponent<BlockyComponent>(); break;
            default:
                comp = Library.GetPrefabFromNumber(dictionary[component]).GetComponent<BlockyComponent>();
                break;
        }
        Vector3Int possibleLocation = spaceship.naiveVacantSpaceSearch(comp, x, y, z);

        //Debug.Log(comp.dimensions + " " + new Vector3Int(x,y,z) + " " + possibleLocation);
        if (!WithinBounderies(comp, possibleLocation.x, possibleLocation.y, possibleLocation.z)) { Debug.Log("outside"); return; }
        if (!ValidPlace(comp, possibleLocation.x, possibleLocation.y, possibleLocation.z)) { Debug.Log("can't place"); return; }
        //Debug.Log(component);
        switch (component) {
            case wavefunction.Wall:
                WallFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z);
                break;
            case wavefunction.Generator:
                GeneratorFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z);
                break;
            case wavefunction.Floor:
                FloorFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z);
                break;
            case wavefunction.Nothing:
                fakeWallFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z);
                break;
            case wavefunction.EngineBack:
                EngineFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z, 90, 0, 0);
                break;
            case wavefunction.EngineForward:
                EngineFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z, 270,0,0);
                break;
            case wavefunction.EngineUp:
                EngineFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z, 0,0,0);
                break;
            case wavefunction.EngineDown:
                EngineFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z, 180,0,0);
                break;
            case wavefunction.EngineLeft:
                EngineFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z, 0, 0,90);
                break;
            default:
                fakeWallFootprint(Library.GetPrefabFromNumber(dictionary[component]), possibleLocation.x, possibleLocation.y, possibleLocation.z);
                break;
        }
    }

    public void EngineFootprint(GameObject prefab, int x, int y, int z, int dx, int dy, int dz)
    {
        GameObject gameObject = spaceship.AddBigRotatedDetail(prefab, x, y, z, dx, dy, dz);
        if (gameObject == null) { return; }
        BlockyComponent comp = gameObject.GetComponent<BlockyComponent>();
        foreach (var space in comp.DesiredSpace())
        {
            Debug.Log(space.x + " " + space.y + " " + space.z);
            matrix[space.x, space.y, space.z] = new List<wavefunction> { wavefunction.EngineBack };
            affectedCells.Add(space);
        }
        foreach (var neigh in comp.Neighbours())
        {
            if (WithinBounderies(neigh.x, neigh.y, neigh.z))
            {
                lowEntropyCells.Add(neigh);
            }
        }
        //Debug.Log("placed");
    }

    public void WallFootprint(GameObject prefab, int x, int y, int z)
    {
        GameObject gameObject = spaceship.AddBigDetail(prefab, x, y, z);
        BlockyComponent comp = gameObject.GetComponent<BlockyComponent>();
        matrix[x, y, z] = new List<wavefunction> { wavefunction.Wall };
        affectedCells.Add(new Vector3Int(x,y,z));
        foreach (var neigh in comp.Neighbours())
        {
            if (WithinBounderies(neigh.x, neigh.y, neigh.z))
            {
                lowEntropyCells.Add(neigh);
            }
        }
    }
    public void fakeWallFootprint(GameObject prefab, int x, int y, int z)
    {
        GameObject gameObject = spaceship.AddBigDetail(prefab, x, y, z);
        BlockyComponent comp = gameObject.GetComponent<BlockyComponent>();
        matrix[x, y, z] = new List<wavefunction> { wavefunction.Nothing };
        affectedCells.Add(new Vector3Int(x, y, z));
        foreach (var neigh in comp.Neighbours())
        {
            if (WithinBounderies(neigh.x, neigh.y, neigh.z))
            {
                lowEntropyCells.Add(neigh);
            }
        }
        fakeWall.Add(gameObject);
    }

    public void GeneratorFootprint(GameObject prefab, int x, int y, int z)
    {
        GameObject gameObject = spaceship.AddBigDetail(prefab, x, y, z);
        BlockyComponent comp = gameObject.GetComponent<BlockyComponent>();
        foreach (var space in comp.DesiredSpace())
        {
            matrix[space.x, space.y, space.z] = new List<wavefunction> { wavefunction.Generator};
            affectedCells.Add(space);
        }
        foreach (var neigh in comp.Neighbours())
        {
            if (WithinBounderies(neigh.x, neigh.y, neigh.z))
            {
                matrix[neigh.x, neigh.y, neigh.z].Remove(wavefunction.Nothing);
                lowEntropyCells.Add(neigh);
            }
        }
       //Debug.Log("placed");
    }

    public void FloorFootprint(GameObject prefab, int x, int y, int z)
    {
        GameObject gameObject = spaceship.AddBigDetail(prefab, x, y, z);
        BlockyComponent comp = gameObject.GetComponent<BlockyComponent>();
        foreach (var space in comp.DesiredSpace())
        {
            matrix[space.x, space.y, space.z] = new List<wavefunction> { wavefunction.Floor };
            affectedCells.Add(space);
        }
        foreach (var neigh in comp.Neighbours()) {
            if (WithinBounderies(neigh.x, neigh.y, neigh.z)) {
                if (neigh.y == y)
                {
                    matrix[neigh.x, neigh.y, neigh.z].Remove(wavefunction.Nothing);
                }
                if (neigh.y > y)
                {
                    matrix[neigh.x, neigh.y, neigh.z] = new List<wavefunction> { wavefunction.Nothing };
                }
                lowEntropyCells.Add(neigh);
            } 
        }
        //Debug.Log("placed");
    }


    public bool ValidPlace(BlockyComponent comp, int x, int y, int z)
    {
        List<Vector3Int> places = comp.DesiredSpace();
        if (!WithinBounderies(comp, x, y, z)) return false;
        foreach (var place in places) {
            List<BlockyPartsLibrary.detailType> types = new List<BlockyPartsLibrary.detailType>();
            foreach (var item in matrix[place.x + x, place.y + y, place.z + z])
            {
                BlockyPartsLibrary.detailType gg;
                if (dictionary.TryGetValue(item, out gg)) types.Add(gg);
            }
            if (!types.Contains(comp.type)) {
                return false;
        }   }
        return true;
    }

    public bool WithinBounderies(int x, int y, int z)
    {
        if (y >= 10) {
            return false;
        }
        if (y < 0) {
            return false;
        }
        if (x >= 15) {
            return false;
        }
        if (x < 0) {
            return false;
        }
        if (z >= 7) {
            return false;
        }
        if (z < 0) {
            return false;
        }
        return true;
    }
    public bool WithinBounderies(BlockyComponent thing, int x, int y, int z)
    {
        //Debug.Log(thing.dimensions + " " + x + " " + y + " " + z);
        //Debug.Log((thing.dimensions.x + x -1)  + " " + (thing.dimensions.y + y -1) + " " + (thing.dimensions.z + z -1));
        if (x + thing.dimensions.x -1 >= 15) {
            return false;
        }
        if (x < 0){
            return false;
        }
        if (y + thing.dimensions.y -1 >= 10){
            return false;
        }
        if (y < 0)  {
            return false;
        }
        if (z + thing.dimensions.z -1 >= 7) {
            return false;
        }
        if (z < 0) {
            return false;
        }
        return true;
    }

    // Nothing, .Wall, .Floor, .Generator, .EngineForward, .EngineBack, .EngineLeft, .EngineUp, .EngineDown};
    public int RandomWeighted(List<wavefunction> list)
    {
        int weightTotal = 0; foreach (wavefunction w in list) {  weightTotal += weights[w]; }

        int result = 0, total = 0;
        int randVal = Random.Range(0, weightTotal + 1);
        for (result = 0; result < list.Count; result++)
        {
            total += weights[ list[result]];
            if (total >= randVal) break;
        }
        return result;
    }


    public void SillyRandomFill()
    {
        int failsafe = 0;
        while (spaceship.allComponents.Count < 49 && failsafe < 200)
        {
            failsafe++;
            int x = Random.Range(1, spaceship.dimensions.x - 1);
            int y = Random.Range(1, spaceship.dimensions.y - 1);
            int z = Random.Range(1, spaceship.dimensions.z - 1);

            int rx = 0;
            int ry = 0;
            int rz = 0;

            GameObject type = SillyRandomDetail();
            BlockySpaceshipEngines engine;
            if (type.TryGetComponent<BlockySpaceshipEngines>(out engine))
            {
                int dir = Random.Range(0, 6);
                rx = directions[dir].x;
                ry = directions[dir].y;
                rz = directions[dir].z;
            }
            else
            {
                rx = Mathf.Clamp(Random.Range(0, 12) - 8, 0, 3);
                ry = Mathf.Clamp(Random.Range(0, 12) - 8, 0, 3);
                rz = Mathf.Clamp(Random.Range(0, 12) - 8, 0, 3);
            }

            GameObject detail = spaceship.AddBigRotatedDetail(type, x, y, z, rx*90, ry*90, rz*90);
        }

        spaceship.UpdateConnections();
        spaceship.spaceshipMover.RecalculateEngines();        
    }
    public GameObject SillyRandomDetail()
    {
        int comp = Random.Range(0, 9);

        if (comp <= 1) // wall
        {
            return Library.GetPrefabFromNumber(2);
        }
        if (comp > 1 & comp <= 3) // floor
        {
            return Library.GetPrefabFromNumber(1);
        }
        if (comp > 3 & comp <= 5) // 3x3 floor
        {
            return Library.GetPrefabFromNumber(3);
        }
        if (comp > 5 & comp <= 7) // engine
        {
            return Library.GetPrefabFromNumber(4);
        }
        if (comp == 8) // generator
        {
            return Library.GetPrefabFromNumber(8);
        }
        return Library.GetPrefabFromNumber(1);
    }

    public void BulkGeneration(int configurationAmount)
    {
        spaceship.WipeSpaceship();
        for (int i = 0; i < configurationAmount; i++)
        {
            spaceship.WipeSpaceship();
            WaveFunctionCollapse();
            spaceship.saveManager.SaveSavefile();
            //spaceship.WipeSpaceship();

        }
    }

    public Dictionary<int, Vector3Int> directions = new Dictionary<int, Vector3Int>(){
            {0, new Vector3Int(0,0,0) },
            {1, new Vector3Int(1,0,0) },
            {2, new Vector3Int(1,1,0) },
            {3, new Vector3Int(1,2,0) },
            {4, new Vector3Int(1,3,0) },
            {5, new Vector3Int(2,0,0) },
        };

}
/*
public bool naiveVacantSpaceSearch(BlockyComponent comp, int sx, int sy, int sz) {
    Vector3Int antpoint = new Vector3Int(Mathf.Clamp(sx - comp.dimensions.x + 1 ,0, 14), Mathf.Clamp(sy - comp.dimensions.y + 1, 0, 9), Mathf.Clamp(sz - comp.dimensions.z + 1, 0, 6));
    Vector3Int maxpoint = new Vector3Int(Mathf.Clamp(comp.dimensions.x + sx - 1, 0, 14), Mathf.Clamp(comp.dimensions.y + sy - 1, 0, 9), Mathf.Clamp(comp.dimensions.z + sz - 1, 0, 6));

    Vector3Int dims = maxpoint - antpoint + Vector3Int.one;
    Vector3Int displacement = new Vector3Int(sx, sy, sz) - antpoint;
    if (dims.x < comp.dimensions.x && dims.y < comp.dimensions.y && dims.z < comp.dimensions.z) return false;
    bool[,,] localMatrix = new bool[dims.x,dims.y,dims.z];


    for (int x = antpoint.x; x < maxpoint.x; x++) {
        for (int y = antpoint.y; y < maxpoint.y; y++) {
            for (int z = antpoint.z; z < maxpoint.z; z++) {

                List<BlockyPartsLibrary.detailType> types = new List<BlockyPartsLibrary.detailType>();
                foreach (var item in matrix[sx - displacement.x + x, sy - displacement.y + y, sz - displacement.z + z])
                {
                    BlockyPartsLibrary.detailType gg;
                    if (dictionary.TryGetValue(item, out gg)) types.Add(gg);
                }
                localMatrix[x, y, z] = types.Contains(comp.type);
    }   }   }

    Vector3Int freedom = dims - comp.dimensions;

    for (int x = 0; x < freedom.x; x++) {
        for (int y = 0; y < freedom.y; y++) {
            for (int z = 0; z < freedom.z; z++) {


                for (int x = 0; x < freedom.x; x++)
                {
                    for (int y = 0; y < freedom.y; y++)
                    {
                        for (int z = 0; z < freedom.z; z++)
                        {


                        }
                    }
                }
    }   }   }
    return false;
}*/