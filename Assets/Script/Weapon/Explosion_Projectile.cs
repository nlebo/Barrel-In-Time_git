using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Projectile : Projectile {

    public Animator Anim;
    bool Dead = false;
    public SoundManager FX;

    public void Start()
    {
        FX = GameManager.Instance.SoundManager;
    }

    public new void Update()
    {
        if (!Dead)
            base.Update();
    }

    // Use this for initialization
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            FX.EffectPlay(FX.SKILLS.BOMBBULLET_BOMB);
            collision.GetComponent<StateInfo>().Hit(Damage, PushValue, PushPosition);
            Anim.SetBool("Attack", true);
            Dead = true;
        }
        else if (collision.tag == "Obstacle")
        {
            FX.EffectPlay(FX.SKILLS.BOMBBULLET_BOMB);
            Anim.SetBool("Attack", true);
            Dead = true;
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
