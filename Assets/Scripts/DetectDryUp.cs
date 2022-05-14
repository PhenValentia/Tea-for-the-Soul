using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDryUp : MonoBehaviour
{
    int totalLeaves = 0;
    DryUp[] leavesArray;

    // Start is called before the first frame update
    void Start()
    {
        leavesArray = GetComponentsInChildren<DryUp>();
        totalLeaves = leavesArray.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int counter = 0;
        foreach (DryUp d in leavesArray)
        {
            if (d.getFinished())
            {
                counter++;
            }
        }
        if(counter == totalLeaves && PlayerPrefs.GetInt("StoryPoint") == 16)
        {
            PlayerPrefs.SetInt("StoryPoint", 17);
        }
    }
}
