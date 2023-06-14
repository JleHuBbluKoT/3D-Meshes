using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigurableUItile : MonoBehaviour
{
    public ConfigurableUIMain parent;
    public Vector2Int positionInArray;
    public GameObject defaultButton;
    public UITileLibrary.detailType myType;


    public virtual void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;
        Debug.Log(_parent);
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform tmp = this.transform.GetChild(i);
            tmp.gameObject.GetComponent<ConfigurableUIElementAbstract>().editorCamera = this.parent.editorCamera;
        }
    }

    public virtual void GetConnectorData(ConfigurableUIElementAbstract connector, GameObject listUI = null)
    {
        return;
    }

    public virtual void DefaultSelf()
    {
        parent.ReplaceTile(this, parent.UItile);
    }
    public virtual void LoadFromSave()
    {

    }

    public void OpenList()
    {
        parent.ShowEnginesFromSpaceship();
    }
    public virtual void DynamicUIAdjustments()
    {
        return;
    }
    public virtual void GameStarted()
    {
        return;
    }
    public virtual void BackToRedactor()
    {
        return;
    }
    public abstract BlockyComponentInteractive[] ToSavefileData();
    public abstract void LoadDataFromFile(List<int> inter);
}
