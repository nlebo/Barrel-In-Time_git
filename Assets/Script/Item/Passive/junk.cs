using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class junk : Passive
{
    public int ERUP_Value = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "잡동사니";
        Short_Info = "그렇지만 누군가에겐 보물이겠죠.";
        Detail_Info = "습득 시 80의 장갑 즉시 획득";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.ER_UP(ERUP_Value);
    }
}
