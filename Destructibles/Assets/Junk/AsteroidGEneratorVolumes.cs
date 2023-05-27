using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGEneratorVolumes
{
    public AsteroidNode[,,] matrix;
    public List<AsteroidPolygon> polygons;
    public List<AsteroidNode> vertices;
    public void GenerateCuboid(int length, int height, int width)
    {
        if (length == 0 || height == 0 || width == 0) { return; }
        length++;
        height++;
        width++;
        this.vertices = new List<AsteroidNode>();
        this.matrix = new AsteroidNode[length, height, width];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int q = 0; q < width; q++)
                {
                    bool Inside = (i < length - 1 & j < height - 1 & q < width - 1 & i > 0 & j > 0 & q > 0); // Check if a point is inside the matrix
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

        this.polygons = new List<AsteroidPolygon>();

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
    }

    public static List<Polygon> GenerateCuboidBSPCompatible(int length, int height, int width, Vector3Int origin)
    {
        int x = (int)(-length / 2);
        int y = (int)(-length / 2);
        int z = (int)(-length / 2);
        origin = origin + new Vector3Int(x,y,z);
        List<Polygon> polygons = new List<Polygon>();
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                List<Vector3> vertices = new List<Vector3>();
                vertices.Add(new Vector3(i + 0, j + 0, 0) + origin);
                vertices.Add(new Vector3(i + 0, j + 1, 0) + origin);
                vertices.Add(new Vector3(i + 1, j + 1, 0) + origin);
                vertices.Add(new Vector3(i + 1, j + 0, 0) + origin);
                polygons.Add(new Polygon(vertices));

                List<Vector3> oppositeVertices = new List<Vector3>();
                oppositeVertices.Add(new Vector3(length - i - 0, height - j - 0, width) + origin);
                oppositeVertices.Add(new Vector3(length - i - 1, height - j - 0, width) + origin);
                oppositeVertices.Add(new Vector3(length - i - 1, height - j - 1, width) + origin);
                oppositeVertices.Add(new Vector3(length - i - 0, height - j - 1, width) + origin);
                polygons.Add(new Polygon(oppositeVertices));
            }
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                List<Vector3> vertices = new List<Vector3>();
                vertices.Add(new Vector3(0, i + 0, j + 0) + origin);
                vertices.Add(new Vector3(0, i + 0, j + 1) + origin);
                vertices.Add(new Vector3(0, i + 1, j + 1) + origin);
                vertices.Add(new Vector3(0, i + 1, j + 0) + origin);
                polygons.Add(new Polygon(vertices));

                List<Vector3> oppositeVertices = new List<Vector3>();
                oppositeVertices.Add(new Vector3(length, height - i - 0, width - j - 0) + origin);
                oppositeVertices.Add(new Vector3(length, height - i - 1, width - j - 0) + origin);
                oppositeVertices.Add(new Vector3(length, height - i - 1, width - j - 1) + origin);
                oppositeVertices.Add(new Vector3(length, height - i - 0, width - j - 1) + origin);
                polygons.Add(new Polygon(oppositeVertices));
            }
        }
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                List<Vector3> vertices = new List<Vector3>();
                vertices.Add(new Vector3(i + 0, 0, j + 0) + origin);
                vertices.Add(new Vector3(i + 1, 0, j + 0) + origin);
                vertices.Add(new Vector3(i + 1, 0, j + 1) + origin);
                vertices.Add(new Vector3(i + 0, 0, j + 1) + origin);
                polygons.Add(new Polygon(vertices));

                List<Vector3> oppositeVertices = new List<Vector3>();
                oppositeVertices.Add(new Vector3(length - i - 0, height, width - j - 0) + origin);
                oppositeVertices.Add(new Vector3(length - i - 0, height, width - j - 1) + origin);
                oppositeVertices.Add(new Vector3(length - i - 1, height, width - j - 1) + origin);
                oppositeVertices.Add(new Vector3(length - i - 1, height, width - j - 0) + origin);
                polygons.Add(new Polygon(oppositeVertices));
            }
        }

        return polygons;
    }

    public Mesh ReturnMesh(int length, int height, int width)
    {
        Mesh mesh = new Mesh();
        List<Vector3> meshVertice = new List<Vector3>();
        List<int> meshTriangles = new List<int>();

        GenerateCuboid(length, height, width);

        foreach (var vert in this.vertices)
        {
            meshVertice.Add(vert.position);
        }
        mesh.SetVertices(meshVertice);

        foreach (var poly in this.polygons)
        {
            meshTriangles.Add(poly.vertice[0].number);
            meshTriangles.Add(poly.vertice[1].number);
            meshTriangles.Add(poly.vertice[2].number);
        }

        mesh.SetTriangles(meshTriangles, 0);
        return mesh;
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
        node.AddNeighbour(secondNode);
        node.AddNeighbour(firstNode);
        squareNode.AddNeighbour(secondNode);
        squareNode.AddNeighbour(firstNode);


        // Completely opposite side of the cuboid, process is mirrored
        Vector3 oppositeNodePosition = matrixSize - node.position;
        AsteroidNode oppositeNode = MatrixVector(oppositeNodePosition);

        AsteroidNode oppositeSquareNode = MatrixVector(oppositeNodePosition - dir);
        AsteroidNode oppositeFirst = MatrixVector(oppositeNodePosition - first);
        AsteroidNode oppositeSecond = MatrixVector(oppositeNodePosition - second);

        square[2] = new AsteroidPolygon(oppositeNode, oppositeFirst, oppositeSecond);
        square[3] = new AsteroidPolygon(oppositeSquareNode, oppositeSecond, oppositeFirst);
        oppositeNode.AddNeighbour(oppositeFirst);
        oppositeNode.AddNeighbour(oppositeSecond);
        oppositeSquareNode.AddNeighbour(oppositeFirst);
        oppositeSquareNode.AddNeighbour(oppositeSecond);
        return square;
    }


    public AsteroidNode MatrixVector(Vector3 vect)
    {
        return this.matrix[(int)vect.x, (int)vect.y, (int)vect.z];
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
    public Vector3 position;

    public AsteroidNode(int x, int y, int z, List<AsteroidNode> list)
    {
        this.position = new Vector3(x, y, z);
        list.Add(this);
        this.number = list.Count - 1;
        //Debug.Log(position + " " + this.number);
    }
    public AsteroidNode(int x, int y, int z)
    {
        this.position = new Vector3(x, y, z);
    }

    public void SmoothModel(List<AsteroidNode> vertices)
    {

    }

    public void Smooth(float strength) // Smoothens the model by adding/subtracting the average of neighbours's positions from this Node's position
    {
        List<AsteroidNode> neighbours = new List<AsteroidNode>();

        Debug.Log("+x" + plusX + "+y" + plusY);

        Vector3 summ = (plusX == null ? Vector3.zero : plusX.position) + (plusY == null ? Vector3.zero : plusY.position) + (plusZ == null ? Vector3.zero : plusZ.position) +
            (minusX == null ? Vector3.zero : minusX.position) + (minusY == null ? Vector3.zero : minusY.position) + (minusZ == null ? Vector3.zero : minusZ.position);

        int count = (plusX == null ? 0 : 1) + (plusY == null ? 0 : 1) + (plusZ == null ? 0 : 1) + (minusX == null ? 0 : 1) + (minusY == null ? 0 : 1) + (minusZ == null ? 0 : 1);
        float divisor = 1f / count;
        Vector3 difference = (this.position * count - summ) * divisor * strength;



        Debug.Log(summ + " " + count + " " + divisor + " " + strength + " " + difference);

    }

    public List<AsteroidNode> MyNeighbours()
    {
        List<AsteroidNode> neighbours = new List<AsteroidNode>();
        if (plusX != null)
        {
            neighbours.Add(plusX);
        }
        if (plusY != null)
        {
            neighbours.Add(plusY);
        }
        if (plusZ != null)
        {
            neighbours.Add(plusZ);
        }
        if (minusX != null)
        {
            neighbours.Add(minusX);
        }
        if (minusY != null)
        {
            neighbours.Add(minusY);
        }
        if (minusZ != null)
        {
            neighbours.Add(minusZ);
        }
        return neighbours;
    }

    public void AddNeighbour(AsteroidNode neighbour)
    {
        Vector3 diff = this.position - neighbour.position;

        if (diff.Equals(Vector3Int.right)) // 1,0,0 
        {
            plusX = neighbour;
            neighbour.minusX = this;
            return;
        }
        if (diff.Equals(Vector3Int.up)) // 0,1,0 
        {
            plusY = neighbour;
            neighbour.minusY = this;
            return;
        }
        if (diff.Equals(Vector3Int.forward)) // 0,0,1 
        {
            plusZ = neighbour;
            neighbour.minusZ = this;
            return;
        }
        if (diff.Equals(Vector3Int.left)) // -1,0,0 
        {
            minusX = neighbour;
            neighbour.plusX = this;
            return;
        }
        if (diff.Equals(Vector3Int.down)) // 0,-1,0 
        {
            minusY = neighbour;
            neighbour.plusY = this;
            return;
        }
        if (diff.Equals(Vector3Int.back)) // 0,0,-1 
        {
            minusZ = neighbour;
            neighbour.plusZ = this;
            return;
        }
        else
        {
            Debug.Log("not a neighbour!!1!");
            return;
        }



    }
}
