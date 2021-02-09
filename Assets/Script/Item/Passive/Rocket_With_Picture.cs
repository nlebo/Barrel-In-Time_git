using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_With_Picture : Passive
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "사진이 든 로켓";
        Short_Info = "해치웠나?";
        Detail_Info = "사망에 이르는 피해를 입었을 때, 한 번만 피해를 무시";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.HaveRocket = true;
    }
}

