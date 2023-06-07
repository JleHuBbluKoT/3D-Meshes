using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockyComponentInteractive : BlockyComponent
{

    public Sprite myImage;
    public string myName;

    public ConfigurableListElement myListRepresentation;
    public abstract void ComponentAction();
}
