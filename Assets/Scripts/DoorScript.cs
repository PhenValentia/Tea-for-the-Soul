using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    string nextLevel = "Not Set";
    CircleTransition ct;
    EnvManager eM;

    // Start is called before the first frame update
    void Start()
    {
        ct = GameObject.Find("CircleWipeMask").GetComponent<CircleTransition>();
        eM = GameObject.Find("EnvironmentManager").GetComponent<EnvManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetButton("Interact") && PlayerPrefs.GetInt("StoryPoint") == 9 && nextLevel == "HouseInterior")
        {
            ct.endLevel("Cutscene2", GameObject.Find("Lapis").transform);
            PlayerPrefs.SetInt("StoryPoint", 10);
        }
        else if (collision.tag == "Player" && Input.GetButton("Interact") && PlayerPrefs.GetInt("StoryPoint") < 10)
        {
            ct.endLevel(nextLevel, GameObject.Find("Lapis").transform);
        }
    }
/*
    public GameObject prompt;
    private string reactTag = "Player";

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ()
            return;
        if (collision.tag == reactTag)
            prompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == reactTag)
            prompt.SetActive(false);
    }
*/
    }
