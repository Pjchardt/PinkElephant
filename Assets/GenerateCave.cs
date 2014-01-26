using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateCave : MonoBehaviour
{
    public GameObject[] players;
    public int wall_fill = 40; // Openess
    public int generate_iter = 4; // Cycles
    public int cleanup_iter = 3; // Cycles

    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;
    public GameObject Corridor;
    public GameObject Key;
    public GameObject Enemy;

    PCG pcg; // Procedural Content Generation grid
    PCGCave pcgc;
    int pcg_type; // PCG type

    byte[][] grid; // Grid array
    byte[][] roomGrid; // Grid array
    public int grid_width; // Grid width
    public int grid_height; // Grid height

    List<Vector2> corridor;
    float time = 0;

    AStar aStar = new AStar();
    public int corridor_weight = 4;
    public int turning_weight = 4;

    List<Vector2>[] floors;
    List<Vector2> floorList = new List<Vector2>();

    Vector2 p1 = new Vector2(-1, -1);
    List<Vector2> asPath = new List<Vector2>();
    bool generate = false;

    int laddJ = 0;

    void Start()
    {
        generateWorld(1, grid_width, grid_height);
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        //if (time >= 1)
        if (Input.GetButtonDown("Fire1"))
        {
            /*laddJ++;
            while (laddJ < grid_height && floors[laddJ].Count < 1)
                laddJ++;

            if (laddJ < grid_height)
            {
                for (int i = 0; i < floors[laddJ].Count; i++)
                {
                    makeLadder((int)floors[laddJ][i].x - 1, (int)floors[laddJ][i].y - 1);
                    makeLadder((int)floors[laddJ][i].x + 1, (int)floors[laddJ][i].y - 1);
                }
            }
            else*/
            {
                //GameObject player = GameObject.Find("Player");
                for (int i=0; i<players.Length; i++)
                    players[i].transform.position = new Vector3(-1, -1, 0);
                if (this.transform.childCount > 0)
                {
                    for (int i = 0; i < this.transform.childCount; i++)
                    {
                        GameObject.Destroy(this.transform.GetChild(i).gameObject);
                    }
                }
                generate = true;
            }
        }
        else if (generate)
        {
            generateWorld(1, grid_width, grid_height);
            generate = false;
            time = 0;
        }

        /*if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider.gameObject.tag == "Door")
            {
                /*if (p1.x == -1)
                {
                    /*for (int j = 0; j < grid_height; j++)
                    {
                        for (int i = 0; i < grid_width; i++)
                        {
                            if (grid[i][j] == 4)
                                grid[i][j] = 0;
                        }
                    }

                    for (int i = 0; i < path.Count; i++)
                    {
                        GameObject.Destroy(path[i]);
                    }
                    path.Clear();*

                    p1 = new Vector2(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y));
                }
                else*
                {
                    int x1 = Mathf.RoundToInt(hit.point.x);
                    int y1 = Mathf.RoundToInt(hit.point.y);
                    int x2 = x1;
                    int y2 = y1;
                    for (int j = y1-1; j > 0; j--)
                    {
                        for (int i = 0; i < floors[j].Count; i++)
                        {
                            Vector3 v1 = new Vector3(x1, y1, 0);
                            Vector3 d1 = new Vector3(floors[j][i].x, floors[j][i].y, 0) - v1;
                            if (!Physics.Raycast(v1, d1, d1.magnitude - 1))
                            {
                                x2 = (int)floors[j][i].x;
                                y2 = (int)floors[j][i].y;
                                break;
                            }
                        }
                    }
                    asPath.Clear();
                    asPath.Add(new Vector2(x2, y2 - 1));
                    aStar.basicAStar(grid, x1, y1, x2, y2, corridor_weight, turning_weight, asPath);
                    asPath.Add(new Vector2(x1, y1));
                    asPath.Add(new Vector2(x1, y1 - 1));

                    GameObject go;
                    /*for (int j = 0; j < grid_height; j++)
                    {
                        for (int i = 0; i < grid_width; i++)
                        {
                            // Render grid cell content
                            switch (grid[i][j])
                            {
                                case 4: // Corridor
                                    go = GameObject.Instantiate(Key, new Vector3(i, j, 2), Quaternion.identity) as GameObject;
                                    go.transform.parent = this.transform;
                                    path.Add(go);
                                    break;
                            }
                        }
                    }*

                    Vector2 lastPlat = asPath[0];
                    for (int i = 1; i < asPath.Count; i++)
                    {
                        if (Mathf.Abs(asPath[i].x - lastPlat.x) > 3 || Mathf.Abs(asPath[i].y - lastPlat.y) > 2)
                        {
                            go = GameObject.Instantiate(Wall, new Vector3(asPath[i - 1].x, asPath[i - 1].y, 0), Quaternion.identity) as GameObject;
                            go.renderer.material.color = Color.magenta;
                            //go.renderer.material.color *= (float)(i + 1) / asPath.Count;
                            go.transform.parent = this.transform;
                            path.Add(go);
                            lastPlat = asPath[i - 1];
                        }
                    }

                    p1 = new Vector2(-1, -1);
                }
            }
        }*/
    }

    public void fill(List<Vector2> group, int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    if (grid[x + i][y + j] == 0)
                    {
                        grid[x + i][y + j] = 3;
                    }
                    else if (grid[x + i][y + j] == 1)
                    {
                        grid[x + i][y + j] = 2;
                        corridor.Remove(new Vector2(x + i, y + j));
                        group.Add(new Vector2(x + i, y + j));
                        fill(group, x + i, y + j);
                    }
                }
            }
        }
    }

    void generateWorld(int p_t, int g_w, int g_h)
    {
        p1 = new Vector2(-1, -1);

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
            case 1: pcgc = new PCGCave();
                pcgc.updateParam(grid_width, grid_height, wall_fill, generate_iter, cleanup_iter);
                pcgc.generatePCGCave(grid);
                break;
        }

        corridor = new List<Vector2>();
        for (int j = 0; j < grid_height; j++)
        {
            for (int i = 0; i < grid_width; i++)
            {
                if (grid[i][j] == 1)
                {
                    corridor.Add(new Vector2(i, j));
                }
            }
        }

        List<List<Vector2>> groups = new List<List<Vector2>>();
        while (corridor.Count > 0)
        {
            int ci = Random.Range(0, corridor.Count);
            grid[(int)corridor[ci].x][(int)corridor[ci].y] = 2;
            int x = (int)corridor[ci].x;
            int y = (int)corridor[ci].y;
            corridor.RemoveAt(ci);
            groups.Add(new List<Vector2>());
            groups[groups.Count - 1].Add(new Vector2(x, y));
            fill(groups[groups.Count - 1], x, y);
        }
        int maxInd = 0;
        int maxVal = groups[0].Count;
        for (int i = 1; i < groups.Count; i++)
        {
            if (groups[i].Count > maxVal)
            {
                maxInd = i;
                maxVal = groups[i].Count;
            }
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if (i != maxInd)
            {
                for (int j = 0; j < groups[i].Count; j++)
                {
                    grid[(int)groups[i][j].x][(int)groups[i][j].y] = 0;
                }
            }
        }

        floors = new List<Vector2>[grid_height];
        floorList.Clear();
        for (int j = 0; j < grid_height; j++)
        {
            if (j + 1 < grid_height)
                floors[j + 1] = new List<Vector2>();
            for (int i = 0; i < grid_width; i++)
            {
                if (grid[i][j] == 3)
                {
                    if (j + 1 < grid_height && grid[i][j + 1] == 2)
                    {
                        grid[i][j + 1] = 4;
                        floors[j + 1].Add(new Vector2(i, j + 1));
                        floorList.Add(new Vector2(i, j + 1));
                    }
                }
            }
        }

        for (int j = 0; j < grid_height; j++)
        {
            for (int i = 0; i < grid_width; i++)
            {
                if (grid[i][j] == 0)
                    grid[i][j] = 1;
                else if (grid[i][j] == 2)
                    grid[i][j] = 0;
                else if (grid[i][j] == 3)
                    grid[i][j] = 2;
                else if (grid[i][j] == 4)
                    grid[i][j] = 3;
            }
        }

        /*int ri = Random.Range(0, floors.Count);
        int ri2 = Random.Range(0, floors.Count);
        aStar.basicAStar(grid, (int)floors[ri].x, (int)floors[ri].y, (int)floors[ri2].x, (int)floors[ri2].y, corridor_weight, turning_weight);
        */
        GameObject go;
        for (int j = 0; j < grid_height; j++)
        {
            for (int i = 0; i < grid_width; i++)
            {
                // Render grid cell content
                switch (grid[i][j])
                {
                    case 0: // Empty
                        break;
                    case 1: // Floor
                        go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        break;
                    case 2: // Wall
                        go = GameObject.Instantiate(Wall, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        break;
                    /*case 3: // Door
                        go = GameObject.Instantiate(Door, new Vector3(i, j, 2), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        break;
                    case 4: // Corridor
                        go = GameObject.Instantiate(Key, new Vector3(i, j, 2), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        path.Add(go);
                        break;*/
                }
            }
        }
        
        //laddJ = 0;
        
        for (int j = 1; j < grid_height; j++)
        {
            for (int i = 0; i < floors[j].Count; i++)
            {
                makeLadder((int)floors[j][i].x - 1, (int)floors[j][i].y - 1);
                makeLadder((int)floors[j][i].x + 1, (int)floors[j][i].y - 1);
            }
        }


        for (int i = 0; i < players.Length; i++)
        {
            int fi = Random.Range(0, floorList.Count);
            players[i].transform.position = new Vector3(floorList[fi].x, floorList[fi].y, 0);
            floorList.RemoveAt(fi);
        }

        for (int i = 0; i < floorList.Count / 4; i++)
        {
            int fi = Random.Range(0, floorList.Count);
            go = GameObject.Instantiate(Key, new Vector3(floorList[fi].x, floorList[fi].y, 0), Quaternion.identity) as GameObject;
            go.transform.parent = this.transform;
            floorList.RemoveAt(fi);
        }
    }

    /*
    void generateWorldB(int p_t, int g_w, int g_h)
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
            case 1: pcgc = new PCGCave();
                pcgc.updateParam(grid_width, grid_height, wall_fill, generate_iter, cleanup_iter);
                pcgc.generatePCGCave(grid);
                break;
        }

        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().ladders = 0;
        if (this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject.Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        corridor = new List<Vector2>();

        GameObject go;
        for (int j = 0; j < grid_height; j++)
        {
            for (int i = 0; i < grid_width; i++)
            {
                // Render grid cell content
                switch (grid[i][j])
                {
                    case 0:
                        go = GameObject.Instantiate(Floor, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        break;
                    case 1:
                        corridor.Add(new Vector2(i, j));
                        break;
                }
            }
        }

        List<List<Vector2>> groups = new List<List<Vector2>>();
        while (corridor.Count > 0)
        {
            int ci = Random.Range(0, corridor.Count);
            grid[(int)corridor[ci].x][(int)corridor[ci].y] = 2;
            int x = (int)corridor[ci].x;
            int y = (int)corridor[ci].y;
            corridor.RemoveAt(ci);
            groups.Add(new List<Vector2>());
            groups[groups.Count - 1].Add(new Vector2(x, y));
            fill(groups[groups.Count - 1], x, y);
        }
        int maxInd = 0;
        int maxVal = groups[0].Count;
        for (int i = 1; i < groups.Count; i++)
        {
            if (groups[i].Count > maxVal)
            {
                maxInd = i;
                maxVal = groups[i].Count;
            }
        }
        int gi = Random.Range(0, groups[maxInd].Count);
        player.transform.position = new Vector3(groups[maxInd][gi].x, groups[maxInd][gi].y, 0);

        for (int i = 0; i < groups.Count; i++)
        {
            if (i != maxInd)
            {
                for (int j = 0; j < groups[i].Count; j++)
                {
                    go = GameObject.Instantiate(Wall, new Vector3(groups[i][j].x, groups[i][j].y, 0), Quaternion.identity) as GameObject;
                    go.transform.parent = this.transform;
                }
            }
        }

        List<List<Vector2>> floors = new List<List<Vector2>>();
        for (int j = 0; j < grid_height; j++)
        {
            bool first = true;
            for (int i = 0; i < grid_width; i++)
            {
                // Render grid cell content
                switch (grid[i][j])
                {
                    /*case 2:
                        go = GameObject.Instantiate(Key, new Vector3(i, j, 1), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        break;*
                    case 3:
                        if (j + 1 < grid_height && grid[i][j + 1] == 2)
                        {
                            if (first)
                            {
                                floors.Add(new List<Vector2>());
                                first = false;
                            }
                            floors[floors.Count-1].Add(new Vector2(i, j + 1));
                            /*if (i - 1 >= 0)
                                makeLadder(i - 1, j);
                            if (i + 1 < grid_width)
                                makeLadder(i + 1, j);*
                        }
                        break;
                }
            }
        }
        /*
        for (int i = 0; i < 8; i++)
        {
            int fi = Random.Range(0, floors.Count);
            go = GameObject.Instantiate(Key, new Vector3(floors[fi].x, floors[fi].y, 0), Quaternion.identity) as GameObject;
            go.transform.parent = this.transform;
            floors.RemoveAt(fi);
        }*
        for (int j = 0; j < floors.Count; j++)
        {
            for (int i = 0; i < floors[j].Count; i++)
            {
                go = GameObject.Instantiate(Wall, new Vector3(floors[j][i].x, floors[j][i].y, 2), Quaternion.identity) as GameObject;
                go.transform.parent = this.transform;
                go.renderer.material.color *= (float)(j + 1) / floors.Count;
            }
        }

        //aStar.basicAStar(pcgrid, rm1.opening[0][0], rm1.opening[0][1], rm2.opening[0][0], rm2.opening[0][1], corridor_weight, turning_weight);
    }
*/

    public Vector2 getDest(int x, int y)
    {
        Vector3 v1 = new Vector3(x, y, 0);
        for (int j = y-2; j > 0; j--)
        {
            for (int i = 0; i < floors[j].Count; i++)
            {
                Vector3 d1 = new Vector3(floors[j][i].x, floors[j][i].y, 0) - v1;
                if (!Physics.Raycast(v1, d1, d1.magnitude))
                {
                    return floors[j][i];
                }
            }
        }

        return new Vector2(x, y);
    }

    public void makeLadder(int x, int y)
    {
        int x1 = x;
        int y1 = y;

        List<Vector3> ladder = new List<Vector3>();
        while (grid[x][y] == 0)
        {
            ladder.Add(new Vector3(x, y, 1));
            y--;
        }

        if (ladder.Count > 1)
        {
            Vector2 dest = getDest(x1, y1);
            int x2 = (int)dest.x;
            int y2 = (int)dest.y;

            asPath.Clear();
            //asPath.Add(new Vector2(x2, y2 - 1));
            aStar.basicAStar(grid, x1, y1, x2, y2, corridor_weight, turning_weight, asPath);

            if (asPath.Count < 1)
            {
                GameObject go;
                for (int i = 0; i < ladder.Count; i++)
                {
                    go = GameObject.Instantiate(Door, ladder[i], Quaternion.identity) as GameObject;
                    go.transform.parent = this.transform;
                }
            }
            else
            {
                asPath.Add(new Vector2(x1, y1));
                //asPath.Add(new Vector2(x1, y1 - 1));

                /*if (firstLad)
                {
                    GameObject go;
                    for (int i = 0; i < asPath.Count; i++)
                    {
                        go = GameObject.Instantiate(Key, new Vector3(asPath[i].x, asPath[i].y, 2), Quaternion.identity) as GameObject;
                        go.transform.parent = this.transform;
                        go.renderer.material.color *= (float)(i + 1) / asPath.Count;
                    }
                    firstLad = false;
                }*/

                Vector2 lastPlat = new Vector2(asPath[0].x, asPath[0].y - 1);
                GameObject go;
                List<Vector2> plats = new List<Vector2>();
                for (int i = 1; i < asPath.Count; i++)
                {
                    if (Mathf.Abs(asPath[i].x - lastPlat.x) > 3 || Mathf.Abs(asPath[i].y - lastPlat.y) > 2)
                    {
                        int px = (int)asPath[i - 1].x;
                        int py = (int)asPath[i - 1].y;
                        if (grid[px][py + 1] != 2 && ((grid[px - 1][py - 1] != 2 && grid[px - 1][py] != 2 && grid[px - 1][py + 1] != 2)
                                                        || (grid[px + 1][py - 1] != 2 && grid[px + 1][py] != 2 && grid[px + 1][py + 1] != 2)))
                        {
                            plats.Add(new Vector2(px, py));
                            lastPlat = asPath[i - 1];
                        }
                        else
                        {
                            for (int j = 0; j < ladder.Count; j++)
                            {
                                go = GameObject.Instantiate(Door, ladder[j], Quaternion.identity) as GameObject;
                                go.transform.parent = this.transform;
                            }
                            return;
                        }
                    }
                }
                for (int i = 0; i < plats.Count; i++)
                {
                    go = GameObject.Instantiate(Wall, new Vector3(plats[i].x, plats[i].y, 0), Quaternion.identity) as GameObject;
                    //go.renderer.material.color = Color.magenta;
                    go.transform.parent = this.transform;
                    grid[(int)plats[i].x][(int)plats[i].y] = 2;
                    grid[(int)plats[i].x][(int)plats[i].y + 1] = 3;
                    floors[(int)plats[i].y + 1].Add(new Vector2(plats[i].x, plats[i].y + 1));
                    floorList.Remove(new Vector2(plats[i].x, plats[i].y));
                    floorList.Add(new Vector2(plats[i].x, plats[i].y + 1));
                }
                //go = GameObject.Instantiate(Key, new Vector3(asPath[0].x, asPath[0].y, 0), Quaternion.identity) as GameObject;
                //go.transform.parent = this.transform;
            }
        }
    }
}
