using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectDryUp : MonoBehaviour
{
    int totalLeaves = 0;
    DryUp[] leavesArray;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        leavesArray = GetComponentsInChildren<DryUp>();
        totalLeaves = leavesArray.Length;
        text = GameObject.Find("CounterText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("Minigame") == 1)
        {
            int counter = 0;
            foreach (DryUp d in leavesArray)
            {
                if (d.getFinished())
                {
                    counter++;
                }
            }
            text.text = counter + "/" + totalLeaves;
            if (counter == totalLeaves && PlayerPrefs.GetInt("StoryPoint") == 16)
            {
                PlayerPrefs.SetInt("StoryPoint", 17);
            }
        }
    }
}
