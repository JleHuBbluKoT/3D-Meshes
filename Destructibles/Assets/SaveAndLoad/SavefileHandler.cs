using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SavefileHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public SavefileHandler(string _dataDirPath, string _dataFileName)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
    }
    public SpaceshipSavefile Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        SpaceshipSavefile savefile = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string datatoload = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datatoload = reader.ReadToEnd();
                    }
                }
                savefile = JsonUtility.FromJson<SpaceshipSavefile>(datatoload);

            }
            catch (Exception e)
            {
                Debug.Log("Something went wrong while loading file: " + fullPath + "\n" + e);
            }
        }
        Debug.Log(Path.GetFullPath(fullPath));
        return savefile;
    }
    public void Save(SpaceshipSavefile data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("Something went wrong while saving file: " + fullPath + "\n" + e);
        }



    }

}
