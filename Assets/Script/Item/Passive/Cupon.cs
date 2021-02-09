using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupon : Passive
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "땜장이 쿠폰";
        Short_Info = "시간은 금이라고, 친구!";
        Detail_Info = "시간 상점의 상품 하나를 무료로 구매할 수 있습니다.";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.HaveCupon = true;
    }
}