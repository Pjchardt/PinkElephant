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

    public int roomsMin = 8;
    public int roomsMax = 8;

    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;
    public GameObject Corridor;
    public GameObject Key;
    public GameObject Enemy;
    public GameObject Goal;

    byte[][] grid;
    List<byte[][]> roomGrids = new List<byte[][]>();
    List<Vector3> roomPos = new List<Vector3>();

    public static GameObject player;
	private bool movingCamera = false;
	private Vector3 cameraTarget;
	public float CameraSpeed = 50f;

    public static int mapX;
    public static int mapY;
    public static int roomGridWidthStatic;
    public static int roomGridHeightStatic;

    void Start()
    {
        roomGridWidthStatic = roomGridWidth;
        roomGridHeightStatic = roomGridHeight;

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
                    case 3: // Door
                        go = GameObject.Instantiate(Door, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                        //go.GetComponent<Door>().key = roomGrid[i][j] - 1;
                        break;
                    /*case 4: // Corridor
                        //go = GameObject.Instantiate(Corridor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        corridor.Add(new Vector2(i, j));
                        break;*/
                }
            }
        }

        for (int r = 0; r < roomGrids.Count; r++)
        {
            for (int i = 0; i < 4; i++)
            {
                int x = Random.Range(0, roomGrids[r].Length);
                int y = Random.Range(0, roomGrids[r][x].Length);
                roomGrids[r][x][y] = 1;
            }
            if (r == 0)
            {
                roomGrids[r][0][0] = 2;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int x = Random.Range(0, roomGrids[r].Length);
                    int y = Random.Range(0, roomGrids[r][x].Length);
                    roomGrids[r][x][y] = 2;
                }
            }
        }

        roomGrids[roomGrids.Count - 1][roomGrids[roomGrids.Count - 1].Length / 2][roomGrids[roomGrids.Count - 1][0].Length / 2] = 3;

        for (int r = 0; r < roomGrids.Count; r++)
        {
            for (int i = 0; i < roomGrids[r].Length; i++)
            {
                for (int j = 0; j < roomGrids[r][i].Length; j++)
                {
                    // Render grid cell content
                    switch (roomGrids[r][i][j])
                    {
                        case 0: // Empty
                            //go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            break;
                        case 1: // Floor
                            go = GameObject.Instantiate(Key, roomPos[r] + new Vector3(i, j, 0) / roomScale, Quaternion.identity) as GameObject;
                            //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                            //floors[roomGrid[i][j] - 1].Add(new Vector2(i, j));
                            break;
                        case 2: // Wall
                            go = GameObject.Instantiate(Enemy, roomPos[r] + new Vector3(i, j, 0) / roomScale, Quaternion.identity) as GameObject;
                            //go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            //go = GameObject.Instantiate(Wall, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                            break;
                        case 3: // Door
                            go = GameObject.Instantiate(Goal, roomPos[r] + new Vector3(i, j, 0) / roomScale, Quaternion.identity) as GameObject;
                            //go = GameObject.Instantiate(Door, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            //go.renderer.material.color *= (float)roomGrid[i][j] / pcgb.rooms.Count;
                            //go.GetComponent<Door>().key = roomGrid[i][j] - 1;
                            break;
                        /*case 4: // Corridor
                            //go = GameObject.Instantiate(Corridor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                            corridor.Add(new Vector2(i, j));
                            break;*/
                    }
                }
            }
        }

        player.transform.position = roomPos[0] + new Vector3(roomGrids[0].Length / 2, roomGrids[0][0].Length / 2, 0) / roomScale;
        mapX = (int)(player.transform.position.x / roomGridWidth);
        mapY = (int)(player.transform.position.y / roomGridHeight);

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
        this.transform.position = new Vector3(roomGridWidth / 2f - 0.5f + mapX * roomGridWidth,
            roomGridHeight / 2f - 0.5f + mapY * roomGridHeight,
            this.transform.position.z);
        //this.camera.orthographicSize = roomGridHeight * mapGridHeight / 2f;
        //this.transform.position = new Vector3(roomGridWidth * mapGridWidth / 2f - 0.5f, roomGridHeight * mapGridHeight / 2f - 0.5f, this.transform.position.z);
    }

    void Update()
    {
		if (!movingCamera)
		{
			LookAtPlayerPosition();
		}
		else
		{
			MoveCamera();
		}
    }

    void generateWorld()
    {
        grid = new byte[mapGridWidth * roomGridWidth][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new byte[mapGridHeight * roomGridHeight];
        }

        List<int> roomIndexes = new List<int>();
        for (int i = 0; i < mapGridWidth * mapGridHeight; i++)
            roomIndexes.Add(i);

        int numRooms = Random.Range(roomsMin, roomsMax);
        while (roomIndexes.Count > numRooms)
            roomIndexes.RemoveAt(Random.Range(0, roomIndexes.Count));

        for (int i = 0; i < mapGridWidth; i++)
        {
            for (int j = 0; j < mapGridHeight; j++)
            {
                if (roomIndexes.Contains(j * mapGridWidth + i))
                {
                    int roomWidth = Random.Range(roomGridWidth - 4, roomGridWidth - 2);
                    int roomHeight = Random.Range(roomGridHeight - 4, roomGridHeight - 2);
                    int startX = i * roomGridWidth + (roomGridWidth - roomWidth) / 2;
                    int startY = j * roomGridHeight + (roomGridHeight - roomHeight) / 2;
                    for (int x = 0; x < roomWidth; x++)
                    {
                        for (int y = 0; y < roomHeight; y++)
                        {
                            grid[startX + x][startY + y] = 1;
                        }
                    }
                    roomGrids.Add(new byte[roomWidth * roomScale][]);
                    roomPos.Add(new Vector3(startX - 0.5f / roomScale, startY - 0.5f / roomScale, 0));
                    for (int x = 0; x < roomGrids[roomGrids.Count - 1].Length; x++)
                        roomGrids[roomGrids.Count - 1][x] = new byte[roomHeight * roomScale];
                }
                if (i > 0)
                {
                    int y = j * roomGridHeight + roomGridHeight / 2;
                    for (int x = 0; x < roomGridWidth / 2 + 1; x++)
                    {
                        if (grid[i * roomGridWidth + x + 1][y] == 1)
                        {
                            grid[i * roomGridWidth + x][y] = 3;
                            grid[i * roomGridWidth + x][y - 1] = 3;
                            break;
                        }
                        int width = Random.Range(1, 4);
                        if (width == 3)
                            for (int w = 0; w < width; w++)
                                grid[i * roomGridWidth + x][y - w + 1] = 2;
                        else
                            for (int w = 0; w < width; w++)
                                grid[i * roomGridWidth + x][y - w] = 2;
                    }
                }
                if (i < mapGridWidth - 1)
                {
                    int y = j * roomGridHeight + roomGridHeight / 2;
                    for (int x = roomGridWidth - 1; x >= roomGridWidth / 2 - 1; x--)
                    {
                        if (grid[i * roomGridWidth + x - 1][y] == 1)
                        {
                            grid[i * roomGridWidth + x][y] = 3;
                            grid[i * roomGridWidth + x][y - 1] = 3;
                            break;
                        }
                        int width = Random.Range(1, 4);
                        if (width == 3)
                            for (int w = 0; w < width; w++)
                                grid[i * roomGridWidth + x][y - w + 1] = 2;
                        else
                            for (int w = 0; w < width; w++)
                                grid[i * roomGridWidth + x][y - w] = 2;
                    }
                }
                if (j > 0)
                {
                    int x = i * roomGridWidth + roomGridWidth / 2;
                    for (int y = 0; y < roomGridHeight / 2 + 1; y++)
                    {
                        if (grid[x][j * roomGridHeight + y + 1] == 1)
                        {
                            grid[x][j * roomGridHeight + y] = 3;
                            grid[x - 1][j * roomGridHeight + y] = 3;
                            break;
                        }
                        int width = Random.Range(1, 4);
                        if (width == 3)
                            for (int w = 0; w < width; w++)
                                grid[x - w + 1][j * roomGridHeight + y] = 2;
                        else
                            for (int w = 0; w < width; w++)
                                grid[x - w][j * roomGridHeight + y] = 2;
                    }
                }
                if (j < mapGridHeight - 1)
                {
                    int x = i * roomGridWidth + roomGridWidth / 2;
                    for (int y = roomGridHeight - 1; y >= roomGridHeight / 2 - 1; y--)
                    {
                        if (grid[x][j * roomGridHeight + y - 1] == 1)
                        {
                            grid[x][j * roomGridHeight + y] = 3;
                            grid[x - 1][j * roomGridHeight + y] = 3;
                            break;
                        }
                        int width = Random.Range(1, 4);
                        if (width == 3)
                            for (int w = 0; w < width; w++)
                                grid[x - w + 1][j * roomGridHeight + y] = 2;
                        else
                            for (int w = 0; w < width; w++)
                                grid[x - w][j * roomGridHeight + y] = 2;
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

	private void LookAtPlayerPosition()
	{
		if (player.transform.position.x > transform.position.x + roomGridWidth / 2f)
		{
            mapX++;
			movingCamera = true;
			player.GetComponent<Player>().PausePlayer(true);
			cameraTarget = transform.position + new Vector3(roomGridWidth, 0, 0);
			//player.GetComponent<MovePlayer>().StartMoving(transform.position.x + (roomGridWidth * .6f), CameraSpeed * 10f);
		}
		else if (player.transform.position.x < transform.position.x - roomGridWidth / 2f)
		{
            mapX--;
			movingCamera = true;
			player.GetComponent<Player>().PausePlayer(true);
			cameraTarget = transform.position - new Vector3(roomGridWidth, 0, 0);
			//player.GetComponent<MovePlayer>().StartMoving(transform.position + (cameraTarget-transform.position) * .4f , CameraSpeed * 10f);
		}
		
		if (player.transform.position.y > transform.position.y + roomGridHeight / 2f)
		{
            mapY++;
			movingCamera = true;
			player.GetComponent<Player>().PausePlayer(true);
			cameraTarget = transform.position + new Vector3(0, roomGridHeight, 0);
			//player.GetComponent<MovePlayer>().StartMoving(transform.position - (cameraTarget-transform.position) * .4f , CameraSpeed * 10f);
		}
		else if (player.transform.position.y < transform.position.y - roomGridHeight / 2f)
		{
            mapY--;
			movingCamera = true;
			player.GetComponent<Player>().PausePlayer(true);
			cameraTarget = transform.position - new Vector3(0, roomGridHeight, 0);
			//player.GetComponent<MovePlayer>().StartMoving(transform.position - (cameraTarget-transform.position) * .4f , CameraSpeed * 10f);
		}
	}

	private void MoveCamera()
	{
		//move camera over time
		Vector3 temp = cameraTarget - transform.position;
		if ((CameraSpeed * Time.deltaTime) < Vector3.Distance(transform.position, cameraTarget) - .1)
		{
			transform.position += temp.normalized * CameraSpeed * Time.deltaTime;
		}
		else
		{
			transform.position = cameraTarget;
			movingCamera = false;
			player.GetComponent<Player>().PausePlayer(false);
			//player.GetComponent<Player>().UnConnect();
			player.GetComponent<Player>().SetNewMouse(Camera.main.WorldToScreenPoint(player.transform.position));
			Debug.Log ("Finished moving");
		}
	}


}
