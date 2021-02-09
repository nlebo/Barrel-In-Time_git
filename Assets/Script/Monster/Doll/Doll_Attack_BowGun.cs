using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll_Attack_BowGun : MonoBehaviour {

    public GameObject bullet;

    public void Fire()
    {
        Transform Bullet;
        Bullet = Instantiate(bullet, transform.GetChild(0).position, transform.parent.rotation).transform;
        Bullet.GetComponent<Doll_Attack_BowGun_Bullet>().Parent = transform.parent.parent.GetComponent<Doll>();
    }
}
