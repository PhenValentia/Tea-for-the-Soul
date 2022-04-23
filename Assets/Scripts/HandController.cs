using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    SpriteRenderer handSR;
    Sprite openHand;
    Sprite closeHand;

    Collider2D[] itemsInHand;
    Vector2[] itemsPos;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        handSR = GetComponentInChildren<SpriteRenderer>();
        openHand = handSR.sprite;
        closeHand = Resources.LoadAll<Sprite>("Art/Items/ShiteSheet")[1];
        ////Debug.Log(closeHand.name);

        itemsInHand = new Collider2D[0];
        itemsPos = new Vector2[0];
    }

    // Update is called once per frame
    void Update()
    {
        armFollowMouse();

        if (itemsInHand.Length > 0)
        {
            holdItems();
        }


        if (Input.GetButtonDown("Fire1"))
        {
            grabItems();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            releaseItems();
        }
    }

    void armFollowMouse()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos;

    }

    void grabItems()
    {
        handSR.sprite = closeHand;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1);
        if (cols.Length > 0)
        {
            int numOfGrab = 0;
            foreach (var hitCollider in cols)
            {
                if (hitCollider.tag == "Grabbable")
                {
                    numOfGrab++;
                }
            }
            int counter2 = 0;
            itemsInHand = new Collider2D[numOfGrab];
            itemsPos = new Vector2[numOfGrab];
            foreach (var hitCollider in cols)
            {
                //Debug.Log(hitCollider.gameObject.name);
                if (hitCollider.tag == "Grabbable")
                {
                    //Debug.Log("Item Grabbed: " + hitCollider.gameObject.name);
                    itemsInHand[counter2] = hitCollider;
                    itemsPos[counter2] = hitCollider.transform.position - transform.position;
                    counter2++;
                }
            }
        }
    }

    void releaseItems()
    {
        handSR.sprite = openHand;
        foreach (var item in itemsInHand)
        {
            item.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
            itemsInHand = new Collider2D[0];
        itemsPos = new Vector2[0];
    }

    void holdItems()
    {
        int counter1 = 0;
        //Debug.Log(itemsInHand.Length);
        foreach (var item in itemsInHand)
        {
            if (item.tag == "Grabbable")
            {
                //Debug.Log("Held: " + item.gameObject.name + " Count: " + counter1);
                item.transform.position = transform.position + new Vector3(itemsPos[counter1].x, itemsPos[counter1].y, 0);
                counter1++;
            }
        }
    }
}
