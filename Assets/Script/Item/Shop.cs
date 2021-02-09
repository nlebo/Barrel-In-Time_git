using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public enum TYPE{ _PASSIVE,_ACTIVE};

    public delegate GameObject ItemGet(ShopManager shop);
    public GameObject ShopImage;
    public Text Name, Info;
    public SoundManager FX;
    bool check = true;
    static int Cost;
    int _Cost;
    GameObject Spawn;
    public TYPE what;
    ItemGet[] functions = { PASSIVE,ACTIVE};
    public Text HelpUI;
    public GameObject Show_UI;

// Use this for initialization
    void Start () {
        ItemGet a = functions[(int)what];

        Spawn = a(transform.parent.GetComponent<ShopManager>());
        _Cost = Cost;
        Transform child = Instantiate(Spawn, transform).transform;
        child.GetComponent<BoxCollider2D>().enabled = false;
        FX = GameManager.Instance.SoundManager;
        transform.GetChild(0).GetComponent<TextMesh>().text = _Cost + "s";
        
        ShopImage = GameObject.Find("HelpUI").transform.GetChild(6).gameObject;
        Name = ShopImage.transform.GetChild(0).GetComponent<Text>();
        Info = ShopImage.transform.GetChild(1).GetComponent<Text>();

    }

    static GameObject PASSIVE(ShopManager shop)
    {
        return shop.RandomPItemGet(out Cost);
    }

    static GameObject ACTIVE(ShopManager shop)
    {
        return shop.RandomAItemGet(out Cost);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (check)
            {
                if (transform.GetChild(1).GetComponent<ItemManager>().BuyItem(_Cost))
                {
                    
                    FX.EffectPlay(FX.SYSTEMS.SYSTEM_BUYITEMS);
                    //if ((int)what == 0) {
                    //    UIShow UI = Instantiate(Show_UI, transform.position, Quaternion.identity).GetComponent<UIShow>();
                    //    UI.ShopImage = ShopImage;
                    //    Name.text = transform.GetChild(1).GetComponent<ItemManager>().Name;
                    //    Info.text = transform.GetChild(1).GetComponent<ItemManager>().Short_Info;
                    //    UI.Name = Name;
                    //    UI.Info = Info;
                    //}

                    Destroy(this.gameObject);
                }
                else
                {
                    FX.EffectPlay(FX.SYSTEMS.SYSTEM_FAIL);
                    StartCoroutine(CanNotBuyUI());
                }
            }

            check = false;
        }
    }
    IEnumerator CanNotBuyUI()
    {
        HelpUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        HelpUI.gameObject.SetActive(false);
    }

}
