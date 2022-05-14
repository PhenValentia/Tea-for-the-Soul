using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sr;
    SpriteRenderer[] srArray;
    bool faded = true;

    // Start is called before the first frame update
    void Start()
    {
        if (sr == null)
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }
        srArray = sr.gameObject.GetComponentsInChildren<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, 0);
        foreach (SpriteRenderer arrayRend in srArray)
        {
            arrayRend.color = new Color(sr.color.r, sr.color.b, sr.color.g, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fade()
    {
        if (faded)
        {
            StartCoroutine(fadeInR());
        }
        else
        {
            StartCoroutine(fadeOutR());
        }
    }

    public void fadeIn()
    {
        StartCoroutine(fadeInR());
    }

    public void fadeOut()
    {
        StartCoroutine(fadeOutR());
    }

    IEnumerator fadeInR()
    {
        for (float i = 0; i < 1; i+=0.01f)
        {
            yield return new WaitForFixedUpdate();
            sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, i);
            if(srArray != null)
            {
                foreach(SpriteRenderer arrayRend in srArray)
                {
                    arrayRend.color = new Color(sr.color.r, sr.color.b, sr.color.g, i);
                }
            }
        }
        faded = false;
    }

    IEnumerator fadeOutR()
    {
        for (float i = 1; i > 0; i-=0.01f)
        {
            yield return new WaitForFixedUpdate();
            sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, i);
            if (srArray != null)
            {
                foreach (SpriteRenderer arrayRend in srArray)
                {
                    arrayRend.color = new Color(sr.color.r, sr.color.b, sr.color.g, i);
                }
            }
        }
        faded = true;
    }
}
