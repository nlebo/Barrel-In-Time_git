using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

     
    SpriteRenderer ParentImage, image;

    // Use this for initialization
    void Start () {

        if (transform.parent)
        {
            image = GetComponent<SpriteRenderer>();

            if (transform.parent.GetComponent<SpriteRenderer>())
                ParentImage = transform.parent.GetComponent<SpriteRenderer>();
            else
                ParentImage = transform.parent.parent.GetComponent<SpriteRenderer>();

            if (ParentImage == null)
                ParentImage = transform.parent.parent.parent.GetComponent<SpriteRenderer>();

            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, ParentImage.color.a);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (image != null)
            image.color = new Color(image.color.r, image.color.g, image.color.b, ParentImage.color.a);
    }
}
