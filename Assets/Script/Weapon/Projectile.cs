using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float MoveSpeed;
    public float Damage;
    public float PushValue;
    public Vector3 PushPosition;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
        transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
	}

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            collision.GetComponent<StateInfo>().Hit(Damage,PushValue,PushPosition);
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }
}
