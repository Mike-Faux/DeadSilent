using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(MazeSettings))]
public class DrawCircleMaze : MonoBehaviour
{
    public MazeSettings mazeSettings;

    public float baseDistance = 30f;
    public float wallHeight = 8f;
    public float postModifier = .1f;

    public float cellSize = 5f;
    private float halfSize;
    private float postWidth;

    public float wallWidth = 1f;
    private float halfWidth;
    private float halfPost;

    float degreesPerCell;

    public int curvedWallPoints = 20;

    private Maze maze;

    //Still to impliment
    public Vector3 offset = Vector3.zero;

    [SerializeField] NavMeshSurface surface;
    [SerializeField] GameObject EndPortal;
    [SerializeField] GameObject StartPortal;


    public void DrawMaze(Maze maze, int level)
    {
        this.maze = maze;
        //GameObject startRoom = Instantiate(StartRoom);
        //startRoom.transform.position = Vector3.zero;

        baseDistance *= ((level + 1) / 2);

        degreesPerCell = 360f / maze.GetWidth();
        postWidth = wallWidth * (1f + postModifier);

        if (curvedWallPoints < 2) curvedWallPoints = 2;
        halfPost = postWidth / 2f;
        halfSize = cellSize / 2f;
        halfWidth = wallWidth / 2f;
        DrawMazePosts();

        GameObject cells = new GameObject("Cells");
        cells.transform.parent = this.transform;

        DrawEntranceAndExit();

        for (int x = 0; x < maze.GetWidth(); x++)
        {
            for (int y = 0; y < maze.GetHeight(); y++)
            {
                GameObject cell = DrawCell(maze.GetCell(x, y));
                cell.transform.parent = cells.transform;
            }
        }

        BuildNavMesh();
    }

    private void DrawEntranceAndExit()
    {
        Maze.Cell entrance = new Maze.Cell(maze.ExitCell.x, maze.ExitCell.y - 1, false);
        entrance.walls.Add(Maze.Direction.South);
        Maze.Cell entranceL = new Maze.Cell(maze.ExitCell.x - 1, maze.ExitCell.y - 1);
        entranceL.walls.Remove(Maze.Direction.East);
        Maze.Cell entranceR = new Maze.Cell(maze.ExitCell.x + 1, maze.ExitCell.y - 1);
        entranceR.walls.Remove(Maze.Direction.West);

        Maze.Cell exit = new Maze.Cell(maze.EntranceCell.x, maze.EntranceCell.y + 1);
        exit.walls.Remove(Maze.Direction.South);

        DrawCell(entrance, 2);
        DrawCell(entranceL, -1);
        DrawCell(entranceR, -1);
        DrawCell(exit, 1);

        DrawMazePost(entrance.x - 1, entrance.y);
        DrawMazePost(entrance.x, entrance.y);
        DrawMazePost(entrance.x + 1, entrance.y);
        DrawMazePost(entrance.x + 2, entrance.y);

        DrawMazePost(exit.x, exit.y);
        DrawMazePost(exit.x + 1, exit.y);
    }

    private void BuildNavMesh()
    {
        surface.BuildNavMesh();
    }

    private void DrawMazePosts()
    {
        GameObject posts = new GameObject("Posts");
        posts.transform.position = new Vector3(0, 0, 0);
        posts.transform.parent = this.transform;

        for (int x = 0; x < maze.GetWidth() + 1; x++)
        {
            for (int y = 0; y < maze.GetHeight() + 1; y++)
            {
                GameObject post = DrawMazePost(x, y);
                post.transform.parent = posts.transform;

                //NavMesh Implimentation
                NavMeshModifier NMM = post.AddComponent<NavMeshModifier>();
                NMM.area = NavMesh.GetAreaFromName("Not Walkable");
                NMM.overrideArea = true;
            }
        }
    }

