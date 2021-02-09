using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandfather_Clock : MonsterManager {


    bool isDashAttack, isJumpAttack;
    public Transform LeftFoot, RightFoot;
    public GameObject projectileA,projectileB;
    public GameObject Shadow;
    public int DashProjectileCount, JumpProjectileCount;
    Animator Anim;

	// Use this for initialization
	new void Start () {
        isDashAttack = true;
        isJumpAttack = false;
        Anim = GetComponent<Animator>();
        base.Start();	
	}
	
	// Update is called once per frame
	new void Update () {
        ThisTime = Time.time;

        


        if (!attack)
        {
            if (ThisTime - AttackTime >= AR)
            {

                attack = true;
            }
        }

        This_State = MON_STATE.ATTACK;

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

            case MON_STATE.DIRECTATTACK:
                STATE_DIRECTATTACK();
                break;
        }
    }

    public override void STATE_ATTACK()
    {
        //AttackTime = Time.time;
        if (!attack)
            return;

        Attack();
    }

    void Attack()
    {
        if (!isJumpAttack && !isDashAttack)
        {
            StartCoroutine(DashAttack());
        }
        else if (!isJumpAttack)
        {
            StartCoroutine(JumpAttack());
        }

        
    }

    void SpawnProjectileLeftFoot()
    {
        for(int i =0;i<DashProjectileCount;i++)
        {
            Instantiate(projectileA, LeftFoot.position, Quaternion.Euler(0, 0, (float)Random.Range(0, 360)));
        }
    }

    void SpawnProjectileRightFoot()
    {
        for (int i = 0; i < DashProjectileCount; i++)
        {
            Instantiate(projectileA, RightFoot.position, Quaternion.Euler(0, 0, (float)Random.Range(0, 360)));
        }
    }

    void SpawnProjectileJump()
    {
        float Degree = 360.0f / JumpProjectileCount;

        for(int i=0;i<JumpProjectileCount;i++)
        {
            Instantiate(projectileB, transform.position, Quaternion.Euler(0, 0, Degree * i ));
        }
    }

    IEnumerator JumpAttack()
    {
        isJumpAttack = true;
        Anim.SetBool("Jump",true);
        yield return new WaitForSeconds(0.5f);
        FX.EffectPlay(FX.ENEMYS.KNIGHT_JUMP);

        yield return new WaitForSeconds(0.3f);
        
        float Scale_X, Scale_Y;
        Scale_X = Shadow.transform.localScale.x;
        Scale_Y = Shadow.transform.localScale.y;
        while (Shadow.transform.localScale.x > 0)
        {
            Shadow.transform.localScale -= new Vector3(Mathf.Lerp(0, Scale_X, Time.deltaTime / 0.3f), Mathf.Lerp(0,Scale_Y, Time.deltaTime / 0.3f), 1);
            //Shadow.transform.localScale -= new Vector3(0.5f, 0.5f, 1);
            yield return null;
        }
        Shadow.transform.localScale = new Vector3(0, 0, 1);
        transform.position += new Vector3(0,30,0);

        yield return new WaitForSeconds(3);
        Vector3 Pos = _Player.transform.position;
        
        Anim.SetBool("Jump", false);
        
        

        yield return new WaitForSeconds(1);

        transform.position = Pos;
        while (Shadow.transform.localScale.x < Scale_X)
        {
            Shadow.transform.localScale += new Vector3(Mathf.Lerp(0, Scale_X, Time.deltaTime / 0.5f), Mathf.Lerp(0, Scale_Y, Time.deltaTime / 0.5f), 1);
            yield return null;
        }
        Shadow.transform.localScale = new Vector3(Scale_X, Scale_Y, 1);
        Anim.SetBool("Attack", true);
        FX.EffectPlay(FX.ENEMYS.KNIGHT_ATTACK_01);
        yield return null;
        FX.EffectPlay(FX.ENEMYS.KNIGHT_ATTACK_02);
        Anim.SetBool("Attack", false);
        yield return new WaitForSeconds(0.5f);
        AttackTime = Time.time;
        attack = false;
        isJumpAttack = false;
        isDashAttack = false;
    }

    IEnumerator DashAttack()
    {
        isDashAttack = true;
        isJumpAttack = true;
       //FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y / 2 + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        Destroy(Ready);

        Anim.SetBool("Move", true);
        yield return null;
        
        float ThisTime = Time.time;

        while(Time.time - ThisTime <= 4)
        {
            direction = _Player.transform.position - transform.position;
            if (direction.magnitude >= 0.5f)
            {
                direction.Normalize();

                Ray2D ray = new Ray2D(transform.position, direction);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, MS * Time.deltaTime, 9);

                Ray2D ray1 = new Ray2D(transform.position + Vector3.up * GetComponent<SpriteRenderer>().size.y, direction);
                RaycastHit2D hit1 = Physics2D.Raycast(ray.origin, ray.direction, MS * Time.deltaTime, 9);

                Ray2D ray2 = new Ray2D(transform.position - Vector3.up * GetComponent<SpriteRenderer>().size.y, direction);
                RaycastHit2D hit2 = Physics2D.Raycast(ray.origin, ray.direction, MS * Time.deltaTime, 9);

                if (hit.collider == null || hit.collider.tag != "Obstacle")
                    if (hit1.collider == null || hit1.collider.tag != "Obstacle")
                        if (hit2.collider == null || hit2.collider.tag != "Obstacle")
                            Move(1);
            }
            yield return null;
        }
        Anim.SetBool("Move", false);
        isJumpAttack = false;

        yield return new WaitForSeconds(0.2f);
    }

    public void SOUND()
    {
        FX.EffectPlay(FX.ENEMYS.KNIGHT_MOVE);
    }
}
