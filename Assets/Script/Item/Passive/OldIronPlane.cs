using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldIronPlane : Passive
{
    public int ARMORYUP_Value = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "낡은 철판";
        Short_Info = "낡아 빠졌지만 옷 아래에 덧대기엔 충분합니다.";
        Detail_Info = "방어도 15 증가";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.ARMORY += ARMORYUP_Value;
    }
}