    private GameObject DrawMazePost(int x, int y)
    {
        GameObject post = new GameObject("Post (" + x + "," + y + ")");


        float radius = baseDistance + (cellSize * y) + (wallWidth * y) + (wallWidth / 2);
        float angle = degreesPerCell * x;


        float postHeight = wallHeight + (wallHeight * .1f);

        Vector3[] verts = new Vector3[20];
        Vector2[] uvs = new Vector2[20];
        Vector3[] normals = new Vector3[20];

        for (int i = 0; i < 4; i++)
        {
            float mod = 1f;
            float dir = 1f;
            switch (i)
            {
                case 0:
                    mod = 1f;
                    dir = 1f;
                    break;
                case 1:
                    mod = 1f;
                    dir = -1f;
                    break;
                case 2:
                    mod = -1f;
                    dir = 1f;
                    break;
                case 3:
                    mod = -1f;
                    dir = -1f;
                    break;
            }


            verts[i * 4] = new Vector3((mod) * halfPost, 0f, (mod * -dir) * halfPost);
            verts[i * 4 + 1] = new Vector3((mod * dir) * halfPost, 0f, (mod) * halfPost);
            verts[i * 4 + 2] = new Vector3((mod) * halfPost, postHeight, (mod * -dir) * halfPost);
            verts[i * 4 + 3] = new Vector3((mod * dir) * halfPost, postHeight, (mod) * halfPost);

            Vector3 normal = new Vector3(0f, 0f, 0f);

            normal = -Vector3.Cross(verts[i * 4 + 1] - verts[i * 4 + 0], verts[i * 4 + 2] - verts[i * 4 + 0]);
            normal.Normalize();

            normals[i * 4] = normal;
            normals[i * 4 + 1] = normal;
            normals[i * 4 + 2] = normal;
            normals[i * 4 + 3] = normal;

            uvs[i * 4 + 0] = new Vector2(.25f * i, 0);
            uvs[i * 4 + 1] = new Vector2(.25f * (i + 1), 0);
            uvs[i * 4 + 2] = new Vector2(.25f * i, 1);
            uvs[i * 4 + 3] = new Vector2(.25f * (i + 1), 1);
        }

        //top
        verts[16] = new Vector3(halfPost, postHeight, -halfPost);
        verts[17] = new Vector3(halfPost, postHeight, halfPost);
        verts[18] = new Vector3(-halfPost, postHeight, -halfPost);
        verts[19] = new Vector3(-halfPost, postHeight, halfPost);

        normals[16] = Vector3.up;
        normals[17] = Vector3.up;
        normals[18] = Vector3.up;
        normals[19] = Vector3.up;

        uvs[16] = new Vector2(0, 0);
        uvs[17] = new Vector2(.1f, 0);
        uvs[18] = new Vector2(0, .1f);
        uvs[19] = new Vector2(.1f, .1f);

        int[] tris = new int[30];
        for (int i = 0; i < 5; i++)
        {
            //triangles
            tris[i * 6 + 0] = i * 4 + 0;
            tris[i * 6 + 1] = i * 4 + 2;
            tris[i * 6 + 2] = i * 4 + 1;

            tris[i * 6 + 3] = i * 4 + 3;
            tris[i * 6 + 4] = i * 4 + 1;
            tris[i * 6 + 5] = i * 4 + 2;
        }

        post.transform.SetPositionAndRotation(new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * radius) + offset, Quaternion.Euler(0, angle, 0));

        MeshRenderer meshRenderer = post.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mazeSettings.postMat;

        MeshFilter meshFilter = post.AddComponent<MeshFilter>();
        MeshCollider meshCollider = post.AddComponent<MeshCollider>();

