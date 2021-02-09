using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Little_Doll : Passive
{
    public int ERUP_Value = 0;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "작은 인형";
        Short_Info = "당신이 저의 주인입니까?";
        Detail_Info = "습득 후, 전투 종료 시마다 12의 장갑 획득";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.LittleDoll = ERUP_Value;
    }
}