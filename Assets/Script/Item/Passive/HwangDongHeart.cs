using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HwangDongHeart : Passive
{
    public int MaxHPUP_Value = 0;
    public int HPUP_Vaule = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "황동 심장";
        Short_Info = "두근거립니다. 아니, 철컥거린다는 표현이 맞을까요?";
        Detail_Info = "습득 시 생명력 50 회복, 최대 생명력 25 증가";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.MaxHP += MaxHPUP_Value;
        GameManager.Instance.LocalPlayer.HPUP(HPUP_Vaule);
    }
}
