using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falcon : MonsterManager {
    bool attacking;
    bool _ATTACK;
    float distance;
    public float AttackDistance;
    int CNT = 0;
    // Use this for initialization
    new void Start () {
        attacking = false;
        _ATTACK = true;
        base.Start();
	}

    new void Update()
    {
        //Debug.Log(This_State);
        ThisTime = Time.time;

        QDirection = QDirection * Quaternion.Euler(0, 0, Rotate * Time.deltaTime * 4);

        rayDirection.x = QDirection.x;
        rayDirection.y = QDirection.y;
        rayDirection.Normalize();
        Inverse_rayDirection = -rayDirection;

        ray = new Ray2D(transform.position, rayDirection);

        hit = Physics2D.Raycast(ray.origin, ray.direction, raydistance, 9);



        if (!attack && This_State != MON_STATE.ATTACK)
        {
            if (ThisTime - AttackTime >= AR)
            {

                attack = true;
            }
        }



        if (hit.collider != null && hit.transform.tag == "Player" && This_State != MON_STATE.ATTACK && attack)
        {

            PlayerPosition = hit.transform.position;
            This_State = MON_STATE.TRACKING;
            PrevTime = Time.time;


        }

        switch (This_State)
        {
            case MON_STATE.IDLE:
                STATE_IDLE();
                break;
            case MON_STATE.IDLE_MOVE:
                STATE_IDLE_MOVE();
                break;

            case MON_STATE.TRACKING:
                STATE_TRACKING();
                break;

            case MON_STATE.MISS:
                STATE_MISS();
                break;

            case MON_STATE.ATTACK:
                STATE_ATTACK();
                break;
        }
    }

    //-------------------------------------------------------------------------
    // Use those for Manage Monster
    public override void STATE_IDLE()
    {
        if (ThisTime - PrevTime >= 5 || CNT>=10)
        {
            CNT = 0;
            transform.position = _Player.transform.position;
            return;
        }
        if (ThisTime - PrevTime >= 3)
        {
            raydistance =10;
            if (hit.collider == null)
            {
                direction = rayDirection;
                raydistance = 100;
                PrevTime = Time.time;
                moveTime = Random.Range(2, 4);
                This_State = MON_STATE.IDLE_MOVE;
                CNT++;
            }
        }
    }
    public override void STATE_IDLE_MOVE()
    {
        if (ThisTime - PrevTime >= moveTime)
        {
            PrevTime = Time.time;
            This_State = MON_STATE.IDLE;
        }
        PlayerPosition = transform.position + new Vector3(direction.x, direction.y);
        base.Move(1);
    }
    public override void STATE_TRACKING()
    {
        CNT = 0;
        if (attack)
        {
            This_State = MON_STATE.ATTACK;


            direction = PlayerPosition - transform.position;
            distance = direction.magnitude;
            Inverse_rayDirection = -direction.normalized;
            Ray2D InverseRay = new Ray2D(transform.position, Inverse_rayDirection);
            RaycastHit2D Inverse_Hit = Physics2D.Raycast(InverseRay.origin, InverseRay.direction, 1, 9);

            direction.Normalize();

            if (distance >= 4)
            {
                base.Move(1f);
            }
            else if (This_State != MON_STATE.ATTACK)
            {
                This_State = MON_STATE.IDLE;

            }
            else
            {
                if (PlayerPosition.x > transform.position.x && Front == -1)
                {
                    transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
                    Front = 1;
                }
                else if (PlayerPosition.x <= transform.position.x && Front == 1)
                {
                    transform.rotation = new Quaternion(0, 180, 0, transform.rotation.w);
                    Front = -1;
                }
            }
        }
        else
            This_State = MON_STATE.IDLE;

    }
    public override void STATE_ATTACK()
    {
        AttackTime = Time.time;
        if (attack)
        {
            StartCoroutine(AttackCoru());
            attack = false;
        }
    }
    public override void Move(float Speed)
    {
        Vector2 Size = GetComponent<SpriteRenderer>().size;
        Ray2D ray = new Ray2D(transform.position, Vector3.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin + Vector2.up * (Size.y / 2), ray.direction, MS * Time.deltaTime * Speed, 9, 1);
        RaycastHit2D hit2 = Physics2D.Raycast(ray.origin - Vector2.up * (Size.y / 2), ray.direction, MS * Time.deltaTime * Speed, 9, 1);


        if (hit.collider == null || hit.collider.tag != "Obstacle")
            if (hit2.collider == null || hit2.collider.tag != "Obstacle")
                transform.Translate(Vector3.right * MS * Time.deltaTime * Speed);
            else
                Attach_Wall = true;
        else
            Attach_Wall = true;

        
        //GetComponent<Rigidbody2D>().AddForce(Vector2.right * MS * Speed);
    }


    IEnumerator AttackCoru()
    {
        int Dash_Count = 3;
        Attach_Wall = false;
       //FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y / 2 + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        yield return new WaitForSeconds(1.5f);
        Destroy(Ready);
        GetComponent<Animator>().SetBool("Attack",true);
        yield return new WaitForSeconds(0.3f);
        while (Dash_Count > 0)
        {
            direction = PlayerPosition - transform.position;
            direction.Normalize();
            Vector2 PPosition = transform.position;
            float PrevTime = Time.time;

            if (PlayerPosition.x > transform.position.x)
            {
                Vector2 diff;
                diff = PlayerPosition - transform.position;

                diff.Normalize();

                float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                transform.localRotation = Quaternion.Euler(0f, 0, rot_z);
                Front = 1;


            }
            else if (PlayerPosition.x <= transform.position.x)
            {

                Vector2 diff;
                diff = (PlayerPosition + (new Vector3(transform.position.x - PlayerPosition.x, 0, 0) * 2)) - transform.position;

                diff.Normalize();

                float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                transform.localRotation = Quaternion.Euler(0f, 180, rot_z);
                Front = -1;
            }

            _ATTACK = false;
            FX.EffectPlay(FX.ENEMYS.HAWK_RUSH);
            while ((PPosition - new Vector2(transform.position.x,transform.position.y)).magnitude <= AttackDistance && !_ATTACK)
            {
                Move(6);
                if (Attach_Wall)
                {
                    Attach_Wall = false;
                    break;
                }
                yield return null;
            }

            Dash_Count--;
            _ATTACK = true;
            yield return new WaitForSeconds(0.2f);
            PlayerPosition = _Player.transform.position;
        }

        GetComponent<Animator>().SetBool("Attack", false);

        if (PlayerPosition.x > transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            Front = 1;


        }
        else if (PlayerPosition.x <= transform.position.x)
        {

            transform.localRotation = Quaternion.Euler(0f, 180, 0);
            Front = -1;
        }
        yield return new WaitForSeconds(2);

        AttackTime = Time.time;
        attack = false;
        This_State = MON_STATE.IDLE;
        PrevTime = Time.time;


        yield return null;
    }


    new private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (_ATTACK == false)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<StateInfo>().Hit(20);
            }

            if (collision.tag == "Obstacle")
            {
                _ATTACK = true;
                   
            }
        }
    }
}
