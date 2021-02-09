using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Clock : StateInfo {

    public bool Active = false;
    public bool AttackStart = false;
    public bool Finish_Pattern00 = false;
    public bool Spawning = false;
    public GameObject _Bird;
    public GameObject _GrandPather;
    public GameObject[] SpawnPos;
    public GameObject _Spawning;
    public GameObject _SONG,_SONG2;
    public GameObject[] Bird_Spawn;
    public GameObject _Projectile;
    public GameObject GunFire;
    public GameObject DeadEffect;
    public BossHP HPBAR;
    public SoundManager FX;
    public Player _Player;
    public Animator Bird_King;
    public Animator Doors;
    public Animator Wheel;
    public float PrevMS;
    public float Pattern03_Cool;
    public Transform GunSpawn;
    public Transform Gun;
    bool CanPattern03 = false;
    bool CanHit = true;
    public float PrevTime;
    float MoveSpeed = 1;

    IEnumerator Pattern;
    float Front = 1;
    Vector2 SpriteSize;

	// Use this for initialization
	void Start () {
        StartCoroutine(Pattern00());
        _Player = GameManager.Instance.LocalPlayer;
        SpriteSize = GetComponent<SpriteRenderer>().size;
        Pattern = Pattern01();
        PrevTime = Time.time;
        FX = GameManager.Instance.SoundManager;
        HPBAR = BossHP.Instance;
        HPBAR.BOSS = this;
        HPBAR.transform.GetChild(0).gameObject.SetActive(true);
        HPBAR.transform.GetChild(1).gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (!AttackStart)
            return;

        if (!CanPattern03)
        {
            if (Time.time - PrevTime >= Pattern03_Cool)
                CanPattern03 = true;
        }

        Move(MoveSpeed);
        Vector2 diff = _Player.transform.position - GunSpawn.position;

        diff.Normalize();

        float rot_z2 = (Mathf.Atan2(-diff.y, -diff.x) * Mathf.Rad2Deg);
        Gun.rotation = Quaternion.Euler(0f, 0, rot_z2);

    }



    public override void Hit(float Damage)
    {
        if (CanHit)
        {
            
            base.Hit(Damage);
            StartCoroutine(InDamage());
        }
    }
    public override void Hit(float Damage, float Push, Vector3 Position)
    {
        if (CanHit)
        {
            Hit(Damage);
        }

    }
    public override void Death()
    {
        Alive = false;

        GameManager.Instance.InputManager.Active = false;
        transform.parent.parent.GetComponent<RoomManager>().KillEnemyAll();
        HPBAR.gameObject.SetActive(false);
        Instantiate(DeadEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

    }

    public void Move(float Speed)
    {

            if (_Player.transform.position.x > transform.position.x)
            {
                transform.rotation = new Quaternion(0, 180, 0, transform.rotation.w);
                Front = -1;
            }
            else if (_Player.transform.position.x <= transform.position.x)
            {
                transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
                Front = 1;
            }

        Vector3 direction = _Player.transform.position - (transform.position + (Vector3.down * SpriteSize.y /2 ) + (Vector3.left * SpriteSize.x/2));
        if ((_Player.transform.position - transform.position).magnitude >= 1.5f)
        {
            direction.Normalize();
            Vector3 move = new Vector3(direction.x * Front, direction.y, 0);

            Ray2D ray = new Ray2D(transform.position, direction);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, MS * Time.deltaTime, 9);

            Ray2D ray1 = new Ray2D(transform.position + Vector3.up * GetComponent<SpriteRenderer>().size.y, direction);
            RaycastHit2D hit1 = Physics2D.Raycast(ray.origin, ray.direction, MS * Time.deltaTime, 9);

            Ray2D ray2 = new Ray2D(transform.position - Vector3.up * GetComponent<SpriteRenderer>().size.y, direction);
            RaycastHit2D hit2 = Physics2D.Raycast(ray.origin, ray.direction, MS * Time.deltaTime, 9);

            if (hit.collider == null || hit.collider.tag != "Obstacle")
                if (hit1.collider == null || hit1.collider.tag != "Obstacle")
                    if (hit2.collider == null || hit2.collider.tag != "Obstacle")
                        transform.Translate(move * MS * Time.deltaTime * Speed);
        }
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag == "HitBox")
        {
            HitCheck hit = collision.GetComponent<HitCheck>();
            if (CanHit)
                FX.EffectPlay(FX.PCS.PC_HIT);
            Hit(hit.Weapon.ATK * ((100 + hit.Player.ATK) / 100), hit.Weapon.PushValue, hit.Weapon.Player.transform.position);
            
        }


    }

    IEnumerator Pattern00()
    {
        
        Debug.Log("Pattern00");
        while (!Active)
        {
            yield return null;
        }

        FX.EffectPlay(FX.ENEMYS.BOSS_CUCKOO_FIRST_BEEP);
        Bird_King.SetBool("Song", true);
        yield return null;
        GameObject This = Instantiate(_Spawning,SpawnPos[0].transform.position, Quaternion.identity);
        GameObject This2 = Instantiate(_Spawning,SpawnPos[1].transform.position, Quaternion.identity);
        Bird_King.SetBool("Song", false);
        yield return new WaitForSeconds(1);
        Destroy(This);
        Destroy(This2);

        for (int i = 0; i < 2; i++)
        {
            
            Bird bird = Instantiate(_Bird, SpawnPos[i].transform.position, Quaternion.identity).GetComponent<Bird>();
            bird.transform.parent = transform.parent;
            yield return null;
            bird.whatState = 2;
            bird.entityValue = 0;
            bird.GetComponent<Animator>().SetInteger("WhatState", 2);
            

        }

        FX.AudioPlay(FX.BGMS.BOSS_BGM);
        yield return new WaitForSeconds(1);
        Finish_Pattern00 = true;
        while(!AttackStart)
        {
            yield return null;
        }
        
        Wheel.SetBool("Move", true);
        StartCoroutine(Pattern);
    }
    IEnumerator Pattern01()
    {
        Debug.Log("Pattern01");
        for (int j = 0; j < 2; j++)
        {
            Doors.SetInteger("Door", 0);
            Doors.SetBool("Open", true);
            FX.EffectPlay(FX.ENEMYS.CUCKOO_DOOR_OPEN);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(SpawnBird());

            Vector2 diff;
            diff = _Player.transform.position - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            Bird_King.SetBool("Go", true);
            FX.EffectPlay(FX.ENEMYS.CUCKOO_CLOCK);
            yield return null;
            Bird_King.SetBool("Go", false);
            for (int i = 0; i < 5; i++)
            {
                Song song;
                if (i == 0)
                {
                    song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, rot_z)).GetComponent<Song>();
                    song.direction = _Player.transform.position - transform.position;
                    song.direction.Normalize();
                }

                else if (i <= 2)
                {
                    song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, rot_z + 20)).GetComponent<Song>();
                    song.direction = _Player.transform.position - transform.position;
                    song.direction.Normalize();
                    song.moveSpeed += 1;
                }
                else
                {
                    song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, rot_z + 40)).GetComponent<Song>();
                    song.direction = _Player.transform.position - transform.position;
                    song.direction.Normalize();
                    song.moveSpeed += 2;
                }
            }
            
            
            StartCoroutine(SetSpeed());
            for (; ; )
            {
                if (Spawning)
                    break;
                yield return null;
            }
            
            Spawning = false;
            
            yield return new WaitForSeconds(0.5f);
            Doors.SetInteger("Door", 1);
            Doors.SetBool("Open", true);
            FX.EffectPlay(FX.ENEMYS.CUCKOO_DOOR_OPEN);
            yield return new WaitForSeconds(0.2f);

            StartCoroutine(SpawnBird());
            diff = _Player.transform.position - transform.position;

            diff.Normalize();

            rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            Bird_King.SetBool("Go", true);
            FX.EffectPlay(FX.ENEMYS.CUCKOO_CLOCK);
            yield return null;
            Bird_King.SetBool("Go", false);
            for (int i = 0; i < 5; i++)
            {
                Song song;
                if (i == 0)
                {
                    song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, rot_z)).GetComponent<Song>();
                    song.direction = _Player.transform.position - transform.position;
                    song.direction.Normalize();
                    song.ATK = 16;
                    song.moveSpeed = 4;
                }

                else if (i <= 2)
                {
                    song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, rot_z + 20)).GetComponent<Song>();
                    song.direction = _Player.transform.position - transform.position;
                    song.direction.Normalize();
                    song.ATK = 16;
                    song.moveSpeed = 5;
                }
                else
                {
                    song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, rot_z + 40)).GetComponent<Song>();
                    song.direction = _Player.transform.position - transform.position;
                    song.direction.Normalize();
                    song.ATK = 16;
                    song.moveSpeed = 6;
                }
            }
            
            
            StartCoroutine(SetSpeed());
            for (; ; )
            {
                if (Spawning)
                    break;
                yield return null;
            }

            Spawning = false;
            
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3.5f);

        if (!CanPattern03)
            Pattern = Pattern02();
        else
        {
            Pattern = Pattern03(2);
            FX.EffectPlay(FX.ENEMYS.BOSS_CUCKOO_CANNONREADY);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(Pattern);
    }
    IEnumerator Pattern02()
    {
        Debug.Log("Pattern02");
        yield return null;
        Bird_King.SetBool("Song", true);
        FX.EffectPlay(FX.ENEMYS.CUCKOO_HUNT);
        
        yield return null;
        FX.EffectPlay(FX.ENEMYS.BOSS_CUCKOO_BEEP);
        Bird_King.SetBool("Song", false);
        for (int i =0;i<30;i++)
        {
            Song song = Instantiate(_SONG, transform.position, Quaternion.Euler(0, 0, 12 * i)).GetComponent<Song>();
            song.direction = _Player.transform.position - transform.position;
            song.direction.Normalize();
            song.ATK = 16;
            song.moveSpeed = 4;
           
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 20; i++)
        {
            Song song = Instantiate(_SONG2, transform.position, Quaternion.Euler(0, 0, 18 * i)).GetComponent<Song>();
            song.direction = _Player.transform.position - transform.position;
            song.direction.Normalize();
            song.ATK = 22;
            song.moveSpeed = 5;
            song.GetComponent<SpriteRenderer>().color = Color.red;
           
        }

        Vector3[] SP={ SpawnPos[0].transform.position, SpawnPos[1].transform.position};
        GameObject This = Instantiate(_Spawning, SpawnPos[0].transform.position, Quaternion.identity);
        GameObject This2 = Instantiate(_Spawning, SpawnPos[1].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(This);
        Destroy(This2);

        for (int i = 0; i < 2; i++) {
            Bird b = Instantiate(_Bird, SP[i], Quaternion.identity).GetComponent<Bird>();
            b.transform.parent = transform.parent;
            yield return null;
            b.whatState = 2;
            b.entityValue = 0;
            b.GetComponent<Animator>().SetInteger("WhatState", 2);
        }
        

        yield return new WaitForSeconds(4);


        if (!CanPattern03)
            Pattern = Pattern01();
        else
        {
            Pattern = Pattern03(1);
            FX.EffectPlay(FX.ENEMYS.BOSS_CUCKOO_CANNONREADY);
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(Pattern);
    }
    IEnumerator Pattern03(int Next)
    {
        Debug.Log("Pattern03");
        CanPattern03 = false;
        PrevTime = Time.time;
        yield return null;
        MoveSpeed = 0.5f;
        
        while (Time.time - PrevTime <= 6)
        {
            Vector2 diff;
            diff = _Player.transform.position - GunSpawn.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            Instantiate(GunFire, GunSpawn.position, Quaternion.Euler(0f, 0, rot_z));
            FX.EffectPlay(FX.ENEMYS.BOSS_CUCKOO_CANNONFIRE);
            GUGU_GUN a = Instantiate(_Projectile, GunSpawn.position, Quaternion.Euler(0f, 0, rot_z)).GetComponent<GUGU_GUN>();
            a.Damage = 30;
            a.moveSpeed = 20;
            yield return new WaitForSeconds(0.2f);
        }

        PrevTime = Time.time;
        MoveSpeed = 1;
        if (Next == 1)
            Pattern = Pattern01();
        else
            Pattern = Pattern02();

        StartCoroutine(Pattern);
    }

    IEnumerator SetSpeed()
    {
        PrevMS = MS;
        MS *= 1.5f;
        yield return new WaitForSeconds(0.2f);
        MS = PrevMS;   
    }
    IEnumerator SpawnBird()
    {
        yield return new WaitForSeconds(0.5f);


        Bird bird = Instantiate(_Bird, Bird_Spawn[Doors.GetInteger("Door")].transform.position, Quaternion.identity).GetComponent<Bird>();
        bird.transform.parent = transform.parent;
        yield return null;
        bird.whatState = 0;
        bird.direction = _Player.transform.position - Bird_Spawn[Doors.GetInteger("Door")].transform.position;
        bird.PlayerPosition = _Player.transform.position;
        bird.Front = (int)Front;
        bird.direction.Normalize();
        bird.This_State = MonsterManager.MON_STATE.DIRECTATTACK;
        bird.entityValue = 0;
        bird.GetComponent<Animator>().SetInteger("WhatState", 0);
        


        yield return new WaitForSeconds(0.2f);
        FX.EffectPlay(FX.ENEMYS.CUCKOO_DOOR_CLOSE);
        Doors.SetBool("Open", false);
        Spawning = true;
        yield return null;
    }


    IEnumerator InDamage()
    {
        CanHit = false;
        Color OriginColor = GetComponent<SpriteRenderer>().color;
        yield return null;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = OriginColor;
        CanHit = true;
    }
}
