using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialText : MonoBehaviour
{
    CanvasGroup cg;

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        if(SceneManager.GetActiveScene().name == "HouseExterior" && PlayerPrefs.GetInt("StoryPoint") == 8)
        {
            cg.alpha = 1;
            StartCoroutine(fadeTutorial());
        }
        else
        {
            cg.alpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator fadeTutorial()
    {
        yield return new WaitForSeconds(10);
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.01f;
            yield return new WaitForFixedUpdate();
        }
        cg.alpha = 0;
    }
}
