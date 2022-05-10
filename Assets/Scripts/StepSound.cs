using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepSound : MonoBehaviour
{
    string floor = "Wood";
    AudioSource aS;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "HouseInterior":
                floor = "Wood";
                break;
            case "HouseExterior":
                floor = "Grass";
                break;
            default:
                break;
        }
    }

    public void step()
    {
        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.PlayOneShot(Resources.Load<AudioClip>("Audio/"+floor+"Step"));
    }
}
