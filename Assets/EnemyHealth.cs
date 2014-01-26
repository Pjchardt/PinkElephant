using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{
	public float health;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeHealth(float delta)
	{
		health += delta;
		if (health <= 0)
		{
			Destroy (this.gameObject);
		}
	}
}
