using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public struct INVENTORY
    {
        public ItemManager item;
        public int cnt;
        public Sprite sprite;
        public ItemManager.TYPE ThisType;

        public void CountUp()
        {
            cnt++;
        }

        public INVENTORY(ItemManager ITEM,int _cnt, Sprite _sprite, ItemManager.TYPE _ThisType)
        {
            item = ITEM;
            cnt = _cnt;
            sprite = _sprite;
            ThisType = _ThisType;
        }
    }
    public struct WEAPONTORY
    {
        public WeaponManager Weapon;
        public Sprite sprite;

        public WEAPONTORY(WeaponManager WEAPON,Sprite _sprite)
        {
            Weapon = WEAPON;
            sprite = _sprite;
        }
    }

   
    public static InventoryManager Instance;
    public GameObject Inventory;
    public Transform[] Child;
    public Transform[] Erase;
    public int PassiveCOUNT;
    public int Cog,cogUp;
    public Text Name,Short_Info,Detail_Info;
    public Image Info_Sprite;

    public List<INVENTORY> Passives;
    public WEAPONTORY[] Weapons;
    public GameObject[] PassiveSlots;
    public GameObject[] WeaponSlots;
    public Text[] ForceSlots;
    public Text[] Force;
    WeaponManager weapon;
    bool Show;
    int SelectForce = 0;
    int SelectWeapon = 0;
    public SoundManager FX;
    // Use this for initialization

    private void Awake()
    {
        Instance = this;
        Passives = new List<INVENTORY>();
        Weapons = new WEAPONTORY[2];
    }
    void Start () {
        PassiveCOUNT = 0;
        cogUp = 0;
        FX = GameManager.Instance.SoundManager;
	}


    //Use this for Update
    public void Update()
    {
        if (GameManager.Instance.Timer.TimeSlow)
            return;

        if (GameManager.Instance.InputManager.Inventory)
        {

            Inventory.SetActive(!Inventory.activeInHierarchy);

            for (int i = 0; i < Child.Length; i++)
            {
                Child[i].gameObject.SetActive(false);
            }
            Child[0].gameObject.SetActive(true);

            if (!Inventory.activeInHierarchy)
            {
                Name.transform.parent.gameObject.SetActive(false);
                for (int i = 0; i < Erase.Length; i++)
                {
                    Erase[i].gameObject.SetActive(false);
                }
                GameManager.Instance.Timer.TimeStop = false;
            }
            else
            {
                FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
                GameManager.Instance.Timer.TimeStop = true;
            }
        }

        if(GameManager.Instance.InputManager.ESC)
        {
            if (Inventory.activeInHierarchy)
            {
                Name.transform.parent.gameObject.SetActive(false);
                Inventory.SetActive(false);
                GameManager.Instance.Timer.TimeStop = false;
                for (int i = 0; i < Erase.Length; i++)
                {
                    Erase[i].gameObject.SetActive(false);
                }
            }
            else
                Application.Quit();
        }

        if (Input.GetKey(KeyCode.Insert))
            GetCogs(10);
    }


    // Use those When Click Button
    public void OnClick_Equip()
    {
        for(int i = 0;i<Child.Length;i++)
        {
            Child[i].gameObject.SetActive(false);
        }
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        Child[0].gameObject.SetActive(true);
    }
    public void OnClick_Reinforce()
    {
        for (int i = 0; i < Child.Length; i++)
        {
            Child[i].gameObject.SetActive(false);
        }
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        Child[1].gameObject.SetActive(true);
    }
    public void OnClick_Gear()
    {
        for (int i = 0; i < Child.Length; i++)
        {
            Child[i].gameObject.SetActive(false);
        }
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        Child[2].gameObject.SetActive(true);
    }
    public void OnClick_Passive(int i)
    {
        ItemManager item = Passives[i].item;
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        Name.text = item.Name;
        Short_Info.text = item.Short_Info;
        Detail_Info.text = item.Detail_Info;
        Info_Sprite.sprite = Passives[i].sprite;

        Name.transform.parent.gameObject.SetActive(true);
    }
    public void OnClick_Weapon(int i)
    {
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        weapon = Weapons[i].Weapon;
        SelectWeapon = i;
        for(int j=0;j<6;j++)
        {
            if (j < weapon.Force_Name.Length)
            {
                ForceSlots[j].text = weapon.Force_Name[j] + "(부품 " + weapon.Price[j] + ")";

                if (weapon.CanShow[j])
                    ForceSlots[j].gameObject.SetActive(true);
                else
                    ForceSlots[j].gameObject.SetActive(false);
            }
            else
                ForceSlots[j].gameObject.SetActive(false);

        }
        Force[0].gameObject.SetActive(false);
        Force[1].gameObject.SetActive(false);
    }
    public void OnClick_Force(int i)
    {
        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        SelectForce = i;
        Force[0].text = weapon.This_Ability;
        Force[1].text = weapon.Force_Ability[i] + "\nCOST : " + weapon.Price[i];
        Force[0].gameObject.SetActive(true);
        Force[1].gameObject.SetActive(true);
    }
    public void OnClick_ForceAllow()
    {
        if (!Force[1].gameObject.activeInHierarchy)
            return;

        if (Cog < weapon.Price[SelectForce])
        {
            FX.EffectPlay(FX.SYSTEMS.SYSTEM_FAIL);
            return;
        }

        FX.EffectPlay(FX.SYSTEMS.UI_SELECT);
        GetCogs(-(int)weapon.Price[SelectForce]);
        weapon.AllowForce(SelectForce);

        for (int j = 0; j < weapon.Force_Name.Length; j++)
        {
            ForceSlots[j].text = weapon.Force_Name[j] + "(부품 " + weapon.Price[j] + ")"; ;

            if (weapon.CanShow[j])
                ForceSlots[j].gameObject.SetActive(true);
            else
                ForceSlots[j].gameObject.SetActive(false);
        }
        Force[0].gameObject.SetActive(false);
        Force[1].gameObject.SetActive(false);

        if (SelectWeapon == 0)
            SetWeaponSprite(weapon.Down, SelectWeapon);

    }
    public void SetWeaponSprite(Sprite _sprite, int i)
    {
        Weapons[i].sprite = _sprite;
        WeaponSlots[i].GetComponent<Image>().sprite = Weapons[i].sprite;
        WeaponSlots[i+2].GetComponent<Image>().sprite = Weapons[i].sprite;
    }

    public bool GetPassive(ItemManager _ITEM)
    {

        if (Passives.Count > 0)
        {
            if (AlreadyHave(_ITEM.ItemCode))
                return false;
        }

        if (Passives.Count >= PassiveSlots.Length)
            return false;

        INVENTORY i = new INVENTORY(_ITEM, 1, _ITEM.GetComponent<SpriteRenderer>().sprite,_ITEM.ThisType);

        Passives.Add(i);
        if (!PassiveSlots[PassiveCOUNT].activeInHierarchy)
        {
            PassiveSlots[PassiveCOUNT].GetComponent<Image>().sprite = Passives[PassiveCOUNT].sprite;
            PassiveSlots[PassiveCOUNT].SetActive(true);
            Passives[PassiveCOUNT].item.UseItem();
        }
        PassiveCOUNT++;
        return true;
    }
    public bool GetWeapon(WeaponManager Weapon)
    {
        if(Weapon.ThisState == WeaponManager.State.MainWeapon)
        {
            Weapons[0] = new WEAPONTORY(Weapon, Weapon.GetComponent<SpriteRenderer>().sprite);
            WeaponSlots[0].GetComponent<Image>().sprite = Weapons[0].sprite;
            WeaponSlots[0].SetActive(true);
            WeaponSlots[2].GetComponent<Image>().sprite = Weapons[0].sprite;
            WeaponSlots[2].SetActive(true);
        }
        else
        {
            Weapons[1] = new WEAPONTORY(Weapon, Weapon.GetComponent<SpriteRenderer>().sprite);
            WeaponSlots[1].GetComponent<Image>().sprite = Weapons[1].sprite;
            WeaponSlots[1].SetActive(true);
            WeaponSlots[3].GetComponent<Image>().sprite = Weapons[1].sprite;
            WeaponSlots[3].SetActive(true);
        }

        return true;
    }
    public bool GetCogs(int Value)
    {
        if (Value < 0 && GameManager.Instance.LocalPlayer.HaveMental)
        {
            Cog += (int)(Value - (Value * 0.25f));
        }
        else
        {
            Cog += Value;
        }
        FindObjectOfType<Ingredient>().CountUp();
        return true;
    }

    public bool CheckHave(int Code)
    {
        for (int i = 0; i < Passives.Count; i++)
        {
            if (Passives[i].item.ItemCode == Code)
            {
                Passives[i].CountUp();
                return true;
            }
        }

        return false;
    }
    public bool AlreadyHave(int Code)
    {
        for (int i = 0; i < Passives.Count; i++)
        {
            if (Passives[i].item.ItemCode == Code)
            {
                return true;
            }
        }

        return false;
    }

    public void OnHighlight()
    {
        FX.EffectPlay(FX.SYSTEMS.UI_CURSORON);
    }
}
