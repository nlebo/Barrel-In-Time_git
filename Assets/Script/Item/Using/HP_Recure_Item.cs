using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Recure_Item : ItemManager {
    public int value = 0;
    bool Check = true;

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
        GameManager.Instance.LocalPlayer.HPUP(value);
        Destroy(this.gameObject);
    }

    public override bool BuyItem(int VALUE)
    {
        Clock _Clock = FindObjectOfType<Clock>();
        if (_Clock.Real_Sec + VALUE > 540 && GameManager.Instance.LocalPlayer.HP == GameManager.Instance.LocalPlayer.MaxHP)
            return false;


        if (ThisType == TYPE.ACTIVE)
            UseItem();
        else
        {
            if (GetItem())
            {
            }
            else
                return false;
        }

        if (!GameManager.Instance.LocalPlayer.HaveCupon)
            _Clock.Real_Sec += VALUE;
        else
            GameManager.Instance.LocalPlayer.HaveCupon = false;

        return true;
    }



}
