using UnityEngine;
using System.Collections;

public class SpinMelee : MonoBehaviour 
{
    public float speed;
    public float Damage;
    bool spinning = false;
    float degreesSpun;

	void Start () 
	{
	}
	
	void Update () 
	{
        if (spinning)
        {
            this.transform.RotateAround(transform.parent.position, Vector3.forward, speed * Time.deltaTime);
            //transform.parent.rigidbody.angularVelocity = new Vector3(0, 0, speed);
            degreesSpun += speed * Time.deltaTime;
            if (degreesSpun >= 360)
            {
                transform.parent.rigidbody.angularVelocity = new Vector3(0, 0, 0);
                spinning = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("spin");
                spinning = true;
                degreesSpun = 0;
            }
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && spinning)
        {
            col.gameObject.GetComponent<EnemyHealth>().ChangeHealth(Damage);
        }
    }
}
