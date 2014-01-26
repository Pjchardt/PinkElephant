using UnityEngine;
using System.Collections;

public class HUDStuff : MonoBehaviour
{
    public GUIStyle style;
    public Player player;

    void OnGUI()
    {
        if (player.currentMethod == Player.InputMethod.MouseControl)
        {
            style.alignment = TextAnchor.UpperLeft;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Remaining: " + player.remaining, style);
        }
        if (player.timeRunning)
        {
            style.alignment = TextAnchor.UpperRight;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Time: " + Mathf.CeilToInt(Mathf.Max(0, player.timeLeft)), style);
        }
    }
}
