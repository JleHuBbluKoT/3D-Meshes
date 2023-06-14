using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public BasicGameplay parent;


    private void OnDestroy()
    {
        if (parent.points < parent.goal)
        {
            parent.points += 1;
            parent.treasures.Remove(this.gameObject);
            parent.Check();
        }
        
    }
}
