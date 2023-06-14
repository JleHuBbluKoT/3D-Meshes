using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigurableUIElementAbstract : MonoBehaviour
{
    public ConfigurableUItile masterTile;
    public GameObject referencedComponent;
    public MoveCamera editorCamera;
    public abstract void OnButtonPress();
    public abstract void FirstPress(); // First button press
    public abstract void SecondPress(); // Second button press
    public abstract void VoidPress(GameObject comp = null); // Press on the screen that is not UI
    public abstract bool ShouldSelectOtherUI(ConfigurableUIElementAbstract otherUI);
    public abstract void ActivateComponent();
    // The idea behind this:
    // If returns true, this ThisUI element should try to use OtherUI element, otherUI is not activated
    // If returns false, ThisUI element does nothing, and OtherUI element is called via First Press or other function

    //public abstract void OtherUIInteraction(); Will keep it perhaps i can realize all i want inside ShouldSelectOtherUI()


}
