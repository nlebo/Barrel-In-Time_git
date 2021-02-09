using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour {

    public enum MAP_TYPE { NULL = 0,SPAWN_ROOM,DEFAULT_ROOM,BOSS_ROOM,CHEST_ROOM,TIMESHOP };
    public enum ROOM_TYPE { EN = 0,ES,EW,SN,WN,WS,ESN,EWN,EWS,WNS ,E,N,S,W ,NONE};
    

    // Use this for Manage Bridge
    public struct Bridge
    {
        public bool Up;
        public bool Down;
        public bool left;
        public bool right;
    }

    // Use this for Manage Door
    public struct Door
    {
        public GameObject Left;
        public GameObject Right;
        public GameObject Up;
        public GameObject Down;
    }

    // Use this for Manage Map
    public class Mapinfo
    {
        public Vector2 Pos;
        public int prevCount;
        public Bridge Connect;
        public Door _Door;
        public MAP_TYPE MapType;
        public ROOM_TYPE RoomType;
        public SoundManager FX;
        public GameObject ActiveMap;
        public bool DoorsOpen;
        GameObject MyMap;

        //-----------------------------------------------------------------
        // Use this for Class Initialize
        public Mapinfo()
        {
            Pos = Vector2.zero;
            Connect = new Bridge();
            MapType = new MAP_TYPE();
            RoomType = new ROOM_TYPE();
            prevCount = 0;
            FX = GameManager.Instance.SoundManager;

        }


        //-----------------------------------------------------------------
        // Use this for ConnectBridge;
        public bool ConnectBridge(bool up, bool down, bool left, bool right)
        {
            Connect.Up = up;
            Connect.Down = down;
            Connect.left = left;
            Connect.right = right;

            RoomType = ROOM_TYPE.NONE;

            if (left)
            {
                if (up && right)
                    RoomType = ROOM_TYPE.EWN;
                else if (up && down)
                    RoomType = ROOM_TYPE.WNS;
                else if (right && down)
                    RoomType = ROOM_TYPE.EWS;
                else if (up)
                    RoomType = ROOM_TYPE.WN;
                else if (right)
                    RoomType = ROOM_TYPE.EW;
                else if (down)
                    RoomType = ROOM_TYPE.WS;
                else
                    RoomType = ROOM_TYPE.W;
            }
            else if (right)
            {
                if (up && down)
                    RoomType = ROOM_TYPE.ESN;
                else if (down)
                    RoomType = ROOM_TYPE.ES;
                else if (up)
                    RoomType = ROOM_TYPE.EN;
                else
                    RoomType = ROOM_TYPE.E;
            }
            else if (up)
            {
                if (down)
                    RoomType = ROOM_TYPE.SN;
                else
                    RoomType = ROOM_TYPE.N;
            }
            else if (down)
                RoomType = ROOM_TYPE.S;
                
            return true;
        }


        //-----------------------------------------------------------------
        // Use those for Setting Map;
        public bool SetMap(GameObject Map)
        {
            MyMap = Map;
            return true;
        }
        public GameObject GetMap()
        {
            return MyMap;
        }


        //-----------------------------------------------------------------
        //Use those for Get SpriteSize
        public float GetMapWidth()
        {
            float width = 0;

            width = MyMap.GetComponent<SpriteRenderer>().size.x;
            return width;
        }
        public float GetMapHeight()
        {
            float height = 0;

            height = MyMap.GetComponent<SpriteRenderer>().size.y;
            return height;
        }


        //----------------------------------------------------------------
        // Use those for Manage Door
        public bool OpenDoor()
        {
            //ActiveMap.GetComponent<RoomManager>().SetStatus(this);
            if (Connect.left)
            {
                _Door.Left.GetComponent<Animator>().SetBool("Open", true);
                _Door.Left.GetComponent<SpriteRenderer>().sortingOrder = 1;
                
                _Door.Left.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
                _Door.Left.SetActive(false);

            if (Connect.right)
            {
                _Door.Right.GetComponent<Animator>().SetBool("Open", true);
                _Door.Right.GetComponent<SpriteRenderer>().sortingOrder = 1;
                
                _Door.Right.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
                _Door.Right.SetActive(false);

            if (Connect.Up)
            {

                _Door.Up.GetComponent<Animator>().SetBool("Open", true);
                _Door.Up.GetComponent<SpriteRenderer>().sortingOrder = 1;
                _Door.Up.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
                _Door.Up.SetActive(false);

            if (Connect.Down)
            {
                _Door.Down.GetComponent<Animator>().SetBool("Open", true);
                _Door.Down.GetComponent<SpriteRenderer>().sortingOrder = 3;
                _Door.Down.GetComponent<SpriteRenderer>().flipY = false;
                _Door.Down.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
                _Door.Down.SetActive(false);











            DoorsOpen = true;
            return true;
        }
        public bool CloseDoor()
        {
           FX.EffectPlay(FX.SYSTEMS.DOOR_CLOSE);
            if (Connect.left)
            {
                _Door.Left.GetComponent<Animator>().SetBool("Open", false);
                _Door.Left.GetComponent<BoxCollider2D>().enabled = true;
            }

            if (Connect.right)
            {
                _Door.Right.GetComponent<Animator>().SetBool("Open", false);
                _Door.Right.GetComponent<BoxCollider2D>().enabled = true;
            }

            if (Connect.Up)
            {

                _Door.Up.GetComponent<Animator>().SetBool("Open", false);
                _Door.Up.GetComponent<BoxCollider2D>().enabled = true;
            }

            if (Connect.Down)
            {
                _Door.Down.GetComponent<Animator>().SetBool("Open", false);
                _Door.Down.GetComponent<BoxCollider2D>().enabled = true;
            }
            DoorsOpen = false;
            return true;
        }
        public bool SetDoor()
        {
            _Door.Left = ActiveMap.transform.GetChild(1).transform.GetChild(0).gameObject;
            _Door.Left.transform.position += Vector3.up * 0.225f;
            _Door.Left.GetComponent<BoxCollider2D>().size += Vector2.up * 1.5f;
            _Door.Left.tag = "Obstacle";


            _Door.Right = ActiveMap.transform.GetChild(1).transform.GetChild(1).gameObject;
            _Door.Right.transform.position += Vector3.up * 0.225f;
            _Door.Right.GetComponent<BoxCollider2D>().size += Vector2.up * 1.5f;
            _Door.Right.tag = "Obstacle";

            _Door.Up = ActiveMap.transform.GetChild(1).transform.GetChild(2).gameObject;
            _Door.Up.tag = "Obstacle";
            _Door.Up.GetComponent<BoxCollider2D>().size = new Vector3(1.2f, 0.5f, 0);
            _Door.Up.GetComponent<BoxCollider2D>().offset = new Vector3(0, -0.35f, 0);

            _Door.Down = ActiveMap.transform.GetChild(1).transform.GetChild(3).gameObject;
            _Door.Down.tag = "Obstacle";
            _Door.Down.GetComponent<BoxCollider2D>().size = new Vector3(1.2f, 0.5f, 0);
            _Door.Down.GetComponent<BoxCollider2D>().offset = new Vector3(0, -0.35f, 0);

            
            return true;
        }
    }

    public Mapinfo[] MapStatus = new Mapinfo[22];

	// Use this for initialization
	void Awake () {
        for (int i = 0; i < 22; i++)
        {
            MapStatus[i] = new Mapinfo();
        }

	}


}
