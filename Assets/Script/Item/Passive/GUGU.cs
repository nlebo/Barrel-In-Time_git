using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUGU : Passive
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Name = "뻐꾹이";
        Short_Info = "뻐꾹!";
        Detail_Info = "습득 후,저주 중첩이 1 증가할 때마다\n위력,방어도,이동 속도,공격 속도가 3씩 증가";
    }

    public override void UseItem()
    {
        FindObjectOfType<CurseManager>().HaveGUGU = true;
    }
}
