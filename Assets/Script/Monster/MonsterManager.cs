using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : StateInfo {

    public CurseManager Curse;
    public float entityValue;
    public float spawnFrequency;
	public float ItemSpawn;
    public Player _Player;
    public enum MON_STATE
    {
        IDLE=0,
        IDLE_MOVE,
        TRACKING,
        MISS,
        ATTACK,
        DIRECTATTACK,
    };
    public MON_STATE This_State = MON_STATE.IDLE;
    public GameObject[] Spawn_Item;
    public SoundManager FX;
    public GameObject WaitAttack;
    public GameObject Dead_Ani;
	public Vector2 direction;
    public Vector2 Map_Size;
	protected Vector3 rayDirection;
	protected Vector3 Inverse_rayDirection;
	protected Quaternion QDirection;
	public Vector3 PlayerPosition;
	protected float raydistance;
	public int Front;
	protected float Rotate;
	protected float ThisTime;
	protected float PrevTime;
	protected float AttackTime;
	protected bool attack;
    protected float moveTime;
	protected Ray2D ray;
	protected RaycastHit2D hit;
    protected bool Attach_Wall; 

    bool CanHit;

    public new void Awake()
    {
        base.Awake();
        Curse = FindObjectOfType<CurseManager>();
        
    }

    public void Start () {


		rayDirection = new Vector2(1, 0);
		QDirection = new Quaternion(1, 0, 0, 0);
		PlayerPosition = new Vector3(0, 0, 0);
		Front = 1;
        _Player = GameManager.Instance.LocalPlayer;
		Rotate = 360;
		PrevTime = Time.time;
		raydistance = 100;
		AttackTime = Time.time;
		attack = true;
		This_State = MON_STATE.IDLE;
        CanHit = true;
        FX = GameManager.Instance.SoundManager;
        Map_Size = transform.parent.parent.GetComponent<SpriteRenderer>().size;
    }

	// Update is called once per frame
	public void Update () {

        if (_Player == null)
            return;

        if (transform.localPosition.y >= Map_Size.y / 2 || transform.localPosition.y <= -Map_Size.y / 2
            || transform.localPosition.x >= Map_Size.x / 2 || transform.localPosition.x <= -Map_Size.x / 2)
            transform.position = _Player.transform.position;

        //Debug.Log(This_State);
        ThisTime = Time.time;

		QDirection = QDirection *  Quaternion.Euler(0, 0, Rotate * Time.deltaTime * 4);

		rayDirection.x = QDirection.x;
		rayDirection.y = QDirection.y;
		rayDirection.Normalize();
		Inverse_rayDirection = -rayDirection;

		ray = new Ray2D(transform.position ,rayDirection);

		hit = Physics2D.Raycast(ray.origin, ray.direction, raydistance,9);



		if (!attack && This_State != MON_STATE.ATTACK)
		{
			if ( ThisTime - AttackTime >= AR)
			{

				attack = true;
			}
		}



		if (hit.collider != null && hit.transform.tag == "Player" && This_State != MON_STATE.ATTACK && This_State != MON_STATE.DIRECTATTACK)
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

            case MON_STATE.DIRECTATTACK:
                STATE_DIRECTATTACK();
                break;


        }
	}

	public virtual void STATE_ATTACK (){}
    public virtual void STATE_DIRECTATTACK() { }
    public virtual void STATE_MISS (){}
	public virtual void STATE_TRACKING (){}
	public virtual void STATE_IDLE_MOVE (){}
	public virtual void STATE_IDLE (){}

	// Use those for TriggerCheck
	protected void OnTriggerEnter2D(Collider2D collision)
	{

  
		if (collision.tag == "HitBox")
		{
            HitCheck hit = collision.GetComponent<HitCheck>();
            if (CanHit)
                FX.EffectPlay(FX.PCS.PC_HIT);
			Hit(hit.Weapon.ATK * ((100 + hit.Player.ATK) / 100),hit.Weapon.PushValue,hit.Weapon.Player.transform.position);
		}
        

	}



    public override void Hit(float Damage)
	{
        if (CanHit)
        {
            base.Hit(Damage);
            StartCoroutine(InDamage());
        }
	}


    public override void Hit(float Damage, float Push, Vector3 Position)
    {
        if (CanHit)
        {
            base.Hit(Damage);
            StartCoroutine(InDamage());
            StartCoroutine(Popping(Push, Position));
        }

    }
    public override void Death()
	{
		Alive = false;
		this.transform.parent.parent.GetComponent<SpawnManager>().monsterCount--;
       
        StartCoroutine(Deading());

	}

	virtual public void Move(float Speed) {


        if (This_State != MON_STATE.ATTACK)
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



        Vector3 move = new Vector3(direction.x * Front, direction.y, 0);
        Vector2 Size = GetComponent<SpriteRenderer>().size;
        Ray2D ray = new Ray2D(transform.position, move.normalized);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin+ Vector2.up * (Size.y /2 ), ray.direction, MS * Time.deltaTime * Speed, 9,1);
        RaycastHit2D hit2 = Physics2D.Raycast(ray.origin - Vector2.up * (Size.y / 2), ray.direction, MS * Time.deltaTime * Speed, 9,1);


        if (hit.collider == null || hit.collider.tag != "Obstacle")
            if (hit2.collider == null || hit2.collider.tag != "Obstacle")
                transform.Translate(move * MS * Time.deltaTime * Speed);
            else
                Attach_Wall = true;
        else
            Attach_Wall = true;
    }



	IEnumerator InDamage()
	{
        CanHit = false;
        Color OriginColor = GetComponent<SpriteRenderer>().color;
		yield return null;
		GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
		yield return new WaitForSeconds(0.2f);
		GetComponent<SpriteRenderer>().color = OriginColor;
        CanHit = true;
	}
    IEnumerator Popping(float push,Vector3 Position)
    {
        yield return null;
        float PTime = Time.time;
        Vector3 dir = Position - transform.position;
        dir.Normalize();
        Ray2D ray = new Ray2D(transform.position, -dir);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 3, 9);


        if (hit.collider == null || hit.collider.tag != "Obstacle")
        {
            transform.Translate(new Vector3(-dir.x * Front, -dir.y, -dir.z) * (push / 50));
        }

        yield return new WaitForSeconds(0.2f);

    }
    IEnumerator Deading()
    {
        GameObject THIS = Instantiate(Dead_Ani, transform.position, Quaternion.identity);
        yield return null;
        FX.EffectPlay(FX.SYSTEMS.ENEMY_DEAD);

        if (_Player.HaveLever)
            _Player.ER_UP(1);

        if (Spawn_Item != null)
        {
            int CogUp = InventoryManager.Instance.cogUp;
            for (int i = 0; i < entityValue + CogUp; i++)
            {
                float a = Random.Range(0, 360);
                if (i + 5 <= entityValue + CogUp)
                {
                    Instantiate(Spawn_Item[1], transform.position, Quaternion.Euler(0, 0, a));
                    i += 4;
                }
                else
                    Instantiate(Spawn_Item[0], transform.position, Quaternion.Euler(0, 0, a));
            }
        }
        Destroy(this.gameObject);
        yield return null;
    }



}
