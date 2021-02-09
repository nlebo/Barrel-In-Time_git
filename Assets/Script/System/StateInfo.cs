using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateInfo : MonoBehaviour {

    public float HP, MaxHP;
    public float ATK, CRI, CP;
    public float AR, AP, ER;
    public float AS, MS, DS;
    public float ARMORY;
    public float MAX_ER;
    public bool STOP = true;

    public bool Alive = true;
	// Use this for initialization
	protected void Awake () {
        HP = MaxHP;
	}
	
    virtual public void Hit(float Damage)
    { 
        HP -= Damage;
        if (HP <= 0) Death();

    }

    virtual public void Hit(float Damage,float Push,Vector3 Position)
    {


    }

    virtual public void Death()
    {
        Alive = false;
      
        Destroy(this.gameObject);
        
    }

    public void HPUP(int value)
    {
        HP += value;
        if (HP >= MaxHP)
            HP = MaxHP;
    }

    public void MAXER_UP(int value)
    {
        MAX_ER += value;
        ER = MAX_ER;
    }

    public void ER_UP(int value)
    {
        ER += value;
        if (ER > MAX_ER)
            ER = MAX_ER;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
