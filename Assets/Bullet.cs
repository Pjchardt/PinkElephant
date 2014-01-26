using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public float Damage;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Enemy")
		{
			col.gameObject.GetComponent<EnemyHealth>().ChangeHealth(Damage);
		}

			Destroy (this.gameObject);
	}
}
