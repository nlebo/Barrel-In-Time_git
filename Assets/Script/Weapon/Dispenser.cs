using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour {

    public int durability;
    public float Speed;
    public float Damage;
    public float Push;
    public float AttackSpeed;
    public GameObject _Projectile;
    public Transform Gun_Spawn;
    public LayerMask Lay;
    public SoundManager FX;

    float SpawnTime;
    float AttackTime;
    Quaternion QDirection;
    float Rotate;
    Vector3 rayDirection;
    Ray2D ray;
    RaycastHit2D hit;
    public GameObject Monster;
    bool CanAttack;
    
    // Use this for initialization
    void Start () {
        rayDirection = new Vector2(1, 0);
        QDirection = new Quaternion(1, 0, 0, 0);
        SpawnTime = Time.time;
        CanAttack = true;
        Monster = null;
        Rotate = 360;
        FX = GameManager.Instance.SoundManager;
    }

    // Update is called once per frame
    void Update() {
        if (durability <= 0)
        {
            Destroy(this.gameObject);
            return;
        }

        if (Time.time - SpawnTime >= 1)
        {
            durability--;
            SpawnTime = Time.time;
        }

        if (!CanAttack)
        {
            if (Time.time - AttackTime >= AttackSpeed)
                CanAttack = true;
        }

        if (Monster == null)
        {
            QDirection = QDirection * Quaternion.Euler(0, 0, Rotate * Time.deltaTime * 4);
           
            rayDirection.x = QDirection.x;
            rayDirection.y = QDirection.y;
            rayDirection.Normalize();

            ray = new Ray2D(Gun_Spawn.position, rayDirection);
            hit = Physics2D.Raycast(ray.origin, ray.direction, 100, Lay);
            
            if (hit.collider != null)
                 Debug.Log(hit.collider.name);

            if (hit.collider != null && hit.collider.tag == "Monster")
                Monster = hit.collider.gameObject;
        }
        else
        {
            rayDirection = Monster.transform.position - transform.position;
            rayDirection.Normalize();
            Vector3 diff = Monster.transform.position - Gun_Spawn.position;
            diff.Normalize();


            hit = Physics2D.Raycast(ray.origin, rayDirection, 100, Lay);

            if (hit.collider != null &&  hit.collider.tag == "Obstacle")
            {
                Monster = null;
                return;
            }
            float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            float rot_z2 = (Mathf.Atan2(-rayDirection.y, -rayDirection.x) * Mathf.Rad2Deg);
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, rot_z2);
            if (CanAttack)
            {
                FX.EffectPlay(FX.SKILLS.TURRET_FIRE);
                Projectile a = Instantiate(_Projectile, Gun_Spawn.position, Quaternion.Euler(0f, 0, rot_z)).GetComponent<Projectile>();
                a.Damage = Damage;
                a.MoveSpeed = Speed;
                a.PushValue = Push;
                a.PushPosition = transform.position;
                CanAttack = false;
                AttackTime = Time.time;
            }  

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().Dispenser = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().Dispenser = false;
        }
    }
}
