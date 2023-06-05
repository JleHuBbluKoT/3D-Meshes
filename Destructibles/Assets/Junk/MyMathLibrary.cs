using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMathLibrary {



    public static Vector2 LinearEquasion(Vector2 p1, Vector2 p2)
    {
        Vector2 vector = p1 - p2;
        if (vector.magnitude < 0.001)
        {
            return Vector2.zero;
        }
        float Slope = vector.y / vector.x;
        float y_int = p1.y - Slope * p1.x;
        return new Vector2(Slope, -y_int); // Slope * x - y = y_int
    }

    public static Vector2 lineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        //Debug.Log(p1 + " " + p2 + " " + p3 + " " + p4);

        Vector2 line1 = p2 - p1;
        Vector2 line2 = p4 - p3;
        if (line1.magnitude < 0.001 && line2.magnitude < 0.001)
        {
            return Vector2.zero;
        }
        Vector2 equasion1 = LinearEquasion(p1, p2);
        Vector2 equasion2 = LinearEquasion(p3, p4);

        Debug.Log(equasion1 + " " + equasion2);

        float xCoordinate = (equasion1.y - equasion2.y) / (equasion1.x - equasion2.x);
        float yCoordinate = xCoordinate * equasion1.x - equasion1.y;
        return new Vector2(xCoordinate, yCoordinate);
    }



}
