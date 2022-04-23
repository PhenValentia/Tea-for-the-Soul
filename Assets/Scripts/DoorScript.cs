using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    string nextLevel = "Not Set";
    CircleTransition ct;

    // Start is called before the first frame update
    void Start()
    {
        ct = GameObject.Find("CircleWipeMask").GetComponent<CircleTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetButton("Interact"))
        {
            Debug.Log("test");
            ct.endLevel(nextLevel, GameObject.Find("Lapis").transform);
        }
    }
}
