using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


[System.Serializable]
public class SpaceshipSavefile
{
    // Data
    public List<int> configurationLength;
    public List<int> blockID;
    public List<string> blockName;
    // Spaceship parts in general
    public List<int> listGameobjectType;
    public List<Vector3Int> listPosition;
    public List<Vector3Int> listRotation;
    public List<int> listMass;
    public List<int> listEnergy;
    // engines
    public List<Vector3> engineOrientation;
    public List<float> enginePower;
    // the other stuff
    public List<bool> Connected;


    public SpaceshipSavefile()
    {
        configurationLength = new List<int>();
        blockID = new List<int>();
        blockName = new List<string>();

        listGameobjectType = new List<int>();
        listPosition = new List<Vector3Int>();
        listRotation = new List<Vector3Int>();
        listMass = new List<int>();
        listEnergy = new List<int>();

        engineOrientation = new List<Vector3>();
        enginePower = new List<float>();

        Connected = new List<bool>();
    }

    public void BreakGameobjects(List<GameObject> list) {
        blockID.Clear();
        blockName.Clear();
        Array fff = Enum.GetValues(typeof(BlockyPartsLibrary.detailType));
        for (int i = 0; i < fff.Length; i++)
        {
            blockID.Add((int)fff.GetValue(i));
            blockName.Add(fff.GetValue(i).ToString());
        }
        Debug.Log("i am here");

        this.configurationLength.Add(list.Count);
        for (int i = 0; i < list.Count; i++) {
            listGameobjectType.Add( (int)list[i].GetComponent<BlockyComponent>().type);
            listPosition.Add(list[i].GetComponent<BlockyComponent>().positionInArray);
            listRotation.Add(list[i].GetComponent<BlockyComponent>().myRotation);
            listMass.Add(list[i].GetComponent<BlockyComponent>().mass);
            listEnergy.Add(list[i].GetComponent<BlockyComponent>().energy);

            BlockySpaceshipEngines engine;
            if (list[i].TryGetComponent<BlockySpaceshipEngines>(out engine) )
            {
                engineOrientation.Add( engine.orientation);
                enginePower.Add(engine.force);
            }
            else
            {
                engineOrientation.Add(Vector3.zero);
                enginePower.Add(0);
            }
            //Debug.Log(list.Count);
            Connected.Add(list[i].GetComponent<BlockyComponent>().Connected);
        }

        //Debug.Log(this.listConfigurations[0].listGameobjectType[0]);
    }

    public void ClearList()
    {
        //listConfigurations.Clear();
    }
}

/*
public struct SpaceshipConfiguration {
    public List<int> listGameobjectType;
    public List<Vector3Int> listPosition;
    public List<Vector3Int> listRotation;
    public List<int> listMass;
    public List<int> listEnergy;

    public List<Vector3> engineOrientation;
    public List<float> enginePower;

    public List<bool> Connected;

    public SpaceshipConfiguration(List<int> _listGameobjectType,
     List<Vector3Int> _listPosition,
     List<Vector3Int> _listRotation,
     List<int> _listMass,
     List<int> _listEnergy,

     List<Vector3> _engineOrientation,
     List<float> _enginePower,

     List<bool> _Connected)
    {
        listGameobjectType = _listGameobjectType;
        listPosition = _listPosition;
        listRotation = _listRotation;
        listMass = _listMass;
        listEnergy = _listEnergy;

        engineOrientation = _engineOrientation;
        enginePower = _enginePower;

        Connected = _Connected;
    }
}*/