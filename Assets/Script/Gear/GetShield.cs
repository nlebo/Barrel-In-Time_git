using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetShield : SkillManager {
    public int UP_ER;
    public GameObject Shield;
    public override void UseSkill()
    {

        Use = false;

        if (GameManager.Instance.LocalPlayer.ER != GameManager.Instance.LocalPlayer.MAX_ER)
            GameManager.Instance.LocalPlayer.ER_UP(UP_ER);

        else
            return;

        GameObject This = Instantiate(Shield, GameManager.Instance.LocalPlayer.transform.position,Quaternion.identity);
        This.transform.parent = GameManager.Instance.LocalPlayer.transform;
        FX.EffectPlay(FX.SKILLS.ARMOR);
        InventoryManager.Instance.GetCogs((int)-price);

        
    }
}
