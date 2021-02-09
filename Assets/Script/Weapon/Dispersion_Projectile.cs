using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispersion_Projectile : Projectile {

	// Use this for initialization
	void Start () {
        MoveSpeed = 24;
        Damage = 6;
        PushValue = 60;
        PushPosition = transform.parent.position;
        transform.localRotation = Quaternion.Euler(0, 0, transform.GetSiblingIndex() * 3);
	}


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(collision.tag == "MonsterAttack")
        {
            Destroy(collision.gameObject);
        }
    }
}
