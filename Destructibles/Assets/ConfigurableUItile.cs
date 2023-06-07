using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableUItile : MonoBehaviour
{
    public ConfigurableUIMain parent;
    public Vector2Int positionInArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform tmp = this.transform.GetChild(i);
            tmp.gameObject.GetComponent<ConfigurableUIElementAbstract>().editorCamera = this.parent.editorCamera;
        }
    }

    public void OpenList()
    {
        parent.ShowEnginesFromSpaceship();
    }
}
