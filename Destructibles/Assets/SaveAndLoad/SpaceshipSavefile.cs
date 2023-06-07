using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceshipSavefile
{
    public int detailCount;
    public List<int> listGameobjectType;
    public List<Vector3Int> listPosition;
    public List<Vector3Int> listRotation;
    public List<bool> Connected;

    public SpaceshipSavefile()
    {
        detailCount = 0;
        listGameobjectType = new List<int>();
        listPosition = new List<Vector3Int>();
        listRotation = new List<Vector3Int>();
        Connected = new List<bool>();
    }

    public void BreakGameobjects(List<GameObject> list) {
        listGameobjectType.Clear();
        listPosition.Clear();
        listRotation.Clear();
        Connected.Clear();

        for (int i = 0; i < list.Count; i++) {
            listGameobjectType.Add( (int)list[i].GetComponent<BlockyComponent>().type);
            listPosition.Add(list[i].GetComponent<BlockyComponent>().positionInArray);
            listRotation.Add(list[i].GetComponent<BlockyComponent>().myRotation);
            Connected.Add(list[i].GetComponent<BlockyComponent>().Connected);
        }
    }
}
