using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int mapX = 0;
    public int mapY = 0;
    GameObject target = null;

    void Start()
    {
        mapX = (int)(this.transform.position.x / GenerateRooms.roomGridWidthStatic);
        mapY = (int)(this.transform.position.y / GenerateRooms.roomGridHeightStatic);
    }

    void Update()
    {
        if (target == null)
        {
            if (GenerateRooms.mapX == mapX && GenerateRooms.mapY == mapY)
                target = GenerateRooms.player;
        }
        else
        {
            Vector3 dir = target.transform.position - this.transform.position;
            this.transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<Health>().ChangeHealth(-5);
		}
	}

	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<Health>().ChangeHealth(-1);
		}
	}
}