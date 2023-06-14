using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI tile List", menuName = "ScriptableObjects/UITileList")]
public class UITileLibrary : ScriptableObject
{
    public enum detailType
    {
        Menutiles = 0,
        Camera = 1,
        Engine = 2,
        Simple = 3,
        Empty = 4,
    }

    public GameObject menutiles;
    public GameObject camera;
    public GameObject engine;
    public GameObject simple;
    public GameObject empty;


    public GameObject GetPrefabFromNumber(detailType num)
    {
        switch (num)
        {
            case detailType.Menutiles:
                return menutiles;
            case detailType.Camera:
                return camera;
            case detailType.Engine:
                return engine;
            case detailType.Simple:
                return simple;
            case detailType.Empty:
                return empty;
            default:
                return empty;
        }
    }
    public GameObject GetPrefabFromNumber(int num)
    {
        return GetPrefabFromNumber((detailType)num);
    }
}
