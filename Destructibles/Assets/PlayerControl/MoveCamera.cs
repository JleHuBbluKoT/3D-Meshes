using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class MoveCamera : MonoBehaviour
{
    public GameObject spaceship;
    public Camera myCamera;
    public Transform cameraPosition;
    public Transform orientation;


    float horizontalInput;
    float verticalInput;
    float UpDown;
    float RunCrouch = 1f;
    Vector3 moveDirection;


    public LayerMask spaceshipComponents;
    public LayerMask spaceshipUI;
    public LayerMask justUI;

    public op currentOperation = op.Nothing;
    private GameObject target;
    public GameObject buildingBlock;

    public GraphicRaycaster graphic;
    private PointerEventData pointerRR = new PointerEventData(null);

    public ConfigurableUIElementAbstract UItarget;
    public ConfigurableUIElementAbstract secondaryUItarget;
    public enum op//operation
    {
        Nothing = 0,
        Build = 1,
        Delete = 2,
        Drag = 3,
        Edit = 4,
        UIInteraction = 5,
        UISetButtonValue = 6
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        UpDown = Input.GetAxisRaw("UpDown");
        if (Input.GetKey(KeyCode.X))
        {
            RunCrouch = 1.6f;
        }
        else
        {
            RunCrouch = 1f;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput * RunCrouch + orientation.right * horizontalInput + Vector3.up * UpDown * 0.8f ;
        cameraPosition.position = cameraPosition.position + moveDirection * 0.08f;
        transform.position = cameraPosition.position;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void SelectItem() {
        //Debug.Log("hi");
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 100f;
        
        Ray ray = myCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // UI check
        pointerRR.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphic.Raycast(pointerRR, results);
        if (results.Count > 0) {
            if (currentOperation == op.UIInteraction) {
                ConfigurableUIElementAbstract comp;
                for (int i = 0; i < results.Count; i++) {
                    if (results[i].gameObject.TryGetComponent<ConfigurableUIElementAbstract>(out comp)) {
                        return;
                    }
                }
                SelectItemUI(null, null);
            }
            return;
        }
        // UI check end
        // Check if there is an active UI element, perhaps it wants camera to shoot a raycast to select an object
        if (currentOperation == op.UIInteraction)
        {
            Physics.Raycast(ray, out hit, maxDistance: 200f, layerMask: spaceshipComponents);
            if (hit.collider != null){
                SelectItemUI(null, hit.collider.gameObject);
            }
            else {
                SelectItemUI(null, null);
            }
            return;
        }
        // End

        Physics.Raycast(ray, out hit, maxDistance: 200f, layerMask: spaceshipComponents);
        if (hit.collider != null &&  hit.collider.gameObject.layer == 5) { Debug.Log("UI got hit"); return; }

        if (currentOperation == op.Build && hit.collider != null)
        {
            Vector3Int neigh = spaceship.GetComponent<BlockySpaceship>().GetAdjacentSpace(hit.point, hit.collider.gameObject);
            spaceship.GetComponent<BlockySpaceship>().AddBigDetail(buildingBlock, neigh.x, neigh.y, neigh.z);
        }
        if (currentOperation == op.Delete && hit.collider != null)
        {
            spaceship.GetComponent<BlockySpaceship>().DeleteDetail(hit.collider.gameObject);
        }
        if (currentOperation == op.Edit)
        {
            ChangeTarget(hit.collider);
        } 
    }

    public void ChangeTarget(Collider go)
    {
        GameObject previousTarget = target;
        if (previousTarget != null)
        {
            spaceship.GetComponent<BlockySpaceship>().DeselectDetail(previousTarget);
        }
        
        if (go == null)
        {
            target = null;
        }
        if (go != null)  {
            this.target = go.gameObject;
            spaceship.GetComponent<BlockySpaceship>().SelectDetail(target);
        }
        //Debug.Log(target);
    }
    public void DeselectItemUI()
    {
        UItarget = null;
        currentOperation = op.Nothing;
    }

    public void SelectItemUI(ConfigurableUIElementAbstract newUItarget, GameObject comp = null) // Indentify which operation it is
    {
        if (UItarget == null)
        {
            currentOperation = op.UIInteraction;
            UItarget = newUItarget;
            UItarget.FirstPress();
            return;
        }
        if (UItarget == newUItarget && newUItarget != null)
        {
            currentOperation = op.UIInteraction;
            UItarget = newUItarget;
            UItarget.SecondPress();
            return;
        }
        if (UItarget != newUItarget && newUItarget != null)
        {
            Debug.Log("move camera should select");
            if (!UItarget.ShouldSelectOtherUI(newUItarget))
            {
                currentOperation = op.UIInteraction;
                UItarget = null;
                SelectItemUI(newUItarget);
            }
            return;
        }
        if (newUItarget == null)
        {
            UItarget.VoidPress(comp);
        }
    }

    public void ChangeBlock(GameObject prefab)
    {
        this.buildingBlock = prefab;
    }
    public void ChangeAction(int action)
    {
        switch (action)
        {
            case 1:
                this.currentOperation = op.Build; break;
            case 2:
                this.currentOperation = op.Delete; break;
            case 3:
                this.currentOperation = op.Drag; break;
            case 4:
                this.currentOperation = op.Edit; break;
            default:
                this.currentOperation = op.Nothing; break;
        }
    }


    void OnGUI()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseUp)
        {
            //Debug.Log(currentOperation);
            if (currentOperation != op.UIInteraction || currentOperation != op.UISetButtonValue)
            {
                SelectItem();
            }
            else
            {
                currentOperation = op.Nothing;
            }
        }
    }

    public void MoveTargetUp()
    {
        Debug.Log(target);
        if (target != null)
        {
            spaceship.GetComponent<BlockySpaceship>().MoveComponent(target, Vector3Int.up);
        }
    }
    public void MoveTargetDown()
    {
        if (target != null)
            spaceship.GetComponent<BlockySpaceship>().MoveComponent(target, Vector3Int.down);
    }
    public void MoveTargetForward()
    {

        if (target != null)

            spaceship.GetComponent<BlockySpaceship>().MoveComponent(target, Vector3Int.forward);

    }
    public void MoveTargetBack()
    {
        if (target != null)

            spaceship.GetComponent<BlockySpaceship>().MoveComponent(target, Vector3Int.back);

    }
    public void MoveTargetRight()
    {
        if (target != null)

            spaceship.GetComponent<BlockySpaceship>().MoveComponent(target, Vector3Int.right);

    }
    public void MoveTargetLeft()
    {
        if (target != null)

            spaceship.GetComponent<BlockySpaceship>().MoveComponent(target, Vector3Int.left);
    }
    public void RotateX()
    {
        if (target != null)
        spaceship.GetComponent<BlockySpaceship>().AndRotate(target, 90, 0, 0);
    }
    public void RotateY()
    {
        if (target != null)
            spaceship.GetComponent<BlockySpaceship>().AndRotate(target, 0, 90, 0);
    }
    public void RotateZ()
    {
        if (target != null)
            spaceship.GetComponent<BlockySpaceship>().AndRotate(target, 0, 0, 90);
    }


    /*
    private void BeginDrag()
    {
    Vector3 mousePosition = Input.mousePosition;
    Ray ray = myCamera.ScreenPointToRay(mousePosition);
    RaycastHit hit;


    if (Physics.Raycast(ray, out hit, maxDistance: 100f, layerMask: spaceshipUI))
    {
        Debug.DrawRay(myCamera.transform.position, hit.point - myCamera.transform.position, Color.green, 10f);
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<SpaceshipUIDraggers>().StartCoroutineDrag(hit.collider.gameObject, myCamera);
        }
    }
    }
    */

}
