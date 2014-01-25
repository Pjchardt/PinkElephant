using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour
{
    public GameObject block;
    public GameObject deadEnd;
    public int maze_size_x = 33;
    public int maze_size_y = 17;
    bool[][] maze;

    public struct pair
    {
        public int x;
        public int y;

        public pair(int f, int s)
        {
            x = f;
            y = s;
        }
    }

    void drill(int x, int y)
    {
        int first = Random.Range(0, 4);
        int x0 = x;
        int y0 = y;
        for (int i = 0; i < 4; i++)
        {
            bool remove_driller = false;
            x = x0;
            y = y0;
            switch ((first + i) % 4)
            {
                case 0:
                    y -= 2;
                    if (y < 0 || maze[y][x])
                    {
                        remove_driller = true;
                    }
                    else
                    {
                        maze[y + 1][x] = true;
                    }
                    break;
                case 1:
                    y += 2;
                    if (y >= maze_size_y || maze[y][x])
                    {
                        remove_driller = true;
                    }
                    else
                    {
                        maze[y - 1][x] = true;
                    }
                    break;
                case 2:
                    x -= 2;
                    if (x < 0 || maze[y][x])
                    {
                        remove_driller = true;
                    }
                    else
                    {
                        maze[y][x + 1] = true;
                    }
                    break;
                case 3:
                    x += 2;
                    if (x >= maze_size_x || maze[y][x])
                    {
                        remove_driller = true;
                    }
                    else
                    {
                        maze[y][x - 1] = true;
                    }
                    break;
            }
            if (remove_driller)
            {
                //GameObject.Instantiate(deadEnd, new Vector3(x, y, -1), Quaternion.identity);
            }
            else
            {
                maze[y][x] = true;
                drill(x, y);
            }
        }
    }

    void Start()
    {
        //LinkedList<pair> drillers = new LinkedList<pair>();

        maze = new bool[maze_size_y][];
        for (int y = 0; y < maze_size_y; y++)
            maze[y] = new bool[maze_size_x];

        for (int y = 0; y < maze_size_y; y++)
            for (int x = 0; x < maze_size_x; x++)
                maze[y][x] = false;

        //drill(maze_size_x / 2, maze_size_y / 2);
        drill(0, 0);

        //drillers.AddLast(new pair(maze_size_x / 2, maze_size_y / 2));

        /*maze_size_x -= 2;
        maze_size_y -= 2;

        bool[][] newMaze = new bool[maze_size_y][];
        for (int y = 0; y < maze_size_y; y++)
            newMaze[y] = new bool[maze_size_x];

        for (int y = 0; y < maze_size_y; y++)
            for (int x = 0; x < maze_size_x; x++)
                newMaze[y][x] = !maze[y+1][x+1];
        maze = newMaze;*/

        List<pair> open = new List<pair>();
        for (int y = 0; y < maze_size_y; y++)
            for (int x = 0; x < maze_size_x; x++)
            {
                if (maze[y][x])
                    GameObject.Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
                else if (x == 0 || x == maze_size_x-1 || y == 0 || y == maze_size_y-1)
                    open.Add(new pair(x, y));
            }

        List<List<pair>> trails = new List<List<pair>>();
        List<List<pair>> ends = new List<List<pair>>();
        for (int i=0; i<open.Count; i++)
        {
            trails.Add(new List<pair>());
            trails[i].Add(open[i]);
            ends.Add(new List<pair>());
            traverse(trails[i], ends[i]);
        }
        
        /*for (int i = 0; i < 20; i++)
        while (open.Count > 0)
        {
            int oi = Random.Range(0, open.Count);
            GameObject.Instantiate(deadEnd, new Vector3(open[oi].x, open[oi].y, 0), Quaternion.identity);
            open.RemoveAt(oi);
        }*/
        
        for (int i = 0; i < trails.Count; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                GameObject go = GameObject.Instantiate(deadEnd, new Vector3(trails[i][j].x, trails[i][j].y, 0), Quaternion.identity) as GameObject;
                TrailHead th = go.AddComponent<TrailHead>();
                th.deadEnd = deadEnd;
                th.trail = trails[i];
            }
        }
        /*for (int i = 0; i < ends.Count; i++)
        {
            for (int j = 0; j < ends[i].Count; j++)
            {
                GameObject go = GameObject.Instantiate(deadEnd, new Vector3(ends[i][j].x, ends[i][j].y, 0), Quaternion.identity) as GameObject;
            }
        }*/
    }

    void traverse(List<pair> trail, List<pair> ends)
    {
        List<int> ind = new List<int>();
        ind.Add(0);
        while (ind.Count > 0)
        {
            int lastCnt = ind.Count;
            for (int j = 0; j < lastCnt; j++)
            {
                int cnt = ind.Count;
                for (int i = 0; i < 4; i++)
                {
                    int x = trail[ind[j]].x;
                    int y = trail[ind[j]].y;
                    bool oob = false;
                    switch (i)
                    {
                        case 0:
                            y -= 1;
                            oob = y < 0;
                            break;
                        case 1:
                            y += 1;
                            oob = y >= maze_size_y;
                            break;
                        case 2:
                            x -= 1;
                            oob = x < 0;
                            break;
                        case 3:
                            x += 1;
                            oob = x >= maze_size_x;
                            break;
                    }
                    if (!oob && !maze[y][x] && !trail.Contains(new pair(x, y)))
                    {
                        trail.Add(new pair(x, y));
                        ind.Add(trail.Count - 1);
                    }
                }
                if (cnt == ind.Count)
                    ends.Add(trail[ind[j]]);
            }
            ind.RemoveRange(0, lastCnt);
        }
    }
    /*
    void traverse(List<pair> trail)
    {
        int ti = trail.Count-1;
        for (int i = 0; i < 4; i++)
        {
            int x = trail[ti].x;
            int y = trail[ti].y;
            bool oob = false;
            switch (i)
            {
                case 0:
                    y -= 1;
                    oob = y < 0;
                    break;
                case 1:
                    y += 1;
                    oob = y >= maze_size_y;
                    break;
                case 2:
                    x -= 1;
                    oob = x < 0;
                    break;
                case 3:
                    x += 1;
                    oob = x >= maze_size_x;
                    break;
            }
            if (!oob && !maze[y][x] && !trail.Contains(new pair(x, y)))
            {
                trail.Add(new pair(x, y));
                traverse(trail);
                //break;
            }
        }
    }*/
}
