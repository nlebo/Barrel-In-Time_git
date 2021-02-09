using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : ItemManager {

	// Use this for initialization
	 public override void Start () {
        ThisType = TYPE.PASSIVE;
        base.Start();
	}

    public override bool GetItem()
    {
        base.GetItem();
        if (!_Inventory.AlreadyHave(ItemCode))
        {
            if (_Inventory.GetPassive(this))
            {
                return true;
            }
        }
        else
            return false;

        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GetItem()) 
                Destroy(this.gameObject);
        }
    }
}
