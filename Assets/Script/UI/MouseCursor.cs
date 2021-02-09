using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {

    public Texture2D[] cursorTexture;
    public int WhatCursor=0;
    Vector2 HotSpot = Vector2.zero;
	// Use this for initialization
	void Start () {
        StartCoroutine(MyCursor());
	}

    public void SetCursor()
    {
        StartCoroutine(MyCursor());
    }
	
	IEnumerator MyCursor()
    {
        yield return new WaitForEndOfFrame();

        if(WhatCursor == 0)
        {
            HotSpot.x = 0;
            HotSpot.y = 0;
        }
        else
        {
            HotSpot.x = cursorTexture[1].width / 2;
            HotSpot.y = cursorTexture[1].height / 2;
        }
        Cursor.SetCursor(cursorTexture[WhatCursor], HotSpot, CursorMode.Auto);
    }
}
