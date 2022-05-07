using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EstablishPrefs : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Cutscene1")
        {
            PlayerPrefs.SetInt("Flower1", 0);
            PlayerPrefs.SetInt("Flower2", 0);
            PlayerPrefs.SetInt("Flower3", 0);
            PlayerPrefs.SetInt("Flower1D", 0);
            PlayerPrefs.SetInt("Flower2D", 0);
            PlayerPrefs.SetInt("Flower3D", 0);
            PlayerPrefs.SetInt("StoryPoint", 0);
        }
        else if (SceneManager.GetActiveScene().name == "Cutscene2")
        {
            PlayerPrefs.SetInt("StoryPoint", 10);
            //PlayerPrefs.SetInt("StoryPoint", 17);
            PlayerPrefs.SetInt("Minigame", 0);
            //PlayerPrefs.SetInt("Minigame", 1);
        }
    }
}
