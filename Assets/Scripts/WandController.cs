using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour
{
    SpriteRenderer sr;
    int lastStoryPoint = 0;
    bool resetWand = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        resetWand = false;
    }

    private void Update()
    {
        armFollowMouse();
        if(lastStoryPoint != PlayerPrefs.GetInt("StoryPoint") && PlayerPrefs.GetInt("StoryPoint") < 23)
        {
            //Debug.Log("Switching Wands");
            switch (PlayerPrefs.GetInt("StoryPoint"))
            {
                case 15:
                    StartCoroutine(setWand(Resources.LoadAll<Sprite>("Art/Items/SpriteSheet")[11]));
                    break;
                case 20:
                    StartCoroutine(switchWand(Resources.LoadAll<Sprite>("Art/Items/SpriteSheet")[4]));
                    break;
                case 30:
                    StartCoroutine(switchWand(Resources.LoadAll<Sprite>("Art/Items/SpriteSheet")[14]));
                    break;
                default:
                    //Debug.Log("OtherPoint");
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("StoryPoint") > 22 && !resetWand)
        {
            resetWand = true;
            StartCoroutine(removeWand());
            Debug.Log("ResetWand");
        }
            lastStoryPoint = PlayerPrefs.GetInt("StoryPoint");
    }

    void armFollowMouse()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos;
    }

    IEnumerator switchWand(Sprite s)
    {
        //Debug.Log("CoRunning");
        Cursor.visible = false;
        for(float f = 1; f > 0; f -= 0.01f)
        {
            sr.color = new Color(1, 1, 1, f);
            yield return new WaitForFixedUpdate();
        }
        sr.sprite = s;
        for (float f = 0; f < 1; f += 0.01f)
        {
            sr.color = new Color(1, 1, 1, f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator setWand(Sprite s)
    {
        Cursor.visible = false;
        sr.color = new Color(1, 1, 1, 0);
        sr.sprite = s;
        for (float f = 0; f < 1; f += 0.01f)
        {
            sr.color = new Color(1, 1, 1, f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator removeWand()
    {
        Cursor.visible = true;
        sr.color = new Color(1, 1, 1, 1);
        for (float f = 0; f < 1; f += 0.01f)
        {
            sr.color = new Color(1, 1, 1, 1-f);
            yield return new WaitForFixedUpdate();
        }
        sr.color = new Color(1, 1, 1, 0);
    }
}
