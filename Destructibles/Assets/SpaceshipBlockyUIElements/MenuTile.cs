using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTile : ConfigurableUItile
{
    public BlockySpaceship spaceship;

    public override void SetVariables(ConfigurableUIMain _parent, Vector2Int _positionInArray)
    {
        return;
    }
    public override void GetConnectorData(ConfigurableUIElementAbstract connector, GameObject listUI = null)
    {
        return;
    }

    public void EndMission()
    {
        spaceship.UIGameEnd();
    }

    public override BlockyComponentInteractive[] ToSavefileData()
    {
        BlockyComponentInteractive[] myComp = new BlockyComponentInteractive[5];
        return myComp;
    }
    public override void LoadDataFromFile(List<int> inter)
    {
        return;
    }
}
