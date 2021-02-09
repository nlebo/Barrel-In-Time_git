using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPanel : Passive
{
    public int ASUP_VALUE = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "고장난 계기판";
        Short_Info = "오우! 속도 최대로!";
        Detail_Info = "공격 속도 20 증가";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.AS += ASUP_VALUE;
    }
}
