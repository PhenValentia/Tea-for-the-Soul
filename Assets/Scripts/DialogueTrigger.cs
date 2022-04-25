using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    Collider2D col;
    DialogueManager dM;
    [SerializeField]
    string dialogueName;
    [SerializeField]
    bool isBubble = false;
    bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        dM = GameObject.FindGameObjectsWithTag("DialoguePanel")[0].GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !activated)
        {
            activated = true;
            if (isBubble)
            {
                dM.startSpeechBubbles(dialogueName);
            }
            else
            {
                dM.startDialogue(dialogueName);
            }
        }
        Destroy(this.gameObject);
    }
}
