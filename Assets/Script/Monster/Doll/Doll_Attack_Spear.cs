using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll_Attack_Spear : MonoBehaviour {

    Doll Parent;
    public float Damage;

	// Use this for initialization
	void Start () {
        Parent = transform.parent.parent.GetComponent<Doll>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //-------------------------------------------------------------------------
    // Use this for Check Crush
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(Damage);
            collision.GetComponent<Player>().Hit(Damage * ((100 + Parent.ATK) / 100.0f));
 
        }
    }


    //-------------------------------------------------------------------------
    // Use this for Destroy Obj
    public void End()
    {
        GetComponent<Animator>().SetBool("Attack", false);
    }
}
