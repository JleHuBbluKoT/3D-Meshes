using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUITileEmptyTile : ConfigurableUItile
{
    public override void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            ConfigurableUIConnector conn;
            if (this.transform.GetChild(i).TryGetComponent(out conn))
            {
                conn.editorCamera = this.parent.editorCamera;
            }
        }
    }
    public void ReplaceSelf(GameObject otherUIelement)
    {
        Debug.Log("hello");
        parent.ReplaceTile(this, otherUIelement);
    }
}
