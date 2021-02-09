using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonsterManager {

    bool attacking;
    bool _ATTACK ;
    bool Boom;
    public int whatState;
    public int Plus = 0;
    int CNT = 0;
    float distance;
    public float AttackDistance;
    public GameObject _SONG;
	// Use this for initialization
	new void Start () {
        attacking = false;
        Boom = false;
        _ATTACK = true;
        whatState = Random.Range(1, 101);
        SelectState();
        base.Start();
	}
	
    void SelectState()
    {
        int Basic = 100 - (int)Curse.SPAWN_SONG - (int)Curse.SPAWN_BOMB;
        if (whatState <= Basic)
            whatState = 0;
        else if (whatState <= Basic + Curse.SPAWN_BOMB)
            whatState = 1;
        else
            whatState = 2;

        MaxHP += Curse.Curse * 3;
        HP = MaxHP;
        ATK += Curse.Curse * 8;
        AS += Curse.Curse * 0.12f;
        AR -= Curse.Curse * 0.4f;
        MS += Curse.Curse * 0.05f;
        GetComponent<Animator>().SetInteger("WhatState", whatState);
    }
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    
    public override void STATE_ATTACK()
    {
        AttackTime = Time.time;
        
        if (attack)
        {
            attack = false;
            if (!attacking)
            {
                attacking = true;

                if (whatState == 2)
                    StartCoroutine(Attack_02());
                else
                    StartCoroutine(Attack_01());
            }
        }


    }
    public override void STATE_DIRECTATTACK()
    {
        AttackTime = Time.time;

        if (!attacking)
        {
            StartCoroutine(Direct_Attack());
            attacking = true;
        }
        
    }
    public override void STATE_MISS()
    {
        if(Time.time - PrevTime >= 3)
        {
            //transform.position = _Player.transform.position;
            This_State = MON_STATE.IDLE;
            PrevTime = Time.time;
        }
        direction = PlayerPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        if (distance <= 0.5f)
        {
            This_State = MON_STATE.IDLE;
        }
        else
        {
            Move(1);
        }
    }
    public override void STATE_TRACKING()
    {
        CNT = 0;

        if (whatState == 2)
        {
            if (attack)
                This_State = MON_STATE.ATTACK;


            direction = PlayerPosition - transform.position;
            distance = direction.magnitude;
            Inverse_rayDirection = -direction.normalized;
            Ray2D InverseRay = new Ray2D(transform.position, Inverse_rayDirection);
            RaycastHit2D Inverse_Hit = Physics2D.Raycast(InverseRay.origin, InverseRay.direction, 1, 9);

            direction.Normalize();

            if (distance <= 3)
            {
                if (Inverse_Hit.collider == null)
                {
                    direction = -direction;
                    base.Move(0.8f);
                }
            }
            else if (distance >= 5)
            {
                base.Move(0.8f);
            }

            if (ThisTime - PrevTime >= 1.5f)
            {
                This_State = MON_STATE.MISS;
            }

            return;
        }

        direction = PlayerPosition - transform.position;

        distance = direction.magnitude;
        direction.Normalize();

        if (distance >= 4f)
            base.Move(1);
        else
        {
            if (attack)
                This_State = MON_STATE.ATTACK;
        }

        if (ThisTime - PrevTime >= 1.5f)
        {
            This_State = MON_STATE.MISS;
        }

    }
    public override void STATE_IDLE_MOVE()
    {
        raydistance = 100f;
        if (ThisTime - PrevTime >= moveTime)
        {
            PrevTime = Time.time;
            This_State = MON_STATE.IDLE;
        }
        PlayerPosition = transform.position + new Vector3(direction.x, direction.y);

        if (whatState == 2)
            base.Move(0.8f);
        else
            base.Move(1);
    }
    public override void STATE_IDLE()
    {
        if (ThisTime - PrevTime >= 5 || CNT >= 20)
        {
            transform.position = _Player.transform.position;
            CNT = 0;
            return;
        }

        if (whatState == 2)
        {
            if (ThisTime - PrevTime <= 3)
                return;

            raydistance = 5;
            if (hit.collider == null)
            {
                CNT++;
                direction = rayDirection;
                raydistance = 100;
                PrevTime = Time.time;
                moveTime = Random.Range(2, 4);
                This_State = MON_STATE.IDLE_MOVE;
            }

            return;
        }


        raydistance = 5;
        if (hit.collider == null)
        {
            direction = rayDirection;
            raydistance = 100;
            PrevTime = Time.time;
            moveTime = Random.Range(1, 2);
            This_State = MON_STATE.IDLE_MOVE;
        }
    }
    public override void Move(float Speed)
    {
        Vector2 Size = GetComponent<SpriteRenderer>().size;
        Ray2D ray = new Ray2D(transform.position, Vector3.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin + Vector2.up * (Size.y / 2), ray.direction, MS * Time.deltaTime * Speed, 9, 1);
        RaycastHit2D hit2 = Physics2D.Raycast(ray.origin - Vector2.up * (Size.y / 2), ray.direction, MS * Time.deltaTime * Speed, 9, 1);


        if (hit.collider == null || hit.collider.tag != "Obstacle")
            if (hit2.collider == null || hit2.collider.tag != "Obstacle")
                transform.Translate(Vector3.right * MS * Time.deltaTime * Speed);
            else
                Attach_Wall = true;
        else
            Attach_Wall = true;
    }

    //-------------------------------------------------------------
    // Use this for Attack
    IEnumerator Attack_01()
    {
        Attach_Wall = false;
        yield return null;
        float ThisTime = Time.time;
        
        
        direction = PlayerPosition - transform.position;
        Vector2 PPosition = transform.position;
       ////FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y / 2 + WaitAttack.GetComponent<SpriteRenderer>().size.y/2)), Quaternion.identity);
        Ready.transform.parent = transform;

        while (Time.time - ThisTime <= 1f)
        {
            
            yield return null;
        }

        Destroy(Ready);

        GetComponent<Animator>().SetBool("Attack", true);
        yield return null;
        ThisTime = Time.time;
        
        direction.Normalize();
        FX.EffectPlay(FX.ENEMYS.BIRD_RUSH);
        if (PlayerPosition.x > transform.position.x)
        {
            Vector2 diff;
            diff = PlayerPosition - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(0f, 0, rot_z);
            Front = 1;


        }
        else if (PlayerPosition.x <= transform.position.x)
        {

            Vector2 diff;
            diff = (PlayerPosition + (new Vector3(transform.position.x - PlayerPosition.x, 0, 0) * 2)) - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(0f, 180, rot_z);
            Front = -1;
        }


        yield return new WaitForSeconds(0.3f);
        _ATTACK = false;
        while ((PPosition - new Vector2(transform.position.x, transform.position.y)).magnitude <= AttackDistance + 0.2f && !_ATTACK)
        {
            Move(5);
            if(Attach_Wall)
            {
                Attach_Wall = false;
                break;
            }
            yield return null;
        }

        GetComponent<Animator>().SetBool("Attack", false);
        _ATTACK = true;
        yield return null;
        ThisTime = Time.time;

        while(Time.time - ThisTime <= 1)
        {
            yield return null;
        }


        attacking = false;
        AttackTime = Time.time;
        This_State = MON_STATE.IDLE;
        PrevTime = Time.time;
        
        yield return null;

    }
    IEnumerator Attack_02()
    {

        if (PlayerPosition.x > transform.position.x)
        {
            Vector2 diff;
            diff = PlayerPosition - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(0f, 0, rot_z);
            Front = 1;

            
        }
        else if (PlayerPosition.x <= transform.position.x)
        {

            Vector2 diff;
            diff = (PlayerPosition + (new Vector3(transform.position.x - PlayerPosition.x,0,0) * 2))  - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(0f, 180, rot_z);
            Front = -1;
        }

        //FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y / 2 + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        yield return new WaitForSeconds(1);
        Destroy(Ready);
        GetComponent<Animator>().SetBool("Attack", true);
        //음표 발사
        FX.EffectPlay(FX.ENEMYS.CUCKOO_SONG);
        direction = PlayerPosition - transform.position;
        direction.Normalize();


        Song a = Instantiate(_SONG, transform.position, transform.rotation).GetComponent<Song>();
        a.direction = direction;
        a.ATK = a.ATK * (100 + ATK) / 100;

        if (Curse.Song_Level >= 1 || Plus >= 1)
        {
            a = Instantiate(_SONG, transform.position,transform.rotation * Quaternion.Euler(0,0,20)).GetComponent<Song>();
            a.direction = direction;
            a.moveSpeed += 1;
            a.ATK = a.ATK * (100 + ATK) / 100;

            a = Instantiate(_SONG, transform.position, transform.rotation * Quaternion.Euler(0, 0, -20)).GetComponent<Song>();
            a.direction = direction;
            a.moveSpeed += 1;
            a.ATK = a.ATK * (100 + ATK) / 100;
        }

        if(Curse.Song_Level >= 2 || Plus >= 2)
        {
            a = Instantiate(_SONG, transform.position, transform.rotation * Quaternion.Euler(0, 0, 40)).GetComponent<Song>();
            a.direction = direction;
            a.moveSpeed += 2;
            a.ATK = a.ATK * (100 + ATK) / 100;


            a = Instantiate(_SONG, transform.position, transform.rotation * Quaternion.Euler(0, 0, -40)).GetComponent<Song>();
            a.direction = direction;
            a.moveSpeed += 2;
            a.ATK = a.ATK * (100 + ATK) / 100;
        }

        yield return new WaitForSeconds(0.3f);

        if (PlayerPosition.x > transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            Front = 1;


        }
        else if (PlayerPosition.x <= transform.position.x)
        {

            transform.localRotation = Quaternion.Euler(0f, 180, 0);
            Front = -1;
        }

        AttackTime = Time.time;
        attack = false;
        attacking = false;
        This_State = MON_STATE.IDLE;
        PrevTime = Time.time;
        GetComponent<Animator>().SetBool("Attack", false);
    }
    IEnumerator Direct_Attack()
    {
        float ThisTime = Time.time;


        if (PlayerPosition.x > transform.position.x)
        {
            Vector2 diff;
            diff = PlayerPosition - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(0f, 0, rot_z);
            Front = 1;


        }
        else if (PlayerPosition.x <= transform.position.x)
        {

            Vector2 diff;
            diff = (PlayerPosition + (new Vector3(transform.position.x - PlayerPosition.x, 0, 0) * 2)) - transform.position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(0f, 180, rot_z);
            Front = -1;
        }

        GetComponent<Animator>().SetBool("Attack", true);

        yield return new WaitForSeconds(0.3f);

        ThisTime = Time.time;

        direction.Normalize();

        if (PlayerPosition.x > transform.position.x && Front == -1)
        {
            transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
            Front = 1;
        }
        else if (PlayerPosition.x <= transform.position.x && Front == 1)
        {
            transform.rotation = new Quaternion(0, 180, 0, transform.rotation.w);
            Front = -1;
        }

        _ATTACK = false;
        while (Time.time - ThisTime <= 1.5f && !_ATTACK)
        {
            Move(5);
            yield return null;
        }

        GetComponent<Animator>().SetBool("Attack", false);
        _ATTACK = true;
        yield return null;
        ThisTime = Time.time;

        yield return new WaitForSeconds(1);

        attacking = false;
        AttackTime = Time.time;
        attack = false;
        This_State = MON_STATE.IDLE;
        PrevTime = Time.time;
        yield return null;

    }

    //--------------------------------------------------------------------------
    // Use this for Check Collider
    new private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (!_ATTACK)
        {
            if (collision.tag == "Obstacle")
            {
                _ATTACK = true;
                Hit(3);
                if (whatState == 1 && !Boom)
                {
                    Boom = true;
                    GetComponent<Animator>().SetBool("Boom", true);
                    FX.EffectPlay(FX.ENEMYS.CUCKOO_BOOM);
                    GetComponent<Animator>().SetFloat("BoomLevel", Curse.Bomb_Level); 
                }
                
            }
            else if (collision.tag == "Player")
            {
                collision.GetComponent<StateInfo>().Hit(15 * ((100 + ATK) / 100));
                if (whatState == 1 && !Boom)
                {
                    Boom = true;
                    GetComponent<Animator>().SetBool("Boom", true);
                    FX.EffectPlay(FX.ENEMYS.CUCKOO_BOOM);
                    GetComponent<Animator>().SetFloat("BoomLevel", Curse.Bomb_Level);
                }
            }
        }

        else if(Boom)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<StateInfo>().Hit(15 * ((100 + ATK) / 100));
            }
        }
    }
}
