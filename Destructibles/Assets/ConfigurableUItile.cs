using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigurableUItile : MonoBehaviour
{
    public ConfigurableUIMain parent;
    public Vector2Int positionInArray;
    public GameObject defaultButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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


    public void OpenList()
    {
        parent.ShowEnginesFromSpaceship();
    }
}
