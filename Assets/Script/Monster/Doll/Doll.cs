using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll : MonsterManager {

	public GameObject[] Weapon = new GameObject[2];
    public Transform MyWeapon;
    Animator Anim;
    public Sprite[] Mask;
	int WhatWeapon = 0;
	int WhatColor = 0;

    //-------------------------------------------------------------------------
    // Use those for initialization
    new void Start () {
		base.Start ();
        Anim = GetComponent<Animator>();
		WhatWeapon = Random.Range (0, 2);
		WhatColor = Random.Range (0, 10);

		if (WhatColor <= 5)
			WhatColor = 0;
		else if (WhatColor <= 8)
			WhatColor = 1;
		else
			WhatColor = 2;

		InitialzeState ();
	}
	void InitialzeState()
	{
        switch (WhatColor)
        {

            case 0:
                Anim.SetInteger("WhatColor", WhatColor);
                GetComponent<SpriteRenderer>().sprite = Mask[0];
                MaxHP = 30;
                ATK = 0;
                AS = 0;
                AR = 3;
                MS = 2.15f;
                ItemSpawn = 5;
                break;
            case 1:
                Anim.SetInteger("WhatColor", WhatColor);
                GetComponent<SpriteRenderer>().sprite = Mask[1];
                MaxHP = 36;
                ATK = 10;
                AS = 10;
                AR = 2.7f;
                MS = 2.2f;
                ItemSpawn = 6;
                break;
            case 2:
                Anim.SetInteger("WhatColor", WhatColor);
                GetComponent<SpriteRenderer>().sprite = Mask[2];
                MaxHP = 42;
                ATK = 20;
                AS = 20;
                AR = 2.4f;
                MS = 2.25f;
                ItemSpawn = 7;
                break;
        }
        HP = MaxHP;
        MyWeapon = Instantiate(Weapon[WhatWeapon], transform.GetChild(0).position, Weapon[WhatWeapon].transform.rotation).transform;
        MyWeapon.parent = transform.GetChild(0);
        
	}

    //-------------------------------------------------------------------------
    // Use this for Update Per Frame
    new void Update()
	{
		base.Update ();
        if (_Player == null)
            return;
        if (This_State != MON_STATE.ATTACK)
        {
            Vector2 diff = _Player.transform.position - transform.GetChild(0).position;
            if (Front == -1)
                diff *= -1;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) * Front;
            transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z);
        }
    }


    //-------------------------------------------------------------------------
    // Use those for Manage Monster 
    public override void STATE_ATTACK()
    {
        AttackTime = Time.time;

        if (attack)
        {
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

            Vector2 diff = _Player.transform.position - transform.GetChild(0).position;
            if (Front == -1)
                diff *= -1;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) * Front;
            transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, rot_z);
            attack = false;

            if (WhatWeapon == 0)
            {
                
                StartCoroutine(Attack_01_Coru());
            }
            else
            {
                StartCoroutine(Attack_02_Coru());
            }
        }

    }
    public override void STATE_MISS() {
        direction = PlayerPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        if (distance <= 0.5f)
        {
            This_State = MON_STATE.IDLE;
        }
        else
        {
            if (WhatWeapon == 0)
                Move(1);
            else
                Move(0.8f);
        }
    }
    public override void STATE_TRACKING() {

       
        direction = PlayerPosition - transform.position;

        Inverse_rayDirection = -direction.normalized;
        Ray2D InverseRay = new Ray2D(transform.position, Inverse_rayDirection);
        RaycastHit2D Inverse_Hit = Physics2D.Raycast(InverseRay.origin, InverseRay.direction, 1, 9);



        float distance = direction.magnitude;
        direction.Normalize();

        switch(WhatWeapon)
        {
            case 0:
                if (distance <= 1.2f)
                {
                    if (attack)
                    {
                        This_State = MON_STATE.ATTACK;
                    }
                }
                else
                {
                    Move(1);
                }
                break;

            case 1:

                if (attack)
                    This_State = MON_STATE.ATTACK;

                if (distance <= 3)
                {
                    if (Inverse_Hit.collider == null)
                    {
                        direction = -direction;
                        Move(0.6f);
                    }
                }
                else if(distance >= 5)
                {
                    Move(0.6f);
                }
                break;
        }
       

         if (ThisTime - PrevTime >= 1.5f)
        {
            This_State = MON_STATE.MISS;
        }
        
        

       
    }
    public override void STATE_IDLE_MOVE() {
        if (ThisTime - PrevTime >= moveTime)
        {
            PrevTime = Time.time;
            This_State = MON_STATE.IDLE;
        }
        PlayerPosition = transform.position + new Vector3(direction.x, direction.y);
        Move(0.8f);
    }
    public override void STATE_IDLE() {
        if (ThisTime - PrevTime >= 3)
        {
            raydistance = 5;
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


    //-------------------------------------------------------------------------
    // Use those for Attack
    IEnumerator Attack_01_Coru()
    {
       //FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y / 2 + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        Destroy(Ready);
        float ThisTime = Time.time;
        FX.EffectPlay(FX.ENEMYS.DOLLS_SPEAR);
        MyWeapon.GetComponent<Animator>().SetBool("Attack", true);

        while (Time.time - ThisTime <= 0.1f)
        {
            Move(2.5f);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        AttackTime = Time.time;
        This_State = MON_STATE.IDLE;

    }
    IEnumerator Attack_02_Coru()
    {
       //FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y / 2 + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        Destroy(Ready);
        FX.EffectPlay(FX.ENEMYS.DOLLS_GUN);
        MyWeapon.GetComponent<Doll_Attack_BowGun>().Fire();
        AttackTime = Time.time;
        This_State = MON_STATE.IDLE;
    }

}
