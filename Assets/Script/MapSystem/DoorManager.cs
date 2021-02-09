using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    RoomManager Room;

    // Use this for initialization
    void Start () {
        Room = GetComponentInParent<RoomManager>();

    }

    // Use this When Trigger Enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Room.Status.DoorsOpen && !Room.isClear)
            {
                Room.Status.CloseDoor();
                Room._StartRoom();
            }

       }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
