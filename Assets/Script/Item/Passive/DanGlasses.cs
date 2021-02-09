using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanGlasses : Passive {
    public int ATKUP_Value = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "단안경";
        Short_Info = "모든 기계 정비사들의 필수품입니다.";
        Detail_Info = "위력 15 증가";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.ATK += ATKUP_Value;
    }
}
