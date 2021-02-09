using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogUp_Item : ItemManager {

    public int value = 0;
    bool Check = true;

	// Use this for initialization

 private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Check)
                UseItem();
               
            Check = false;
            
        }
    }

    public override void UseItem()
    {
        FX.EffectPlay(FX.SYSTEMS.SYSTEM_GETITEMS);
        InventoryManager.Instance.GetCogs(value);
        Destroy(this.gameObject);
    }

}
