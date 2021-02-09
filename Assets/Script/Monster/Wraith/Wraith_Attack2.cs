using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith_Attack2 : MonoBehaviour {
    public float Damage;
    public GameObject parent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (parent == null) Destroy(this.gameObject);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
			if (collision.GetComponent<Player> ().CanHit) {
				collision.GetComponent<StateInfo> ().Hit (Damage);
			}
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void Bomb()
    {
        GameManager.Instance.SoundManager.EffectPlay(GameManager.Instance.SoundManager.ENEMYS.CUCKOO_BOOM);
    }
}
