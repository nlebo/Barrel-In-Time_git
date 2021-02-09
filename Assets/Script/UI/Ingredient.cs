using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour {

    public Text[] IngreCount;
    public int count;

	// Use this for initialization
	void Start () {
        count = 0;
        UpdateText();
	}

    public void UpdateText()
    {
        count = InventoryManager.Instance.Cog;
        for (int i = 0; i < IngreCount.Length; i++)
        {
            IngreCount[i].text = "" + count;
        }
    }

    public void CountUp()
    {
        count = InventoryManager.Instance.Cog;
        UpdateText();
    }
}
