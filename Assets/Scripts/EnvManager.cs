using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvManager : MonoBehaviour
{
    DialogueManager dM;
    GameObject ant;
    FadeIn f;
    CircleTransition cT;
    CameraController cC;

    // Start is called before the first frame update
    void Start()
    {
        dM = GameObject.Find("Dialogue Panel").GetComponent<DialogueManager>();
        cT = GameObject.Find("CircleWipeMask").GetComponent<CircleTransition>();
        ant = GameObject.Find("Antoine");
        cC = Camera.main.GetComponent<CameraController>();
        if (ant != null)
        {
            f = ant.GetComponent<FadeIn>();
            if (PlayerPrefs.GetInt("StoryPoint") < 1)
            {
                PlayerPrefs.SetInt("StoryPoint", 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("StoryPoint: " + PlayerPrefs.GetInt("StoryPoint"));
        //Debug.Log("Minigame: " + PlayerPrefs.GetInt("Minigame"));
        if (PlayerPrefs.GetInt("StoryPoint") == 1)
        {
            f.fadeIn();
            PlayerPrefs.SetInt("StoryPoint", 2);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 2 && !dM.runningBubble)
        {
            Debug.Log("Beginning D2");
            dM.startSpeechBubbles("D2");
            PlayerPrefs.SetInt("StoryPoint", 3);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 4)
        {
            f.fadeOut();
            PlayerPrefs.SetInt("StoryPoint", 5);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 6 && !dM.runningBubble)
        {
            PlayerPrefs.SetInt("StoryPoint", 7);
            cT.endLevel("HouseExterior", GameObject.Find("Lapis").transform);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 7 && SceneManager.GetActiveScene().name == "HouseExterior")
        {
            PlayerPrefs.SetInt("StoryPoint", 8);
            dM.startSpeechBubbles("D3");
        }
        if (PlayerPrefs.GetInt("Flower1") == 1 && PlayerPrefs.GetInt("Flower1D") != 1)
        {
            dM.startSpeechBubbles("F1");
            PlayerPrefs.SetInt("Flower1D", 1);
        }
        if (PlayerPrefs.GetInt("Flower2") == 1 && PlayerPrefs.GetInt("Flower2D") != 1)
        {
            dM.startSpeechBubbles("F2");
            PlayerPrefs.SetInt("Flower2D", 1);
        }
        if (PlayerPrefs.GetInt("Flower3") == 1 && PlayerPrefs.GetInt("Flower3D") != 1)
        {
            dM.startSpeechBubbles("F3");
            PlayerPrefs.SetInt("Flower3D", 1);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 8 && PlayerPrefs.GetInt("Flower1D") == 1 && PlayerPrefs.GetInt("Flower2D") == 1 && PlayerPrefs.GetInt("Flower3D") == 1)
        {
            PlayerPrefs.SetInt("StoryPoint", 9);
            dM.startSpeechBubbles("D4");
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 10 && SceneManager.GetActiveScene().name == "Cutscene2")
        {
            PlayerPrefs.SetInt("StoryPoint", 11);
            dM.startDialogue("D5");
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 12)
        {
            PlayerPrefs.SetInt("StoryPoint", 13);
            cC.setSmoothCamera(2.5f, new Vector3(0, 0, -10));
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 14)
        {
            PlayerPrefs.SetInt("StoryPoint", 15);
            cC.setSmoothCamera(0.8f, new Vector3(-2.3f, 0.3f, -10));
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 15 && !dM.runningDialogue)
        {
            PlayerPrefs.SetInt("StoryPoint", 16);
            PlayerPrefs.SetInt("Minigame", 1);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 17)
        {
            PlayerPrefs.SetInt("StoryPoint", 18);
            dM.startDialogue("D6");
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 19)
        {
            PlayerPrefs.SetInt("StoryPoint", 20);
            cC.setSmoothCamera(1.4f, new Vector3(1.3f, 1.2f, -10));
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 20 && !dM.runningDialogue)
        {
            PlayerPrefs.SetInt("StoryPoint", 21);
            PlayerPrefs.SetInt("Minigame", 2);
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 22)
        {
            cC.setSmoothCamera(5f, new Vector3(0, 0, -10));
            PlayerPrefs.SetInt("StoryPoint", 23);
            GameObject.Find("Lapis").GetComponent<FadeIn>().fadeIn();
            GameObject.Find("Table").GetComponent<FadeIn>().fadeIn();
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 23 && !cC.changing)
        {
            PlayerPrefs.SetInt("StoryPoint", 24);
            dM.startSpeechBubbles("D7");
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 25)
        {
            PlayerPrefs.SetInt("StoryPoint", 26);
            f.fadeIn();
        }
        if (PlayerPrefs.GetInt("StoryPoint") == 27)
        {
            PlayerPrefs.SetInt("StoryPoint", 28);
            f.fadeOut();
        }

        if (PlayerPrefs.GetInt("StoryPoint") == 100 && !dM.runningBubble)
        {
            PlayerPrefs.SetInt("StoryPoint", 101);
            cT.endLevel("EndScreen", GameObject.Find("Lapis").transform);
        }
    }

    public void setStoryPoint(int i)
    {
        PlayerPrefs.SetInt("StoryPoint", i);
    }

    public int getStoryPoint()
    {
        return PlayerPrefs.GetInt("StoryPoint");
    }
}
