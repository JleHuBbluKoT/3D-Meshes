using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Parts List", menuName = "ScriptableObjects/PartList")]
public class BlockyPartsLibrary : ScriptableObject
{
    public enum detailType
    {
        Cockpit = 0,
        Floor = 1,
        Wall = 2,
        FloorBig = 3,
        Engine = 4,
        Camera = 5,
        Tractor = 6,
        Light = 7,
        Generator = 8
    }

    public GameObject cockpit;
    public GameObject floor;
    public GameObject wall;
    public GameObject floorBig;
    public GameObject engine;
    public GameObject camera;
    public GameObject tractor;
    public GameObject light;
    public GameObject generator;

    public GameObject GetPrefabFromNumber(detailType num)
    {
        switch (num)
        {
            case detailType.Cockpit:
                return cockpit;
            case detailType.Floor:
                return floor;
            case detailType.Wall:
                return wall;
            case detailType.FloorBig:
                return floorBig;
            case detailType.Engine:
                return engine;
            case detailType.Camera:
                return camera;
            case detailType.Tractor:
                return tractor;
            case detailType.Light:
                return light;
            case detailType.Generator:
                return generator;
            default:
                return floor;
        }
    }
    public GameObject GetPrefabFromNumber(int num)
    {
        return GetPrefabFromNumber((detailType)num);
    }
}
