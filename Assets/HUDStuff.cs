using UnityEngine;
using System.Collections;

public class HUDStuff : MonoBehaviour
{
    public GUIStyle style;
    public Player player;

    void OnGUI()
    {
        if (player.currentMethod == Player.InputMethod.MouseControl)
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Remaining: " + player.remaining, style);
    }
}
