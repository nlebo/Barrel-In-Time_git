using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour {

    public enum State { MainWeapon= 0 ,SubWeapon};

    public GameObject[] Effect;
    public float ATK;
    public float PushValue;
    public Player Player;
    public bool Attack = false;
    public float AttackSpeed = 1;
    public State ThisState;
    public float[] Price;
    public WeaponManager OtherWeapon;
    public Sprite Up;
    public Sprite Down;
    public bool STOP = true;
    public bool CanAttack = false;
    [TextArea]
    public string[] Force_Name, Force_Ability;

    public bool[] CanShow;
    public string This_Ability;
    
    protected InputManager Controller;
    protected Animator Anim;
    protected bool IsAttatch;
    protected bool CameraShake;
    public bool Change;
    public bool IsUse = false;
    public SoundManager FX;

    // Use this for initialization
    public virtual void Start() {
        Controller = GameManager.Instance.InputManager;
        Anim = GetComponent<Animator>();
        IsAttatch = false;
        CameraShake = false;
        This_Ability = "공격력 : " + ATK + "\n" + "공격속도 : " + AttackSpeed;
        FX = GameManager.Instance.SoundManager;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(GameManager.Instance.Timer.TimeSlow)
            return;

        if (!IsAttatch||!IsUse)
            return;

        if (Change)
        {
            Change = false;
            return;
        }

        //transform.LookAt(Controller.MousePosition,new Vector3(0,0,transform.position.z));


        if (!Controller.MiniMap && Controller.fire1 && !Attack)
        {
            StartAttack();
        }

        if(Controller.fire2)
        {
           if(OtherWeapon!=null)
            {
                IsUse = false;
                OtherWeapon.gameObject.SetActive(true);
                OtherWeapon.Equip();
                gameObject.SetActive(false);
            }
        }

    }

    private void LateUpdate()
    {
        if (GameManager.Instance.Timer.TimeSlow && STOP)
            return;

        if (!IsAttatch)
            return;

        Vector2 diff = Controller.MousePosition - transform.position;
        if (Player.GetComponent<Player>().Lookat == -1)
            diff *= -1;

        diff.Normalize();

        float rot_z = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) * transform.parent.parent.GetComponent<Player>().Lookat;
        //if(rot_z>=-85 && rot_z <= -20)
        //    rot_z = -20;

        transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
    }


    //-------------------------------------------------------------------
    // Use those for Attack
    void FinishAttack()
    {
        Attack = false;

        Anim.SetBool("Attack", false);

        //Anim.SetFloat("State", 1);
    }
    public virtual void SpawnEffect()
    {

    }
    public virtual void StartAttack()
    {
        
    }
    public virtual void AllowForce(int i)
    {

    }
    public virtual void Equip()
    {
        
    }



    //-------------------------------------------------------------------
    // Use those Corutine for Attack
    
    protected IEnumerator CameraEffect()
    {
        CameraShake = true;

        yield return null;
        float PTime = Time.time;

        while(Time.time - PTime <= 0.08f)
        {
            if (Camera.main.transform.localPosition == new Vector3(0,0,-10))
                Camera.main.transform.position += new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
            else
                Camera.main.transform.localPosition = new Vector3(0, 0, -10);
            yield return null;
        }
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
        CameraShake = false;
    }
}
