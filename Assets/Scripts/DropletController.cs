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
    public int size = 1;
    public int dropID = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
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
        if (transform.localScale.magnitude < (Vector3.one * size * 0.1f).magnitude)
        {
            transform.localScale += Vector3.one * 0.1f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<DropletController>() != null)
        {
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
    }

    IEnumerator moveToDropAndDestroy(GameObject obj)
    {
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
        int sizeIncrease = sizeIn;
        size += sizeIncrease;
        Vector3 targetPos = transform.position + (objectIn.transform.position - transform.position) / 2;
        objectIn.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        while ((targetPos - transform.position).magnitude > 0.1f)
        {
            transform.position = Vector3.Normalize(targetPos - transform.position) * Time.deltaTime + transform.position;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
