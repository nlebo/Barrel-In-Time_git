using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Band_Sceond_Hand : Passive
{
    public override void Start()
    {
        base.Start();
        Name = "휘어진 초침";
        Short_Info = "시계에서 떨어진 걸까요?";
        Detail_Info = "시간이 증가하는 속도가 초당 1에서 0.8으로 감소";
    }

    public override void UseItem()
    {
        FindObjectOfType<Clock>().Band_Clock = 0.8f;
    }
}
