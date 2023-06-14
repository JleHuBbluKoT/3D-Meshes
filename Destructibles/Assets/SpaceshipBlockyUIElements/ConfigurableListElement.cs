using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurableListElement : ConfigurableUIElementAbstract
{

    public Color NormalColor;
    public Color ActiveColor;

    public ConfigurableScroller scroller;
    public GameObject mainPanel;
    public GameObject imageHolder;
    public GameObject nameHolder;
    public GameObject positionHolder;

    // Referenced componenet is here 


    public override void OnButtonPress()
    {
        editorCamera.SelectItemUI(this);
    }
    public override void FirstPress()
    {
        Debug.Log("first press");
        this.GetComponent<Image>().color = ActiveColor;
        referencedComponent.GetComponent<BlockyComponent>().spaceShip.SelectDetail(referencedComponent);
    }
    public override void SecondPress()
    {
        Debug.Log("second press");
        this.GetComponent<Image>().color = NormalColor;
        referencedComponent.GetComponent<BlockyComponent>().spaceShip.DeselectDetail(referencedComponent);
        editorCamera.DeselectItemUI();
    }
    public override bool ShouldSelectOtherUI(ConfigurableUIElementAbstract otherUI)
    {
        Debug.Log("other ui press");
        this.GetComponent<Image>().color = NormalColor;
        referencedComponent.GetComponent<BlockyComponent>().spaceShip.DeselectDetail(referencedComponent);
        editorCamera.DeselectItemUI();
        return false;
    }
    public override void VoidPress(GameObject comp = null)
    {
        Debug.Log("void press");
        this.GetComponent<Image>().color = NormalColor;
        referencedComponent.GetComponent<BlockyComponent>().spaceShip.DeselectDetail(referencedComponent);
        editorCamera.DeselectItemUI();
    }
    public override void ActivateComponent()
    {
        //this.referencedComponent.GetComponent<ConfigurableUIConnector>().ActivateComponent();
    }


    public void DeleteSelf()
    {
        scroller.RemoveElement(this.gameObject);
    }
    
    public void UpdateValues(BlockyComponentInteractive comp)
    {
        imageHolder.GetComponent<Image>().sprite = comp.myImage;
        nameHolder.GetComponent<Text>().text = comp.myName;
        positionHolder.GetComponent<Text>().text = comp.gameObject.GetComponent<BlockyComponent>().positionInArray.ToString();
    }

    public void SetValues(BlockyComponentInteractive comp, GameObject _mainPanel, ConfigurableScroller _scroller)
    {
        imageHolder.GetComponent<Image>().sprite = comp.myImage;
        nameHolder.GetComponent<Text>().text = comp.myName;
        positionHolder.GetComponent<Text>().text = comp.gameObject.GetComponent<BlockyComponent>().positionInArray.ToString();
        referencedComponent = comp.gameObject;
        scroller = _scroller;
        this.editorCamera = _scroller.editorCamera;

        comp.myListRepresentation = this;

        this.mainPanel = _mainPanel;
    }
}
