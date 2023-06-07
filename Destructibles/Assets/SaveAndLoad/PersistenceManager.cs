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
        this.savefile = handler.Load();
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Several instances detected");
        }

        instance = this;
    }

    public void NewSavefile()
    {
        this.savefile = new SpaceshipSavefile();
    }

    public void LoadSavefile()
    {
        this.savefile = handler.Load();

        if (this.savefile == null)
        {
            Debug.Log("No save file");
            NewSavefile();
        }
        this.spaceship.LoadSavefile(savefile);
    }
    public void SaveSavefile()
    {
        this.spaceship.SaveSavefile(ref savefile);
        //Debug.Log(savefile.listGameobjectType.Count);

        this.handler.Save(savefile);
    }

}
