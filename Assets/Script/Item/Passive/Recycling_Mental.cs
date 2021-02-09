using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycling_Mental : Passive
{

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "재활용 정신";
        Short_Info = "아끼고 나누고... 아끼고 아낍시다.";
        Detail_Info = "기어의 사용에 소모되는 부품이 25% 감소";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.HaveMental = true;
    }
}