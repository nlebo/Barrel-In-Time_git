using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buldozer : MonoBehaviour {

    public float Damage;
    public float PushValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().Hit(Damage,PushValue,transform.position);

        }
    }
}
