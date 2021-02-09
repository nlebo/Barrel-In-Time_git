using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_Wheel : Passive {

    public int MoveSpeed_Value = 0;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "기차 바퀴";
        Short_Info = "철마는 달리고 싶습니다.";
        Detail_Info = "이동속도 30 증가";
    }

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.MS += MoveSpeed_Value;
    }
}
