using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    [SerializeField] private string filename;

    private SpaceshipSavefile savefile;
    public BlockySpaceship spaceship;
    public SavefileHandler handler;
    public static PersistenceManager instance { get; private set; }


    private void Start()
    {
        this.handler = new SavefileHandler(Application.persistentDataPath, filename);
        NewSavefile(false);
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Several instances detected");
        }
        instance = this;
    }

    public void NewSavefile(bool newOne = false)
    {
        if (this.savefile == null | newOne)
        {
            this.savefile = new SpaceshipSavefile();
        }
    }

    public void LoadSavefile()
    {
        this.savefile = handler.Load();

        if (this.savefile == null)
        {
            Debug.Log("No save file");
            NewSavefile(true);
        }
        this.spaceship.LoadSavefile(savefile);
    }
    public void SaveSavefile()
    {
        this.spaceship.SaveSavefile(ref savefile);
        //Debug.Log(savefile.listGameobjectType.Count);
        //Debug.Log(savefile.listConfigurations.Count);

        this.handler.Save(savefile);
    }

}
