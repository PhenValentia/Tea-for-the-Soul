using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownScript : MonoBehaviour
{
    Collider2D physCol;
    Collider2D detectionCol;
    MovementController mC;
    bool isPhysColDetecting = false;

    // Start is called before the first frame update
    void Start()
    {
        detectionCol = GetComponent<Collider2D>();
        physCol = transform.parent.GetComponent<Collider2D>();
        mC = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Checking Stay of any kind: "+collision.gameObject.tag);
        if(collision.gameObject.tag == "Player" && !isPhysColDetecting && !mC.dropInitiated)
        {
            //Debug.Log("Trigger Stay Success");
            physCol.isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            physCol.isTrigger = true;
        }
    }

    public void relayPlayerPresence()
    {
        isPhysColDetecting = true;
        physCol.isTrigger = true;

    }

    public void relayPlayerAbsence()
    {
        isPhysColDetecting = false;
    }
}
