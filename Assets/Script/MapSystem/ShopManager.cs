using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public GameObject[] PItems;
    public GameObject[] AItems;
    public int[] PItemsCost;
    public int[] AItemsCost;
    public GameObject[] Shop;
    int PItemCount,AItemCount;
    bool Check;

    // Use this for initialization
    void Start () {
        PItemCount = PItems.Length;
        AItemCount = AItems.Length;
	}

    private void Update()
    {
        if (Check)
            return;

        if (transform.parent.GetComponent<SpriteRenderer>().color.a >= 0.5f)
        {
            for (int i = 0; i < Shop.Length; i++)
                Shop[i].SetActive(true);
            Check = true;
        }
        

    }

    public GameObject RandomPItemGet(out int Cost)
    {
        int r;
        GameObject R;
        InventoryManager Inven = InventoryManager.Instance;
        for (; ;)
        {
            r = Random.Range(0, PItemCount);

            R = PItems[r];
            ItemManager _Item = R.GetComponent<ItemManager>();

            int i;
            for (i = 0; i < Inven.PassiveCOUNT; i++)
            {
                if (_Item.ItemCode == Inven.Passives[i].item.ItemCode)
                    break;
            }

            if (i == Inven.PassiveCOUNT)
                break;
        }

        if (PItemCount < 1)
            PItemCount = PItems.Length;

        PItems[r] = PItems[PItemCount - 1];
        PItems[PItemCount - 1] = R;

        PItemCount--;
        Cost = PItemsCost[r];
        return R;
        
    }

    public GameObject RandomAItemGet(out int Cost)
    {
        int r = Random.Range(0, AItemCount);

        GameObject R = AItems[r];

        if (AItemCount < 1)
            AItemCount = AItems.Length;

        AItems[r] = AItems[AItemCount - 1];
        AItems[AItemCount - 1] = R;
        Cost = AItemsCost[r];
        AItemCount--;
        return R;

    }
}
