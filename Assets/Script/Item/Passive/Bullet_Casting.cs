using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Casting : Passive
{
    public int COST_PERCENT;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "탄환 주물";
        Short_Info = "오직 탄환을 효율적으로 만들기 위해 만들어졌습니다.";
        Detail_Info = "탄창을 충전하는 데 소모되는 부품이 25% 감소";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.Cost_Percent += COST_PERCENT;
    }
}