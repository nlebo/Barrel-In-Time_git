using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : WeaponManager {
    IEnumerator CAttack;
   
    public Sprite[] UP, UP2, UP3;
    public Sprite[] DOWN, DOWN2, DOWN3;
    public string ETC = "";
    int Si = 0, Sj = 0;

    public override void Start()
    {
        base.Start();
        CAttack = Attack1();
        Player = GameManager.Instance.LocalPlayer;
        IsAttatch = true;
        InventoryManager.Instance.GetWeapon(this);
        IsUse = true;
        OtherWeapon = transform.parent.GetChild(1).GetComponent<WeaponManager>();
        OtherWeapon.OtherWeapon = this;
    }
    public override void Update()
    {
        if (Anim.GetInteger("WeaponState") != Sj)
            Anim.SetInteger("WeaponState", Sj);
        base.Update();
    }

    public override void StartAttack()
    {
        AttackSpeed = GameManager.Instance.LocalPlayer.SeetAttackSpeed(AttackSpeed);
        FX.EffectPlay(FX.PCS.PC_SWING);

        if (Anim.GetFloat("State") == 0)
        {
            StopCoroutine(CAttack);
            CAttack = Attack1();
            StartCoroutine(CAttack);
        }
        else
        {

            StopCoroutine(CAttack);
            StartCoroutine(Attack2());

        }
    }
    public override void Equip()
    {
        IsUse = true;
        Change = true;
        GameManager.Instance.MouseCursor.WhatCursor = 0;
        GameManager.Instance.MouseCursor.SetCursor();
    }
    public override void SpawnEffect()
    {
        HitCheck child = Instantiate(Effect[Si], transform.position, transform.rotation).GetComponentInChildren<HitCheck>();
        child.Weapon = this;
    }

    public override void AllowForce(int i)
    {

        switch(i)
        {
            case 0:
                ATK += 2;
                AttackSpeed -= 0.1f;
                Up = UP2[Si];
                Down = DOWN2[Si];
                Sj = 1;
                
                CanShow[1] = true;
                if (Anim.GetFloat("State") == 1)
                {
                    GetComponent<SpriteRenderer>().sprite = Down;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = Up;
                }

                break;
            case 1:
                ATK += 3;
                AttackSpeed -= 0.2f;
                AttackSpeed -= 0.1f;
                Anim.SetInteger("WeaponState", 2);
                Sj = 2;
                Up = UP3[Si];
                Down = DOWN3[Si];
                if (Anim.GetFloat("State") == 1)
                {
                    GetComponent<SpriteRenderer>().sprite = Down;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = Up;
                }
                break;
            case 2:
                Si = 1;
                Anim.SetInteger("WeaponState", 3);

                if (Sj == 0)
                {
                    Up = UP[Si];
                    Down = DOWN[Si];
                }else if (Sj == 1)
                {
                    Up = UP2[Si];
                    Down = DOWN2[Si];
                }
                else if (Sj == 2)
                {
                    Up = UP3[Si];
                    Down = DOWN3[Si];
                }
                if (Anim.GetFloat("State") == 1)
                {
                    GetComponent<SpriteRenderer>().sprite = Down;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = Up;
                }
                AttackSpeed -= 0.2f;
                break;
            case 3:
                AttackSpeed += 0.4f;
                CanShow[4] = true;
                break;
            case 4:
                AttackSpeed += 0.6f;
                break;
            case 5:
                InventoryManager.Instance.cogUp = 2;
                ETC = "\n 적 처치 시 부품2 추가 획득";
                break;
        }
        This_Ability = "공격력 : " + ATK + "\n" + "공격속도 : " + AttackSpeed + ETC;
        CanShow[i] = false;

    }

    IEnumerator Attack1()
    {


        if (!CameraShake)
        {
            StartCoroutine(CameraEffect());
        }

        if (!Attack)
        {
            Attack = true;
            Anim.SetBool("Attack", Attack);
            Anim.SetFloat("State", 1);
            Anim.speed = AttackSpeed;
            GetComponent<SpriteRenderer>().sprite = Down;

            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }



        yield return new WaitForSeconds(2f);

        GetComponent<SpriteRenderer>().sprite = Up;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        Anim.SetFloat("State", 0);
        StopCoroutine(CAttack);

    }
    IEnumerator Attack2()
    {

        if (!CameraShake)
        {
            StartCoroutine(CameraEffect());
        }

        if (!Attack)
        {
            Attack = true;
            Anim.SetBool("Attack", Attack);
            Anim.SetFloat("State", 0);
            Anim.speed = AttackSpeed;
            GetComponent<SpriteRenderer>().sprite = Up;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        yield return null;

    }
}
