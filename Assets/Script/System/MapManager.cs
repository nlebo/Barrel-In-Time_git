using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    InputManager input;
    public MapCreater Map;

    bool Complete = false;

    int where;
    int StackOverFlow = 0;
   
	// Use this for initialization
	void Start	 () {
        
        
        Map = gameObject.GetComponent<MapCreater> ();
		input = GameManager.Instance.InputManager;
        while (!Complete)
        {
            if (!CompleteMap())
            {
                Map.InitializeMap();
                Map.MaxMapCount = where;
            }
            else Complete = true;
        }
        Complete = false;

        GameManager.Instance.SoundManager.AudioPlay(GameManager.Instance.SoundManager.BGMS.MAIN_BGM);
    }

    //----------------------------------------------------------------------------------------------------------
    // Use this for MapSpawn
    bool SpawnMap(int MapCount)
    {
        StackOverFlow = 0;

        while (Map.MaxMapCount > Map.MapCount)
        {
            StackOverFlow++;

            if (Map.MapCount == 0)
            {
                if (!Map.MakeMap((int)Map.Mapinfo.MapStatus[MapCount].Pos.x, (int)Map.Mapinfo.MapStatus[MapCount].Pos.y))
                {
                }
                MapCount = Map.MapCount - 1;
            }

            else
            {
                if (!Map.MakeMap((int)Map.Mapinfo.MapStatus[MapCount].Pos.x, (int)Map.Mapinfo.MapStatus[MapCount].Pos.y))
                {

                    
                    MapCount = Map.MapCount - 1;

                }
            }

            if (StackOverFlow>=100)
            {
                Debug.Log("Can't Make Room anymore In SpawnMap");
                if(Map.MaxMapCount >= where)
                {
                    while(Map.MapCount > Map.MaxMapCount-3)
                    {
                        Map.MapStruct[(int)Map.Mapinfo.MapStatus[MapCount].Pos.y, (int)Map.Mapinfo.MapStatus[MapCount].Pos.x] = 0;
                        Map.Mapinfo.MapStatus[MapCount] = new MapInfo.Mapinfo();

                        Map.MapCount--;
                        Map.W = (int)Map.Mapinfo.MapStatus[MapCount].Pos.x;
                        Map.H = (int)Map.Mapinfo.MapStatus[MapCount].Pos.y;
                        
                    }
                }
                return false;
            }
        }

        return true;

    }

    //----------------------------------------------------------------------------------------------------------
    // Use this for CompleteMap
    bool CompleteMap()
    {
        int StackOver = 0;
        where = Map.MaxMapCount;
        int[] Make = new int[3];

        Map.Mapinfo.MapStatus[0].Pos = new Vector2(25,25);


        while (Map.MaxMapCount != where + 3)
        {

            if(StackOver >= 40)
            {
                Debug.Log("Failed Make Map 1st Way");
                break;
            }

            StackOver++;

            if (SpawnMap(Map.MapCount))
            {
                Map.MaxMapCount += 3;
            }
        }

        StackOver = StackOver >= 40 ? 40 : 0;

        //----------------------------------------------------------------------------

        Make[0] = Random.Range(1, where - 2);
        Map.C = Make[0];
        Map.W = (int)Map.Mapinfo.MapStatus[Make[0]].Pos.x;
        Map.H = (int)Map.Mapinfo.MapStatus[Make[0]].Pos.y;
        
        while (Map.MaxMapCount != where + 6)
        {
            if (StackOver >= 40)
            {
                Debug.Log("Failed Make Map 2st Way");
                break;
            }

            StackOver++;

            if (SpawnMap(Make[0]))
            {
                Map.MaxMapCount += 3;

            }

            else
            {
                Debug.Log("Failed Make Map 2st Way" + StackOver);
                Map.MapCount = Map.MaxMapCount - 3;
                Make[0] = Random.Range(1, where - 2);
                Map.C = Make[0];
                Map.W = (int)Map.Mapinfo.MapStatus[Make[0]].Pos.x;
                Map.H = (int)Map.Mapinfo.MapStatus[Make[0]].Pos.y;
                if (SpawnMap(Make[0]))
                {
                    Map.MaxMapCount += 3;
                }
            }

            
        }

        //----------------------------------------------------------------------------

        StackOver = StackOver >= 40 ? 40 : 0;


        do
        {
            Make[1] = Random.Range(1, where - 2);
            Map.C = Make[1];
        } while (Make[1] == Make[0]);

        Map.W = (int)Map.Mapinfo.MapStatus[Make[1]].Pos.x;
        Map.H = (int)Map.Mapinfo.MapStatus[Make[1]].Pos.y;

        while (Map.MaxMapCount != where + 9)
        {
            if (StackOver >= 40)
            {
                Debug.Log("Failed Make Map 3st Way");
                break;
            }
            StackOver++;


            if (SpawnMap(Make[1]))
            {
                Map.MaxMapCount += 3;
                
            }
            else
            {
                Debug.Log("Failed Make Map 3st Way" + StackOver);
                Map.MapCount = Map.MaxMapCount - 3;
                int stack = 0;
                do
                {
                    if (stack >= 10) break;
                    Make[1] = Random.Range(1, where - 3);
                    stack++;

                } while (Make[1] == Make[0]);

                if (stack >= 10)
                    continue;
                Map.C = Make[1];
                Map.W = (int)Map.Mapinfo.MapStatus[Make[1]].Pos.x;
                Map.H = (int)Map.Mapinfo.MapStatus[Make[1]].Pos.y;
                if (SpawnMap(Make[1]))
                {
                    Map.MaxMapCount += 3;
                }
            }
            
        }

        //----------------------------------------------------------------------------

        StackOver = StackOver >= 40 ? 50 : 0;


        do
        {
            Make[2] = Random.Range(1, where - 3);
            Map.C = Make[2];

        } while (Make[2] == Make[0] || Make[2] == Make[1]);

        Map.W = (int)Map.Mapinfo.MapStatus[Make[2]].Pos.x;
        Map.H = (int)Map.Mapinfo.MapStatus[Make[2]].Pos.y;

        while (Map.MaxMapCount != where + 12)
        {
            if (StackOver >= 40)
            {
                do
                {
                    Make[2] = Random.Range(1, where - 3);

                } while (Make[2] == Make[0] || Make[2] == Make[1]);

                Map.W = (int)Map.Mapinfo.MapStatus[Make[2]].Pos.x;
                Map.H = (int)Map.Mapinfo.MapStatus[Make[2]].Pos.y;
                Debug.Log("Failed Make Map Last Way");

            }

            if (StackOver >= 50)
                break;

            StackOver++;

            if (SpawnMap(Make[2]))
            {
                Map.MaxMapCount += 3;

            }
            else
            {
                Debug.Log("Failed Make Map Last Way" + StackOver);
                Map.MapCount = Map.MaxMapCount - 3;

                int stack = 0;
                do
                {
                    if (stack >= 10) break;
                    Make[2] = Random.Range(1, where - 3);
                    stack++;

                } while (Make[2] == Make[0] || Make[2] == Make[1]);

                if (stack >= 10)
                    continue;

                Map.C = Make[2];
                Map.W = (int)Map.Mapinfo.MapStatus[Make[2]].Pos.x;
                Map.H = (int)Map.Mapinfo.MapStatus[Make[2]].Pos.y;
                if (SpawnMap(Make[2]))
                {
                    Map.MaxMapCount += 3;
                }
            }
           
        }

        if (StackOver >= 50)
            return false;

        int count = 0;
        while (count < Map.MaxMapCount - 3)
        {
            int x = (int)Map.Mapinfo.MapStatus[count].Pos.x;
            int y = (int)Map.Mapinfo.MapStatus[count].Pos.y;

            Map.CheckMap(x, y);
            Map.Mapinfo.MapStatus[count].ConnectBridge(Map.up, Map.down, Map.left, Map.right);

            Map.SetSpriteRoom(count);
            Map.InstanceMap(count);
            Map.Mapinfo.MapStatus[count].SetDoor();
            count++;
        }

        return true;
    }


    void Update()
    {
        
        if (input.Initialize || input.PlayerDead)
        {
            while (!Complete)
            {
                Map.MaxMapCount = where;
                GameObject.Find("MiniMap_Cam").transform.parent = null;
                GameObject.FindObjectOfType<MiniMap>().On();
                Map.InitializeMap();
                if (CompleteMap())
                {
                    Complete = true;
                    input.PlayerDead = false;
                }
            }
            Complete = false;
        }



    }
}
