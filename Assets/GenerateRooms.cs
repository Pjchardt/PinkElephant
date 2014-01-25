using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateRooms : MonoBehaviour
{
    public float areaWidth;
    public float areaHeight;

    public int room_min_size = 9;
    public int room_max_size = 16;
    public int corridor_num = 2;
    public int corridor_weight = 4;
    public int turning_weight = 4;
    public int room_type = 0; // Room type

    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;
    public GameObject Corridor;
    public GameObject Key;
    public GameObject Enemy;

    PCG pcg; // Procedural Content Generation grid
    PCGBasic pcgb; // Procedural Content Generation grid Basic
    int pcg_type; // PCG type

    byte[][] grid; // Grid array
    byte[][] roomGrid; // Grid array
    int grid_width; // Grid width
    int grid_height; // Grid height

    GameObject player;

    void Start()
    {
        generateWorld(1, 40, 30);

        player = GameObject.Find("Player");

        List<Vector2> corridor = new List<Vector2>();
        List<List<Vector2>> floors = new List<List<Vector2>>();
        for (int i = 0; i < pcgb.rooms.Count; i++)
            floors.Add(new List<Vector2>());
        GameObject go;
        for (int j = 0; j < grid_height; j++)
        {
            for (int i = 0; i < grid_width; i++)
            {
                // Render grid cell content
                switch (grid[i][j])
                {
                    case 0: // Empty
                        go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        break;
                    case 1: // Floor
                        //go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                        floors[roomGrid[i][j] - 1].Add(new Vector2(i, j));
                        break;
                    case 2: // Wall
                        go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        //go = GameObject.Instantiate(Wall, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                        break;
                    case 3: // Door
                        go = GameObject.Instantiate(Door, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                        go.GetComponent<Door>().key = roomGrid[i][j] - 1;
                        break;
                    case 4: // Corridor
                        //go = GameObject.Instantiate(Corridor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        corridor.Add(new Vector2(i, j));
                        break;
                }
            }
        }
        int ci = Random.Range(0, corridor.Count);
        player.transform.position = new Vector3(corridor[ci].x, corridor[ci].y, 0);
        corridor.RemoveAt(ci);
        for (int i = 0; i < pcgb.rooms.Count-1; i++)
        {
            int fi = Random.Range(0, floors[i].Count);
            go = GameObject.Instantiate(Key, new Vector3(floors[i][fi].x, floors[i][fi].y, 0), Quaternion.identity) as GameObject;
            go.renderer.material.color *= (float)(i + 1) / pcgb.rooms.Count;
            go.GetComponent<Key>().door = i + 1;
            floors[i].RemoveAt(fi);
            for (int j = 0; j < 2; j++)
            {
                fi = Random.Range(0, floors[i].Count);
                go = GameObject.Instantiate(Enemy, new Vector3(floors[i][fi].x, floors[i][fi].y, 0), Quaternion.identity) as GameObject;
            }
        }
    }

    void Update()
    {
        if (player.transform.position.x > transform.position.x + areaWidth / 2f)
            transform.position += new Vector3(areaWidth, 0, 0);
        else if (player.transform.position.x < transform.position.x - areaWidth / 2f)
            transform.position -= new Vector3(areaWidth, 0, 0);

        if (player.transform.position.y > transform.position.y + areaHeight / 2f)
            transform.position += new Vector3(0, areaHeight, 0);
        else if (player.transform.position.y < transform.position.y - areaHeight / 2f)
            transform.position -= new Vector3(0, areaHeight, 0);
    }

    void generateWorld(int p_t, int g_w, int g_h)
    {
        pcg_type = p_t; // PGC type
        grid_width = g_w; // Grid width
        grid_height = g_h; // Grid height

        grid = new byte[grid_width][];
        for (int i = 0; i < grid_width; i++)
        {
            grid[i] = new byte[grid_height];
            for (int j = 0; j < grid_height; j++)
            {
                grid[i][j] = 0; // Initialize all cell as empty
            }
        }

        roomGrid = new byte[grid_width][];
        for (int i = 0; i < grid_width; i++)
        {
            roomGrid[i] = new byte[grid_height];
            for (int j = 0; j < grid_height; j++)
            {
                roomGrid[i][j] = 0; // Initialize all cell as empty
            }
        }

        // Generate PCG
        switch (pcg_type)
        {
            case 0: pcg = new PCG();
                pcg.updateParam(grid_width, grid_height);
                pcg.generatePCG(grid);
                break;
            case 1: pcgb = new PCGBasic();
                pcgb.updateParam(grid_width, grid_height, room_type, room_min_size, room_max_size, corridor_num, corridor_weight, turning_weight);
                pcgb.generatePCGBasic(grid, roomGrid);
                break;
        }
    }
}
