using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    public void FinishGame()
    {
        Application.LoadLevel("ending");
    }
}
