using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : StateInfo {

    public GameObject Dash_Effect;
    public GameObject HitImage;
    GameManager _GameManager;
    SoundManager FX;
    InputManager Controller;
    Rigidbody2D CharBody;
    Animator thisAnim;
    public GameObject Dead_Effect;
    bool CanMove;
    public bool ConnectMon;
    public bool CanHit;
    public int Lookat;
    public bool RunGear;
    public float RunDamage;
    public float RunPush;
    public Vector2 RunDir;
    public float prevMS;
    public Vector3 BossRoomPos;


    // value about Items
    public bool HaveLever = false;
    public bool Dispenser = false;
    public bool HaveCupon = false;
    public bool HaveRocket = false;
    public bool HaveDetector = false;
    public bool HaveMental = false;
    public bool HaveBirdMask = false;
    public int LittleDoll = 0;
    public float Cost_Percent = 0;
    public float Gear_Percent = 0;

    // value for Sounds
    public float MoveTime;


    // Use this for initialization
    new void Awake () {
        //Input.mousePosition = new Vector3(-1, 0, 0);

        //Cursor.visible = false;
        base.Awake();
        GameManager.Instance.LocalPlayer = this;
        _GameManager = GameManager.Instance;
        Controller = _GameManager.InputManager;
        Camera.main.transform.parent = transform.parent;
        CharBody = transform.GetComponent<Rigidbody2D>();
        thisAnim = GetComponentInChildren<Animator>();
        Lookat = 0;
        ConnectMon = false;
        CanMove = true;
        CanHit = true;
        FX = GameManager.Instance.SoundManager;
        MoveTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        //transform.Translate(new Vector3(Controller.Horizontal * Time.deltaTime * moveSpeed, Controller.Vertical * Time.deltaTime * moveSpeed));

        if(RunGear)
        {
            CanHit = false;
            Move();
            transform.parent.position += transform.position - transform.parent.position;
            transform.position = transform.parent.position;
            Instantiate(Dash_Effect, this.transform.position, this.transform.rotation);
            Flip();
            return;
        }

        if (GameManager.Instance.Timer.TimeSlow)
        {
            CharBody.velocity = Vector2.zero;
            transform.parent.position += transform.position - transform.parent.position;
            transform.position = transform.parent.position;

            if (!STOP)
                Flip();
            return;
        }

        Flip();

        if (CanMove)
        {
            if (Controller.dash)
                Dash();
            else
                Move();
            
        }

        transform.parent.position += transform.position - transform.parent.position;
        transform.position = transform.parent.position;

        
        thisAnim.SetFloat("LookAt", Lookat);

        if (Controller.Initialize || Controller.PlayerDead)
        {
            transform.position = new Vector3(0, 0, 0);
            HP = MaxHP;
            


        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            MaxHP += 100;
            HP = MaxHP;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if(MaxHP> 100)
            MaxHP -= 100;
            HP = MaxHP;
        }

        if (Input.GetKeyDown(KeyCode.End))
            transform.position = BossRoomPos;

    }

    //-----------------------------------------------------------------------------------
    // Use those for Moveation
    void Flip()
    {
        if (transform.position.x >= Controller.MousePosition.x)
        {
            if (Lookat != -1)
            {
                transform.rotation = new Quaternion(0, 180, 0, transform.rotation.w);

                Lookat = -1;
            }
        }
        else if (transform.position.x < Controller.MousePosition.x)
        {
            if (Lookat != 1)
            {
                transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
                Lookat = 1;
            }

        }
    }
    void Move()
    {
        
        if (RunGear)
            CharBody.velocity = new Vector2(RunDir.x * MS, RunDir.y * MS);
        else
            CharBody.velocity = new Vector2(Controller.Horizontal * MS, Controller.Vertical * MS);
        transform.GetChild(1).GetComponent<Animator>().SetBool("Move", true);
        
        if(Controller.Horizontal == 0 && Controller.Vertical == 0)
        {
            transform.GetChild(1).GetComponent<Animator>().SetBool("Move", false);
        }
        else if (Time.time - MoveTime >= 0.5f)
        {
            FX.EffectPlay(FX.PCS.PC_MOVE);
            MoveTime = Time.time;
        }

    }
    void Dash()
    {

        transform.GetChild(1).GetComponent<Animator>().SetBool("Move", false);
        FX.EffectPlay(FX.PCS.PC_DASH);
        Vector2 Direction = Controller.MousePosition - transform.parent.position;
        Direction.Normalize();


        CanMove = false;
        CanHit = false;
        CharBody.velocity = new Vector2(0,0);
        
        
        StartCoroutine(EDash(Direction));
    }
    IEnumerator EDash(Vector3 Direction)
    {
        float FTime = Time.deltaTime;
        float DTime = FTime;
        int count = 0;

        Vector2 move = new Vector2(Direction.x * DS, Direction.y * DS) * 5;
        while (DTime - FTime < 0.125f)
        {

            count++;

            if (count % 2 == 1)
                Instantiate(Dash_Effect, this.transform.position, this.transform.rotation);

            DTime += Time.deltaTime;

            if(CharBody.velocity == Vector2.zero)
            CharBody.velocity= move;
             yield return null;
        }


        CharBody.velocity = Vector2.zero;
        CanHit = true;

        CanMove = true;

        yield return null;
    }

    //-----------------------------------------------------------------------------------
    // Use those for TriggerCheck
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(RunGear)
        {
            if (collision.tag == "Monster")
            {
                collision.GetComponent<StateInfo>().Hit(RunDamage, RunPush, transform.position);
            }
            else if(collision.tag == "MonsterAttack")
            {
                Destroy(collision.gameObject);
            }
            else if (collision.transform.tag == "Obstacle")
            {
                RunGear = false;
                MS = prevMS;
                CanHit = true;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (RunGear)
    //    {
    //        if (collision.transform.tag == "Monster")
    //        {
    //            collision.transform.GetComponent<StateInfo>().Hit(RunDamage, RunPush, transform.position);
    //        }
    //        else if (collision.transform.tag == "Obstacle")
    //        {
    //            RunGear = false;
    //            MS = prevMS;
    //            CanHit = true;
    //        }
    //    }
    //}

    //-----------------------------------------------------------------------------------
    // Use this for Hit
    public override void Hit(float Damage)
    {
        if (CanHit)
        {
            StartCoroutine(TakeDamage());
            FX.EffectPlay(FX.PCS.PC_OUCH);
            if(HaveRocket &&HP + ER - Damage * Mathf.Pow((98 / 100.0f), ARMORY) <= 0)
            {
                HaveRocket = false;
                return;
            }

            ER -= Damage * Mathf.Pow((98/100.0f),ARMORY);
            if (ER <= 0)
            {
                HP += ER;
                ER = 0;
            }
        }
        if (HP <= 0) Death();

    }
    public override void Hit(float Damage, float Push, Vector3 Position)
    {
        if (CanHit)
        {
            Hit(Damage);
            if(this.gameObject.activeInHierarchy)
            StartCoroutine(Popping(Push, Position));
        }

    }
    public override void Death()
    {
        //Controller.PlayerDead = true;
        HitImage.SetActive(false);
        Instantiate(Dead_Effect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    IEnumerator TakeDamage()
    {
        
        SpriteRenderer Head;
        SpriteRenderer Body;
        Head = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Body = transform.GetChild(1).GetComponent<SpriteRenderer>();

            CanHit = false;
            float FTime = Time.deltaTime;
            float DTime = FTime;
            
            StartCoroutine(DamageCamera());

            while (DTime - FTime < 0.6f)
            {
                DTime += Time.deltaTime;

                Head.color = new Color(Head.color.r, Head.color.g, Head.color.b, 0.5f);
                Body.color = new Color(Body.color.r, Body.color.g, Body.color.b, 0.5f);

                yield return null;
            }

            Head.color = new Color(Head.color.r, Head.color.g, Head.color.b, 1);
            Body.color = new Color(Body.color.r, Body.color.g, Body.color.b, 1);
           
            yield return null;

        CanHit = true;
        yield return null;
    }
    IEnumerator DamageCamera()
    {
        HitImage.SetActive(true);
        Camera.main.transform.localPosition += new Vector3(0.25f, 0.25f, 0);
        yield return new WaitForSeconds(0.0125f);
        Camera.main.transform.localPosition -= new Vector3(0.25f, 0.25f, 0);
        yield return new WaitForSeconds(0.0125f);
        Camera.main.transform.localPosition -= new Vector3(0.25f, 0.25f, 0);
        yield return new WaitForSeconds(0.0125f);
        Camera.main.transform.localPosition += new Vector3(0.25f, 0.25f, 0);
        HitImage.SetActive(false);
    }
    IEnumerator Popping(float push, Vector3 Position)
    {
        yield return null;
        float PTime = Time.time;
        Vector3 dir = Position - transform.position;
        dir.Normalize();
        Ray2D ray = new Ray2D(transform.position, -dir);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, (push / 50) + 0.5f, 9);


        if (hit.collider == null || hit.collider.tag != "Obstacle")
            transform.Translate(new Vector3(-dir.x * Lookat, -dir.y, -dir.z) * (push / 50));

        yield return new WaitForSeconds(0.2f);

    }


    public float SeetAttackSpeed(float AttackSpeed)
    {
        return  (100 + AS) / 100.0f;
    }

}
