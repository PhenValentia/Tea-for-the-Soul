using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "HouseInterior" || SceneManager.GetActiveScene().name == "HouseExterior" || SceneManager.GetActiveScene().name == "Forest")
        {
            GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
