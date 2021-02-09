using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour {

    public Vector3 direction;
    
    public float ATK;
    public float moveSpeed;
    public float cnt = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<StateInfo>().Hit(ATK);
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }

    }
}
