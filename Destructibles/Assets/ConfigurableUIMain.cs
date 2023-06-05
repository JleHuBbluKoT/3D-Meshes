using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ConfigurableUIMain : MonoBehaviour
{
    public BlockySpaceship spacesip;
    public RectTransform mainPanel;

    public ConfigurableScroller scroller;

    public GameObject UItile;

    public Vector2Int dimensions;
    public GameObject[,] uiGrid;
    public Vector2 tileSize;
    public Vector2 padding;
    // Start is called before the first frame update
    void Start()
    {
        FillUI(dimensions);
    }

    public void FillUI(Vector2Int dimensions)
    {
        if (dimensions.x == 0 || dimensions.y == 0)
        {
            return;
        }

        uiGrid = new GameObject[dimensions.x, dimensions.y];
        tileSize = new Vector2(mainPanel.rect.width / dimensions.x, mainPanel.rect.height / dimensions.y);

        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                GameObject newUIelement = Instantiate(UItile);
                newUIelement.transform.SetParent(this.transform);
                SetSizeAndPosition(new Vector2Int(i, j), newUIelement);
            }
        }
    }

    public void SetSizeAndPosition(Vector2Int pos, GameObject UIElement)
    {
        RectTransform uirect = UIElement.GetComponent<RectTransform>();

        uirect.anchoredPosition = Vector2.zero + new Vector2(tileSize.x, -tileSize.y) / 2 + new Vector2(tileSize.x * pos.x, tileSize.y * pos.y) ;
        uirect.sizeDelta = new Vector2( tileSize.x, tileSize.y ) - padding;
        UIElement.transform.localScale = Vector3.one;

        UIElement.GetComponent<ConfigurableUItile>().SetVariables(this, pos);
    }

    public void ShowEnginesFromSpaceship()
    {
        this.scroller.FillList(spacesip.GetEngines());
    }

}
