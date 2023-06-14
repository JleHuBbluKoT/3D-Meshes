using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CUITileEngines : ConfigurableUItile
{
    public CUIEngineSliders engineSliders;
    public List<CUIEngineSliders> engineSlidersList = new List<CUIEngineSliders>();


    public override void GetConnectorData(ConfigurableUIElementAbstract connector, GameObject listUI = null)
    {
        BlockySpaceshipEngines engine;
        if (!listUI.TryGetComponent<BlockySpaceshipEngines>(out engine)) return;

        CUIEngineSliders engineSlider = connector.gameObject.transform.parent.gameObject.GetComponent<CUIEngineSliders>();
        engineSlider.myEngine = engine;
        engine.associatedSpeedometer = engineSlider;
    }

    public void Start()
    {
        
    }

    public override void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        this.parent = _parent;
        this.positionInArray = _positionInArray;
        for (int i = 0; i < 5; i++)
        {
            engineSlidersList.Add(Instantiate(engineSliders));
            engineSlidersList[i].transform.SetParent(this.transform);
            engineSlidersList[i].transform.localScale = Vector3.one;
            engineSlidersList[i].connector.editorCamera = this.parent.editorCamera;
            engineSlidersList[i].connector.masterTile = this;
        }
        DynamicUIAdjustments();
    }

    public override void DynamicUIAdjustments()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        float width = rect.sizeDelta.x / engineSlidersList.Count - 5 * engineSlidersList.Count;
        float height = rect.sizeDelta.y;
        for (int i = 0; i < engineSlidersList.Count; i++)
        {
            RectTransform sRect = engineSlidersList[i].GetComponent<RectTransform>();
            sRect.anchoredPosition = new Vector2( i * width + 5, -5);
            RectTransform sSlider = engineSlidersList[i].controlSlider.GetComponent<RectTransform>();
            sSlider.sizeDelta = new Vector2(sSlider.sizeDelta.x, height - 40);

            RectTransform sEnginePowerBackground = engineSlidersList[i].speedometer.parent.GetComponent<RectTransform>();
            sEnginePowerBackground.sizeDelta = new Vector2(sEnginePowerBackground.sizeDelta.x, height - 40);
            engineSlidersList[i].maxHeight = height - 40 + 1;

            RectTransform sConn = engineSlidersList[i].connector.GetComponent<RectTransform>();
            sConn.anchoredPosition = new Vector2(0, -height+40);

            RectTransform defConn = defaultButton.GetComponent<RectTransform>();
            defConn.anchoredPosition = new Vector2(0,0);
        }
    }
    public void OnDestroy()
    {
        int failsafe = 0;
        while (engineSlidersList.Count > 0 && failsafe < 50)
        {
            failsafe++;
            Destroy(engineSlidersList[0]);

        }
    }
    public override void GameStarted()
    {
        this.defaultButton.SetActive(false);
        foreach (var slider in engineSlidersList)
        {
            slider.connector.gameObject.SetActive(false);
        }
    }
    public override void BackToRedactor()
    {
        this.defaultButton.SetActive(true);
        foreach (var slider in engineSlidersList)
        {
            slider.connector.gameObject.SetActive(true);
        }
    }


}