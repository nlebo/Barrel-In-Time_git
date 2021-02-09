using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith_Attack_1_1 : MonoBehaviour {
    public float Damage;
    public float Speed;
    public float lifeTime;
    public float PrevTime;
	// Use this for initialization
	void Start () {
        PrevTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, Speed * 0.5f * Time.deltaTime));
        if (Time.time - PrevTime >= lifeTime)
            Destroy(this.gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
        if( collision.tag == "Player")
        {
			if (collision.GetComponent<Player> ().CanHit) {
				collision.GetComponent<Player> ().Hit (Damage);
				Destroy (this.gameObject);
			}
        }
    }
}
