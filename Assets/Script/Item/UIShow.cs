using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShow : MonoBehaviour {

    public GameObject ShopImage;
    public Text Name, Info;

    public void Start()
    {
        StartCoroutine(BuyUI());
    }

    IEnumerator BuyUI()
    {
        ShopImage.SetActive(true);
        Name.gameObject.SetActive(true);
        Info.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        ShopImage.SetActive(false);
        Name.gameObject.SetActive(false);
        Info.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
