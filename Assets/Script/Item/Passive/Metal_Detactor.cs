using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal_Detactor : Passive
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "금속 탐지기";
        Short_Info = "삐삐삐삐삐...";
        Detail_Info = "일반 방 클리어 시, 소모품이 출현할 확률 2배";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.HaveDetector = true;
    }
}
