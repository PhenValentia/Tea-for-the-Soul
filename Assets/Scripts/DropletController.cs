using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletController : MonoBehaviour
{
    [SerializeField]
    float mouseAttractRange = 3f;
    [SerializeField]
    float accel = 1f;
    [SerializeField]
    float deccel = 0.01f;
    Rigidbody2D rb;
    SpriteRenderer sr;
    public int size = 1;
    public int dropID = 0;
    float startScale = 0;
    [SerializeField]
    bool decreasing = false;
    bool moving = false;
    CapsuleCollider2D c;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        c = GetComponent<CapsuleCollider2D>();
        startScale = transform.localScale.x;
        decreasing = false;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = size;
        if (Input.GetButton("Fire1") && PlayerPrefs.GetInt("Minigame") == 2 && !moving)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            //Debug.Log((mousePos - transform.position).magnitude);
            if ((mousePos - transform.position).magnitude < mouseAttractRange)
            {
                //Debug.Log("Movement");
                Vector3 toMove = Vector3.Normalize(mousePos - transform.position) * accel * Time.deltaTime;
                rb.velocity += new Vector2(Mathf.Clamp(toMove.x, -10f, 10f), Mathf.Clamp(toMove.y, -10f, 10f));
            }
        }
        if (rb.velocity.magnitude > 0)
        {
            if(rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - deccel * Time.deltaTime, rb.velocity.y);
            }
            else if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x + deccel * Time.deltaTime, rb.velocity.y);
            }
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - deccel * Time.deltaTime);
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + deccel * Time.deltaTime);
            }
        }
        Debug.Log("Test: " + (transform.localScale.magnitude < (Vector3.one * size * 0.1f * startScale).magnitude));
        if (transform.localScale.magnitude < (Vector3.one * size * startScale).magnitude && !decreasing)
        {
            transform.localScale += Vector3.one * Time.deltaTime * startScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<DropletController>() != null)
        {
            c.enabled = false;
            DropletController dC = collision.gameObject.GetComponent<DropletController>();
            if (dC.size > size)
            {
                StartCoroutine(moveToDropAndDestroy(collision.gameObject));
            }
            else if (dC.size < size)
            {
                StartCoroutine(moveToDropAndIncrease(collision.gameObject, dC.size));
            }
            else if (dC.dropID > dropID)
            {
                StartCoroutine(moveToDropAndDestroy(collision.gameObject));
            }
            else
            {
                StartCoroutine(moveToDropAndIncrease(collision.gameObject, dC.size));
            }
        }
        else if (collision.gameObject.name == "LidCol")
        {
            Debug.Log("Lid Collided");
            StartCoroutine(decreaseIntoPot(collision.transform.position));
        }
    }

    IEnumerator moveToDropAndDestroy(GameObject obj)
    {
        moving = true;
        obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        while ((obj.transform.position - transform.position).magnitude > 0.1f)
        {
            transform.position = Vector3.Normalize(obj.transform.position - transform.position)*Time.deltaTime + transform.position;
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.01f);
        Debug.Log("Destroyed");
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator moveToDropAndIncrease(GameObject objectIn, int sizeIn)
    {
        moving = true;
        int sizeIncrease = sizeIn;
        size += sizeIncrease;
        Vector3 targetPos = transform.position + (objectIn.transform.position - transform.position) / 2;
        objectIn.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        while ((targetPos - transform.position).magnitude > 0.1f)
        {
            transform.position = Vector3.Normalize(targetPos - transform.position) * Time.deltaTime + transform.position;
            yield return new WaitForSeconds(0.01f);
        }
        moving = false;
        c.enabled = true;
    }

    IEnumerator decreaseIntoPot(Vector3 target)
    {
        moving = true;
        decreasing = true;
        rb.velocity = Vector2.zero;
        while ((target - transform.position).magnitude > 0.01f)
        {
            transform.position = Vector3.Normalize(target - transform.position) * Time.deltaTime * 0.1f + transform.position;
            yield return new WaitForFixedUpdate();
        }
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * 1f * Time.deltaTime * startScale;
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = Vector3.zero;
        Debug.Log("PutInPot");
        Destroy(this.gameObject);
    }
}
