using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockyComponentInteractive : BlockyComponent
{

    public Sprite myImage;
    public string myName;

    public ConfigurableListElement myListRepresentation;
    public abstract void ComponentAction();
    public abstract void DetailVariables();
    public abstract void DetailUpdate();
    public abstract void DetailDelete();
}
