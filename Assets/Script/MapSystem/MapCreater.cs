using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour {

    
	public int MaxMapCount;
	public int Width,Height;
    public int W, H,C;
    public int MapCount;
    public int[,] MapStruct;

    public MapInfo Mapinfo;

    public GameObject Parent;
    public GameObject[] EN, ES, EW, SN, WN, WS, ESN, EWN, EWS, WNS, E, N, S, _W, NONE;
    public int EN_Count, ES_Count, EW_Count, SN_Count, WN_Count, WS_Count, ESN_Count, EWN_Count, EWS_Count, WNS_Count;

    GameObject[] child = null;
    public bool left, right, up, down;
    bool BossRoom;
    int RandomW, RandomH;
    int ChestRoom = 0;
    int PrevCount = 0;
    


    //-----------------------------------------------------------------------------------------------------
    // Use this for initialization
    void Awake () {
		if (MaxMapCount == 0)
			MaxMapCount = 5;

		if (Width == 0 || Height == 0) {
			Width = 5;
			Height = 5;
		}
        C = 0;
		MapCount = 0;
		MapStruct = new int[Height + 2, Width + 2] ;
        Mapinfo = GameManager.Instance.Mapinfo;
        BossRoom = false;
	}
    private void Start()
    {
        EN_Count = EN.Length;
        ES_Count = ES.Length;
        EW_Count = EW.Length;
        SN_Count = SN.Length;
        WN_Count = WN.Length;
        WS_Count = WS.Length;
        ESN_Count = ESN.Length;
        EWN_Count = EWN.Length;
        EWS_Count = EWS.Length;
        WNS_Count = WNS.Length;

    }
    //-----------------------------------------------------------------------------------------------------
    // Use this for Check Can Make Map
    public bool CheckMap(int w,int h)
	{
        int Check = 0;

        left = false;
        up = false;
        down = false;
        right = false;

        if (h - 1 > 1) if (MapStruct[h - 1, w] != 0) { Check++; up = true; }
        if (h + 1 <= Height) if (MapStruct[h + 1, w] != 0) { Check++; down = true; }
        if (w - 1 > 1) if (MapStruct[h, w - 1] != 0) { Check++; right = true; }
        if (w + 1 <= Width) if (MapStruct[h, w + 1] != 0) { Check++; left = true; }

        if (Check >= 2)
            return false;

        return true;
	}
    public bool CheckMap(int w, int h,int a)
    {
        int Check = 0;

        left = false;
        up = false;
        down = false;
        right = false;

        if (h - 1 > 1) if (MapStruct[h - 1, w] != 0) { Check++; up = true; }
        if (h + 1 <= Height) if (MapStruct[h + 1, w] != 0) { Check++; down = true; }
        if (w - 1 > 1) if (MapStruct[h, w - 1] != 0) { Check++; right = true; }
        if (w + 1 <= Width) if (MapStruct[h, w + 1] != 0) { Check++; left = true; }

        if (Check >= a)
            return false;

        return true;
    }

    //-----------------------------------------------------------------------------------------------------
    // Use this for Make MapTile

    void MapTile(int w, int h, int StackOverFlow = 0)
    {

        if (MapCount >= MaxMapCount) return;


        if (MapStruct[h, w] == 0)
        {
            Mapinfo.MapStatus[MapCount].prevCount = C;
            W = w;
            H = h;

            if (MapCount == 0)
            {
                MapStruct[h, w] = 1;
                Mapinfo.MapStatus[MapCount].MapType = MapInfo.MAP_TYPE.SPAWN_ROOM;
            }
            else if (MapCount == MaxMapCount - 1)
            {
                MapStruct[h, w] = 3;
                if (!BossRoom)
                {
                    Mapinfo.MapStatus[MapCount].MapType = MapInfo.MAP_TYPE.BOSS_ROOM;
                    BossRoom = true;
                    
                }
                else if(ChestRoom < 2)
                {
                    ChestRoom++;
                    Mapinfo.MapStatus[MapCount].MapType = MapInfo.MAP_TYPE.CHEST_ROOM;
                }
                else
                {
                    Mapinfo.MapStatus[MapCount].MapType = MapInfo.MAP_TYPE.TIMESHOP;
                }
            }
            else
            {
                MapStruct[h, w] = 2;
                Mapinfo.MapStatus[MapCount].MapType = MapInfo.MAP_TYPE.DEFAULT_ROOM;
            }

            Mapinfo.MapStatus[MapCount].Pos = new Vector2(w, h);

            C = MapCount;
            MapCount++;
            
            
        
            
        }


        if (MapCount >= MaxMapCount) return;

        int r = Random.Range(0, 4);


        if (W == w && H == h)
        {
            switch (r)
            {
                case 0:
                    if (w - 1 > 1) if (CheckMap(w - 1, h)) MapTile(w - 1, h, StackOverFlow);
                    break;
                case 1:
                    if (w + 1 <= Width ) if (CheckMap(w + 1, h)) MapTile(w + 1, h, StackOverFlow);
                    break;
                case 2:
                    if (h - 1 > 1) if (CheckMap(w, h - 1)) MapTile(w, h - 1, StackOverFlow);
                    break;
                case 3:
                    if (h + 1 <= Height) if (CheckMap(w, h + 1)) MapTile(w, h + 1, StackOverFlow);
                    break;
            }
        }

        
        return;
    }

    //-----------------------------------------------------------------------------------------------------
    // Use this for Use MapTile Function at out Acess

    public bool MakeMap(int w,int h)
	{

		if (MapCount < MaxMapCount) {
			MapTile (w, h,0);
		}

        if (MapCount >= MaxMapCount)
        {
            //for (int i = 0; i < MaxMapCount; i++)
            //{
            //    //StartCoroutine(DrawMap(i));
            //    int x = (int)Mapinfo.MapStatus[i].Pos.x;
            //    int y = (int)Mapinfo.MapStatus[i].Pos.y;

            //    CheckMap(x, y);
            //    Mapinfo.MapStatus[i].ConnectBridge(up,down,left,right);

            //    InstanceMap(i);
                   
            //}
            return true;
        }

		return false;
		
	}

    //-----------------------------------------------------------------------------------------------------
    // Use this for InitializeMap

    public bool InitializeMap()
	{
		MapCount = 0;

        for (int i = 0; i < Width+2; i++) {

			for (int j = 0; j < Height+2; j++) {
				MapStruct [j,i] = 0;
			}
		}

		Transform[] MapList = Parent.GetComponentsInChildren<Transform> ();
		for (int i = 1; i < MapList.Length; i++) {

			if (MapList [i] != transform) {
				Destroy (MapList [i].gameObject);
			}
		}

        for (int i = 0; i < Mapinfo.MapStatus.Length; i++)
        {
            Mapinfo.MapStatus[i] = new MapInfo.Mapinfo();

            if(child!= null)
            if(child[i])
                child[i] = null;
        }
        

        BossRoom = false;
        ChestRoom = 0;

		return true;
	}

    //-----------------------------------------------------------------------------------------------------
    // Use this for InstantiateMap
   public void InstanceMap(int count)
    {
        float PrevWidth = 0 ;
        float PrevHeight = 0;

        PrevCount = Mapinfo.MapStatus[count].prevCount;
        if (child == null)
            child = new GameObject[50];

        if(count != 0)
        {
            //PrevWidth = Mapinfo.MapStatus[count].GetMapWidth() * (Mapinfo.MapStatus[PrevCount].Pos.x - Mapinfo.MapStatus[count].Pos.x) + child[PrevCount].transform.position.x;
            PrevWidth = (Mapinfo.MapStatus[PrevCount].GetMapWidth() /2 + Mapinfo.MapStatus[count].GetMapWidth() /2) * (Mapinfo.MapStatus[PrevCount].Pos.x - Mapinfo.MapStatus[count].Pos.x) + child[PrevCount].transform.position.x;
            PrevHeight = (Mapinfo.MapStatus[PrevCount].GetMapHeight() / 2 + Mapinfo.MapStatus[count].GetMapHeight() / 2) * (Mapinfo.MapStatus[PrevCount].Pos.y - Mapinfo.MapStatus[count].Pos.y) + child[PrevCount].transform.position.y;
        }

        if (child[count] != null) return;

        child[count] = Instantiate(Mapinfo.MapStatus[count].GetMap(), new Vector3(PrevWidth,
            PrevHeight,
            0), Quaternion.identity);


        if (child != null)
        {
            child[count].transform.parent = Parent.transform;
            child[count].layer = 8;
            Mapinfo.MapStatus[count].ActiveMap = child[count];
            child[count].GetComponent<RoomManager>().SetStatus(Mapinfo.MapStatus[count]);
            Mapinfo.MapStatus[count].SetDoor(); 
        }

 
    }

    //-----------------------------------------------------------------------------------------------------
    // Use this for DrawMap to Wait
    IEnumerator DrawMap(int count)
    {
        InstanceMap(count);
        yield return null;
        
    }

    public void SetSpriteRoom(int count)
    {
        int R;
        GameObject Temp;
        switch (Mapinfo.MapStatus[count].RoomType)
        {

            case MapInfo.ROOM_TYPE.EN:
                
                R = Random.Range(0, EN_Count);

                Mapinfo.MapStatus[count].SetMap(EN[R]);

                Temp = EN[R];
                EN[R] = EN[EN_Count-1];
                EN[EN_Count - 1] = Temp;

                Temp = ES[R];
                ES[R] = ES[EN_Count - 1];
                ES[EN_Count - 1] = Temp;

                Temp = WS[R];
                WS[R] = WS[EN_Count - 1];
                WS[EN_Count - 1] = Temp;

                Temp = WN[R];
                WN[R] = WN[EN_Count - 1];
                WN[EN_Count - 1] = Temp;

                EN_Count--;

                if (EN_Count == 0)
                    EN_Count = EN.Length;
                break;
            case MapInfo.ROOM_TYPE.ES:
                R = Random.Range(0, EN_Count);

                Mapinfo.MapStatus[count].SetMap(ES[R]);

                Temp = EN[R];
                EN[R] = EN[EN_Count - 1];
                EN[EN_Count - 1] = Temp;

                Temp = ES[R];
                ES[R] = ES[EN_Count - 1];
                ES[EN_Count - 1] = Temp;

                Temp = WS[R];
                WS[R] = WS[EN_Count - 1];
                WS[EN_Count - 1] = Temp;

                Temp = WN[R];
                WN[R] = WN[EN_Count - 1];
                WN[EN_Count - 1] = Temp;

                EN_Count--;
                if (EN_Count == 0)
                    EN_Count = EN.Length;
                break;
            case MapInfo.ROOM_TYPE.EW:
                R = Random.Range(0, EW_Count);

                Mapinfo.MapStatus[count].SetMap(EW[R]);

                Temp = EW[R];
                EW[R] = EW[EW_Count - 1];
                EW[EW_Count - 1] = Temp;

                EW_Count--;
                if (EW_Count == 0)
                    EW_Count = EW.Length;
                break;
            case MapInfo.ROOM_TYPE.SN:
                R = Random.Range(0, SN_Count);

                Mapinfo.MapStatus[count].SetMap(SN[R]);

                Temp = SN[R];
                SN[R] = SN[SN_Count - 1];
                SN[SN_Count - 1] = Temp;

                SN_Count--;
                if (SN_Count == 0)
                    SN_Count = SN.Length;
                break;
            case MapInfo.ROOM_TYPE.WS:
                R = Random.Range(0, EN_Count);

                Mapinfo.MapStatus[count].SetMap(WS[R]);

                Temp = EN[R];
                EN[R] = EN[EN_Count - 1];
                EN[EN_Count - 1] = Temp;

                Temp = ES[R];
                ES[R] = ES[EN_Count - 1];
                ES[EN_Count - 1] = Temp;

                Temp = WS[R];
                WS[R] = WS[EN_Count - 1];
                WS[EN_Count - 1] = Temp;

                Temp = WN[R];
                WN[R] = WN[EN_Count - 1];
                WN[EN_Count - 1] = Temp;

                EN_Count--;
                if (EN_Count == 0)
                    EN_Count = EN.Length;
                break;
            case MapInfo.ROOM_TYPE.WN:
                R = Random.Range(0, EN_Count);

                Mapinfo.MapStatus[count].SetMap(WN[R]);

                Temp = EN[R];
                EN[R] = EN[EN_Count - 1];
                EN[EN_Count - 1] = Temp;

                Temp = ES[R];
                ES[R] = ES[EN_Count - 1];
                ES[EN_Count - 1] = Temp;

                Temp = WS[R];
                WS[R] = WS[EN_Count - 1];
                WS[EN_Count - 1] = Temp;

                Temp = WN[R];
                WN[R] = WN[EN_Count - 1];
                WN[EN_Count - 1] = Temp;

                EN_Count--;
                if (EN_Count == 0)
                    EN_Count = EN.Length;
                break;
            case MapInfo.ROOM_TYPE.ESN:
                R = Random.Range(0, ESN_Count);

                Mapinfo.MapStatus[count].SetMap(ESN[R]);

                Temp = ESN[R];
                ESN[R] = ESN[ESN_Count - 1];
                ESN[ESN_Count - 1] = Temp;

                Temp = EWN[R];
                EWN[R] = EWN[ESN_Count - 1];
                EWN[ESN_Count - 1] = Temp;

                Temp = EWS[R];
                EWS[R] = EWS[ESN_Count - 1];
                EWS[ESN_Count - 1] = Temp;

                Temp = WNS[R];
                WNS[R] = WNS[ESN_Count - 1];
                WNS[ESN_Count - 1] = Temp;

                ESN_Count--;
                if (ESN_Count == 0)
                    ESN_Count = ESN.Length;
                break;
            case MapInfo.ROOM_TYPE.EWN:
                R = Random.Range(0, ESN_Count);

                Mapinfo.MapStatus[count].SetMap(EWN[R]);

                Temp = ESN[R];
                ESN[R] = ESN[ESN_Count - 1];
                ESN[ESN_Count - 1] = Temp;

                Temp = EWN[R];
                EWN[R] = EWN[ESN_Count - 1];
                EWN[ESN_Count - 1] = Temp;

                Temp = EWS[R];
                EWS[R] = EWS[ESN_Count - 1];
                EWS[ESN_Count - 1] = Temp;

                Temp = WNS[R];
                WNS[R] = WNS[ESN_Count - 1];
                WNS[ESN_Count - 1] = Temp;

                ESN_Count--;
                if (ESN_Count == 0)
                    ESN_Count = ESN.Length;
                break;
            case MapInfo.ROOM_TYPE.EWS:
                R = Random.Range(0, ESN_Count);

                Mapinfo.MapStatus[count].SetMap(EWS[R]);

                Temp = ESN[R];
                ESN[R] = ESN[ESN_Count - 1];
                ESN[ESN_Count - 1] = Temp;

                Temp = EWN[R];
                EWN[R] = EWN[ESN_Count - 1];
                EWN[ESN_Count - 1] = Temp;

                Temp = EWS[R];
                EWS[R] = EWS[ESN_Count - 1];
                EWS[ESN_Count - 1] = Temp;

                Temp = WNS[R];
                WNS[R] = WNS[ESN_Count - 1];
                WNS[ESN_Count - 1] = Temp;

                ESN_Count--;
                if (ESN_Count == 0)
                    ESN_Count = ESN.Length;
                break;
            case MapInfo.ROOM_TYPE.WNS:
                R = Random.Range(0, ESN_Count);

                Mapinfo.MapStatus[count].SetMap(WNS[R]);

                Temp = ESN[R];
                ESN[R] = ESN[ESN_Count - 1];
                ESN[ESN_Count - 1] = Temp;

                Temp = EWN[R];
                EWN[R] = EWN[ESN_Count - 1];
                EWN[ESN_Count - 1] = Temp;

                Temp = EWS[R];
                EWS[R] = EWS[ESN_Count - 1];
                EWS[ESN_Count - 1] = Temp;

                Temp = WNS[R];
                WNS[R] = WNS[ESN_Count - 1];
                WNS[ESN_Count - 1] = Temp;

                ESN_Count--;
                if (ESN_Count == 0)
                    ESN_Count = ESN.Length;
                break;
            case MapInfo.ROOM_TYPE.NONE:
                Mapinfo.MapStatus[count].SetMap(NONE[Random.Range(0, NONE.Length)]);
                break;
            case MapInfo.ROOM_TYPE.E:
                if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.SPAWN_ROOM)
                    Mapinfo.MapStatus[count].SetMap(E[0]);
                else if(Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.CHEST_ROOM)
                    Mapinfo.MapStatus[count].SetMap(E[1]);
                else if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.TIMESHOP)
                    Mapinfo.MapStatus[count].SetMap(E[2]);
                else
                    Mapinfo.MapStatus[count].SetMap(E[3]);
                break;
            case MapInfo.ROOM_TYPE.N:
                if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.SPAWN_ROOM)
                    Mapinfo.MapStatus[count].SetMap(N[0]);
                else if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.CHEST_ROOM)
                    Mapinfo.MapStatus[count].SetMap(N[1]);
                else if(Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.TIMESHOP)
                    Mapinfo.MapStatus[count].SetMap(N[2]);
                else
                    Mapinfo.MapStatus[count].SetMap(N[3]);
                break;
            case MapInfo.ROOM_TYPE.S:
                if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.SPAWN_ROOM)
                    Mapinfo.MapStatus[count].SetMap(S[0]);

                else if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.CHEST_ROOM)
                    Mapinfo.MapStatus[count].SetMap(S[1]);

                else if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.TIMESHOP)
                    Mapinfo.MapStatus[count].SetMap(S[2]);

                else
                    Mapinfo.MapStatus[count].SetMap(S[3]);
                break;
            case MapInfo.ROOM_TYPE.W:
                if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.SPAWN_ROOM)
                    Mapinfo.MapStatus[count].SetMap(_W[0]);

                else if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.CHEST_ROOM)
                    Mapinfo.MapStatus[count].SetMap(_W[1]);

                else if (Mapinfo.MapStatus[count].MapType == MapInfo.MAP_TYPE.TIMESHOP)
                    Mapinfo.MapStatus[count].SetMap(_W[2]);

                else
                    Mapinfo.MapStatus[count].SetMap(_W[3]);
                break;
        }
    }


}
