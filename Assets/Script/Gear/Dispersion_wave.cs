using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispersion_wave : SkillManager {

    public GameObject Dispersion;

    public override void UseSkill()
    {

        Use = false;
        InventoryManager.Instance.GetCogs((int)-price);
        FX.EffectPlay(FX.SKILLS.SHOCKWAVE);
        Instantiate(Dispersion,GameManager.Instance.LocalPlayer.transform.position,Quaternion.identity);
    }
}
