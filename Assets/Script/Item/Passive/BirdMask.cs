using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMask : Passive
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "새 가면";
        Short_Info = "뻐꾸기의 친구!";
        Detail_Info = "대규모 습격이 더이상 일어나지 않음";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.HaveBirdMask = true;
    }
}
