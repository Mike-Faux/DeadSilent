using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(DrawCircleMaze))]
public class MazeGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;

    public bool circular = true;

    private Maze maze;

    private Dictionary<string, Maze.Cell> cellsUnvisited;


    // Start is called before the first frame update
    void Awake()
    {
        RebuildMaze();
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void RebuildMaze()
    {
        if (transform.childCount > 0)
        {
            while (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }

        PopulateCellsAndWalls();

        GenerateMaze();
        DrawMaze(maze);

    }

    public void DrawMaze(Maze maze)
    {
        DrawCircleMaze dcm = GetComponent<DrawCircleMaze>();
        dcm.DrawMaze(maze);
    }

    public void PopulateCellsAndWalls()
    {
        maze = new Maze(width, height, circular);

        cellsUnvisited = new Dictionary<string, Maze.Cell>();


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cellsUnvisited.Add(Maze.Cell.GetCellString(x, y), new Maze.Cell(x, y));
            }
        }
    }

    public void GenerateMaze()
    {
        string t = Random.Range(0, width) + "," + Random.Range(0, height);
        cellsUnvisited.Remove(t, out Maze.Cell target);

        while (cellsUnvisited.Count > 0)
        {
            target = Walk(target);
            if (target == null) target = Hunt();
            if (target == null)
            {
                Debug.Log("Critical Error: NO CELL FOUND!");
                break;
            }
            maze.VisitCell(target);
            cellsUnvisited.Remove(Maze.Cell.GetCellString(target));
        }

        maze.MakeEntrance();
        maze.MakeExit();
    }

    public Maze.Cell Walk(Maze.Cell cell)
    {
        Maze.Direction[] dir = GetRandomDirections();

        Maze.Cell target = null;

        for (int i = 0; i < dir.Length && target == null; i++)
        {
            TryGetCellInDir(cell, dir[i], out target, false);
        }

        if (target == null) return null;

        if (!TryRemoveWall(cell, target)) Debug.Log("Wall could not be Removed!");



        return target;
    }

    private bool TryGetCellInDir(Maze.Cell startCell, Maze.Direction dir, out Maze.Cell cell, bool includeUnvisited)
    {
        return TryGetCellInDir(startCell.x, startCell.y, dir, out cell, includeUnvisited);
    }

    public bool TryGetCellInDir(int x, int y, Maze.Direction dir, out Maze.Cell cell, bool includeVisited)
    {
        switch (dir)
        {
            case Maze.Direction.North:
                y++;
                break;
            case Maze.Direction.South:
                y--;
                if (y < 0)
                {
                    cell = null;
                    return false;
                }
                break;
            case Maze.Direction.East:
                x++;
                if (x >= width)
                {
                    if (circular)
                        x = 0;
                    else
                    {
                        cell = null;
                        return false;
                    }
                }
                break;
            case Maze.Direction.West:
                x--;
                if (x < 0)
                {
                    if(circular)
                        x = width - 1;
                    else
                    {
                        cell = null;
                        return false;
                    }
                }
                break;
        }

        // Debug.Log(x + " " + y);
        if (includeVisited) return maze.GetCell(x, y, out cell);
        return cellsUnvisited.TryGetValue(Maze.Cell.GetCellString(x, y), out cell);
    }

    public Maze.Direction[] GetRandomDirections()
    {
        Maze.Direction[] directions = new Maze.Direction[4];
        List<Maze.Direction> dirs = new List<Maze.Direction>
        {
            Maze.Direction.North,
            Maze.Direction.South,
            Maze.Direction.East,
            Maze.Direction.West
        };



        for (int i = 0; i < 4; i++)
        {
            directions[i] = dirs[Random.Range(0, dirs.Count)];
            dirs.Remove(directions[i]);
        }

        return directions;
    }

    public Maze.Cell Hunt()
    {

        foreach (Maze.Cell cell in cellsUnvisited.Values)
        {
            Maze.Cell[] neighbors = GetNeighbors(cell);
            //Debug.Log("Cell " + cell.ToString() + " has " + neighbors.Length + " neighbors.");

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i].visited)
                {
                    TryRemoveWall(cell, neighbors[i]);
                    //Debug.Log("Cell " + cell.ToString() + " has a valid neighbor.");

                    //Debug.Log(cell.ToString() + " " + cell.walls.Count + " : " + neighbors[i].ToString() + " " + neighbors[i].walls.Count);

                    return cell;
                }
            }
        }
        Debug.Log("No Valid Cell Found on Hunt. Cells Left:" + cellsUnvisited.Count);
        return null;
    }

    public Maze.Cell[] GetNeighbors(Maze.Cell cell)
    {
        List<Maze.Cell> neighbors = new List<Maze.Cell>();

        if (TryGetCellInDir(cell, Maze.Direction.North, out Maze.Cell North, true)) neighbors.Add(North);
        if (TryGetCellInDir(cell, Maze.Direction.South, out Maze.Cell South, true)) neighbors.Add(South);
        if (TryGetCellInDir(cell, Maze.Direction.East, out Maze.Cell East, true)) neighbors.Add(East);
        if (TryGetCellInDir(cell, Maze.Direction.West, out Maze.Cell West, true)) neighbors.Add(West);

        return neighbors.ToArray();
    }

    public bool TryRemoveWall(Maze.Cell cell1, Maze.Cell cell2)
    {
        if (maze.TryRemoveWall(cell1, cell2))
        {
            //Debug.Log("Cell walls Removed!");
            return true;
        }
        return false;
    }
}

