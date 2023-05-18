using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator {
    public AsteroidNode[,,] matrix;
    public List<AsteroidPolygon> GenerateCuboid(int length, int height, int width)
    {
        if (length == 0 || height == 0 || width == 0){return null; }
        length++;
        height++;
        width++;
        List<AsteroidNode> vertices = new List<AsteroidNode>();
        this.matrix = new AsteroidNode[length, height, width];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int q = 0; q < width; q++)
                {
                    bool Inside = (i < length -1 & j < height -1 & q < width - 1 & i > 0  & j > 0 & q > 0); // Check if a point is inside the matrix
                    if (!Inside)
                    {
                        matrix[i, j, q] = new AsteroidNode(i, j, q, vertices);
                    }
                }
            }
        }
        length--;
        height--;
        width--;

        List<AsteroidPolygon> polygons = new List<AsteroidPolygon>();

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                polygons.AddRange(
                MakeSquare(matrix[i, j, 0], new Vector3Int(1, 1, 0), new Vector3Int(length, height, width))
                );
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                polygons.AddRange(
                MakeSquare(matrix[0, i, j], new Vector3Int(0, 1, 1), new Vector3Int(length, height, width))
                );
            }
        }

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                polygons.AddRange(
                MakeSquare(matrix[i, 0, j], new Vector3Int(1, 0, 1), new Vector3Int(length, height, width))
                );
            }
        }


        return polygons;

    }

    public AsteroidPolygon[] MakeSquare(AsteroidNode node, Vector3Int dir, Vector3Int matrixSize) // Vector 3 must contain two "1" and one "0" (0,1,1) / (1,0,1) / (1,1,0) 
    {
        AsteroidPolygon[] square = new AsteroidPolygon[4]; // It returns 2 polygons in a shape of a square from one side, and 2 plygons from the opposite side

        AsteroidNode squareNode = MatrixVector(node.position + dir);
        Vector3Int first = new Vector3Int();
        Vector3Int second = new Vector3Int();
        if (dir.x == 0)
        {
            first = new Vector3Int(0, 1, 0);
            second = new Vector3Int(0, 0, 1);
        }
        else if (dir.y == 0)
        {
            first = new Vector3Int(0, 0, 1);
            second = new Vector3Int(1, 0, 0);
        }
        else
        {
            first = new Vector3Int(1, 0, 0);
            second = new Vector3Int(0, 1, 0);
        }

        AsteroidNode firstNode = MatrixVector(node.position + first);
        AsteroidNode secondNode = MatrixVector(node.position + second);
        square[0] = new AsteroidPolygon(node, secondNode, firstNode);
        square[1] = new AsteroidPolygon(squareNode, firstNode, secondNode);

        

        Vector3Int oppositeNodePosition = matrixSize - node.position;
        AsteroidNode oppositeNode = MatrixVector(oppositeNodePosition);

        AsteroidNode oppositeSquareNode = MatrixVector(oppositeNodePosition - dir);
        AsteroidNode oppositeFirst = MatrixVector(oppositeNodePosition - first);
        AsteroidNode oppositeSecond = MatrixVector(oppositeNodePosition - second);

        square[2] = new AsteroidPolygon(oppositeNode, oppositeFirst, oppositeSecond);
        square[3] = new AsteroidPolygon(oppositeSquareNode, oppositeSecond, oppositeFirst);

        Debug.Log(square[0].ToString());
        Debug.Log(square[1].ToString());
        Debug.Log(square[2].ToString());
        Debug.Log(square[3].ToString());

        return square;
    }

    public AsteroidNode MatrixVector(Vector3Int vect)
    {
        return this.matrix[vect.x, vect.y, vect.z];
    }

}

public class AsteroidPolygon
{
    public AsteroidNode[] vertice = new AsteroidNode[3];

    public AsteroidPolygon(AsteroidNode one, AsteroidNode two, AsteroidNode three)
    {
        vertice[0] = one;
        vertice[1] = two;
        vertice[2] = three;
    }

    public override string ToString()
    {
        string vert = "";
        foreach (var vertice in this.vertice)
        {
            vert = vert + vertice.position.ToString();
        }
        return $"[{vertice.Length}] Vertices: {vert}";
    }

}

public class AsteroidNode
{
    public int number;
    public AsteroidNode plusX = null;
    public AsteroidNode minusX = null;
    public AsteroidNode plusY = null;
    public AsteroidNode minusY = null;
    public AsteroidNode plusZ = null;
    public AsteroidNode minusZ = null;
    public Vector3Int position;

    public AsteroidNode(int x, int y, int z, List<AsteroidNode> list)
    {
        this.position = new Vector3Int(x,y,z);
        list.Add(this);
        this.number = list.Count;
        //Debug.Log(position + " " + this.number);
    }
    public AsteroidNode(int x, int y, int z)
    {
        this.position = new Vector3Int(x, y, z);
    }
}


/*
public void Connect(AsteroidNode vert, int length, int height, int width)
{
    Debug.Log(vert.position + " " + vert.number);
    if (vert.position.x < length && vert.minusX != null) //&& matrix[vert.position.x + 1, vert.position.y, vert.position.z] != null
    {
        vert.plusX = matrix[vert.position.x + 1, vert.position.y, vert.position.z];
    }
    if(vert.position.y < height) // && matrix[vert.position.x, vert.position.y + 1, vert.position.z] != null
    {
        vert.plusX = matrix[vert.position.x, vert.position.y + 1, vert.position.z];
    }
    if (vert.position.z < width) // && matrix[vert.position.x, vert.position.y, vert.position.z + 1] != null
    {
        vert.plusX = matrix[vert.position.x, vert.position.y, vert.position.z + 1];
    }
}*/