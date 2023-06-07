using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableScroller : MonoBehaviour
{
    public MoveCamera editorCamera;

    public GameObject mainPanel;
    public GameObject scroller;
    public GameObject scrollableSpace;
    public List<GameObject> scrollableContents = new List<GameObject>();

    public GameObject listElementPrefab;

    public void Start()
    {

    }

    public void RemoveElement(GameObject element)
    {
        scrollableContents.Remove(element);
        Destroy(element);
    }

    public void FillList(List<BlockyComponentInteractive> list)
    {
        ClearList();
        for (int i = 0; i < list.Count; i++)
        {
            AddListElement(list[i]);
        }
    }

    public void ClearList()
    {
        if (scrollableContents.Count == 0) return;

        for (int i = 0; i < scrollableContents.Count; i++)
        {
            Destroy( scrollableContents[i]);
        }
        scrollableContents.Clear();
    }
    public void AddListElement(BlockyComponentInteractive comp)
    {
        GameObject newUIElement = Instantiate(listElementPrefab);
        newUIElement.transform.SetParent(scrollableSpace.transform);
        scrollableContents.Add(newUIElement);
        newUIElement.transform.localScale = Vector3.one;

        newUIElement.GetComponent<ConfigurableListElement>().SetValues(comp, this.mainPanel, this);
        //this.scrollableSpace;
    }
}
