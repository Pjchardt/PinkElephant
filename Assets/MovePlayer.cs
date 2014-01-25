using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour 
{
	private bool moving = false;
	private Vector3 target;
	private float s;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!moving)
		{
			return;
		}

		Vector3 temp = target - transform.position;
		if (Vector3.Magnitude(temp.normalized * s * Time.deltaTime) < Vector3.Distance(transform.position, target))
		{
			this.gameObject.GetComponent<CharacterController>().Move(temp.normalized * s * Time.deltaTime);
		}
		else
		{
			transform.position = target;
			moving = false;
		}
	}

	public void StartMoving(Vector3 t, float speed)
	{
		target = t;
		s = speed;
		moving = true;
	}
}
