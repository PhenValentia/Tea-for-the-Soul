using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWater : MonoBehaviour
{
    DropletController[] drops;
    int totalDrops = 0;

    // Start is called before the first frame update
    void Start()
    {
        drops = GetComponentsInChildren<DropletController>();
        totalDrops = drops.Length;
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        foreach(DropletController d in drops)
        {
            if(d == null)
            {
                counter++;
            }
        }
        if(counter == totalDrops && PlayerPrefs.GetInt("StoryPoint") == 21)
        {
            PlayerPrefs.SetInt("StoryPoint", 22);
        }
    }
}
