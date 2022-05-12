using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    AudioClip clip;
    AudioSource aS;
    float fadeTime = 5;
    [SerializeField]
    float vol = 1;
    string musicName;

    // Start is called before the first frame update
    void Start()
    {
        aS = GameObject.Find("Lapis").GetComponent<AudioSource>();
        //aS = Camera.main.GetComponent<AudioSource>();
        vol = aS.volume;
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                musicName = "MenuMusic";
                break;
            case "HouseExterior":
                musicName = "OutdoorMusic";
                break;
            case "HouseInterior":
                musicName = "OutdoorMusic";
                break;
            case "Forest":
                musicName = "OutdoorMusic";
                break;
            case "Cutscene1":
                musicName = "MinigameMusic";
                break;
            case "Cutscene2":
                musicName = "MinigameMusic";
                break;
            default:
                break;
        }
        clip = Resources.Load<AudioClip>("Audio/" + musicName);
        aS.clip = clip;
        aS.loop = true;
        aS.volume = 0;
        aS.time = PlayerPrefs.GetFloat(musicName);
        aS.Play();
    }

    IEnumerator fadeInMusic()
    {
        float f = 0;
        while(f < fadeTime)
        {
            aS.volume = f / fadeTime;
            f += 0.02f;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator fadeOutMusic()
    {
        float f = 0;
        while (f < fadeTime)
        {
            aS.volume = f / fadeTime;
            f += 0.02f;
            yield return new WaitForFixedUpdate();
        }
    }

    public void setVol(float newVol)
    {
        aS.volume = newVol * vol;
        //Debug.Log(newVol * vol);
    }

    public void endLevel()
    {
        StopCoroutine(fadeInMusic());
    }

    public void saveTime()
    {
        PlayerPrefs.SetFloat(musicName, aS.time);
    }
}
