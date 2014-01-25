using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateRooms : MonoBehaviour
{
    public int mapGridWidth = 4;
    public int mapGridHeight = 4;
    public int roomGridWidth = 8;
    public int roomGridHeight = 6;
    public int roomScale = 2;

    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;
    public GameObject Corridor;
    public GameObject Key;
    public GameObject Enemy;

    byte[][] grid;
    List<byte[][]> roomGrids;

    GameObject player;

    void Start()
    {
        generateWorld();

        player = GameObject.Find("Player");

        //List<Vector2> corridor = new List<Vector2>();
        //List<List<Vector2>> floors = new List<List<Vector2>>();

        GameObject go;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
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
                        //floors[roomGrid[i][j] - 1].Add(new Vector2(i, j));
                        break;
                    case 2: // Wall
                        //go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        //go = GameObject.Instantiate(Wall, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                        break;
                    /*case 3: // Door
                        go = GameObject.Instantiate(Door, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                        go.GetComponent<Door>().key = roomGrid[i][j] - 1;
                        break;
                    case 4: // Corridor
                        //go = GameObject.Instantiate(Corridor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        corridor.Add(new Vector2(i, j));
                        break;*/
                }
            }
        }
        /*int ci = Random.Range(0, corridor.Count);
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
        }*/

        this.camera.orthographicSize = roomGridHeight / 2f;
        this.transform.position = new Vector3(roomGridWidth / 2f - 0.5f, roomGridHeight / 2f - 0.5f, this.transform.position.z);
        //this.camera.orthographicSize = roomGridHeight * mapGridHeight / 2f;
        //this.transform.position = new Vector3(roomGridWidth * mapGridWidth / 2f - 0.5f, roomGridHeight * mapGridHeight / 2f - 0.5f, this.transform.position.z);
    }

    void Update()
    {
        if (player.transform.position.x > transform.position.x + roomGridWidth / 2f)
            transform.position += new Vector3(roomGridWidth, 0, 0);
        else if (player.transform.position.x < transform.position.x - roomGridWidth / 2f)
            transform.position -= new Vector3(roomGridWidth, 0, 0);

        if (player.transform.position.y > transform.position.y + roomGridHeight / 2f)
            transform.position += new Vector3(0, roomGridHeight, 0);
        else if (player.transform.position.y < transform.position.y - roomGridHeight / 2f)
            transform.position -= new Vector3(0, roomGridHeight, 0);
    }

    void generateWorld()
    {
        grid = new byte[mapGridWidth * roomGridWidth][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new byte[mapGridHeight * roomGridHeight];
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = 0;
            }
        }

        for (int i = 0; i < mapGridWidth; i++)
        {
            for (int j = 0; j < mapGridHeight; j++)
            {
                if (Random.value > 0.5f)
                {
                    int roomWidth = Random.Range(roomGridWidth / 2, roomGridWidth - 2);
                    int roomHeight = Random.Range(roomGridHeight / 2, roomGridHeight - 2);
                    int startX = i * roomGridWidth + (roomGridWidth - roomWidth) / 2;
                    int startY = j * roomGridHeight + (roomGridHeight - roomHeight) / 2;
                    for (int x = 0; x < roomWidth; x++)
                    {
                        for (int y = 0; y < roomHeight; y++)
                        {
                            grid[startX + x][startY + y] = 1;
                        }
                    }
                    //roomGrids.Add(new byte[][]);
                }
                //else
                {
                    if (i > 0)
                    {
                        int y = j * roomGridHeight + roomGridHeight / 2;
                        for (int x = 0; x < roomGridWidth / 2; x++)
                        {
                            grid[i * roomGridWidth + x][y] = 2;
                        }
                    }
                    if (i < mapGridWidth - 1)
                    {
                        int y = j * roomGridHeight + roomGridHeight / 2;
                        for (int x = roomGridWidth / 2; x < roomGridWidth; x++)
                        {
                            grid[i * roomGridWidth + x][y] = 2;
                        }
                    }
                    if (j > 0)
                    {
                        int x = i * roomGridWidth + roomGridWidth / 2;
                        for (int y = 0; y < roomGridHeight / 2; y++)
                        {
                            grid[x][j * roomGridHeight + y] = 2;
                        }
                    }
                    if (j < mapGridHeight - 1)
                    {
                        int x = i * roomGridWidth + roomGridWidth / 2;
                        for (int y = roomGridHeight / 2; y < roomGridHeight; y++)
                        {
                            grid[x][j * roomGridHeight + y] = 2;
                        }
                    }
                }
            }
        }

        /*
        roomGrid = new byte[grid_width][];
        for (int i = 0; i < grid_width; i++)
        {
            roomGrid[i] = new byte[grid_height];
            for (int j = 0; j < grid_height; j++)
            {
                roomGrid[i][j] = 0; // Initialize all cell as empty
            }
        }*/
    }
}