        Mesh mesh = new();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = tris;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        return post;
    }

    private GameObject DrawCell(Maze.Cell cell, int type = 0)
    {
        GameObject cellObj = new GameObject("Cell " + cell.ToString());
        //set cell pos
        float radius = baseDistance + (cell.y * cellSize) + ((cell.y + 1) * wallWidth) + halfSize;
        float cellAngle = degreesPerCell * (cell.x + .5f);
        Vector3 pos = new Vector3(Mathf.Sin(cellAngle * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(cellAngle * Mathf.Deg2Rad) * radius);

        cellObj.transform.position = pos;
        Cell c = cellObj.AddComponent<Cell>();
        c.cell = cell;

        DrawCellWalls(cellObj);
        DrawCellFloor(cellObj);

        GameObject MainObj;
        switch (type)
        {
            case -1:
                MainObj = null;
                break;
            default:
                MainObj = mazeSettings.GetMainSpawnable();
                break;
            case 1:
                MainObj = Instantiate(EndPortal);
                break;
            case 2:
                MainObj = Instantiate(StartPortal);
                break;
        }

        if(MainObj != null)
        {
            MainObj.name = "MainObj " + cell.ToString();
            pos.y = 1;
            MainObj.transform.position = pos;
            if (type != 0) MainObj.transform.LookAt(Vector3.zero);
            MainObj.transform.parent = cellObj.transform;
        }



        //GameObject cellFloor = DrawCellFloor(cell);
        //cellFloor.transform.SetParent(cellObj.transform);





        return cellObj;
    }

    private void DrawCellWalls(GameObject cell)
    {
        Maze.Cell c = cell.GetComponent<Cell>().cell;
        foreach (Maze.Direction dir in c.walls)
        {
            GameObject wallObj = DrawCellWall(c, dir);
            wallObj.transform.parent = cell.transform;


            //NavMesh Implimentation
            NavMeshModifier NMM = wallObj.AddComponent<NavMeshModifier>();
            NMM.area = NavMesh.GetAreaFromName("Not Walkable");
            NMM.overrideArea = true;
        }
    }

    private GameObject DrawCellWall(Maze.Cell cell, Maze.Direction direction)
    {
        GameObject wall;

        if (direction == Maze.Direction.North || direction == Maze.Direction.South)
        {
            wall = DrawCurvedWall(cell, direction);
        }
        else
        {
            wall = DrawStraightWall(cell, direction);
        }

        return wall;
    }

    private GameObject DrawStraightWall(Maze.Cell cell, Maze.Direction dir)
    {
        GameObject wall = new GameObject();
        wall.name = dir + " straight wall";

        int wallPos;
        float angleMod;

        switch (dir)
        {
            case Maze.Direction.East:
                wallPos = 1;
                angleMod = -1;
                break;
            case Maze.Direction.West:
                wallPos = 0;
                angleMod = 1;
                break;
            default:
                throw new System.Exception("Could not draw straight wall in direction " + dir + "for cell " + cell.ToString());
        }

        float[] radaii = new float[4];
        radaii[0] = baseDistance + (cellSize * cell.y) + (wallWidth * cell.y) + halfPost + halfWidth;
        radaii[1] = baseDistance + (cellSize * (cell.y + 1)) + (wallWidth * (cell.y + 1)) - halfPost + halfWidth;
        radaii[2] = Mathf.Sqrt((radaii[0] * radaii[0]) + (halfWidth * halfWidth));
        radaii[3] = Mathf.Sqrt((radaii[1] * radaii[1]) + (halfWidth * halfWidth));

        float[] angles = new float[3];
        angles[0] = degreesPerCell * (cell.x + wallPos);
        angles[1] = angles[0] + (angleMod * (Mathf.Tan(halfWidth / radaii[2])) * Mathf.Rad2Deg);
        angles[2] = angles[0] + (angleMod * (Mathf.Tan(halfWidth / radaii[3])) * Mathf.Rad2Deg);

        Vector3[] verts = 
        {
            new Vector3(Mathf.Sin(angles[1] * Mathf.Deg2Rad) * radaii[2], 0, Mathf.Cos(angles[1] * Mathf.Deg2Rad) * radaii[2]),
            new Vector3(Mathf.Sin(angles[2] * Mathf.Deg2Rad) * radaii[3], 0, Mathf.Cos(angles[2] * Mathf.Deg2Rad) * radaii[3]),
            new Vector3(Mathf.Sin(angles[1] * Mathf.Deg2Rad) * radaii[2], wallHeight, Mathf.Cos(angles[1] * Mathf.Deg2Rad) * radaii[2]),
            new Vector3(Mathf.Sin(angles[2] * Mathf.Deg2Rad) * radaii[3], wallHeight, Mathf.Cos(angles[2] * Mathf.Deg2Rad) * radaii[3]),

            new Vector3(Mathf.Sin(angles[1] * Mathf.Deg2Rad) * radaii[2], wallHeight, Mathf.Cos(angles[1] * Mathf.Deg2Rad) * radaii[2]),
            new Vector3(Mathf.Sin(angles[2] * Mathf.Deg2Rad) * radaii[3], wallHeight, Mathf.Cos(angles[2] * Mathf.Deg2Rad) * radaii[3]),
            new Vector3(Mathf.Sin(angles[0] * Mathf.Deg2Rad) * radaii[0], wallHeight, Mathf.Cos(angles[0] * Mathf.Deg2Rad) * radaii[0]),
            new Vector3(Mathf.Sin(angles[0] * Mathf.Deg2Rad) * radaii[1], wallHeight, Mathf.Cos(angles[0] * Mathf.Deg2Rad) * radaii[1])
        };

        Vector3 normal = -angleMod * Vector3.Cross(verts[1] - verts[0], verts[2] - verts[0]);

        Vector3[] normals =
        {
            normal,
            normal,
            normal,
            normal,

            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        Vector2[] uvs =
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),

            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, .9f),
            new Vector2(1, .9f)
        };

        int[] tris;

        if (dir == Maze.Direction.East)
        {
            int[] e =
            {
                1,2,0,
                2,1,3,
                5,6,4,
                6,5,7
            };
            tris = e;
        }
        else
        {
            int[] w =
            {
                0,2,1,
                3,1,2,
                4,6,5,
                7,5,6
            };
            tris = w;
        }

        MeshRenderer meshRenderer = wall.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mazeSettings.wallMat;

        MeshFilter meshFilter = wall.AddComponent<MeshFilter>();
        MeshCollider meshCollider = wall.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = tris;

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        return wall;
    }

    private GameObject DrawCurvedWall(Maze.Cell cell, Maze.Direction dir)
    {
        bool outer = true;
        if (dir == Maze.Direction.South) outer = false;

        GameObject wall = new GameObject(cell.ToString() + " " + dir + " curved wall");

        Vector3[] verts = new Vector3[curvedWallPoints * 4];
        Vector2[] uvs = new Vector2[curvedWallPoints * 4];
        Vector3[] normals = new Vector3[curvedWallPoints * 4];

        float degreesPerPoint = degreesPerCell / (curvedWallPoints - 1);
        int offset;
        float innerRadius;
        float outerRadius;
        if (outer)
        {
            innerRadius = baseDistance + (cellSize * (cell.y + 1)) + (wallWidth * cell.y) + wallWidth;
            outerRadius = baseDistance + (cellSize * (cell.y + 1)) + (wallWidth * (cell.y + 1)) + halfWidth;
            offset = 1;
        }
        else
        {
            outerRadius = baseDistance + (cellSize * cell.y) + (wallWidth * cell.y) + halfWidth;
            innerRadius = baseDistance + (cellSize * cell.y) + (wallWidth * cell.y) + wallWidth;
            offset = 0;
        }

        float[] radaii = new float[4];
        radaii[0] = baseDistance + (cellSize * (cell.y + offset)) + (wallWidth * cell.y) + halfWidth + (offset * halfWidth);//exterior 
        radaii[1] = baseDistance + (cellSize * (cell.y + offset)) + (wallWidth * cell.y) + wallWidth + (offset * halfWidth);//interior
        radaii[2 + offset] = Mathf.Sqrt((radaii[1] * radaii[1]) + (halfPost * halfPost));//interior
        radaii[3 - offset] = Mathf.Sqrt((radaii[0] * radaii[0]) + (halfPost * halfPost));//exterior 



        float[] angles = new float[6];
        angles[0] = degreesPerCell * cell.x;
        angles[1] = angles[0] + (Mathf.Tan(halfPost / radaii[2]) * Mathf.Rad2Deg);//exterior 
        angles[2] = angles[0] + (Mathf.Tan(halfPost / radaii[3]) * Mathf.Rad2Deg);//interior
        angles[3] = angles[0] + degreesPerCell;
        angles[4] = angles[3] - (Mathf.Tan(halfPost / radaii[2]) * Mathf.Rad2Deg);//exterior 
        angles[5] = angles[3] - (Mathf.Tan(halfPost / radaii[3]) * Mathf.Rad2Deg);//interior

        //Wall Surface
        verts[0] = new Vector3(Mathf.Sin(angles[1] * Mathf.Deg2Rad) * radaii[2], 0, Mathf.Cos(angles[1] * Mathf.Deg2Rad) * radaii[2]);
        verts[1] = new Vector3(Mathf.Sin(angles[1] * Mathf.Deg2Rad) * radaii[2], wallHeight, Mathf.Cos(angles[1] * Mathf.Deg2Rad) * radaii[2]);

        verts[(curvedWallPoints * 2) - 2] = new Vector3(Mathf.Sin(angles[4] * Mathf.Deg2Rad) * radaii[2], 0, Mathf.Cos(angles[4] * Mathf.Deg2Rad) * radaii[2]);
        verts[(curvedWallPoints * 2) - 1] = new Vector3(Mathf.Sin(angles[4] * Mathf.Deg2Rad) * radaii[2], wallHeight, Mathf.Cos(angles[4] * Mathf.Deg2Rad) * radaii[2]);

        uvs[0]                          = new Vector2(0, 0);
        uvs[1]                          = new Vector2(0, 1);
        uvs[(curvedWallPoints * 2) - 2] = new Vector2(1, 0);
        uvs[(curvedWallPoints * 2) - 1] = new Vector2(1, 1);

        //Wall Top
        verts[(curvedWallPoints * 2) + 0] = new Vector3(Mathf.Sin(angles[1] * Mathf.Deg2Rad) * radaii[2], wallHeight, Mathf.Cos(angles[1] * Mathf.Deg2Rad) * radaii[2]);
        verts[(curvedWallPoints * 2) + 1] = new Vector3(Mathf.Sin(angles[2] * Mathf.Deg2Rad) * radaii[3], wallHeight, Mathf.Cos(angles[2] * Mathf.Deg2Rad) * radaii[3]);

        verts[(curvedWallPoints * 4) - 2] = new Vector3(Mathf.Sin(angles[4] * Mathf.Deg2Rad) * radaii[2], wallHeight, Mathf.Cos(angles[4] * Mathf.Deg2Rad) * radaii[2]);
        verts[(curvedWallPoints * 4) - 1] = new Vector3(Mathf.Sin(angles[5] * Mathf.Deg2Rad) * radaii[3], wallHeight, Mathf.Cos(angles[5] * Mathf.Deg2Rad) * radaii[3]);

        uvs[(curvedWallPoints * 2) + 0] = new Vector2(0f, 0f);
        uvs[(curvedWallPoints * 2) + 1] = new Vector2(0f, 0.1f);
        uvs[(curvedWallPoints * 4) - 2] = new Vector2(1f, 0f);
        uvs[(curvedWallPoints * 4) - 1] = new Vector2(1f, 0.1f);

        //Debug.Log(uvs[(curvedWallPoints * 2) + ((curvedWallPoints - 1) * 2)]);

        for (int i = 1; i < curvedWallPoints - 1; i++)
        {
            float angle = angles[0] + (degreesPerPoint * i);
            verts[i * 2] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * innerRadius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * innerRadius);
            verts[i * 2 + 1] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * innerRadius, wallHeight, Mathf.Cos(angle * Mathf.Deg2Rad) * innerRadius);

            uvs[i * 2]     = new Vector2(i * (1f / (curvedWallPoints - 1)), 0);
            uvs[i * 2 + 1] = new Vector2(i * (1f / (curvedWallPoints - 1)), 1);


            verts[(curvedWallPoints * 2) + (i * 2) + 0] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * innerRadius, wallHeight, Mathf.Cos(angle * Mathf.Deg2Rad) * innerRadius);
            verts[(curvedWallPoints * 2) + (i * 2) + 1] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * outerRadius, wallHeight, Mathf.Cos(angle * Mathf.Deg2Rad) * outerRadius);

            uvs[(curvedWallPoints * 2) + (i * 2) + 0] = new Vector2(i * (1f / (curvedWallPoints - 1)), 0f);
            uvs[(curvedWallPoints * 2) + (i * 2) + 1] = new Vector2(i * (1f / (curvedWallPoints - 1)), 0.1f);
        }
        /*
        for(int i = 0; i < uvs.Length; i++)
        {
            Debug.Log(i + ":" + uvs[i]);
        }*/

        //normals
        for(int i = 0; i < curvedWallPoints; i++)
        {
            if(i == 0)
            {
                Vector3 normal = Vector3.Cross(verts[0] - verts[1], verts[2] - verts[1]);

                normals[(i * 2) + 0] = normal;
                normals[(i * 2) + 1] = normal;
            }else if (i == curvedWallPoints - 1)
            {
                Vector3 normal = -Vector3.Cross(verts[(i * 2) + 0] - verts[(i * 2) + 1], verts[(i * 2) - 1] - verts[(i * 2) + 1]);

                normals[(i * 2) + 0] = normal;
                normals[(i * 2) + 1] = normal;
            }
            else
            {
                Vector3 normal = Vector3.Cross(verts[(i * 2) - 2] - verts[(i * 2) - 1], verts[(i * 2) + 2] - verts[(i * 2) - 1]);

                if (outer)
                {
                    normals[(i * 2) + 0] = -normal;
                    normals[(i * 2) + 1] = -normal;
                }
                else
                {
                    normals[(i * 2) + 0] = normal;
                    normals[(i * 2) + 1] = normal;
                }
            }

            normals[(curvedWallPoints * 2) + (i * 2) + 0] = Vector3.up;
            normals[(curvedWallPoints * 2) + (i * 2) + 1] = Vector3.up;
        }

        int[] tris = new int[(curvedWallPoints - 1) * 12];
        offset *= 2;

        for (int i = 0; i < curvedWallPoints - 1; i++)
        {
            tris[i * 6 + 2] = i * 2 + 0 + offset;
            tris[i * 6 + 1] = i * 2 + 1;
            tris[i * 6 + 0] = i * 2 + 2 - offset;

            tris[i * 6 + 5] = i * 2 + 3 - offset;
            tris[i * 6 + 4] = i * 2 + 2;
            tris[i * 6 + 3] = i * 2 + 1 + offset;
            
            tris[i * 6 + 2 + ((curvedWallPoints - 1) * 6)] = (i * 2) + (curvedWallPoints * 2) + 0 + offset;
            tris[i * 6 + 1 + ((curvedWallPoints - 1) * 6)] = (i * 2) + (curvedWallPoints * 2) + 1;
            tris[i * 6 + 0 + ((curvedWallPoints - 1) * 6)] = (i * 2) + (curvedWallPoints * 2) + 2 - offset;
            
            tris[i * 6 + 5 + ((curvedWallPoints - 1) * 6)] = (i * 2) + (curvedWallPoints * 2) + 3 - offset;
            tris[i * 6 + 4 + ((curvedWallPoints - 1) * 6)] = (i * 2) + (curvedWallPoints * 2) + 2;
            tris[i * 6 + 3 + ((curvedWallPoints - 1) * 6)] = (i * 2) + (curvedWallPoints * 2) + 1 + offset;
        }

        MeshRenderer meshRenderer = wall.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mazeSettings.wallMat;

        MeshFilter meshFilter = wall.AddComponent<MeshFilter>();
        MeshCollider meshCollider = wall.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = tris;

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;


        return wall;
    }

    private GameObject DrawCellFloor(GameObject cell)
    {
        Maze.Cell c = cell.GetComponent<Cell>().cell;

        GameObject floor = new GameObject(c.ToString() + " floor");
        floor.transform.parent = cell.transform;

        //NavMesh Implimentation
        NavMeshModifier NMM = floor.AddComponent<NavMeshModifier>();
        NMM.area = NavMesh.GetAreaFromName("Walkable");
        NMM.overrideArea = true;

        float startingAngle = degreesPerCell * c.x;
        float degreesPerPoint = degreesPerCell / (curvedWallPoints - 1);

        float innerRadius = baseDistance + (cellSize * c.y) + (wallWidth * c.y) + halfWidth;
        float outerRadius = baseDistance + (cellSize * (c.y + 1)) + (wallWidth * (c.y + 1)) + halfWidth;

        float uvMod = 1f / (curvedWallPoints - 1f);

        Vector3[] verts = new Vector3[curvedWallPoints * 2];
        Vector2[] uvs = new Vector2[curvedWallPoints * 2];
        Vector3[] normals = new Vector3[curvedWallPoints * 2];

        for (int i = 0; i < curvedWallPoints; i++)
        {
            float angle = startingAngle + (degreesPerPoint * i);
            verts[i * 2 + 0] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * innerRadius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * innerRadius);
            verts[i * 2 + 1] = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * outerRadius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * outerRadius);

            uvs[i * 2 + 0] = new Vector2(i * uvMod, 0f);
            uvs[i * 2 + 1] = new Vector2(i * uvMod, 1f);

            normals[i * 2 + 0] = Vector3.up;
            normals[i * 2 + 1] = Vector3.up;

            normals[i * 2 + 0].Normalize();
            normals[i * 2 + 1].Normalize();
        }

        int[] tris = new int[(curvedWallPoints - 1) * 6];


        for (int i = 0; i < curvedWallPoints - 1; i++)
        {
            tris[i * 6 + 0] = i * 2 + 0;
            tris[i * 6 + 1] = i * 2 + 1;
            tris[i * 6 + 2] = i * 2 + 2;

            tris[i * 6 + 3] = i * 2 + 3;
            tris[i * 6 + 4] = i * 2 + 2;
            tris[i * 6 + 5] = i * 2 + 1;
        }


        MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mazeSettings.floorMat;

        MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
        MeshCollider meshCollider = floor.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = tris;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        return floor;
    }
}
