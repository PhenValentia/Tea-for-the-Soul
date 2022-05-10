using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerGather : MonoBehaviour
{
    [SerializeField]
    int flowerNum = 0;
    bool collected = false;
    Image flower;
    RectTransform rt;
    Vector2 originalPos;
    AudioClip c;
    AudioSource aS;

    // Start is called before the first frame update
    void Start()
    {
        c = Resources.Load<AudioClip>("Audio/Snip");
        aS = Camera.main.gameObject.GetComponent<AudioSource>();
        rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition;
        flower = GetComponent<Image>();
        //Debug.Log(PlayerPrefs.GetInt("Flower" + flowerNum));
        if (PlayerPrefs.GetInt("Flower"+flowerNum) == 1)
        {
            collected = true;
            flower.color = Color.white;
        }
        else
        {
            collected = false;
            flower.color = Color.clear;
        }
    }

    public void collectFlower(Vector2 pos)
    {
        if (!collected)
        {
            StartCoroutine(collect(pos));
        }
    }

    IEnumerator collect(Vector2 pos)
    {
        collected = true;
        if (flowerNum != 0)
        {
            aS.clip = c;
            aS.time = 0;
            aS.Play();
            PlayerPrefs.SetInt("Flower" + flowerNum, 1);
        }
        else
        {
            Debug.LogError("Error: Flower Num is still 0 [Set as 1-3]");
        }
        //rt.anchoredPosition = pos;
        for (float f = 0; f < 1; f+=0.01f)
        {
            flower.color = new Color(1, 1, 1, f);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Finished Fade");
        /*while((rt.anchoredPosition.magnitude - originalPos.magnitude) < 0.01f)
        {
            rt.anchoredPosition += new Vector2((rt.anchoredPosition.x - originalPos.x) / 100, (rt.anchoredPosition.y - originalPos.y) / 100);
            yield return new WaitForSeconds(0.01f);
        }*/
    }
}
