using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ExplainTextManager : MonoBehaviour
{
    CanvasGroup cg;
    TextMeshProUGUI text;
    IEnumerator storedCoroutine;
    int step = 0;

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        text = GameObject.Find("ExplainText").GetComponent<TextMeshProUGUI>();
        step = 0;
        //Debug.Log("T: "+text.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Cutscene2")
        {
            if (step == 0 && PlayerPrefs.GetInt("Minigame") == 1)
            {
                step = 1;
                StartCoroutine(fadeIn());
            }
            if (step == 1 && PlayerPrefs.GetInt("StoryPoint") > 16)
            {
                storedCoroutine = fadeOut();
                StartCoroutine(storedCoroutine);
                step = 2;              
            }
            if (step == 2 && PlayerPrefs.GetInt("StoryPoint") > 19)
            {
                StopCoroutine(storedCoroutine);
                StartCoroutine(fadeIn());
                step = 4;
            }
            if (step == 4 && PlayerPrefs.GetInt("StoryPoint") > 21)
            {
                step = 5;
                StartCoroutine(fadeOut());
            }
        }
        else
        {
            cg.alpha = 0;
        }
    }

    IEnumerator fadeIn()
    {
        while (cg.alpha < 1)
        {
            cg.alpha += 0.01f;
            Debug.Log("FadingIn");
            yield return new WaitForFixedUpdate();
        }
        cg.alpha = 1;
    }

    IEnumerator fadeOut()
    {
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.01f;
            Debug.Log("FadingOut");
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("FinishedFadeOut");
        cg.alpha = 0;
        text.SetText("Left Click to Attract Water Drops to your Wand." + System.Environment.NewLine + "Attract all the water to the Teapot Lid");
        //text.text = "Left Click to Attract Water Drops to your Wand." + System.Environment.NewLine + "Attract all the water to the Teapot Lid";
    }
}
