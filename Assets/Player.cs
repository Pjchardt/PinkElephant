using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    char[,] qwerty = { { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' },
                   { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';' },
                   { 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/' } };

    char[] wasd = null;

    public int index;
    public float speed = 2;
    List<int> keys = new List<int>();
    CharacterController controller;
    Vector3 vel;
    public int ladders = 0;
    static Color[] colors = { Color.cyan, Color.red, Color.green, Color.magenta };
    public int score = 0;

    void Start()
    {
        keys.Add(0);
        controller = this.gameObject.GetComponent<CharacterController>();
        vel = Vector3.zero;
        this.gameObject.renderer.material.color = colors[index];
    }

    void Update()
    {
        if (wasd == null)
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
                        
                        break;
                    }
        }

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

            controller.Move(vel * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        /*TrailHead th = col.gameObject.GetComponent<TrailHead>();
        if (th != null)
            th.TurnOn();
        GameObject.Destroy(col.gameObject);*/
        if (col.gameObject.tag == "Door")
        {
            Door dc = col.gameObject.GetComponent<Door>();
            if (keys.Contains(dc.key))
                GameObject.Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Key")
        {
            Key kc = col.gameObject.GetComponent<Key>();
            keys.Add(kc.door);
            GameObject.Destroy(col.gameObject);
        }
        /*if (col.gameObject.tag == "Door")
        {
            ladders++;
        }
        else if (col.gameObject.tag == "Key")
        {
            GameObject.Destroy(col.gameObject);
            score++;
        }*/
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Door")
        {
            ladders--;
        }
    }
}
