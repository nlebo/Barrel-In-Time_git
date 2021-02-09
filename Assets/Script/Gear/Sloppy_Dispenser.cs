using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sloppy_Dispenser : SkillManager {
    public int durability;
    public float Speed;
    public float Damage;
    public float Push;
    public float AttackSpeed;
    public GameObject _Dispen;

    public override void UseSkill()
    {
        Use = false;

        if (GameManager.Instance.LocalPlayer.Dispenser)
            return;

        InventoryManager.Instance.GetCogs((int)-price);
        FX.EffectPlay(FX.SKILLS.TURRET_SET);
        Dispenser a = Instantiate(_Dispen, GameManager.Instance.LocalPlayer.transform.position, Quaternion.identity).GetComponent<Dispenser>();
        a.durability = durability;
        a.Speed = Speed;
        a.Damage = Damage;
        a.Push = Push;
        a.AttackSpeed = AttackSpeed;
    }

}
