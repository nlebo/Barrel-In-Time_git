using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

    RoomManager Room;
    List<GameObject> Monster;
    public GameObject[] Birds;
    CurseManager _Curse;
    Clock _Clock;
    bool _Spawn;
    public GameObject Spawning;
    public GameObject HelpUI;
    public GameObject[] TitleUI;
    SoundManager FX;
    bool isSpawn = false;
    bool ShowUI = false, ShowTitle = false;
    public int monsterCount = 0;
    public int count = 0;
    int SPAWNMON = 0;
    float MaxFrequency = 0;

	// Use this for initialization
	void Start () {
        SetStatus(GetComponent<RoomManager>());
        //StartCoroutine(CheckEnd());
        _Curse = FindObjectOfType<CurseManager>();
        _Clock = FindObjectOfType<Clock>();
        //StartCoroutine(MobSpawn());
        TitleUI = new GameObject[2];
        FX = GameManager.Instance.SoundManager;
    }

    void Update()
    {
        if(ShowUI)
        {
            ShowUI = false;
            _ShowUI();
        }

        
    }

    public void SetStatus(RoomManager _Room)
    {
        Room = _Room;
        Monster = Room.SpawnMonster;
    }
    
    void CutMonster()
    {
        MaxFrequency = 0;
        for(count =0;count<Monster.Count;count++)
        {
            if (Monster[count].GetComponent<MonsterManager>().entityValue > Room.spawnValue)
                break;

            MaxFrequency += Monster[count].GetComponent<MonsterManager>().spawnFrequency;
        }
        
    }

    void SelectMonster()
    {
        float Spawn;
        float percent = 0;

        Spawn = Random.Range(1, MaxFrequency);
        Debug.Log(Spawn);
        for (int i = 0; i < count; i++)
        {
            percent += Monster[i].GetComponent<MonsterManager>().spawnFrequency;

            if (Spawn <= percent) {
                SpawnMonster(Monster[i]);
                break;
            } 

        }
    }

    bool SpawnMonster(GameObject Mob)
    {
        
        Transform MobSpawner;

        MobSpawner = Room.transform.GetChild(3);

        if (Room.spawnValue >= Mob.GetComponent<MonsterManager>().entityValue)
        {
            Room.spawnValue -= Mob.GetComponent<MonsterManager>().entityValue;
            int position = Random.Range(0, Room.SpawnPosition.Count - SPAWNMON);
            
            StartCoroutine(SPAWN(Mob, Room.SpawnPosition[position].transform.position));
            GameObject a;
            a = Room.SpawnPosition[position];
            Room.SpawnPosition[position] = Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON];
            Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON] = a;

            monsterCount++;
            SPAWNMON++;
            _Spawn = false;
        }

        return true;
    }

    public bool SpawnMon()
    {
        
        StopCoroutine(MobSpawn());
        StartCoroutine(MobSpawn());
        return true;
    }
    IEnumerator SPAWN(GameObject Mob, Vector3 Position)
    {
        GameObject child;
        GameObject This = Instantiate(Spawning, Position, Quaternion.identity);
        
        yield return new WaitForSeconds(1);
        FX.EffectPlay(FX.SYSTEMS.ENEMY_DEAD);
        Destroy(This);

        child = (GameObject)Instantiate(Mob, new Vector3(Position.x, Position.y, 0), Quaternion.identity);
        child.transform.parent = Room.transform.GetChild(3);

    }
    public bool CurseBirdSpawn()
    {


        if (_Curse.Curse < 2 )
            return false;

        Transform MobSpawner;
        MobSpawner = Room.transform.GetChild(3);

        for (int i = 0; i * 2 < _Curse.Curse; i++)
        {

            Debug.Log("Spawn Bird Because of Curse");
            int position = Random.Range(0, Room.SpawnPosition.Count - SPAWNMON);


            GameObject child = (GameObject)Instantiate(Birds[0], new Vector3(Room.SpawnPosition[position].transform.position.x, Room.SpawnPosition[position].transform.position.y, 0), Quaternion.identity);
            child.transform.parent = MobSpawner;

            GameObject a;
            a = Room.SpawnPosition[position];
            Room.SpawnPosition[position] = Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON];
            Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON] = a;

            monsterCount++;
            SPAWNMON++;

        }
        return true;
    }
    public bool CurseBirdClockSpawn()
    {
        if (_Curse.Curse < 6)
            return false;

        Transform MobSpawner;
        MobSpawner = Room.transform.GetChild(3);

        for (int i = 0; i*6 < _Curse.Curse / 6; i++)
        {

            Debug.Log("Spawn BirdClock Because of Curse");
            int position = Random.Range(0, Room.SpawnPosition.Count - SPAWNMON);


            GameObject child = (GameObject)Instantiate(Birds[1], new Vector3(Room.SpawnPosition[position].transform.position.x, Room.SpawnPosition[position].transform.position.y, 0), Quaternion.identity);
            child.transform.parent = MobSpawner;

            GameObject a;
            a = Room.SpawnPosition[position];
            Room.SpawnPosition[position] = Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON];
            Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON] = a;

            monsterCount++;
            SPAWNMON++;

        }
        return true;
    }
    public bool CurseBirdTime()
    {
        if (_Clock.War_Sec < 100)
            return false;

        FX.EffectPlay(FX.ENEMYS.CUCKOO_HUNT);
        _Clock.War_Sec = 0;
        
        Transform MobSpawner;
        MobSpawner = Room.transform.GetChild(3);

        for (int i = 0; i < _Curse.Spawn_GUGU; i++)
        {

            Debug.Log("Time to Spawn Bird Because of Curse");
            int position = Random.Range(0, Room.SpawnPosition.Count - SPAWNMON);


            GameObject child = (GameObject)Instantiate(Birds[0], new Vector3(Room.SpawnPosition[position].transform.position.x, Room.SpawnPosition[position].transform.position.y, 0), Quaternion.identity);
            child.transform.parent = MobSpawner;
            child.GetComponent<MonsterManager>().entityValue = 0;

            GameObject a;
            a = Room.SpawnPosition[position];
            Room.SpawnPosition[position] = Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON];
            Room.SpawnPosition[Room.SpawnPosition.Count - 1 - SPAWNMON] = a;

            monsterCount++;
            SPAWNMON++;

            if (SPAWNMON >= Room.SpawnPosition.Count)
                SPAWNMON = 0;
        }
        ShowUI = true;
        


        return true;
    }

    public void _ShowUI()
    {
        StartCoroutine(ShowHelpUI());
    }

    IEnumerator MobSpawn()
    {

        while (!isSpawn)
        {
            isSpawn = true;
            if (Room.spawnValue > 0)
            {
                ShowTitle = true;
                yield return new WaitForSeconds(0.6f);
            }
            while (Room.spawnValue > 0)
            {

                
                yield return new WaitForSeconds(0.3f);
                



                CutMonster();
                SelectMonster();

            }
            yield return new WaitForSeconds(0.3f);
            CurseBirdSpawn();
            yield return new WaitForSeconds(0.3f);
            CurseBirdClockSpawn();
            yield return new WaitForSeconds(0.3f);
            CurseBirdTime();

            SPAWNMON = 0;
        }

        isSpawn = false;


    }

    IEnumerator CheckEnd()
    {
        while(monsterCount > 0 || Room.spawnValue > 0)
        {
            yield return null;
        }
        GameManager.Instance.SoundManager.EffectPlay(GameManager.Instance.SoundManager.SYSTEMS.DOOR_OPEN);
        Room.Status.OpenDoor();
    }
    IEnumerator ShowHelpUI()
    {
        HelpUI = Room._MiniMap.HelpUI.transform.parent.GetChild(1).gameObject;
        
        HelpUI.SetActive(true);

        Text original  = HelpUI.GetComponent<Text>();
        original.color = new Color(original.color.r, original.color.g, original.color.b, 0);
        original.text = "뻐꾸기들이 습격해왔습니다!";

        float PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            original.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(0, 1, (Time.time - PTime) * 2));
            yield return null;
        }
        yield return new WaitForSeconds(2);

        PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            original.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(1, 0, (Time.time - PTime) * 2));
            yield return null;
        }


        HelpUI.SetActive(false);

    }
    IEnumerator ShowTitleUI()
    {
         TitleUI[0] = Room._MiniMap.HelpUI.transform.parent.GetChild(2).gameObject;
        TitleUI[1] = Room._MiniMap.HelpUI.transform.parent.GetChild(3).gameObject;

        TitleUI[0].SetActive(true);
        TitleUI[1].SetActive(true);

        Text original  = TitleUI[0].GetComponent<Text>();
        Text original2 = TitleUI[1].GetComponent<Text>();

        original.color = new Color(original.color.r, original.color.g, original.color.b, 0);
        original2.color = new Color(original.color.r, original.color.g, original.color.b, 0);

        float PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            original.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(0, 1, (Time.time - PTime) * 2));
            original2.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(0, 1, (Time.time - PTime) * 2));
            yield return null;
        }
        yield return new WaitForSeconds(2);

        PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            original.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(1, 0, (Time.time - PTime) * 2));
            original2.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(1, 0, (Time.time - PTime) * 2));
            yield return null;
        }


        TitleUI[0].SetActive(false);
        TitleUI[1].SetActive(false);
    }

}
