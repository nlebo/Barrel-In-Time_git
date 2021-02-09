using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break_Clock : SkillManager {

    public Clock _Clock;

    public override void UseSkill()
    {

        Use = false;
        if (_Clock.Break)
            return;

        InventoryManager.Instance.GetCogs((int)-price);
        _Clock.Break = true;
    }
}
