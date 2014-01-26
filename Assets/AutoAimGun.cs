﻿using UnityEngine;
using System.Collections;

public class AutoAimGun : MonoBehaviour 
{
	public float fireDelay = .25f;
	private float timeElapsed = 0f;

	public GameObject Bullet;
	Vector3 target;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= fireDelay)
		{
			Fire();
			timeElapsed -= fireDelay;
		}
	}

	void Fire()
	{
		GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (activeEnemies.Length < 1)
		{
			return;
		}
		target = activeEnemies[0].transform.position;
		for (int i = 1; i < activeEnemies.Length; i++)
		{
			if ((activeEnemies[i].transform.position - this.gameObject.transform.position).magnitude < (target - this.gameObject.transform.position).magnitude)
			{
				target = activeEnemies[i].transform.position;
			}                                                                                         
		}

		Vector3 dir = target - this.gameObject.transform.position;
		if (Physics.Raycast(this.gameObject.transform.position, dir.normalized, dir.magnitude, 1<<LayerMask.NameToLayer("Wall")))
		{
			return;
		}

		GameObject temp = (GameObject)Instantiate(Bullet);
		temp.transform.position = this.gameObject.transform.position + (target - this.gameObject.transform.position).normalized * .5f;
		temp.rigidbody.velocity = (target - this.gameObject.transform.position).normalized * 30f;
		//fire at enemy

	}
}
