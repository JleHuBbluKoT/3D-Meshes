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

    public override BlockyComponentInteractive[] ToSavefileData()
    {
        BlockyComponentInteractive[] myComp = new BlockyComponentInteractive[5];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            ConfigurableUIConnector conn;
            if (this.transform.GetChild(i).TryGetComponent(out conn))
            {
                if (conn.referencedComponent != null)
                {
                    myComp[0] = conn.referencedComponent.GetComponent<BlockyComponentInteractive>();
                    break;
                }
                
            }
        }
        return myComp;
    }

    public override void LoadDataFromFile(List<int> inter)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            ConfigurableUIConnector conn;
            if (this.transform.GetChild(i).TryGetComponent(out conn))
            {
                conn.referencedComponent = parent.spacesip.allComponents[ inter[0]];
            }
        }

    }
}
