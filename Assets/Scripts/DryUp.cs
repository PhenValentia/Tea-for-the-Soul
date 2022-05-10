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
    bool fix = false;
    bool cooking = false;
    bool lastCooked = false;

    float cooked = 1;

    AudioSource aS;

    //private int index = 1;//initialize of 1, and need to put different materials in order

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        transform.localScale = new Vector3(Random.Range(0.05f, 0.15f), Random.Range(0.05f, 0.15f), Random.Range(0.05f, 0.15f));
        aS = GetComponent<AudioSource>();
        aS.loop = true;
        aS.clip = Resources.Load<AudioClip>("Audio/Crackle");
        cooked = Random.Range(0.8f, 1);
        finished = false;
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = true;
        cookP = GetComponentsInChildren<ParticleSystem>()[0];
        cookedP = GetComponentsInChildren<ParticleSystem>()[1];
        burntP = GetComponentsInChildren<ParticleSystem>()[2];
        fixP = GetComponentsInChildren<ParticleSystem>()[3];
        cookP.transform.rotation = Quaternion.Euler(Vector3.zero);
        cookedP.transform.rotation = Quaternion.Euler(Vector3.zero);
        burntP.transform.rotation = Quaternion.Euler(Vector3.zero);
        fixP.transform.rotation = Quaternion.Euler(Vector3.zero);
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
            aS.PlayOneShot(Resources.Load<AudioClip>("Audio/Done"));
            Debug.Log("Cooked");
        }
        if(cooked < 0.5 && finished)
        {
            burntP.Play();
            cooked = 0;
            finished = false;
            aS.PlayOneShot(Resources.Load<AudioClip>("Audio/Char"));
            Debug.Log("Burnt");
        }
        if(fix == true)
        {
            fix = false;
            Debug.Log("PlayingFix: "+ Resources.Load<AudioClip>("Audio/Fix"));
            aS.PlayOneShot(Resources.Load<AudioClip>("Audio/Fix"));
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
                cooking = true;
                //Debug.Log("Cooked: " + cooked);
            }
            else
            {
                cooking = false;
                cookP.Stop();
            }
        }
        else
        {
            cooking = false;
            cookP.Stop();
        }
        if (cooked < 0.5 && Input.GetButton("Interact") && PlayerPrefs.GetInt("Minigame") == 1)
        {
            fix = true;
            fixP.Play();
            cookedP.Stop();
            burntP.Stop();
            cooked = 1;
        }
        if (cooking == true && lastCooked == false)
        {
            aS.Play();
        }
        else if (cooking == false && lastCooked == true)
        {
            aS.Stop();
        }
        lastCooked = cooking;
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