/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour
{
    public GameObject block;
    public GameObject deadEnd;
    public int maze_size_x = 32;
    public int maze_size_y = 16;
    bool[][] maze;

    class pair
    {
        public int first;
        public int second;

        public pair(int f, int s)
        {
            first = f;
            second = s;
        }
    }

    void Start()
    {
        LinkedList<pair> drillers = new LinkedList<pair>();

        maze = new bool[maze_size_y][];
        for (int y = 0; y < maze_size_y; y++)
            maze[y] = new bool[maze_size_x];

        for (int y = 0; y < maze_size_y; y++)
            for (int x = 0; x < maze_size_x; x++)
                maze[y][x] = false;

        drillers.AddLast(new pair(maze_size_x / 2, maze_size_y / 2));
        drillers.AddLast(new pair(maze_size_x / 2, maze_size_y / 2));

        while (drillers.Count > 1)
        {
            LinkedListNode<pair> m, _m, temp;
            m = drillers.First;
            _m = drillers.Last;
            while (m != _m)
            {
                bool remove_driller = false;
                switch (Random.Range(0, 4))
                {
                    case 0:
                        m.Value.second -= 2;
                        if (m.Value.second < 0 || maze[m.Value.second][m.Value.first])
                        {
                            remove_driller = true;
                            break;
                        }
                        maze[m.Value.second + 1][m.Value.first] = true;
                        break;
                    case 1:
                        m.Value.second += 2;
                        if (m.Value.second >= maze_size_y || maze[m.Value.second][m.Value.first])
                        {
                            remove_driller = true;
                            break;
                        }
                        maze[m.Value.second - 1][m.Value.first] = true;
                        break;
                    case 2:
                        m.Value.first -= 2;
                        if (m.Value.first < 0 || maze[m.Value.second][m.Value.first])
                        {
                            remove_driller = true;
                            break;
                        }
                        maze[m.Value.second][m.Value.first + 1] = true;
                        break;
                    case 3:
                        m.Value.first += 2;
                        if (m.Value.first >= maze_size_x || maze[m.Value.second][m.Value.first])
                        {
                            remove_driller = true;
                            break;
                        }
                        maze[m.Value.second][m.Value.first - 1] = true;
                        break;
                }
                if (remove_driller)
                {
                    //GameObject.Instantiate(deadEnd, new Vector3(m.Value.first, m.Value.second, -1), Quaternion.identity);
                    temp = m.Next;
                    drillers.Remove(m);
                    m = temp;
                }
                else
                {
                    //drillers.AddLast(new pair(m.Value.first, m.Value.second));
                    // uncomment the line below to make the maze easier 
                    //if (Random.Range(0, 2) == 1) 
                    //drillers.AddLast(new pair(m.Value.first, m.Value.second));
                    for (int index = 0; index < 10; index++)
                        drillers.AddFirst(new pair(m.Value.first, m.Value.second));

                    maze[m.Value.second][m.Value.first] = true;
                    m = m.Next;
                }
            }
        }

        for (int y = 0; y < maze_size_y; y++)
            for (int x = 0; x < maze_size_x; x++)
            {
                if (!maze[y][x])
                    GameObject.Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
            }
    }
}
*/