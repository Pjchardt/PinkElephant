using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    public GameObject lockObj;
    public GameObject portal;

    bool locked = true;

    public void FinishGame()
    {
        if (locked)
            return;

        Application.LoadLevel("ending");
    }

    public void Unlock()
    {
        lockObj.SetActive(false);
        portal.SetActive(true);
        locked = false;
    }
}
