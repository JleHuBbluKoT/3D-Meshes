using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurableUIConnector : ConfigurableUIElementAbstract
{
    public Color NormalColor;
    public Color ActiveColor;

    public GameObject myImage;
    public GameObject myText;
    // Referenced componenet is here

    public override void OnButtonPress()
    {
        editorCamera.SelectItemUI(this);
    }
    public override void FirstPress()
    {
        //editorCamera.SelectItemUI(this);
        this.GetComponent<Image>().color = ActiveColor;
        Debug.Log("first press");
    }
    public override void SecondPress()
    {
        Debug.Log("Clear element");
        this.GetComponent<Image>().color = NormalColor;
        this.referencedComponent = null;
        editorCamera.DeselectItemUI();
    }
    public override bool ShouldSelectOtherUI(ConfigurableUIElementAbstract otherUI)
    {
        if (otherUI is ConfigurableListElement ){
            this.referencedComponent = otherUI.referencedComponent;
            Debug.Log("Found list element");
            editorCamera.DeselectItemUI();
            this.GetComponent<Image>().color = NormalColor;
            return true;
        }
        return false;
    }
    public override void VoidPress(GameObject comp = null)
    {
        Debug.Log("void press");
        this.GetComponent<Image>().color = NormalColor;
        editorCamera.DeselectItemUI();
    }

    public override void ActivateComponent()
    {
        this.referencedComponent.GetComponent<BlockyComponentInteractive>().ComponentAction();
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
