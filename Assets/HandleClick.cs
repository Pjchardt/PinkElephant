using UnityEngine;
using System.Collections;

public class HandleClick : MonoBehaviour
{
    public GameObject wall;
    public Transform map;
    public GUIStyle style;
    public Player[] players;

    void Update()
    {
        /*if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(this.gameObject.camera.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider.gameObject.tag != "Player")
            {
                GameObject.Destroy(hit.collider.gameObject);
            }
            else
            {
                Vector3 worldPos = this.gameObject.camera.ScreenToWorldPoint(Input.mousePosition);
                worldPos.x = Mathf.Round(worldPos.x);
                worldPos.y = Mathf.Round(worldPos.y);
                worldPos.z = 0;
                GameObject obj = GameObject.Instantiate(wall, worldPos, Quaternion.identity) as GameObject;
                obj.transform.parent = map;
            }
        }*/
    }

    void OnGUI()
    {
        for (int i=0; i<players.Length; i++)
            GUI.Label(new Rect(0, i * 64, 100, 64), "P" + (i+1) + ": " + players[i].score, style);
    }
}
