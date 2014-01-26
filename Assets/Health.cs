using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public float health;
	public Texture blackTexture;
	// Use this for initialization
	void Start () 
	{
		health = 100f;
	}

	void OnGUI()
	{
		float alphaFadeValue = (100f - health) / 100f;
		
		GUI.color = new Color(0, 0, 0, alphaFadeValue);
		GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), blackTexture );
	}

	// Update is called once per frame
	void Update () 
	{
		if (health < 100)
		{
			health += 15f * Time.deltaTime;

			if (health > 100)
			{
				health = 100f;
			}
		}

		this.gameObject.audio.volume = .25f + ((100f - health)/100f) * .75f;
	}

	public void ChangeHealth(float delta)
	{
		health += delta;

		if (health <= 0)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
