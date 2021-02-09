using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    public GameObject[] Mini_Map;
    public GameObject MAP;
    public GameObject HelpUI;
    public Camera C1;
    public bool Move;
    public Vector3 MousePos;
    public Vector3 CamPos;
    int Map_Num;
    bool Off;
	// Use this for initialization
	void Start () {
        Map_Num = 0;
        Off = false;
	}
	
	// Update is called once per frame
	void Update () {
        //if(GameManager.Instance.InputManager.MiniMap && !Off)
        //      {
        //          for(int i =0; i<Mini_Map.Length;i++)
        //          {
        //              Mini_Map[i].SetActive(!Mini_Map[i].activeInHierarchy);
        //          }
        //          if (Mini_Map[1].activeInHierarchy)
        //          {
        //              C1.orthographicSize = 180;  
        //          }
        //          else
        //          {
        //              C1.orthographicSize = 80;
        //          }

        //      }
        if(GameManager.Instance.InputManager.MiniMapUp)HelpUI.SetActive(false);
        if (GameManager.Instance.InputManager.MiniMap)
        {
            if (Off)
                HelpUI.SetActive(true);
            else
            {
                if (!Mini_Map[1].activeInHierarchy)
                {
                    Mini_Map[0].SetActive(false);
                    Mini_Map[1].SetActive(true);


                    C1.orthographicSize = 180;
                    //C1.transform.position = MAP.transform.GetChild(12).position / 2 + new Vector3(0, 0, -10);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    MousePos = Input.mousePosition;
                    CamPos = C1.transform.position;
                    Move = true;
                }
                else if (Input.GetMouseButtonUp(0))
                    Move = false;

                if(Move)
                {
                    Vector3 Pos = Input.mousePosition;
                    Vector2 dir = new Vector2(MousePos.x - Pos.x, MousePos.y - Pos.y);

                    C1.transform.position = CamPos +  new Vector3(dir.x/5,dir.y/5,0);

                }
            }
        }
        else if(!Off)
        {
            Mini_Map[0].SetActive(true);
            Mini_Map[1].SetActive(false);
            C1.transform.localPosition = new Vector3(0,0,-10);
           C1.orthographicSize = 80;
            
        }
	}

    public void AllOff()
    {

        if (Mini_Map[0].activeInHierarchy) Map_Num = 0;

        else if(Mini_Map[1].activeInHierarchy) Map_Num = 1;

        Mini_Map[Map_Num].SetActive(false);
        Off = true;
    }

    public void On()
    {
        Mini_Map[Map_Num].SetActive(true);
        Off = false;
    }
}
