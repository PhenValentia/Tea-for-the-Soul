using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;  
    public GameObject PauseMenuUI; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GameIsPaused && SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "OptionMenu" && SceneManager.GetActiveScene().name != "EndScreen") 
            {
                Resume(); 
            } else 
            {
                Pause(); 
            }
        }
    } 

    public void Resume ()
    {
      PauseMenuUI.SetActive(false); 
      Time.timeScale = 1f; 
      GameIsPaused = false;   
    }  
    public void QuitGame()
    {
        
        Application.Quit(); 
    }

    public void Pause () 
    {
      PauseMenuUI.SetActive(true); 
      Time.timeScale = 0f; 
      GameIsPaused = true; 
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }




}


