using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRoom : MonoBehaviour {

    public GameObject[] TitleUI;

    // Use this for initialization
    void Start()
    {
        TitleUI = new GameObject[2];

        StartCoroutine(ShowTitleUI());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ShowTitleUI()
    {
        GameManager.Instance.SoundManager.EffectPlay(GameManager.Instance.SoundManager.SYSTEMS.SYSTEM_CURSE);
        yield return new WaitForSeconds(0.5f);
        TitleUI[0] =GameObject.Find("HelpUI").transform.GetChild(2).gameObject;
        TitleUI[1] = GameObject.Find("HelpUI").transform.GetChild(3).gameObject;

        TitleUI[0].SetActive(true);
        TitleUI[1].SetActive(true);

        Text original = TitleUI[0].GetComponent<Text>();
        Text original2 = TitleUI[1].GetComponent<Text>();

        original.color = new Color(original.color.r, original.color.g, original.color.b, 0);
        original2.color = new Color(original.color.r, original.color.g, original.color.b, 0);

        float PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            original.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(0, 1, (Time.time - PTime) * 2));
            original2.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(0, 1, (Time.time - PTime) * 2));
            yield return null;
        }
        yield return new WaitForSeconds(2);

        PTime = Time.time;
        while (Time.time - PTime <= 0.5f)
        {
            original.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(1, 0, (Time.time - PTime) * 2));
            original2.color = new Color(original.color.r, original.color.g, original.color.b, Mathf.Lerp(1, 0, (Time.time - PTime) * 2));
            yield return null;
        }


        TitleUI[0].SetActive(false);
        TitleUI[1].SetActive(false);
    }
}
