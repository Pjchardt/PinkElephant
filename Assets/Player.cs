﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    char[,] qwerty = { { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' },
                   { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';' },
                   { 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/' } };

    char[] wasd = null;

    public GameObject map;
    public GameObject pink;

    public int index;
    public float speed = 2;
    List<int> keys = new List<int>();
    //CharacterController controller;
    Vector3 vel;
    static Color[] colors = { Color.cyan, Color.red, Color.green, Color.magenta };
    public static int score = 0;

	enum InputState {InitialInput, Moving, Paused, MouseUnConnected};
	enum InputMethod {KeyboardControl, MouseControl};
	private InputState currentState;
	private InputMethod currentMethod;
	private Vector3 startMousePosition;
	private Vector3 currentMousePosition;
	public float mouseSpeed = 3f;

	private Vector3 oldMousePosition;
	private Vector3 newMousePosition;

	public GameObject WASDObject;
	public GameObject MouseObject;

	public Texture cursorImage;

	private float startTime;

    void Start()
    {
        score = 0;
        keys.Add(0);
        //controller = this.gameObject.GetComponent<CharacterController>();
        vel = Vector3.zero;
        this.gameObject.renderer.material.color = colors[index];

		currentState = InputState.InitialInput;
		startMousePosition = Input.mousePosition;
		currentMousePosition = startMousePosition;
		oldMousePosition = startMousePosition;
		Debug.Log(startMousePosition);

		startTime = Time.timeSinceLevelLoad;
    }

	void OnGUI()
	{
		if (currentMethod == InputMethod.MouseControl)
		{
			//GUI.DrawTexture(new Rect(currentMousePosition.x - cursorImage.width/2, Screen.height - (currentMousePosition.y + cursorImage.height/2), cursorImage.width, cursorImage.height), cursorImage);
		}
	}

    void Update()
    {
		Screen.lockCursor = true;
		Screen.showCursor = false;

		switch (currentState)
		{
		    case InputState.InitialInput:
			    LookForInput();
			    break;

		    case InputState.Moving:
			    Move();
			    break;
		    case InputState.Paused:
			    break;
		    case InputState.MouseUnConnected:
			    LookForNear();
			    break;
		}
	
    }

    void OnTriggerEnter(Collider col)
    {
        /*if (col.gameObject.tag == "Key")
        {
            Key kc = col.gameObject.GetComponent<Key>();
            keys.Add(kc.door);
            GameObject.Destroy(col.gameObject);
        }
        */
        if (col.gameObject.tag == "Key")
        {
            GameObject.Destroy(col.gameObject);
            score++;
        }
        else if (col.gameObject.tag == "Goal")
        {
            col.gameObject.GetComponent<Goal>().FinishGame();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject);
        if (col.gameObject.tag == "Door")
        {
            Door dc = col.gameObject.GetComponent<Door>();
            //if (keys.Contains(dc.key))
            GameObject.Destroy(col.gameObject);
        }
    }

	private void LookForInput()
	{
		for (int i = 0; i < qwerty.GetLength(0); i++)
			for (int j = 0; j < qwerty.GetLength(1); j++)
				if (Input.GetKeyDown(qwerty[i, j].ToString()))
			{
				wasd = new char[4];
				
				if (i == 0)
					i = 1;
				
				if (j == 0)
					j = 1;
				if (j == qwerty.GetLength(1) - 1)
					j = qwerty.GetLength(1) - 2;
				
				wasd[0] = qwerty[i - 1, j];
				wasd[1] = qwerty[i, j - 1];
				wasd[2] = qwerty[i, j];
				wasd[3] = qwerty[i, j + 1];

                pink.SetActive(false);
                map.SetActive(true);

				currentMethod = InputMethod.KeyboardControl;
				currentState = InputState.Moving;
				//Debug.Log ("KeyboardControl");
				float inputDelay = Time.timeSinceLevelLoad - startTime;
				inputDelay /= 5f;
				inputDelay = Mathf.Clamp(inputDelay, 0f, 1f);
				Time.timeScale = 1.5f - inputDelay;

				WASDObject.SetActive(true);
				this.gameObject.renderer.enabled = false;

				GameObject [] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
				for (int ct = 0; ct < allEnemies.Length; ct++)
				{
					allEnemies[ct].GetComponent<Enemy>().speed *= .4f;
					allEnemies[ct].transform.localScale *= 4f;
				}

				return;
			}

		if ( Input.GetAxis ("Mouse X") > .5 || Input.GetAxis("Mouse Y") > .5)
        {
            pink.SetActive(false);
            map.SetActive(true);

			//use mouse
			currentMethod = InputMethod.MouseControl;
			currentState = InputState.Moving;
			float inputDelay = Time.timeSinceLevelLoad - startTime;
			inputDelay /= 4f;
			inputDelay = Mathf.Clamp(inputDelay, 0f, .4f);
			Time.timeScale = 1.2f - inputDelay;
			MouseObject.SetActive(true);
			this.gameObject.renderer.enabled = false;
			//Debug.Log( Input.GetAxis ("Mouse X") + " : " +  Input.GetAxis("Mouse Y"));
			//Debug.Log ("MouseControl");
		}

	}

	private void Move()
	{
		switch (currentMethod)
		{
		case InputMethod.KeyboardControl:
			MoveWithKeyboard();
			break;
		case InputMethod.MouseControl:
			MoveWithMouse();
			break;
		}
	}

	private void MoveWithMouse()
	{
		newMousePosition = Input.mousePosition;
		//Vector3 mouseDelta = newMousePosition - oldMousePosition; 
		Vector3 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f) * 15f;
		currentMousePosition = currentMousePosition * (4f * Time.deltaTime) + (currentMousePosition + mouseDelta) * (1f - 4f * Time.deltaTime);
		oldMousePosition = newMousePosition;
		//Debug.Log (currentMousePosition);

		//cast ray into scene and move player towards position
		float distance = (Camera.main.transform.position - this.gameObject.transform.position).magnitude;
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Ray ray = Camera.main.ScreenPointToRay(currentMousePosition);
		Vector3 point = ray.origin + (ray.direction * distance);

		Vector3 newVel = (point - this.gameObject.transform.position).normalized;
		if ((mouseSpeed * Time.deltaTime) < Vector3.Distance(point, this.gameObject.transform.position))
		{
			newVel.z = 0f;
			vel = newVel * mouseSpeed; 
		}
		else
		{
			vel = Vector3.zero;
			//this.gameObject.transform.position = new Vector3(point.x, point.y, this.gameObject.transform.position.z);
			Debug.Log("At Target");
		}
        //controller.Move(vel * Time.deltaTime);
        //this.transform.position += vel * Time.deltaTime;
        rigidbody.velocity = vel;
	}

	private void MoveWithKeyboard()
	{
		/*if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.down * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up * speed * Time.deltaTime;*/
		
		if (wasd != null)
		{
			if (Input.GetKey(wasd[1].ToString()))
				vel.x = -speed;
			else if (Input.GetKey(wasd[3].ToString()))
				vel.x = speed;
			else
				vel.x = 0;
			
			/*if (controller.isGrounded)
                vel.y = -0.01f;
            else if (ladders > 0)
                vel.y = 0;
            else
                vel.y -= 10f * Time.deltaTime;*/
			
			//if (ladders > 0)
			{
				if (Input.GetKey(wasd[2].ToString()))
					vel.y = -speed;
				else if (Input.GetKey(wasd[0].ToString()))
					vel.y = speed;
				else
					vel.y = 0;
			}
			//else if (Input.GetButtonDown("Jump") && controller.isGrounded)
			{
				//vel.y = speed * 1.75f;
			}

            //controller.Move(vel * Time.deltaTime);
            //this.transform.position += vel * Time.deltaTime;
            rigidbody.velocity = vel;
		}
	}

	public void PausePlayer(bool b)
	{
		if (b)
		{
            currentState = InputState.Paused;
            rigidbody.velocity = Vector3.zero;
		}
		else
		{
			currentState = InputState.Moving;
		}
	}

	public void UnConnect()
	{
		if (currentMethod == InputMethod.MouseControl)
		{
			currentState = InputState.MouseUnConnected;
			//play some effect
		}
	}

	public void SetNewMouse(Vector3 position)
	{
		currentMousePosition = position;
	}

	private void LookForNear()
	{
		//cast ray into scene and move player towards position
		float distance = Mathf.Abs(Camera.main.transform.position.z - this.gameObject.transform.position.z);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = ray.origin + (ray.direction * distance);
		
		Vector3 newVel = (point - this.gameObject.transform.position);
		if (Vector3.Magnitude(newVel) < 1f)
		{
			currentState = InputState.Moving;
		}
	}
}
