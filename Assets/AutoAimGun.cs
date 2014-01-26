using UnityEngine;
using System.Collections;

public class AutoAimGun : MonoBehaviour 
{
	public float fireDelay = .25f;
	private float timeElapsed = 0f;

	public GameObject Bullet;
	Vector3 target;
    GameObject[] activeEnemies;

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
		activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		if (activeEnemies.Length < 1)
		{
			return;
		}

        int i = 0;
        float minDist = 0;
        Vector3 dir;
        target = Vector3.zero;
        while (i < activeEnemies.Length)
        {
            dir = activeEnemies[i].transform.position - this.gameObject.transform.position;
            minDist = dir.magnitude;
            if (!Physics.Raycast(this.gameObject.transform.position, dir.normalized, minDist, 1 << LayerMask.NameToLayer("Wall")))
            {
                target = activeEnemies[i].transform.position;
                i++;
                break;
            }
            i++;
        }
        while (i < activeEnemies.Length)
		{
            dir = activeEnemies[i].transform.position - this.gameObject.transform.position;
            float dist = dir.magnitude;
            if (dist < minDist && !Physics.Raycast(this.gameObject.transform.position, dir.normalized, dist, 1 << LayerMask.NameToLayer("Wall")))
			{
				target = activeEnemies[i].transform.position;
                minDist = dist;
			}
            i++;
		}

        if (target == Vector3.zero)
		{
			return;
		}

		//fire at enemy
		GameObject temp = (GameObject)Instantiate(Bullet);
        dir = (target - this.gameObject.transform.position).normalized;
		temp.transform.position = this.gameObject.transform.position + dir * .5f;
		temp.rigidbody.velocity = dir * 30f;
	}
}
