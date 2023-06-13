using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUITileEmptyTile : ConfigurableUItile
{
    public override void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;
    }
    public void ReplaceSelf(GameObject otherUIelement)
    {
        Debug.Log("hello");
        parent.ReplaceTile(this, otherUIelement);
    }
}
