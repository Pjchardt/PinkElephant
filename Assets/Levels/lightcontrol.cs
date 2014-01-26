using UnityEngine;
using System.Collections;

public class lightcontrol : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(lightcon());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator lightcon()
	{
		light.intensity = Random.Range(0.5f,.9f);
		yield return new WaitForSeconds(Random.Range(.05f,.25f));
		StartCoroutine(lightcon ());
	}
}
