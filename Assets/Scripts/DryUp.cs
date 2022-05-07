using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryUp : MonoBehaviour
{
    //public Material[] materials;//allow input of different materials related to the different drying states of tea
    SpriteRenderer rend;

    ParticleSystem cookP;
    ParticleSystem cookedP;
    ParticleSystem burntP;
    ParticleSystem fixP;

    bool finished = false;

    float cooked = 1;

    //private int index = 1;//initialize of 1, and need to put different materials in order

    // Start is called before the first frame update
    void Start()
    {
        cooked = Random.Range(0.8f, 1);
        finished = false;
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = true;
        cookP = GetComponentsInChildren<ParticleSystem>()[0];
        cookedP = GetComponentsInChildren<ParticleSystem>()[1];
        burntP = GetComponentsInChildren<ParticleSystem>()[2];
        fixP = GetComponentsInChildren<ParticleSystem>()[3];
        cookP.Stop();
        cookedP.Stop();
        burntP.Stop();
        fixP.Stop();
    }

    public bool getFinished()
    {
        return finished;
    }

    private void Update()
    {
        rend.color = new Color(cooked, cooked, cooked, 1);
        if(cooked < 0.6 && cooked > 0.5 && !finished)
        {
            cookedP.Play();
            finished = true;
            Debug.Log("Cooked");
        }
        if(cooked < 0.5 && finished)
        {
            burntP.Play();
            cooked = 0;
            finished = false;
            Debug.Log("Burnt");
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && PlayerPrefs.GetInt("Minigame") == 1)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = transform.position;
            float distance = (mousePos - pos).magnitude;
            if (distance < 0.2)
            {
                cooked -= 0.01f * (0.2f - distance);
                cookP.Play();
                //Debug.Log("Cooked: " + cooked);
            }
            else
            {
                cookP.Stop();
            }
        }
        else
        {
            cookP.Stop();
        }
        if (cooked < 0.5 && Input.GetButton("Interact") && PlayerPrefs.GetInt("Minigame") == 1)
        {
            fixP.Play();
            cookedP.Stop();
            burntP.Stop();
            cooked = 1;
        }
    }

    // Update is called once per frame
    /*void OnMouseDown()
    {
        if (materials.Length == 0) // if there are no materials present nothing happens
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            index += 1;//when mouse is pressed down, the tea become the next material
            if (index == materials.Length + 1)//if the tea is already black, nothing will happen
            {
                return;
            }
            print(index);//for debugging

            rend.sharedMaterial = materials[index - 1];
        }
    }*/
}