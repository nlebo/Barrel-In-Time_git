using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith_Attack1 : MonoBehaviour {

    public float Damage;
    public float Speed;
    public GameObject Bullet;
    public Vector3 Direction;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        

        Vector3 move = new Vector3(Direction.x, Direction.y);
        transform.position += move* Time.deltaTime * Speed;

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            for(int i=0; i<8;i++)
            {
                Instantiate(Bullet, transform.GetChild(i).position, transform.GetChild(i).rotation);
            }
            Destroy(this.gameObject);
        }
        if (collision.tag == "Player")
        {

			if (collision.GetComponent<Player> ().CanHit) {
				collision.GetComponent<StateInfo> ().Hit (Damage);
				Destroy (this.gameObject);
			}
        }
    }
}
