using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockyComponentInteractive : MonoBehaviour
{
    public enum detailType
    {
        Engine = 0,
        Camera = 1,
        Tractor = 2,
        Light = 3
    }

    public Sprite myImage;
    public detailType myType;
    public string myName;

    public ConfigurableListElement myListRepresentation;
    public abstract void ComponentAction();
}
