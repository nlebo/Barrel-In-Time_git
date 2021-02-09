using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HP : MonoBehaviour {
    public StateInfo _Player;

    public Image HP_Bar,ER_Bar;
    public Text HP_TEX,ER_TEX;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        HP_TEX.text = (int)_Player.HP + " / " + (int)_Player.MaxHP;
        HP_Bar.fillAmount = _Player.HP / _Player.MaxHP;

        ER_TEX.text = (int)_Player.ER + " / " + (int)_Player.MAX_ER;
        ER_Bar.fillAmount = _Player.ER / _Player.MAX_ER;
	}
}
