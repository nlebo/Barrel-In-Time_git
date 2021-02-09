using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPUP : Passive {
    public int MaxHPUP_Value = 0;
    public int ATKUP_Value = 0;
	// Use this for initialization
	public override void Start () {
        base.Start();
	}

    public override void UseItem()
    {
        GameManager.Instance.LocalPlayer.MaxHP += MaxHPUP_Value;
        GameManager.Instance.LocalPlayer.ATK += ATKUP_Value;
    }
}
