using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipGenerator : MonoBehaviour
{
    public BlockySpaceship spaceship;
    public BlockyPartsLibrary Library;
    public void SillyRandomFill()
    {
        int failsafe = 0;
        while (spaceship.allComponents.Count + spaceship.invalidComponents.Count < 40 && failsafe < 200)
        {
            failsafe++;
            int x = Random.Range(0, spaceship.dimensions.x);
            int y = Random.Range(0, spaceship.dimensions.y);
            int z = Random.Range(0, spaceship.dimensions.z);
            int rx = Mathf.Clamp( Random.Range(0, 12) - 8, 0, 3 );
            int ry = Mathf.Clamp(Random.Range(0, 12) - 8, 0, 3);
            int rz = Mathf.Clamp(Random.Range(0, 12) - 8, 0, 3);
            GameObject detail = spaceship.AddBigRotatedDetail(SillyRandomDetail(), x, y, z, rx*90, ry*90, rz*90);


        }
    }

    public GameObject SillyRandomDetail()
    {
        int comp = Random.Range(0, 4);
        switch (comp)
        {
            case 0:
                return Library.wall;
            case 1:
                return Library.floor;
            case 2:
                return Library.engine;
            case 3:
                return Library.generator;
            default:
                return Library.wall;
        }
    }

}
