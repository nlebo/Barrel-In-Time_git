using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    public float price;
    public bool Use = false;
    public SoundManager FX;

    public virtual void Start()
    {
        FX = GameManager.Instance.SoundManager;
    }
    

    public virtual void Update() { }

    public virtual void UseSkill()
    {

    }

    public virtual IEnumerator Finish()
    {
        yield return null;
    }
}

