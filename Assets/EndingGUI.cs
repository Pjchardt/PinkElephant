using UnityEngine;
using System.Collections;

public class EndingGUI : MonoBehaviour
{
    public GUIStyle style;

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "You Win\nScore: " + Player.score + "\nPress space or Click", style);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            Application.LoadLevel("test rooms");
    }
}
