using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField]
    int flowerNum = 0;
    FlowerGather fg;
    bool collected = false;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Flower" + flowerNum) == 1)
        {
            collected = true;
            Destroy(this.gameObject);
        }
        //Debug.Log(GameObject.Find("FlowerIcon" + flowerNum).GetComponent<FlowerGather>());
        fg = GameObject.Find("FlowerIcon" + flowerNum).GetComponent<FlowerGather>();
        ps = GameObject.Find("Plant" + flowerNum + "Stem").GetComponentInChildren<ParticleSystem>();
        ps.Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("col");
        if(Input.GetButton("Interact") && collision.tag == "Player" && collected == false)
        {
            collected = true;
            Debug.Log("Collected");
            fg.collectFlower(transform.position);
            Destroy(this.gameObject);
            ps.Play();
        }
    }
}
