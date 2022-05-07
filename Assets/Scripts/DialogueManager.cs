using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    Image headBox;
    GameObject headArea;
    CanvasGroup dialogueArea;
    CanvasGroup bubbleArea;
    EnvManager eM;
    TextMeshProUGUI dialogueText;
    TextMeshProUGUI bubbleText;
    AudioSource source;

    public string dialogueName;
    string file;
    int currentLine;
    bool textCurrentlyWriting;
    string[] lines;
    public bool runningDialogue = false;
    public bool runningBubble = false;

    MovementController pMove;

    float waitTime = 0.1f;
    float timeSinceAnyKey = 0f;

    private void Awake()
    {
        currentLine = 0;

        eM = GameObject.Find("EnvironmentManager").GetComponent<EnvManager>();
        pMove = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<MovementController>();
        dialogueArea = GameObject.FindGameObjectWithTag("DialoguePanel").GetComponent<CanvasGroup>();
        bubbleArea = GameObject.FindGameObjectWithTag("DialogueBubble").GetComponent<CanvasGroup>();
        bubbleText = GameObject.FindGameObjectWithTag("DialogueBubbleText").GetComponent<TextMeshProUGUI>();
        dialogueArea.alpha = 0;
        bubbleArea.alpha = 0;
        headBox = GameObject.FindGameObjectWithTag("DialogueHeadBox").GetComponent<Image>();
        dialogueText = GameObject.FindGameObjectWithTag("DialogueTextBox").GetComponent<TextMeshProUGUI>();
        source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pMove = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<MovementController>();

        
        /*foreach (string line in lines)
        {
            Debug.Log(line);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && Time.time - timeSinceAnyKey > waitTime)
        {
            //StopAllCoroutines();
            timeSinceAnyKey = Time.time;
            if (runningDialogue)
            {
                advanceDialogue();
            }
            else if (runningBubble)
            {
                advanceBubble();
            }
            else if (dialogueArea.alpha > 0)
            {
                StartCoroutine(fadeDialogue());
            }
            else if (bubbleArea.alpha > 0)
            {
                StartCoroutine(fadeBubble());
            }
        }
    }

    IEnumerator fadeDialogue()
    {
        for (int i = 0; dialogueArea.alpha > 0; i++)
        {
            dialogueArea.alpha = dialogueArea.alpha - 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator fadeBubble ()
    {
        for (int i = 0; bubbleArea.alpha > 0; i++)
        {
            bubbleArea.alpha = bubbleArea.alpha - 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    public void startSpeechBubbles(string inputSpeechName)
    {
        dialogueName = inputSpeechName;
        if (Application.dataPath + "/Resources/Dialogue/" + dialogueName + ".txt" != null)
        {
            var sr = new StreamReader(Application.dataPath + "/Resources/Dialogue/" + dialogueName + ".txt");
            var fileContents = sr.ReadToEnd();
            sr.Close();


            lines = fileContents.Split("\n"[0]);
        }
        else
        {
            Debug.LogError("Dialogue Not Found: " + dialogueName);
        }

        runningBubble = true;
        pMove.disableMovement();
        dialogueArea.alpha = 0;
        dialogueText.SetText("");
        currentLine = 0;
        //StartCoroutine(beginBubble());
        StartCoroutine(writeBubble(lines[0]));
    }

    public void startDialogue(string inputDialogueName)
    {
        dialogueName = inputDialogueName;
        if (Application.dataPath + "/Resources/Dialogue/" + dialogueName + ".txt" != null)
        {
            var sr = new StreamReader(Application.dataPath + "/Resources/Dialogue/" + dialogueName + ".txt");
            var fileContents = sr.ReadToEnd();
            sr.Close();


            lines = fileContents.Split("\n"[0]);
        }
        else
        {
            Debug.LogError("Dialogue Not Found: " + dialogueName);
        }

        runningDialogue = true;
        pMove.disableMovement();
        dialogueArea.alpha = 0;
        //overlayImage.color = new Color(1, 1, 1, 0);
        dialogueText.SetText("");
        currentLine = 0;
        StartCoroutine(beginDialogue());
    }

    public void advanceDialogue()
    {
        if (!runningDialogue)
        {

        }
        else if (textCurrentlyWriting)
        {
            dialogueArea.alpha = 1;
            StopCoroutine(writeDialogue(lines[currentLine]));
            StopAllCoroutines();
            textCurrentlyWriting = false;

            string str = lines[currentLine];
            textCurrentlyWriting = true;
            string totalDialogue = "";
            bool soundFound = false;
            bool headFound = false;
            for (int i = 0; i + 1 < str.Length; i++)
            {
                if (str[i] == '[' && !soundFound)
                {
                    int newCounter = i;
                    string soundName = "";
                    for (int j = i + 1; str[j] != ']'; j++)
                    {
                        soundName += str[j];
                        newCounter = j + 1;
                    }
                    soundFound = true;
                    if (soundName != "e")
                    {
                        setSound(soundName);
                    }
                    i = newCounter;
                }
                else if (str[i] == '[' && !headFound)
                {
                    int newCounter = i;
                    string headName = "";
                    for (int j = i + 1; str[j] != ']'; j++)
                    {
                        headName += str[j];
                        newCounter = j + 1;
                    }
                    headFound = true;
                    if (headName == "Lapis")
                    {
                        headBox.color = new Color(1, 1, 1, 1);
                        setHead(headName);
                    }
                    else
                    {
                        headBox.color = new Color(0, 0, 0, 0);
                    }
                    i = newCounter;
                }
                else if (str[i] == '[' && headFound && soundFound)
                {
                    int newCounter = i;
                    string storyPoint = "";
                    for (int j = i + 1; str[j] != ']'; j++)
                    {
                        storyPoint += str[j];
                        newCounter = j + 1;
                    }
                    eM.setStoryPoint(int.Parse(storyPoint));
                    i = newCounter;
                }
                else if (str[i] == '<')
                {
                    int newCounter = i;
                    string alignName = "";
                    for (int j = i; str[j] != '>'; j++)
                    {
                        alignName += str[j];
                        newCounter = j + 1;
                    }
                    headFound = true;
                    totalDialogue += alignName;
                    totalDialogue += '>';
                    i = newCounter;
                }
                else
                {
                    totalDialogue += str[i];
                }
                dialogueText.text = totalDialogue;
            }
            textCurrentlyWriting = false;
        }
        else if (currentLine + 1 < lines.Length)
        {
            currentLine++;
            StartCoroutine(writeDialogue(lines[currentLine]));
        }
        else
        {
            StartCoroutine(endDialogue());
        }
    }

    IEnumerator endDialogue()
    {
        for (int i = 0; dialogueArea.alpha > 0; i++)
        {
            dialogueArea.alpha = dialogueArea.alpha - 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1f;
        //headBox.color = new Color(0, 0, 0, 0);
        pMove.enableMovement();
        dialogueArea.alpha = 0;
        runningDialogue = false;
    }

    // SPEECH BOX SECTION

    IEnumerator beginDialogue()
    {
        for (int i = 0; dialogueArea.alpha < 1; i++)
        {
            dialogueArea.alpha = dialogueArea.alpha + 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        StartCoroutine(writeDialogue(lines[0]));
    }

    IEnumerator writeDialogue(string str)
    {
        textCurrentlyWriting = true;
        string totalDialogue = "";
        bool soundFound = false;
        bool headFound = false;
        for (int i = 0; i + 1 < str.Length; i++)
        {
            if (str[i] == '[' && !soundFound)
            {
                int newCounter = i;
                string soundName = "";
                for (int j = i + 1; str[j] != ']'; j++)
                {
                    soundName += str[j];
                    newCounter = j + 1;
                }
                soundFound = true;
                if (soundName != "e")
                {
                    setSound(soundName);
                }
                i = newCounter;
            }
            else if (str[i] == '[' && !headFound)
            {
                int newCounter = i;
                string headName = "";
                for (int j = i + 1; str[j] != ']'; j++)
                {
                    headName += str[j];
                    newCounter = j + 1;
                }
                headFound = true;
                if (headName != "e")
                {
                    headBox.color = new Color(1, 1, 1, 1);
                    setHead(headName);
                }
                else
                {
                    headBox.color = new Color(0, 0, 0, 0);
                }
                i = newCounter;
            }
            else if (str[i] == '[' && headFound && soundFound)
            {
                int newCounter = i;
                string storyPoint = "";
                for (int j = i + 1; str[j] != ']'; j++)
                {
                    storyPoint += str[j];
                    newCounter = j + 1;
                }
                eM.setStoryPoint(int.Parse(storyPoint));
                i = newCounter;
            }
            else if (str[i] == '<')
            {
                int newCounter = i;
                string alignName = "";
                for (int j = i; str[j] != '>'; j++)
                {
                    alignName += str[j];
                    newCounter = j + 1;
                }
                headFound = true;
                totalDialogue += alignName;
                totalDialogue += '>';
                i = newCounter;
            }
            else
            {
                totalDialogue += str[i];
                yield return new WaitForSecondsRealtime(0.01f);
            }
            dialogueText.text = totalDialogue;
        }
        textCurrentlyWriting = false;
    }

    void setSound(string soundName)
    {
        source.Stop();
        source.clip = Resources.Load<AudioClip>("Audio/" + soundName);
        source.Play();
    }

    void setHead(string headName)
    {
        headBox.sprite = Resources.Load<Sprite>("Art/Characters/" + headName);
    }

    // SPEECH BUBBLE SECTION

    /*IEnumerator beginBubble()
    {
        for (int i = 0; bubbleArea.alpha < 1; i++)
        {
            bubbleArea.alpha = bubbleArea.alpha + 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        StartCoroutine(writeBubble(lines[0]));
    }*/

    IEnumerator writeBubble(string str)
    {
        textCurrentlyWriting = true;
        string totalDialogue = "";
        bool soundFound = false;
        bool headFound = false;
        for (int i = 0; i + 1 < str.Length; i++)
        {
            if (str[i] == '[' && !soundFound)
            {
                int newCounter = i;
                string soundName = "";
                for (int j = i + 1; str[j] != ']'; j++)
                {
                    soundName += str[j];
                    newCounter = j + 1;
                }
                soundFound = true;
                if (soundName != "e")
                {
                    setSound(soundName);
                }
                i = newCounter;
            }
            else if (str[i] == '[' && !headFound)
            {
                int newCounter = i;
                string headName = "";
                for (int j = i + 1; str[j] != ']'; j++)
                {
                    headName += str[j];
                    newCounter = j + 1;
                }
                headFound = true;
                if (headName != "e")
                {
                    setPos(headName);
                }
                else
                {
                    Debug.LogError("No Position Found: Defaulting to Player");
                    setPos("Lapis");
                }
                i = newCounter;
            }
            else if (str[i] == '[' && headFound && soundFound)
            {
                int newCounter = i;
                string storyPoint = "";
                for (int j = i + 1; str[j] != ']'; j++)
                {
                    storyPoint += str[j];
                    newCounter = j + 1;
                }
                eM.setStoryPoint(int.Parse(storyPoint));
                i = newCounter;
            }
            else if (str[i] == '<')
            {
                int newCounter = i;
                string alignName = "";
                for (int j = i; str[j] != '>'; j++)
                {
                    alignName += str[j];
                    newCounter = j + 1;
                }
                headFound = true;
                totalDialogue += alignName;
                totalDialogue += '>';
                i = newCounter;
            }
            else
            {
                totalDialogue += str[i];
            }
            bubbleText.text = totalDialogue;
        }


        for (int i = 0; bubbleArea.alpha < 1; i++)
        {
            bubbleArea.alpha = bubbleArea.alpha + 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        textCurrentlyWriting = false;
    }

    void setPos(string headName)
    {
        Transform t = GameObject.Find(headName).transform;
        Vector2 s = t.GetComponent<BoxCollider2D>().size;
        Vector2 target = t.position + new Vector3(s.x, s.y, 0);
        bubbleArea.transform.position = Camera.main.WorldToScreenPoint(target) + new Vector3(s.x, s.y, 0);

    }

    public void advanceBubble()
    {
        if (!runningBubble)
        {

        }
        else if (textCurrentlyWriting)
        {
            bubbleArea.alpha = 1;
            StopCoroutine(writeDialogue(lines[currentLine]));
            StopAllCoroutines();
            textCurrentlyWriting = false;

            string str = lines[currentLine];
            textCurrentlyWriting = true;
            string totalDialogue = "";
            bool soundFound = false;
            bool headFound = false;
            for (int i = 0; i + 1 < str.Length; i++)
            {
                if (str[i] == '[' && !soundFound)
                {
                    int newCounter = i;
                    string soundName = "";
                    for (int j = i + 1; str[j] != ']'; j++)
                    {
                        soundName += str[j];
                        newCounter = j + 1;
                    }
                    soundFound = true;
                    if (soundName != "e")
                    {
                        setSound(soundName);
                    }
                    i = newCounter;
                }
                else if (str[i] == '[' && !headFound)
                {
                    int newCounter = i;
                    string headName = "";
                    for (int j = i + 1; str[j] != ']'; j++)
                    {
                        headName += str[j];
                        newCounter = j + 1;
                    }
                    headFound = true;
                    if (headName != "e")
                    {
                        setPos(headName);
                    }
                    else
                    {
                        Debug.LogError("No Position Found: Defaulting to Player");
                        setPos("Player");
                    }
                    i = newCounter;
                }
                else if (str[i] == '[' && headFound && soundFound)
                {
                    int newCounter = i;
                    string storyPoint = "";
                    for (int j = i + 1; str[j] != ']'; j++)
                    {
                        storyPoint += str[j];
                        newCounter = j + 1;
                    }
                    eM.setStoryPoint(int.Parse(storyPoint));
                    i = newCounter;
                }
                else if (str[i] == '<')
                {
                    int newCounter = i;
                    string alignName = "";
                    for (int j = i; str[j] != '>'; j++)
                    {
                        alignName += str[j];
                        newCounter = j + 1;
                    }
                    headFound = true;
                    totalDialogue += alignName;
                    totalDialogue += '>';
                    i = newCounter;
                }
                else
                {
                    totalDialogue += str[i];
                }
                bubbleText.text = totalDialogue;
            }

            for (int i = 0; bubbleArea.alpha < 1; i++)
            {
                bubbleArea.alpha = bubbleArea.alpha + 0.04f;
            }

            textCurrentlyWriting = false;
        }
        else if (currentLine + 1 < lines.Length)
        {
            currentLine++;
            StartCoroutine(continueBubble(lines[currentLine]));
        }
        else
        {
            StartCoroutine(endBubble());
        }
    }

    IEnumerator continueBubble(string inputLine)
    {
        for (int i = 0; bubbleArea.alpha > 0; i++)
        {
            bubbleArea.alpha = bubbleArea.alpha - 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        StartCoroutine(writeBubble(inputLine));
    }

    IEnumerator endBubble()
    {
        for (int i = 0; bubbleArea.alpha > 0; i++)
        {
            bubbleArea.alpha = bubbleArea.alpha - 0.04f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1f;
        //headBox.color = new Color(0, 0, 0, 0);
        pMove.enableMovement();
        bubbleArea.alpha = 0;
        runningBubble = false;
    }
}
