using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Bullet : SkillManager {

    public WeaponManager Gun;
    public GameObject Projection;
    public float Speed;
    public float PushValue;
    public float Damage;
    

    bool PrevActive = false;

	// Use this for initialization
	
	
	// Update is called once per frame
	public override void Update () {
        
        if (!Use)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            InventoryManager.Instance.GetCogs((int)-price);
            FX.EffectPlay(FX.SKILLS.BOMBBULLET_FIRE);
            Vector2 diff;
            diff = GameManager.Instance.InputManager.MousePosition - Gun.transform.GetChild(0).position;

            diff.Normalize();

            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

            Projectile a = Instantiate(Projection, Gun.transform.GetChild(0).position, Quaternion.Euler(0f, 0, rot_z)).GetComponent<Projectile>();
            a.MoveSpeed = Speed;
            a.Damage = Damage;
            a.PushValue = PushValue;
            a.PushPosition = GameManager.Instance.LocalPlayer.transform.position;

            Use = false;
            StartCoroutine(Finish());
            GameManager.Instance.MouseCursor.WhatCursor = 0;
            GameManager.Instance.MouseCursor.SetCursor();

        }
        else if(Input.GetMouseButtonDown(1))
        {
            Use = false;
            StartCoroutine(Finish());
            GameManager.Instance.MouseCursor.WhatCursor = 0;
            GameManager.Instance.MouseCursor.SetCursor();
        }
    }

    public override IEnumerator Finish()
    {
        yield return new WaitForSeconds(0.2f);
        Gun.OtherWeapon.IsUse = !PrevActive;
        Gun.OtherWeapon.gameObject.SetActive(!PrevActive);
        if (!PrevActive)
            Gun.OtherWeapon.Equip();
        Gun.gameObject.SetActive(PrevActive);
        Gun.STOP = true;
        Gun.Player.STOP = true;
        
    }

    public override void UseSkill()
    {
        PrevActive = Gun.IsUse;
        Gun.OtherWeapon.IsUse = false;
        Gun.OtherWeapon.gameObject.SetActive(false);
        Gun.gameObject.SetActive(true);
        Gun.STOP = false;
        Gun.Player.STOP = false;
        Use = true;

        GameManager.Instance.MouseCursor.WhatCursor = 1;
        GameManager.Instance.MouseCursor.SetCursor();
    }
}
