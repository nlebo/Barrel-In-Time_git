using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

   
    public float spawnValue;
    float maxSpawnValue;
    public bool isClear = true;
    public MiniMap _MiniMap;
    public GameObject[] MiniMap_UI;
   
    public Transform C1;
    Transform MiniMap;
    SpriteRenderer[] OtherRoom;
    int RoomCnt;
    public List<GameObject> SpawnMonster;
    public List<GameObject> SpawnPosition;
    public MapInfo.Mapinfo Status;
    SpriteRenderer image;
    public List<GameObject> ITEMS;

    // Use this for initialization
    void Awake()
    {
        C1 = GameObject.FindGameObjectWithTag("MiniMap_Cam").transform;
        _MiniMap = GameObject.FindGameObjectWithTag("MiniMap_Script").GetComponent<MiniMap>();
        image = gameObject.GetComponent<SpriteRenderer>();
        transform.GetChild(5).GetComponent<SpriteRenderer>().sortingOrder = 6;
        SpawnPosition = new List<GameObject>();

        MiniMap = Instantiate(MiniMap_UI[0], transform).transform;
        RoomCnt = 0;
        OtherRoom = new SpriteRenderer[3];

        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            SpawnPosition.Add(transform.GetChild(2).GetChild(i).gameObject);
        }

        isClear = true;
        
        if (spawnValue > 0)
            SetSpawnValue();
    }

    private void Start()
    {
        maxSpawnValue = spawnValue;
    }
    //-------------------------------------------------------------------
    // Use those for Set SpawnValue
    bool SetSpawnValue()
    {
        isClear = false;
        int r;
        r = Random.Range(1, 101);

        if (r <= 10) spawnValue = 8;
        else if (r <= 50) spawnValue = 12;
        else if (r <= 85) spawnValue = 16;
        else spawnValue = 20;


        return true;
    }
    public bool SetStatus(MapInfo.Mapinfo Stat)
    {
        Status = Stat;
        if (Status.MapType == MapInfo.MAP_TYPE.BOSS_ROOM)
        {
            isClear = false;
            GameManager.Instance.LocalPlayer.BossRoomPos = transform.position;
        }
        return true;
    }

    //-------------------------------------------------------------------
    // Use those for TriggerCheck
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            Status.OpenDoor();
            C1.parent = transform;
            C1.localPosition = new Vector3(0, 0, -10);
            if (gameObject.GetComponent<SpriteRenderer>().enabled == false)
            {
                
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                StartCoroutine(FadeIn());
                Status.OpenDoor();
            }

            else
                StartCoroutine(FadeIn());

            if (Status.MapType == MapInfo.MAP_TYPE.TIMESHOP)
                GameManager.Instance.SoundManager.AudioPlay(GameManager.Instance.SoundManager.BGMS.SHOP_BGM);
            else if(Status.MapType == MapInfo.MAP_TYPE.BOSS_ROOM)
                GameManager.Instance.SoundManager.FadeOut();

        }
        if (collision.tag == "UI_CHECK")
        {
            OtherRoom[RoomCnt++] = collision.transform.parent.parent.GetComponent<SpriteRenderer>();

        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;

            StartCoroutine(FadeOut());

            Status.OpenDoor();

            if (Status.MapType == MapInfo.MAP_TYPE.TIMESHOP || Status.MapType == MapInfo.MAP_TYPE.BOSS_ROOM)
                GameManager.Instance.SoundManager.AudioPlay(GameManager.Instance.SoundManager.BGMS.MAIN_BGM);

        }
    }


    //-------------------------------------------------------------------
    // Use those for Effect
    IEnumerator FadeIn()
    {
        
        int i = 0;
        float alpha = 0;
        MiniMap.gameObject.SetActive(true);
        


        MiniMap.gameObject.GetComponent<SpriteRenderer>().sprite = MiniMap_UI[1].GetComponent<SpriteRenderer>().sprite;

        alpha = Time.deltaTime * 1.5f;
        while (i == 0)
        {
            yield return new WaitForSeconds(0.0001f);


            gameObject.GetComponent<SpriteRenderer>().color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + alpha);

            if (image.color.a >= 1)
            {
                alpha = 0;
                i = 1;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }



        }

       
          
        yield return null;

    }
    IEnumerator FadeOut()
    {
        int i = 0;
        float alpha = 0;


        MiniMap.gameObject.GetComponent<SpriteRenderer>().sprite = MiniMap_UI[2].GetComponent<SpriteRenderer>().sprite;

        while (i==0)
        {
            yield return new WaitForSeconds(0.0001f);
            alpha += Time.deltaTime * 1.5f;

            gameObject.GetComponent<SpriteRenderer>().color = new Color(image.color.r, image.color.g, image.color.b,1 - alpha);
            if (image.color.a <= 0.4)
            {
                alpha = 0;
                i = 1;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
            }

            

        }

        yield return null;

    }


    //-------------------------------------------------------------------
    // Use those for StartRoom
    public void _StartRoom()
    {
        if (Status.MapType != MapInfo.MAP_TYPE.BOSS_ROOM)
            StartCoroutine(StartRoom());
        else
            StartCoroutine(StartBossRoom());
    }
    IEnumerator StartRoom()
    {
        yield return new WaitForSeconds(1.0f);
        FindObjectOfType<Clock>().War_Sec_On();

        if (gameObject.GetComponent<SpawnManager>())
        {
            if (spawnValue > 0)
            {
                _MiniMap.AllOff();
                GetComponent<SpawnManager>().SpawnMon();
                
                //Status.CloseDoor();
            }
        }
            
        while (!isClear)
        {
            yield return null;
            if (!isClear)
            {
                if (GetComponent<SpawnManager>().monsterCount <= 0 && spawnValue <= 0)
                {
                    SpawnItem();
                    _MiniMap.On();
                    GameManager.Instance.SoundManager.EffectPlay(GameManager.Instance.SoundManager.SYSTEMS.DOOR_OPEN);
                    Status.OpenDoor();
                    FindObjectOfType<Clock>().War_Sec_Off();
                    FindObjectOfType<Clock>().Break = false;
                    GameManager.Instance.LocalPlayer.ER_UP(GameManager.Instance.LocalPlayer.LittleDoll);
                    isClear = true;
                }
                GetComponent<SpawnManager>().CurseBirdTime();
            }
        }

    }
    IEnumerator StartBossRoom()
    {
        if(isClear)
        { }
        else
        {
            
            _MiniMap.AllOff();
            GameObject Can = GameObject.Find("Canvas");
            Can.SetActive(false);
            Bird_Clock Boss = Instantiate(SpawnMonster[0], transform.position, Quaternion.identity).GetComponent<Bird_Clock>();
            Boss.transform.parent = transform.GetChild(3);
            GameManager.Instance.InputManager.Active = false;
            Camera.main.transform.parent = this.transform;
            Vector3 CPos = Camera.main.transform.localPosition;
            float dTime = 0;
            yield return new WaitForSeconds(1);
            while(Camera.main.transform.localPosition != new Vector3(0,0,-10))
            {
                dTime += Time.deltaTime;
                if (dTime >= 1)
                    dTime = 1;
                Camera.main.transform.localPosition = new Vector3(Mathf.Lerp(CPos.x, 0, dTime), Mathf.Lerp(CPos.y, 0, dTime), -10);
                yield return null;
            }
            Boss.Active = true;
            while (!Boss.Finish_Pattern00) { yield return null; }

            Camera.main.transform.parent = GameManager.Instance.LocalPlayer.transform.parent;
            CPos = Camera.main.transform.localPosition;
            dTime = 0;
            while (Camera.main.transform.localPosition != new Vector3(0, 0, -10))
            {
                dTime += Time.deltaTime;
                if (dTime >= 1)
                    dTime = 1;
                Camera.main.transform.localPosition = new Vector3(Mathf.Lerp(CPos.x, 0, dTime), Mathf.Lerp(CPos.y, 0, dTime), -10);
                yield return null;
            }
            Can.SetActive(true);
            GameManager.Instance.InputManager.Active = true;
            Boss.AttackStart = true;
            Boss.PrevTime = Time.time;
        }
        yield return null;
    }

    // Update is called once per frame
    void Update () {
        for(int i =0;i<RoomCnt; i++)
        {
            if (OtherRoom[i].color.a >= 0.1f)
                MiniMap.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Home))
            MiniMap.gameObject.SetActive(true);

        if (GameManager.Instance.InputManager.KillAll)
            KillEnemyAll();
	}

    void SpawnItem()
    {
        int Item = 1;
        if (GameManager.Instance.LocalPlayer.HaveDetector)
            Item = 2;
        if (Random.Range(1, maxSpawnValue + 1) > (maxSpawnValue *Item) / 2)
            return;

        if (ITEMS.Count>0)
            Instantiate(ITEMS[Random.Range(0, ITEMS.Count)], SpawnPosition[Random.Range(0, SpawnPosition.Count)].transform.position, Quaternion.identity);
    }

    public void KillEnemyAll()
    {
        MonsterManager[] _Enemy = transform.GetChild(3).GetComponentsInChildren<MonsterManager>();

        for(int i=0;i<_Enemy.Length;i++)
        {
            _Enemy[i].Death();
        }
    }

}
