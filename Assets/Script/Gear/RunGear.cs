using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGear : SkillManager {

    public float Damage, Push,Speed;
    Player _Player;
    public GameObject Background;
    bool start = false;

    public override void Start()
    {
        _Player = GameManager.Instance.LocalPlayer;
        base.Start();
    }

    public override void Update()
    {
        if (!Use)
            return;

        if(Input.GetMouseButtonDown(0) && start == false)
        {
            InventoryManager.Instance.GetCogs((int)-price);
            _Player.RunGear = true;
            _Player.RunDamage = Damage;
            _Player.RunPush = Push;
            _Player.prevMS = _Player.MS;
            _Player.MS = Speed;
            _Player.GetComponent<CapsuleCollider2D>().isTrigger = true;
            Vector2 dir;
            dir = new Vector2(GameManager.Instance.InputManager.MousePosition.x, GameManager.Instance.InputManager.MousePosition.y) - new Vector2(_Player.transform.position.x, _Player.transform.position.y);
            dir.Normalize();

            _Player.RunDir = dir;
            start = true;
            
            StartCoroutine(Finish());
            GameManager.Instance.MouseCursor.WhatCursor = 0;
            GameManager.Instance.MouseCursor.SetCursor();
        }
        
    }

    public override IEnumerator Finish()
    {
        yield return new WaitForSeconds(0.01f);
        Background.SetActive(false);
        Use = false;
        while (_Player.RunGear)
        {
            
            yield return null;
        }

       
        start = false;

    }

    public override void UseSkill()
    {
        if(!start)
        Use = true;
        GameManager.Instance.MouseCursor.WhatCursor = 1;
        GameManager.Instance.MouseCursor.SetCursor();


    }
}
