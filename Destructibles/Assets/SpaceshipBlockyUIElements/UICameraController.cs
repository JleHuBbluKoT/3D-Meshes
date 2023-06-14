using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraController : ConfigurableUItile
{
    public SpaceshipCameraDetail cameraDetail;
    public Slider pitchSlider;
    public Slider yawSlider;
    public GameObject floodLight;
    public RawImage cameraFootage;
    public Image cameraBackground;
    public Camera connectedCamera;

    public ConfigurableUIConnector Controller;

    private RectTransform rect;
    private int padding = 5;

    public override void DynamicUIAdjustments()
    {
        rect = this.GetComponent<RectTransform>();
        float tmp = Mathf.Min(rect.sizeDelta.x, rect.sizeDelta.y) - padding * 2;

        float width = tmp * 0.9f ;
        float tinyW = tmp - width;

        RectTransform pSlider = pitchSlider.GetComponent<RectTransform>();
        RectTransform ySlider = yawSlider.GetComponent<RectTransform>();
        RectTransform flButton = floodLight.GetComponent<RectTransform>();
        RectTransform camImage = cameraFootage.GetComponent<RectTransform>();
        RectTransform camB = cameraBackground.GetComponent<RectTransform>();
        RectTransform connector = Controller.gameObject.GetComponent<RectTransform>();

        ySlider.sizeDelta = new Vector2(tinyW, width); ySlider.anchoredPosition = new Vector2(0 + padding, 0 - padding);
        pSlider.sizeDelta = new Vector2(width, tinyW); pSlider.anchoredPosition = new Vector2(tinyW + padding, -width - padding);
        camImage.sizeDelta = new Vector2(width, width); camImage.anchoredPosition = new Vector2(tinyW + padding, 0 - padding);
        camB.sizeDelta = new Vector2(width, width); camB.anchoredPosition = new Vector2(tinyW + padding, 0 - padding);
        flButton.sizeDelta = new Vector2(tinyW, tinyW); flButton.anchoredPosition = new Vector2(0 + padding, -width - padding);
        connector.sizeDelta = new Vector2(tinyW, tinyW); connector.anchoredPosition = new Vector2(tinyW + padding, - padding);
    }


    public void SliderController()
    {
        float p = pitchSlider.value;
        float y = yawSlider.value;
        cameraDetail.SetRotation(p,y);
    }

    public override void GetConnectorData(ConfigurableUIElementAbstract connector, GameObject listUI = null)
    {
        SpaceshipCameraDetail _cameraDetail;
        if (!listUI.TryGetComponent<SpaceshipCameraDetail>(out _cameraDetail)) return;
        this.cameraDetail = _cameraDetail;

        cameraFootage.texture = cameraDetail.myRenderTexture;
        connectedCamera = cameraDetail.myCamera;
    }

    public override void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;
        DynamicUIAdjustments();
        Controller.editorCamera = parent.editorCamera;
    }

    public override void DefaultSelf()
    {
        base.DefaultSelf();
    }

    public void ReplaceSelf(GameObject otherUIelement)
    {
        Debug.Log("hello");
        parent.ReplaceTile(this, otherUIelement);
    }

    public override BlockyComponentInteractive[] ToSavefileData()
    {
        BlockyComponentInteractive[] myComp = new BlockyComponentInteractive[5];
        myComp[0] =cameraDetail;
        return myComp;
    }
    public override void LoadDataFromFile(List<int> inter)
    {
        if (inter[0] != -1)
        {
            this.cameraDetail = parent.spacesip.allComponents[inter[0]].GetComponent<SpaceshipCameraDetail>();
            cameraFootage.texture = cameraDetail.myRenderTexture;
            connectedCamera = cameraDetail.myCamera;


        }
        else
        {
            this.cameraDetail = null;
        }
        
    }
}
