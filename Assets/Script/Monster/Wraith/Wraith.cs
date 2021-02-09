using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith : MonsterManager {


    public GameObject Attack01;
    public GameObject Attack02;


    // Use this for initialization
    new  void Start () {
		base.Start ();
    }

	new void Update()
	{
		base.Update ();
	}


    //-------------------------------------------------------------------------
    // Use those for Manage Monster
    public override void STATE_IDLE ()
	{
		if(ThisTime - PrevTime  >= 3)
		{
			raydistance = 10;
			if(hit.collider == null)
			{
				direction = rayDirection;
				raydistance = 100;
				PrevTime = Time.time;
				moveTime = Random.Range(2, 4);
				This_State = MON_STATE.IDLE_MOVE;
			}
		}
	}
	public override void STATE_IDLE_MOVE ()
	{
		if(ThisTime-PrevTime >= moveTime)
		{
			PrevTime = Time.time;
			This_State = MON_STATE.IDLE;
		}
		PlayerPosition = transform.position + new Vector3(direction.x, direction.y);
		Move(1);
	}
	public override void STATE_TRACKING ()
	{
		if(attack)
			This_State = MON_STATE.ATTACK;


		direction = PlayerPosition - transform.position;
        float distance = direction.magnitude;
		Inverse_rayDirection = -direction.normalized;
		Ray2D InverseRay = new Ray2D(transform.position, Inverse_rayDirection);
		RaycastHit2D Inverse_Hit = Physics2D.Raycast(InverseRay.origin, InverseRay.direction, 1, 9);

		direction.Normalize();

        if (distance <= 3)
        {
            if (Inverse_Hit.collider == null)
            {
                direction = -direction;
                Move(0.6f);
            }
        }else if(distance >= 5)
        {
            Move(0.6f);
        }


	}
	public override void STATE_ATTACK ()
	{
		AttackTime = Time.time;
		if (attack)
		{
			if ((int)Random.Range(0, 2) == 0)
			{
				StartCoroutine(Attack_02_Coru());
			}
			else
			{
				StartCoroutine(Attack_O1_Coru());
			}
			attack = false;
		}
	}


    //-------------------------------------------------------------------------
    // Use thos
    public override void Hit(float Damage)
    {
        base.Hit(Damage);
    }
    public override void Move(float Speed)
    {
        base.Move(Speed);
    }

    public void Attack_01()
    {
        Transform Attack;
        Attack = Instantiate(Attack01, transform.position + ((_Player.transform.position - transform.position).normalized), Quaternion.identity).transform;
        Attack.GetComponent<Wraith_Attack1>().Direction = _Player.transform.position - transform.position;
        Attack.GetComponent<Wraith_Attack1>().Direction.Normalize();    
        Attack.parent = transform.parent;
    }

    IEnumerator Attack_02_Coru()
    {
        FX.EffectPlay(FX.ENEMYS.MAGE_ATTACK_02);
        //FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        Transform Attack;
        Attack = Instantiate(Attack02, _Player.transform.position + Attack02.transform.position, Quaternion.identity).transform;
        Attack.parent = transform.parent;
        Attack.GetComponent<Wraith_Attack2>().parent = gameObject;

        yield return new WaitForSeconds(0.5f);
        Destroy(Ready);
        yield return null;

        AttackTime = Time.time;
        This_State = MON_STATE.TRACKING;




    }
    IEnumerator Attack_O1_Coru()
    {
        yield return null;
       ////FX.EffectPlay(FX.SYSTEMS.ENEMY_READY);
        GameObject Ready = Instantiate(WaitAttack, transform.position + (Vector3.up * (GetComponent<SpriteRenderer>().size.y + WaitAttack.GetComponent<SpriteRenderer>().size.y / 2)), Quaternion.identity);
        Ready.transform.parent = transform;
        yield return new WaitForSeconds(0.5f);
        Destroy(Ready);
        FX.EffectPlay(FX.ENEMYS.MAGE_ATTACK_01);
        Attack_01();
        AttackTime = Time.time;
        This_State = MON_STATE.TRACKING;


    }
    
}
