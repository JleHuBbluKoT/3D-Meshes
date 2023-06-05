using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurableListElement : MonoBehaviour
{


    public GameObject mainPanel;
    public GameObject imageHolder;
    public GameObject nameHolder;
    public GameObject positionHolder;

    public GameObject referencedObject;

    public void SetValues(BlockyComponentInteractive comp, GameObject _mainPanel)
    {
        imageHolder.GetComponent<Image>().sprite = comp.myImage;
        nameHolder.GetComponent<Text>().text = comp.myName;
        positionHolder.GetComponent<Text>().text = comp.gameObject.GetComponent<BlockyComponent>().positionInArray.ToString();
        referencedObject = comp.gameObject;
        this.mainPanel = _mainPanel;
    }
}
