using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigarableUIButton : ConfigurableUIElementAbstract
{
    // Referenced componenet is here
    public GameObject myText;

    public override void OnButtonPress()
    {
        ActivateComponent();
        //editorCamera.SelectItemUI(this);
    }
    public override void FirstPress()
    {
        Debug.Log("first press");

    }
    public override void SecondPress()
    {
        Debug.Log("second press");
        editorCamera.DeselectItemUI();
    }
    public override bool ShouldSelectOtherUI(ConfigurableUIElementAbstract otherUI)
    {
        Debug.Log("other ui press");
        editorCamera.DeselectItemUI();
        return false;
    }
    public override void VoidPress(GameObject comp = null)
    {
        Debug.Log("void press");
        editorCamera.DeselectItemUI();
    }
    public override void ActivateComponent()
    {
        this.referencedComponent.GetComponent<ConfigurableUIConnector>().ActivateComponent();
    }



    public void DeleteSelf()
    {
        //scroller.RemoveElement(this.gameObject);
    }

    public void UpdateValues(BlockyComponentInteractive comp)
    {
        //imageHolder.GetComponent<Image>().sprite = comp.myImage;
    }

    public void SetValues(BlockyComponentInteractive comp, GameObject _mainPanel, ConfigurableScroller _scroller)
    {
        /*
        imageHolder.GetComponent<Image>().sprite = comp.myImage;
        nameHolder.GetComponent<Text>().text = comp.myName;
        positionHolder.GetComponent<Text>().text = comp.gameObject.GetComponent<BlockyComponent>().positionInArray.ToString();
        referencedObject = comp.gameObject;
        scroller = _scroller;
        this.editorCamera = _scroller.editorCamera;

        comp.myListRepresentation = this;

        this.mainPanel = _mainPanel;*/
    }

}
