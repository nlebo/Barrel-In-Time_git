using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUGU_GUN : MonoBehaviour {

    public float Damage;
    public float moveSpeed;
    Animator Anim;
    bool Attack;

	// Use this for initialization
	void Start () {
        Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Attack)
            return;

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Attack)
        {
            if(collision.tag == "Player")
            {
                collision.GetComponent<Player>().Hit(Damage);
                GameManager.Instance.SoundManager.EffectPlay(GameManager.Instance.SoundManager.ENEMYS.CUCKOO_BOOM);
                Anim.SetBool("Bomb", true);
                Attack = true; ;
            }
            else if(collision.tag == "Obstacle")
            {
                GameManager.Instance.SoundManager.EffectPlay(GameManager.Instance.SoundManager.ENEMYS.CUCKOO_BOOM);
                Anim.SetBool("Bomb", true);
                Attack = true; ;
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<Player>().Hit(Damage);
            }
        }
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    
}

