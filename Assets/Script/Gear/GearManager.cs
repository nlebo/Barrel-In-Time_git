using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GearManager : MonoBehaviour {

    public GameObject GearUI;
    public GameObject BackGround;
    public GameObject[] Arrows;
    public InputManager Controller;
    public Timer _Timer;
    public Button[] Gears;
    public Button[] Gears_Name;
    public Button[] Gears_Middle;
    public Button Gears_INFO;
    public SkillManager[] Skills;
    public string[] Gears_Info;
    public int[] Price;
    public Text[] Info;
    public SoundManager FX;



    SkillManager USE_SKILL;
    bool UseGear = false;
	// Use this for initialization
	void Start () {
        Controller = GameManager.Instance.InputManager;
        _Timer = GameManager.Instance.Timer;
        FX = GameManager.Instance.SoundManager;
	}
	
	// Update is called once per frame
	void Update () {
        if (_Timer.TimeStop)
            return;
		if(Controller.Gear)
        {
            if (!UseGear)
            {
                GearUI.SetActive(!GearUI.activeInHierarchy);
                if (!GearUI.activeInHierarchy)
                {
                    
                    for (int i = 0; i < Gears_Name.Length; i++)
                    {
                        Gears_Name[i].interactable = false;
                        Arrows[i].SetActive(false);

                        Gears_Middle[i].interactable = false;
                    }
                    Info[0].gameObject.SetActive(false);
                    Info[1].gameObject.SetActive(false);
                    Gears_INFO.interactable = false;
                }
                else
                {
                    FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
                    GameManager.Instance.MouseCursor.WhatCursor = 0;
                    GameManager.Instance.MouseCursor.SetCursor();
                }
                BackGround.SetActive(GearUI.activeInHierarchy);

                _Timer.TimeSlow = GearUI.activeInHierarchy;
            }
        }

        if(UseGear)
        {
            if(!USE_SKILL.Use)
            {
                BackGround.SetActive(false);
                _Timer.TimeSlow = GearUI.activeInHierarchy;
                UseGear = false;
            }
        }
        

        if (!GearUI.activeInHierarchy)
            return;

    }

    public void Highlighted(int i)
    {
        FX.EffectPlay(FX.SYSTEMS.UI_CURSORON);
        Gears_Name[i].interactable = true;
        Arrows[i].SetActive(true);
        Gears_Middle[i].interactable = true;
        if(i + 1>= Gears_Middle.Length)
            Gears_Middle[0].interactable = true;
        else
            Gears_Middle[i+1].interactable = true;

        Info[0].text = Gears_Info[i];
        Info[1].text = "부품 소모 " + Price[i];
        Info[0].gameObject.SetActive(true);
        Info[1].gameObject.SetActive(true);
        Gears_INFO.interactable = true;
    }
    public void Normal(int i)
    {
        Gears_Name[i].interactable = false;
        Arrows[i].SetActive(false);
        Gears_Middle[i].interactable = false;
        if ( i+1 >= Gears_Middle.Length)
            Gears_Middle[0].interactable = false;
        else
            Gears_Middle[i+1].interactable = false;

        Info[0].gameObject.SetActive(false);
        Info[1].gameObject.SetActive(false);
        Gears_INFO.interactable = false;

    }
    public void Click_Gear(int i)
    {
        if (InventoryManager.Instance.Cog < Price[i])
        {
            FX.EffectPlay(FX.SYSTEMS.SYSTEM_FAIL);
            return;
        }
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        Skills[i].price = Price[i] - (Price[i] * (GameManager.Instance.LocalPlayer.Gear_Percent / 100));

        UseGear = true;
        GearUI.SetActive(false);
        Skills[i].UseSkill();
        USE_SKILL = Skills[i];
        Gears_Name[i].interactable = false;
        Info[0].gameObject.SetActive(false);
        Info[1].gameObject.SetActive(false);
    }


}
