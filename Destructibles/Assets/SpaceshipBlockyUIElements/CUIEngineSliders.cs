using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CUIEngineSliders : MonoBehaviour
{
    public Slider controlSlider;
    public RectTransform speedometer;
    public Image speedometerImage;
    public Color speedometerColorZero;
    public Color speedometerColorOne;
    public ConfigurableUIConnector connector;
    public BlockySpaceshipEngines myEngine;
    public float maxHeight;

    public void OnValueChange()
    {
        myEngine.targetForce = Mathf.Lerp(myEngine.minForce, myEngine.maxForce, controlSlider.value);
        //Debug.Log(myEngine.positionInArray);
    }

    public void OnDestroy() {
        if (myEngine != null)
        {
            myEngine.associatedSpeedometer = null;
        }
    }
}
