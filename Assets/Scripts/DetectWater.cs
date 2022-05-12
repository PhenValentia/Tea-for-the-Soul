using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectWater : MonoBehaviour
{
    DropletController[] drops;
    int totalDrops = 0;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        drops = GetComponentsInChildren<DropletController>();
        foreach (DropletController d in drops)
        {
            if (d != null)
            {
                totalDrops += d.size;
            }
        }
        text = GameObject.Find("CounterText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("StoryPoint") > 18)
        {
            text.text = "0/" + totalDrops;
        }
        if (PlayerPrefs.GetInt("Minigame") == 2)
        {
            text.text = PlayerPrefs.GetInt("WaterCollected") + "/" + totalDrops;
            if (PlayerPrefs.GetInt("WaterCollected") == totalDrops && PlayerPrefs.GetInt("StoryPoint") == 21)
            {
                Debug.Log("SettingStory22");
                PlayerPrefs.SetInt("StoryPoint", 22);
            }
        }
    }
}
