using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour {

    public StateInfo Player;
    public WeaponManager Weapon;
	// Use this for initialization
	void Start () {
       Player = GameObject.FindWithTag("Player").GetComponent<StateInfo>();
       
	}
   
    void Delete()
    {
        Destroy(transform.parent.gameObject);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
