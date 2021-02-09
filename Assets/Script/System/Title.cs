using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    public Image Title_Image;
    public Button A, B;
    public AudioClip Cursor_On, Click;
    public AudioSource Audio;

	// Use this for initialization
	void Start () {
        StartCoroutine(StartTitle());
        Audio = gameObject.AddComponent<AudioSource>();
        Audio.loop = false;
        Audio.volume = 0.1f;
	}
	
    public void OnClick_Start()
    {
        Audio.clip = Click;
        Audio.Play();

        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
        StartCoroutine(Click_Start());
    }

    public void OnClick_Quit()
    {
        Audio.clip= Click;
        Audio.Play();


        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
        StartCoroutine(Click_Quit());
    }

    public void OnCursor()
    {
        Audio.clip = Cursor_On;
        Audio.Play();
    }

    IEnumerator StartTitle()
    {
        yield return new WaitForSeconds(5);
        while(Title_Image.color.a <= 1)
        {

            Title_Image.color += new Color(0, 0, 0, Mathf.Lerp(0, 1, Time.deltaTime / 2));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        A.gameObject.SetActive(true);
        B.gameObject.SetActive(true);
    }

    IEnumerator Click_Start()
    {
        while (Title_Image.color.a > 0)
        {

            Title_Image.color -= new Color(0, 0, 0, Mathf.Lerp(0, 1, Time.deltaTime / 2));
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Default");
    }

    IEnumerator Click_Quit()
    {
        while (Title_Image.color.a > 0)
        {

            Title_Image.color -= new Color(0, 0, 0, Mathf.Lerp(0, 1, Time.deltaTime / 2));
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        Application.Quit();
    }
}
