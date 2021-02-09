using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	public bool fire1;
	public bool fire2;
	public bool dash;
    public bool ESC;
	public bool Initialize;
    public bool Reload;
	public bool button2;
	public bool button3;
    public bool MiniMap;
    public bool MiniMapUp;
    public bool Inventory;
    public bool Gear;
    public bool KillAll;
    public bool Pause;
	public float Vertical;
	public float Horizontal;

    public bool PlayerDead = false;
    public bool Active = true;
    public Vector3 MousePosition;

    // Update is called once per frame

    private void Start()
    {
    }
    void Update () {

        if (!Active)
        {
            AllFalse();
            return;
        }
        if (Time.timeScale > 0)
        {

            fire1 = Input.GetMouseButton(0) || Input.GetKey(KeyCode.C);
            fire2 = Input.GetMouseButton(1);
            Vertical = Input.GetAxisRaw("Vertical");
            Horizontal = Input.GetAxisRaw("Horizontal");
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dash = Input.GetKeyDown(KeyCode.Space);
            //Initialize = Input.GetKeyDown(KeyCode.R);
            Reload = Input.GetKeyDown(KeyCode.R);

            MiniMap = Input.GetKey(KeyCode.Tab);
            MiniMapUp = Input.GetKeyUp(KeyCode.Tab);
            //if (Input.GetMouseButtonDown(2))
            //    MiniMap = true;
            //else if (Input.GetMouseButtonUp(2))
            //    MiniMap = false;
        }
        Inventory = Input.GetKeyDown(KeyCode.I);
        Gear = Input.GetKeyDown(KeyCode.LeftShift);
        Pause = Input.GetKeyDown(KeyCode.P);
        ESC = Input.GetKeyDown(KeyCode.Escape);
        KillAll = Input.GetKeyDown(KeyCode.Delete);
	}
    void AllFalse()
    {
        fire1 = false;
        fire2 = false;
        dash = false;
        ESC = false;
        Initialize = false;
        Reload = false;
        button2 = false;
        button3 = false;
        MiniMap = false;
        MiniMapUp = false;
        Inventory = false;
        Gear = false;
        KillAll = false;
        Pause = false;
        Vertical = 0;
        Horizontal = 0;
    }

}
