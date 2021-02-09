using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour {

    public Bird_Clock BOSS=null;
    public Image HP_Bar;
    static public BossHP Instance;


    // Use this for initialization

    private void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
        if (BOSS != null)
            HP_Bar.fillAmount = BOSS.HP / BOSS.MaxHP;
    }
}
