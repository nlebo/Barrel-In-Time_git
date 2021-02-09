using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Passive
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "지렛대";
        Short_Info = "빠루가 아닙니다.";
        Detail_Info = "습득 후, 적 처치 시마다 1의 장갑 획득";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.HaveLever = true;
    }
}