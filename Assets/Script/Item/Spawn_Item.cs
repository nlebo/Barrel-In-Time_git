using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Item : MonoBehaviour {

    public float Mov = 12;
    public int value;
    Vector3 PlayerPosition;
    Vector3 direction;
    bool attach;
    public SoundManager FX;
	// Use this for initialization
	void Start () {
        PlayerPosition = GameManager.Instance.LocalPlayer.transform.position;
        attach = false;
        FX = GameManager.Instance.SoundManager;
        StartCoroutine(AwakeCoru());
	}
	


    IEnumerator AwakeCoru()
    {
        float PrevTime = Time.time;

        while(Time.time - PrevTime <= 0.2f)
        {
            transform.Translate(5 * Time.deltaTime, 0, 0);
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        GetComponent<CircleCollider2D>().enabled = true;
        while(!attach)
        {
            PlayerPosition = GameManager.Instance.LocalPlayer.transform.position;
            direction = PlayerPosition - transform.position;
            direction.Normalize();
            transform.position += direction * Time.deltaTime * Mov;

            yield return null;
        }

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        if (!attach)
    //        {
    //            InventoryManager.Instance.GetCogs(value);
                
    //        }

    //        attach = true;
    //        Destroy(this.gameObject);
    //    }
        
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!attach)
            {
                FX.EffectPlay(FX.SYSTEMS.SYSTEM_GETGEARS);
                InventoryManager.Instance.GetCogs(value);

            }

            attach = true;
            Destroy(this.gameObject);
        }
    }

}
