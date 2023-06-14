using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ConfigurableUIMain : MonoBehaviour
{
    public MoveCamera editorCamera;
    public BlockySpaceship spacesip;
    public RectTransform mainPanel;

    public ConfigurableScroller scroller;

    public GameObject UItile;
    public GameObject MenuTile;

    public Vector2Int dimensions;
    public GameObject[,] uiGrid;
    public Vector2 tileSize;
    public Vector2 padding;

    public Vector2 active;
    public Vector2 inactive;

    public UITileLibrary lib;

    // Start is called before the first frame update
    void Start()
    {
        uiGrid = new GameObject[dimensions.x, dimensions.y];
        tileSize = new Vector2(mainPanel.rect.width / dimensions.x, mainPanel.rect.height / dimensions.y);
    }
    public void FillUI()
    {
        if (dimensions.x == 0 || dimensions.y == 0)  return;
        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                GameObject newUIelement = Instantiate(UItile);
                newUIelement.transform.SetParent(this.transform);
                this.uiGrid[i, j] = newUIelement;
                SetSizeAndPosition(new Vector2Int(i, j), newUIelement);
            }
        }
        ReplaceTile(this.uiGrid[dimensions.x - 1, dimensions.y -1 ].GetComponent<ConfigurableUItile>(), MenuTile);
        this.uiGrid[dimensions.x - 1, dimensions.y - 1].GetComponent<MenuTile>().spaceship = this.spacesip;
    }

    public void SetSizeAndPosition(Vector2Int pos, GameObject UIElement)
    {
        //Debug.Log(UIElement);
        RectTransform uirect = UIElement.GetComponent<RectTransform>();

        uirect.anchoredPosition = Vector2.zero + new Vector2(tileSize.x, -tileSize.y) / 2 + new Vector2(tileSize.x * pos.x, -tileSize.y * pos.y) ;
        uirect.sizeDelta = new Vector2( tileSize.x, tileSize.y ) - padding;
        UIElement.transform.localScale = Vector3.one;

        UIElement.GetComponent<ConfigurableUItile>().SetVariables(this, pos);
    }

    public void ReplaceTile(ConfigurableUItile original, GameObject UIElement)
    {
        GameObject newUIelement = Instantiate(UIElement);
        newUIelement.transform.SetParent(this.transform);
        
        RectTransform uirect = newUIelement.GetComponent<RectTransform>();
        uirect.anchoredPosition = Vector2.zero + new Vector2(tileSize.x, -tileSize.y) / 2 + new Vector2(tileSize.x * original.positionInArray.x, -tileSize.y * original.positionInArray.y);
        uirect.sizeDelta = new Vector2(tileSize.x, tileSize.y) - padding;

        newUIelement.transform.localScale = Vector3.one;

        newUIelement.GetComponent<ConfigurableUItile>().SetVariables(this, original.positionInArray);
        this.uiGrid[original.positionInArray.x, original.positionInArray.y] = null;
        this.uiGrid[original.positionInArray.x, original.positionInArray.y] = newUIelement;
        Destroy(original.gameObject);
        
    }

    public void MoveTilesToOtherInstance(ConfigurableUIMain otherInstance)
    {
        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                Destroy(otherInstance.uiGrid[i, j]);
                otherInstance.uiGrid[i, j] = this.uiGrid[i, j];
                otherInstance.uiGrid[i, j].transform.SetParent(otherInstance.transform);
                otherInstance.uiGrid[i, j].transform.localScale = Vector3.one;

                RectTransform uirect = otherInstance.uiGrid[i, j].GetComponent<RectTransform>();
                uirect.anchoredPosition = Vector2.zero + new Vector2(otherInstance.tileSize.x, -otherInstance.tileSize.y) / 2 + new Vector2(otherInstance.tileSize.x * i, -otherInstance.tileSize.y * j);
                uirect.sizeDelta = new Vector2(otherInstance.tileSize.x, otherInstance.tileSize.y) - padding;

                otherInstance.uiGrid[i, j].GetComponent<ConfigurableUItile>().parent = otherInstance;
                otherInstance.uiGrid[i, j].GetComponent<ConfigurableUItile>().DynamicUIAdjustments();
            }
        }
    }

    public void ShowEnginesFromSpaceship()
    {
        this.scroller.FillList(spacesip.GetEngines());
    }
    public void ShowCamerasFromSpaceship()
    {
        this.scroller.FillList(spacesip.GetCameras());
    }
    public void WipeTable()
    {
        if (uiGrid != null)
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    Destroy(uiGrid[i, j]);
                    uiGrid[i, j] = null;

                }
            }
        }
        uiGrid = new GameObject[dimensions.x, dimensions.y];
    }

    public List<ConfigurableUItile> GetMyTiles()
    {
        List<ConfigurableUItile> list = new List<ConfigurableUItile>();
        for (int i = 0; i < dimensions.x; i++)
        {
            for (int j = 0; j < dimensions.y; j++)
            {
                list.Add(uiGrid[i,j].GetComponent<ConfigurableUItile>());
            }
        }
        return list;
    }

    public void LoadMyTiles(SpaceshipSavefile savefile)
    {
        WipeTable();
        for (int i = 0; i < savefile.UItile.Count; i++)
        {
            Debug.Log(savefile.assocDetails.Count);
            GameObject newUIelement = Instantiate(lib.GetPrefabFromNumber(savefile.UItile[i]));
            newUIelement.transform.SetParent(this.transform);
            Vector2Int pos = new Vector2Int(i / dimensions.x, i % dimensions.x);
            this.uiGrid[pos.x, pos.y] = newUIelement;
            SetSizeAndPosition(new Vector2Int(pos.x, pos.y), newUIelement);

            List<int> values = new List<int>();
            values.Add(savefile.assocDetails[i * 5 + 0]); values.Add(savefile.assocDetails[i * 5 + 1]);
            values.Add(savefile.assocDetails[i * 5 + 2]); values.Add(savefile.assocDetails[i * 5 + 3]);
            values.Add(savefile.assocDetails[i * 5 + 4]);

            newUIelement.GetComponent<ConfigurableUItile>().LoadDataFromFile(values);
        }

    }

}
