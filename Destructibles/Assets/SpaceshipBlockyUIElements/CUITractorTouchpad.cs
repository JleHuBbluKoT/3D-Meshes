using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUITractorTouchpad : ConfigurableUItile
{
    public ConfigurableUIConnector connector;
    public GameObject touchpad;
    public RectTransform touchPadRect;
    public override void GetConnectorData(ConfigurableUIElementAbstract connector, GameObject listUI = null)
    {

    }

    public override void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;

        DynamicUIAdjustments();
    }

    public void GetTouchPadPoint()
    {   
        Vector2 mouse = Input.mousePosition;
        Vector2 posHalf1 = (mouse - touchPadRect.anchorMin);
        Vector2 posHalf2 = (touchPadRect.anchorMax - touchPadRect.anchorMin);
        Debug.Log(mouse);
        Debug.Log(touchPadRect.anchoredPosition);
        Debug.Log(touchPadRect.sizeDelta.x + " " + touchPadRect.sizeDelta.x);
        Vector2 position = new Vector2(posHalf1.x / posHalf2.x, posHalf1.y / posHalf2.y);
        Debug.Log(position);
    }


    public override void DynamicUIAdjustments()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        float width = Mathf.Min(rect.sizeDelta.x, rect.sizeDelta.y);
        RectTransform tTransform = touchpad.GetComponent<RectTransform>();
        tTransform.sizeDelta = new Vector2(width - 10, width - 10);
        tTransform.anchoredPosition = new Vector2(5,  - 5);

        RectTransform cTransform = connector.GetComponent<RectTransform>();
        cTransform.anchoredPosition = new Vector2(width + 5, - 5);
    }

    public override BlockyComponentInteractive[] ToSavefileData()
    {
        BlockyComponentInteractive[] myComp = new BlockyComponentInteractive[5];
        return myComp;
    }
    public override void LoadDataFromFile(List<int> inter)
    {
        return;
    }


}
