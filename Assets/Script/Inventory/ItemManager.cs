using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {
    public int ItemCode;
    public float VALUE;
    public string Name,Short_Info,Detail_Info;
    public enum TYPE { PASSIVE,ACTIVE};
    protected InventoryManager _Inventory;
    public SoundManager FX;
    public GameObject SHOW_UI;

    public GameObject ShopImage;
    public Text _Name, Info;

    public TYPE ThisType;
    //public int Cnt;

	// Use this for initialization
	public virtual void Start () {
        _Inventory = InventoryManager.Instance;
        FX = GameManager.Instance.SoundManager;
        ShopImage = GameObject.Find("HelpUI").transform.GetChild(6).gameObject;
        _Name = ShopImage.transform.GetChild(0).GetComponent<Text>();
        Info = ShopImage.transform.GetChild(1).GetComponent<Text>();
    }
	


    public virtual bool GetItem()
    {
        FX.EffectPlay(FX.SYSTEMS.SYSTEM_GETITEMS);

        if (ThisType == TYPE.PASSIVE)
        {
            UIShow UI = Instantiate(SHOW_UI, transform.position, Quaternion.identity).GetComponent<UIShow>();
            UI.ShopImage = ShopImage;
            _Name.text = Name;
            Info.text = Short_Info;
            UI.Name = _Name;
            UI.Info = Info;
        }

        return true;
    }

    public virtual bool BuyItem(int VALUE)
    {
        Clock _Clock = FindObjectOfType<Clock>();
        if (_Clock.Real_Sec + VALUE > 540)
            return false;


        if (ThisType == TYPE.ACTIVE) 
            UseItem();
        else
        {
            if(GetItem())
            {
            }
            else
                return false;
        }

        if (!GameManager.Instance.LocalPlayer.HaveCupon)
            _Clock.Real_Sec += VALUE;
        else
            GameManager.Instance.LocalPlayer.HaveCupon = false;

        return true;
    }

    public virtual void UseItem()
    {

    }

    public virtual void DropItem()
    {

    }

    public virtual void Combine()
    {

    }




}
