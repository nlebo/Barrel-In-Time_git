using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss_Dead : MonoBehaviour {

    SoundManager FX;
    public GameObject Dead_Effect;
    public Image Blind;

    // Use this for initialization
    void Start()
    {
        FX = GameManager.Instance.SoundManager;
        Blind = GameObject.Find("Blind_All").GetComponent<Image>();
        StartCoroutine(Deading());

    }

    IEnumerator Deading()
    {
        Instantiate(Dead_Effect, transform.position, Quaternion.identity);
        FX.EffectPlay(FX.SKILLS.BOMBBULLET_BOMB);
        FX.FadeOut();

        yield return new WaitForSeconds(3);

        while (Blind.color.a < 1)
        {
            Blind.color += new Color(0, 0, 0, Mathf.Lerp(0, 1, Time.deltaTime / 3));
            yield return null;

        }

        SceneManager.LoadScene("Title");
    }
}
