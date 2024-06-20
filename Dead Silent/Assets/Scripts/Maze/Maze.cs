using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    private int width = 10;
    private int height = 10;

    private bool circular;

    public Cell EntranceCell;
    public Cell ExitCell;

    private Cell[,] cells;

    public Maze(int width, int height, bool Circular) 
    { 
        this.width = width;
        this.height = height;
        this.circular = Circular;

        PopulateCells();
    }

    private void PopulateCells()
    {
        cells = new Cell[width,height];

        for(int x = 0; x < width; x++)
        {
            for(int  y = 0; y < height; y++)
            {
                cells[x,y] = new Cell(x, y);
            }
        }
    }


    public void MakeEntrance()
    {
        EntranceCell = GetCell(Random.Range(0, width), height - 1);
        RemoveWall(EntranceCell, Direction.North);
    }
    public void MakeExit()
    {
        ExitCell = GetCell(Random.Range(0, width), 0);
        RemoveWall(ExitCell, Direction.South);
    }

    public Cell GetCell(int x, int y)
    {
        return cells[x,y];
    }

    public Cell GetCell(Cell cell)
    {
        return GetCell(cell.x, cell.y);
    }


    public bool GetCell(int x, int y, out Cell output)
    {
        if (x < width && y < height)
        {
            //Debug.Log(x + " " + y); 
            output = GetCell(x,y);
            return true;
        }
        output = null;
        return false;

    }
    public bool GetCell(Cell cell, out Cell output)
    {
        return GetCell(cell.x, cell.y, out output);
    }

    public void VisitCell(int x, int y)
    {
        cells[x,y].visited = true;
    }

    public void VisitCell(Cell cell)
    {
        VisitCell(cell.x, cell.y);
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public bool IsCircular()
    {
        return circular;
    }

    public bool TryRemoveWall(Cell cell1, Cell cell2)
    {
        bool a = RemoveWall(cell1, GetCellDirection(cell1, cell2));
        bool b = RemoveWall(cell2, GetCellDirection(cell2, cell1));

        //DebugWall(cell1, cell2);

        return a & b;
    }
    private void DebugWall(Cell cell1, Cell cell2)
    {
        string log = "";
        string wall = "";

        GetCell(cell1, out cell1);
        GetCell(cell2, out cell2);

        foreach (Direction direction in cell1.walls)
        {
            wall += direction.ToString() + " ";
        }
        log += cell1.ToString() + ": " + wall;
        wall = "";
        foreach (Direction direction in cell2.walls)
        {
            wall += direction.ToString() + " ";
        }
        log += cell2.ToString() + ": " + wall;
        Debug.Log(log);
    }

    private bool RemoveWall(Cell cell, Direction direction)
    {
        return cells[cell.x,cell.y].walls.Remove(direction);
    }

    public Direction GetCellDirection(Cell start, Cell target)
    {
        Direction direction = Direction.None;
        if (start.x > target.x) direction = Direction.West;
        if (start.y > target.y) direction = Direction.South;
        if (start.x < target.x) direction = Direction.East;
        if (start.y < target.y) direction = Direction.North;

        if (circular)
        {
            if (start.x == 0)
            {
                if (target.x == width - 1)
                {
                    direction = Direction.West;
                }
            }
            if (start.x == width - 1)
            {
                if (target.x == 0)
                {
                    direction = Direction.East;
                }
            }
        }

        //Debug.Log(start.ToString() + ":" +  target.ToString() + " - " + direction.ToString());

        return direction;
    }

    public class Cell
    {
        public int x;
        public int y;

        public List<Direction> walls;

        public bool visited;

        public Cell(int x, int y, bool generateWalls = true)
        {
            this.x = x;
            this.y = y;
            if(generateWalls) GenerateNewWalls();
            else walls = new List<Direction>();
        }


        public void GenerateNewWalls()
        {
            walls = new List<Direction>
            {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
        }


        public override string ToString()
        {
            return GetCellString(this);
        }

        public static string GetCellString(int x, int y)
        {
            return x + "," + y;
        }

        public static string GetCellString(Cell cell)
        {
            if (cell == null) return "";
            return GetCellString(cell.x, cell.y);
        }
    }

    public enum Direction
    {
        North,
        East,
        South,
        West,
        None
    }
}