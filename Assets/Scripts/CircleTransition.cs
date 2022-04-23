using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CircleTransition : MonoBehaviour
{
    RectTransform rt;
    Transform startTarget;
    [SerializeField]
    string circleOpenTargetName = "Lapis";
    [SerializeField]
    string nextLevel;
    [SerializeField]
    int zoomSpeed = 20;

    public bool waitingToTransition;


    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        startTarget = GameObject.Find(circleOpenTargetName).transform;
        startLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            startLevel();
        }
        if (Input.GetKey(KeyCode.P))
        {
            endLevel(nextLevel, startTarget);
        }
    }

    IEnumerator startLevelTransition()
    {
        waitingToTransition = true;
        for (float f = 0; f < getScreenRadius(); f = f + zoomSpeed)
        {
            yield return new WaitForSeconds(0.01f);
            rt.sizeDelta = new Vector2(f, f);
        }
        waitingToTransition = false;
    }

    IEnumerator endLevelTransition()
    {
        waitingToTransition = true;
        for (float f = getScreenRadius(); f > 0; f = f - zoomSpeed)
        {
            yield return new WaitForSeconds(0.01f);
            rt.sizeDelta = new Vector2(f, f);
        }
        rt.sizeDelta = new Vector2(0, 0);
        waitingToTransition = false;
        SceneManager.LoadScene(nextLevel);
    }

    public void startLevel()
    {
        rt.anchoredPosition = Camera.main.ScreenToWorldPoint(startTarget.position);
        zeroScreenCircle();
        StartCoroutine(startLevelTransition());
    }

    public void endLevel(string levelName, Transform target)
    {
        rt.anchoredPosition = Camera.main.ScreenToWorldPoint(target.position);
        nextLevel = levelName;
        resetScreenCircle();
        StartCoroutine(endLevelTransition());
    }

    public void resetScreenCircle()
    {
        float newSize = getScreenRadius();
        //Debug.Log(newSize + " VS " + Screen.height + " : " + Screen.width);
        rt.sizeDelta = new Vector2(newSize, newSize);
    }

    public void zeroScreenCircle()
    {
        rt.sizeDelta = new Vector2(0,0);
    }

    public float getScreenRadius()
    {
        return 2 * Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
    }
}
