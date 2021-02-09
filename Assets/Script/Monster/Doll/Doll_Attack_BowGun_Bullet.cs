using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll_Attack_BowGun_Bullet : MonoBehaviour {

    public float Damage;
    public float Speed;
    public Doll Parent;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3( Speed  * Time.deltaTime,0));
    }

    //-------------------------------------------------------------------------
    // Use this for Check Crush
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<Player>().CanHit)
            {
                collision.GetComponent<Player>().Hit(Damage * ((100 + Parent.ATK)/ 100.0f));
                Destroy(this.gameObject);
            }
        }
    }
}
