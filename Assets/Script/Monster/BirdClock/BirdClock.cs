using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdClock : MonsterManager {

    public GameObject child;
    Animator Anim;

    // Use this for initialization
    new void Start()
    {
        MaxHP += Curse.Curse * 5;
        HP = MaxHP;
        AR -= Curse.Curse * 0.5f;
        Anim = GetComponent<Animator>();
        base.Start();
    }

    new void Update()
    {
        //Debug.Log(This_State);
        ThisTime = Time.time;

        QDirection = QDirection * Quaternion.Euler(0, 0, Rotate * Time.deltaTime * 4);

        rayDirection.x = QDirection.x;
        rayDirection.y = QDirection.y;
        rayDirection.Normalize();
        Inverse_rayDirection = -rayDirection;

        ray = new Ray2D(transform.position, rayDirection);

        hit = Physics2D.Raycast(ray.origin, ray.direction, raydistance, 9);



        if (!attack && This_State != MON_STATE.ATTACK)
        {
            if (ThisTime - AttackTime >= AR)
            {

                attack = true;
            }
        }



        if (hit.collider != null && hit.transform.tag == "Player" && This_State != MON_STATE.ATTACK && attack)
        {

            PlayerPosition = hit.transform.position;
            This_State = MON_STATE.TRACKING;
            PrevTime = Time.time;


        }

        switch (This_State)
        {
            case MON_STATE.IDLE:
                STATE_IDLE();
                break;
            case MON_STATE.IDLE_MOVE:
                STATE_IDLE_MOVE();
                break;

            case MON_STATE.TRACKING:
                STATE_TRACKING();
                break;

            case MON_STATE.MISS:
                STATE_MISS();
                break;

            case MON_STATE.ATTACK:
                STATE_ATTACK();
                break;
        }
    }


    //-------------------------------------------------------------------------
    // Use those for Manage Monster
    public override void STATE_IDLE()
    {
        if (ThisTime - PrevTime >= 3)
        {
            raydistance = 10;
            if (hit.collider == null)
            {
                direction = rayDirection;
                raydistance = 100;
                PrevTime = Time.time;
                moveTime = Random.Range(2, 4);
                This_State = MON_STATE.IDLE_MOVE;
            }
        }
    }
    public override void STATE_IDLE_MOVE()
    {
        if (ThisTime - PrevTime >= moveTime)
        {
            PrevTime = Time.time;
            This_State = MON_STATE.IDLE;
        }
        PlayerPosition = transform.position + new Vector3(direction.x, direction.y);
        Move(1);
    }
    public override void STATE_TRACKING()
    {
        if (attack)
            This_State = MON_STATE.ATTACK;


    }
    public override void STATE_ATTACK()
    {
        AttackTime = Time.time;
        if (attack)
        {
            StartCoroutine(Attack_Coru());
            attack = false;
        }
    }

    IEnumerator Attack_Coru()
    {
        FX.EffectPlay(FX.ENEMYS.CUCKOO_CLOCK);
        yield return null;
        float PrevTime = Time.time;

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
        Anim.SetBool("Attack", true);
        FX.EffectPlay(FX.ENEMYS.CUCKOO_DOOR_OPEN);
        yield return new WaitForSeconds(0.1f);
        Anim.SetBool("Attack", false);
        Transform Child;
        Child = Instantiate(child, transform.position, transform.rotation).transform;
        Child.parent = transform.parent;
        transform.parent.parent.GetComponent<SpawnManager>().monsterCount++;
        yield return null;
        FX.EffectPlay(FX.ENEMYS.CUCKOO_DOOR_CLOSE);
        Bird Bird;
        Bird = Child.GetComponent<Bird>();
        yield return null;
        Bird.whatState = 0;
        Bird.direction = _Player.transform.position - transform.position;
        Bird.PlayerPosition = _Player.transform.position;
        Bird.Front = Front;
        Bird.direction.Normalize();
        Bird.GetComponent<Animator>().SetInteger("WhatState", 0);
        Bird.This_State = MON_STATE.DIRECTATTACK;
        Bird.entityValue = 0;


        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(0.3f);
        AttackTime = Time.time;
        This_State = MON_STATE.IDLE;

    }
}
