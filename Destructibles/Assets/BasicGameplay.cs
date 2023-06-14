using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGameplay : MonoBehaviour
{
    public BlockySpaceship spaceship;
    public List<Transform> spawnPoints;
    public List<GameObject> treasures = new List<GameObject>();
    public GameObject prefab;
    public GameObject winScreen;
    public int points;
    public int goal;
    public void StartGame()
    {
        treasures = new List<GameObject>();

        foreach (var point in spawnPoints)
        {
            GameObject tmp = Instantiate(prefab);
            tmp.transform.position = point.position;
            tmp.GetComponent<Treasure>().parent = this;
            treasures.Add(tmp);
        }
        goal = spawnPoints.Count - 1;
    }
    public void EndGame()
    {
        winScreen.SetActive(false);
        int failsafe = 0;
        while (failsafe < 50 && treasures.Count > 0)
        {
            failsafe++;
            Destroy(treasures[0]);
            treasures.RemoveAt(0);
        }
        treasures = new List<GameObject>();
    }

    public void Check()
    {
        if (points >= goal)
        {
            Debug.Log("Hooray!");
            winScreen.SetActive(true);
        }
    }

}
