using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCape : Passive
{
    public int DSUP_Value = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "붉은 망토";
        Short_Info = "멋진 망토입니다. 휘날리면 더욱 멋집니다.";
        Detail_Info = "대쉬 거리 30 증가";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.DS += DSUP_Value;
    }
}
