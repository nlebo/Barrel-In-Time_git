using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponManager {

    public GameObject Projectile;
    public GameObject GunUI,Ammo6,Ammo9,Reload;
    public GameObject Basic_UI, Bullet_UI;
    public int MaxBullet;
    public float Reload_Time, Reload_Cost;
    public float Speed;
    public int Bullet = 0;
    public float CostPercent = 0;
    string ETC ="";

    bool IsReloading = false;
    float AttackTime;

    public override void Start()
    {
        base.Start();
       // GunUI = GameObject.Find("GunUI").transform.GetChild(0).gameObject;
        Ammo6 = GunUI.transform.GetChild(0).gameObject;
        Ammo9 = GunUI.transform.GetChild(1).gameObject;
        Reload = GunUI.transform.GetChild(2).gameObject;
        AttackTime = Time.time;
        CanAttack = true;

        for(int i = 0;i<6;i++)
        {
            if(i < Bullet)
            {
                Ammo6.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Ammo6.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        This_Ability = "공격력 : " + ATK + "\n" + "공격속도 : " + AttackSpeed + "\n투사체속도 : " + Speed + "\n최대총알갯수 : " + MaxBullet;

        Player = GameManager.Instance.LocalPlayer;
        IsAttatch = true;
        InventoryManager.Instance.GetWeapon(this);
        gameObject.SetActive(false);
    }

    public override void Update()
    {


        if (!IsAttatch || !IsUse)
            return;

        if(Change)
        {
            Change = false;
            return;
        }

        if (Controller.fire2)
        {
            StartAttack();
        }

        if (Controller.Reload && !IsReloading)
        {
            Reloading();
        }

        if (Controller.fire1)
        {
            if (OtherWeapon != null && !IsReloading )
            {
                IsUse = false;
                OtherWeapon.gameObject.SetActive(true);
                OtherWeapon.Equip();
                OtherWeapon.Attack = false;
                GunUI.SetActive(false);
                gameObject.SetActive(false);
                
            }
        }
        if (!CanAttack && !IsReloading)
        {
            if (Time.time - AttackTime >= AttackSpeed)
                CanAttack = true;
        }

    }

    public override void StartAttack()
    {
        if (Bullet <= 0 || !CanAttack)
        {
            if (Bullet <= 0)
                Reloading();

            return;
        }
        Bullet--;
        CanAttack = false;
        AttackTime = Time.time;


        Vector2 diff;
        diff = Controller.MousePosition - transform.GetChild(0).position;

        diff.Normalize();

        float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        FX.EffectPlay(FX.PCS.PC_GUNFIRE);
        Projectile a = Instantiate(Projectile, transform.GetChild(0).position, Quaternion.Euler(0f, 0, rot_z)).GetComponent<Projectile>();
        a.MoveSpeed = Speed;
        a.Damage = ATK * ((100 + Player.ATK) / 100);
        a.PushValue = PushValue;
        a.PushPosition = Player.transform.position;

        if (MaxBullet == 6)
            Ammo6.transform.GetChild(Bullet).gameObject.SetActive(false);
        else
            Ammo9.transform.GetChild(Bullet).gameObject.SetActive(false);

        
    }
    public override void Equip()
    {
        IsUse = true;
        GunUI.SetActive(true);
        Change = true;
        GameManager.Instance.MouseCursor.WhatCursor = 1;
        GameManager.Instance.MouseCursor.SetCursor();
    }
    public void MaxBulletUp()
    {
        MaxBullet = 9;
        Ammo6.SetActive(false);
        Ammo9.SetActive(true);

        for (int i = 0; i < MaxBullet; i++)
        {
            if (i < Bullet)
            {
                if (MaxBullet == 6)
                    Ammo6.transform.GetChild(i).gameObject.SetActive(true);
                else
                    Ammo9.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if (MaxBullet == 6)
                    Ammo6.transform.GetChild(i).gameObject.SetActive(false);
                else
                    Ammo9.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public override void AllowForce(int i)
    {
        switch (i)
        {
            case 0:
                ATK += 2;
                break;
            case 1:
                MaxBulletUp();
                break;
            case 2:
                Reload_Time -= 0.4f;
                break;
            case 3:
                CostPercent = 25;
                ATK -= 1;
                ETC = "\n재장전시 사용되는 부품 차감 : " + CostPercent + "%";
                break;
            case 4:
                AttackSpeed -= 0.1f;
                break;
        }
        This_Ability = "공격력 : " + ATK + "\n" + "공격속도 : " + AttackSpeed + "\n투사체속도 : " + Speed + "\n최대총알갯수 : " + MaxBullet + ETC;
        CanShow[i] = false;
    }
    

    public void Reloading()
    {
        
        if (InventoryManager.Instance.Cog < Reload_Cost - (Reload_Cost * ((CostPercent + Player.Cost_Percent) / 100)) || IsReloading)
            return;
        FX.EffectPlay(FX.PCS.PC_RELOAD);
        IsReloading = true;
        InventoryManager.Instance.GetCogs((int)-(Reload_Cost - (Reload_Cost * ((CostPercent + Player.Cost_Percent) / 100))));

        StartCoroutine(RELOADING());
    }

    IEnumerator RELOADING()
    {
        CanAttack = false;
        AttackTime = Time.time;
        Reload.SetActive(true);
        yield return new WaitForSeconds(Reload_Time);
        Reload.SetActive(false);
        Bullet = MaxBullet;
        for (int i = 0; i < MaxBullet; i++)
        {
            if (i < Bullet)
            {
                if (MaxBullet == 6)
                    Ammo6.transform.GetChild(i).gameObject.SetActive(true);
                else
                    Ammo9.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if (MaxBullet == 6)
                    Ammo6.transform.GetChild(i).gameObject.SetActive(false);
                else
                    Ammo9.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        CanAttack = true;
        IsReloading = false;
    }
}
