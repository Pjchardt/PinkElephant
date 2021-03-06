﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public float health;
	public Texture blackTexture;
	public Color overlayColor = new Color(0, 0, 0, 0);
	public bool dead = false;
	public AudioClip deathSound;
	// Use this for initialization
	void Start () 
	{
		health = 100f;
	}

	void OnGUI()
	{
		float alphaFadeValue = (100f - health) / 100f;
		overlayColor.a = alphaFadeValue;
		GUI.color = overlayColor;
		GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), blackTexture );
	}

	// Update is called once per frame
	void Update () 
	{
		if (dead)
		{
			return;
		}
		if (health < 100)
		{
			health += 15f * Time.deltaTime;

			if (health > 100)
			{
				health = 100f;
			}
		}

		this.gameObject.audio.volume = .5f + ((100f - health)/100f) * .5f;
	}

	public void ChangeHealth(float delta)
	{
		if (dead)
		{
			return;
		}
		health += delta * this.gameObject.GetComponent<Player>().HealthModifier;

		if (health <= 0)
		{
			audio.PlayOneShot(deathSound);
			dead = true;
			StartCoroutine(WaitToReload());
		}
	}

    public void kill()
    {
        ChangeHealth(-health);
    }

	IEnumerator WaitToReload()
	{
		yield return new WaitForSeconds (1f);

		Application.LoadLevel(Application.loadedLevel);
	}
}
