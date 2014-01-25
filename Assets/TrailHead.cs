using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailHead : MonoBehaviour
{
    public GameObject deadEnd;
    public List<GenerateLevel.pair> trail;

    void Start()
    {

    }

    void Update()
    {

    }

    public void TurnOn()
    {
        for (int j = 1; j < trail.Count; j++)
        {
            GameObject go = GameObject.Instantiate(deadEnd, new Vector3(trail[j].x, trail[j].y, 0), Quaternion.identity) as GameObject;
            go.renderer.material.color = new Color((float)j / trail.Count, 0, 0);
        }
    }
}
