using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparksController : MonoBehaviour
{
    ParticleSystem steelPS;
    [SerializeField]
    float timeBeforeSparksStop = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        steelPS = GameObject.FindGameObjectsWithTag("Sparks")[0].GetComponent<ParticleSystem>();
        steelPS.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.tag == "Steel")
        {
            Debug.Log("Spark Col");
            steelPS.Play();
            StartCoroutine(resetSparks(timeBeforeSparksStop));
        }
    }

    IEnumerator resetSparks(float timeBeforeReset)
    {
        yield return new WaitForSeconds(timeBeforeReset);
        steelPS.Stop();
    }
}
