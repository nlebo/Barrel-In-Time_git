using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Clock : MonoBehaviour {

    public GameObject  Min,CLOCK,EFFECT;
    public CurseManager Curse;
    public Image Gage_Time;
    public float Real_Sec, Sec,War_Sec;
    public float Band_Clock = 1;
    public Text Curse_Name, Curse_Remain, Game_Remain, Effect_Curse,Curse_Next,Help_UI;
    public SoundManager FX;
    bool CanGo, Shake, fill, Bomb;
    public bool Break = false;
    float PrevTime;

    IEnumerator a;

	// Use this for initialization
	void Start () {
        Real_Sec = 0;
        Sec = 0;
        CanGo = true;
        fill = false;
        Shake = false;
        Bomb = false;
        PrevTime = Time.time;
        FX = GameManager.Instance.SoundManager;
	}
	
	// Update is called once per frame
	void Update () {
        if(Break)
        {
            PrevTime = Time.time;
            return;
        }

        if (GameManager.Instance.InputManager.Initialize)
        {
            Real_Sec = 0;
            Sec = 0;
            War_Sec = 0;
            Curse.Curse = 0;
            Min.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //sec.transform.localRotation = Quaternion.Euler(0, 0, 0);
            PrevTime = Time.time;
        }

        if (Sec >= 542)
            return;

        if(Input.GetKey(KeyCode.PageUp))
        {
            Sec++;
            Real_Sec++;
        }

        Curse_Remain.text = (int)(90 - (Sec%90)) + "초";
        Game_Remain.text = (int)(540 - Sec) + "";

		if(Time.time - PrevTime >= 1 && Real_Sec < 540)
        {
            Real_Sec += 1 * Band_Clock;
            PrevTime = Time.time;
        }

        if ((90 - (Sec % 90)) <= 5)
            Shake = true;

        if (Bomb)
        {
            StartCoroutine(_EFFECT());
            Bomb = false;
        }

        if (CanGo)
            StartCoroutine(GoClock());

        
    }

    IEnumerator GoClock()
    {
        CanGo = false;
        yield return null;

        if (Real_Sec > Sec + 5)
        {
            Sec += (5 * Band_Clock);

            float PrevTime = Time.time;
            float minz;

            minz = -(Sec - 5) * 1 / 2;
          //  secz = -(Sec - 5) * 6;

            while (Time.time - PrevTime <= 0.01f)
            {

                Min.transform.localRotation = Quaternion.Euler(0, 0, minz - Mathf.Lerp(0, 1, (Time.time - PrevTime) / 0.01f) * 2 / (5*Band_Clock));
               // sec.transform.localRotation = Quaternion.Euler(0, 0, secz - Mathf.Lerp(0, 1, (Time.time - PrevTime) / 0.01f) * 6 * 5);
                yield return null;
            }
        }
        else if (Real_Sec > Sec)
        {
            FX.EffectPlay(FX.SYSTEMS.CLOCK_TICKING);
            Sec += 1 * Band_Clock;

            float PrevTime = Time.time;
            float minz;

            minz = -(Sec - 1) * 1/2;
            //secz = -(Sec -1 ) * 6;

            while (Time.time - PrevTime <= 0.01f)
            {
                
                Min.transform.localRotation = Quaternion.Euler(0, 0, minz - Mathf.Lerp(0,1, (Time.time - PrevTime) / 0.01f) * 2/ (1 * Band_Clock));
               // sec.transform.localRotation = Quaternion.Euler(0, 0, secz - Mathf.Lerp(0,1, (Time.time - PrevTime) / 0.01f) * 6);
                yield return null;
            }

        }

        if (Curse.Curse < 12)
        {
            if ((int)Sec / 90 > Curse.Curse)
            {
                FX.EffectPlay(FX.SYSTEMS.SYSTEM_CURSE);
                Curse.Curse = (int)(Sec / 90);
                Shake = false;
                Bomb = true;
                CLOCK.transform.localPosition = Vector3.zero;
                if (Curse.Curse == 1)
                {
                    EditCurse("점진적 성장\n날쌘 습격");
                    Curse.Spawn_GUGU = 10;
                    Curse.SPAWN_SONG = 0;
                    Curse.Plus_SpawnTime = 0.2f;
                }
                else if (Curse.Curse == 2)
                {
                    EditCurse("점진적 성장\n날쌘 습격\n노랫소리");
                    Curse_Next.text = "다음 저주까지 남은 시간";
                    Curse.SPAWN_SONG = 30;
                    Curse.Spawn_GUGU = 10;
                    Curse.Plus_SpawnTime = 0.2f;
                    Curse.Song_Level = 0;
                    Curse.Blind[0].SetActive(false);
                    Curse.Blind[1].SetActive(false);
                }
                else if (Curse.Curse == 3)
                {
                    EditCurse("점진적 성장\n사냥 시간\n노랫소리\n폭발성");
                    Curse_Next.text = "다음 저주까지 남은 시간";
                    Curse.Spawn_GUGU = 12;
                    Curse.Plus_SpawnTime = 0.4f;
                    Curse.SPAWN_SONG = 30;
                    Curse.SPAWN_BOMB = 20;
                    Curse.Song_Level = 0;
                    Curse.Blind[0].SetActive(false);
                    Curse.Blind[1].SetActive(false);
                }
                else if (Curse.Curse == 4)
                {
                    EditCurse("점진적 성장\n사냥 시간\n내장 오르골\n폭발성\n군체");
                    Curse_Next.text = "다음 저주까지 남은 시간";
                    Curse.Song_Level = 1;
                    Curse.Blind[0].SetActive(true);
                    Curse.Blind[1].SetActive(false);
                }
                else if (Curse.Curse == 5)
                {
                    EditCurse("점진적 성장\n공습\n내장 오르골\n초폭발\n군체");
                    Curse_Next.text = "마지막 저주까지 남은 시간";
                    Curse.Bomb_Level= 1;
                }
                else if (Curse.Curse == 6)
                {
                    EditCurse("완전체\n공습\n음향 증폭\n초폭발\n군단");
                    Curse_Next.text = "";
                    Curse_Remain.text = "";
                    Curse.Song_Level++;
                    Sec = 542;
                    Curse.Blind[1].SetActive(true);
                    Curse.Blind[0].SetActive(false);
                    StartCoroutine(_EFFECT());
                }

                if (Curse.HaveGUGU)
                {
                    Player _Player = GameManager.Instance.LocalPlayer;
                    _Player.ATK += 3;
                    _Player.ARMORY += 3;
                    _Player.AS += 3;
                    _Player.MS += 1;
                }
            }
            
        }
        
        if(Shake)
        {
            FX.LoopPlay(FX.SYSTEMS.TIME_HEARTBEAT);
            if (CLOCK.transform.localPosition == Vector3.zero)
                CLOCK.transform.localPosition += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            else
                CLOCK.transform.localPosition = Vector3.zero;
        }

        CanGo = true;
    }
    IEnumerator WarSec()
    {
        float PrevTime = Time.time;
        for(;;)
        {
            if (Time.time - PrevTime >= 1)
            {
                War_Sec += 1 + Curse.Plus_SpawnTime;
                PrevTime = Time.time;
                if (!fill)
                {
                    Gage_Time.fillAmount = War_Sec / 100.0f;
                    if (Gage_Time.fillAmount >= 1)
                        fill = true;
                }
              
                
            }

            if (fill)
            {
                Gage_Time.fillAmount = Gage_Time.fillAmount - (1 * Time.deltaTime);
                if (Gage_Time.fillAmount <= 0)
                    fill = false;
            }
            yield return null;
        }
    }
    IEnumerator _EFFECT()
    {
        FX.StopLoop();
        EFFECT.SetActive(true);

        EFFECT.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        EFFECT.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        yield return new WaitForSeconds(0.05f);
        EFFECT.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        EFFECT.transform.localScale = new Vector3(2, 2, 1);
        yield return new WaitForSeconds(0.05f);
        EFFECT.GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
        EFFECT.transform.localScale = new Vector3(2.5f, 2.5f, 1);
        yield return new WaitForSeconds(0.05f);
        EFFECT.GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        EFFECT.transform.localScale = new Vector3(2.75f, 2.75f, 1);
        yield return new WaitForSeconds(0.05f);
        EFFECT.GetComponent<Image>().color = new Color(1, 1, 1, 0.05f);
        EFFECT.transform.localScale = new Vector3(3, 3, 1);
        yield return new WaitForSeconds(0.05f);
        EFFECT.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        EFFECT.SetActive(false);

        Help_UI.gameObject.SetActive(true);

        switch ((int)Curse.Curse)
        {
            case 1:
                Help_UI.text = "새들이 당신을 눈치챘습니다.";
                break;
            case 2:
                Help_UI.text = "노랫소리가 들려옵니다.";
                break;
            case 3:
                Help_UI.text = "화약 냄새가 납니다.";
                break;
            case 4:
                Help_UI.text = "군단이 몰려듭니다.";
                break;
            case 5:
                Help_UI.text = "한계가 찾아옵니다.";
                break;
            case 6:
                Help_UI.text = "시간이 되었습니다.";
                Game_Remain.text =  "0";
                break;
        }

        float PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            Help_UI.color = new Color(Help_UI.color.r, Help_UI.color.g, Help_UI.color.b, Mathf.Lerp(0,1,(Time.time - PTime) * 2));
            yield return null;
        }
        yield return new WaitForSeconds(2);

        PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            Help_UI.color = new Color(Help_UI.color.r, Help_UI.color.g, Help_UI.color.b, Mathf.Lerp(1, 0, (Time.time - PTime) * 2));
            yield return null;
        }

        Help_UI.gameObject.SetActive(false);
    }

    public void War_Sec_On()
    {
        if (GameManager.Instance.LocalPlayer.HaveBirdMask)
            return;
        a = WarSec();
        StartCoroutine(a);
    }

    public void War_Sec_Off()
    {
        StopCoroutine(a);
    }

    public void AddCurse(string a)
    {
        Effect_Curse.text = Effect_Curse.text + "\n" + a;
    }

    public void EditCurse(string a)
    {
        Effect_Curse.text = a;
    }
}
