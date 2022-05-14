using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidTilter : MonoBehaviour
{
    GameObject lid;
    bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        lid = GameObject.Find("Lid");
        moving = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<DropletController>() != null)
        {
            StartCoroutine(openCloseLid(collision.gameObject));
        }
    }

    IEnumerator openCloseLid(GameObject g)
    {
        if (!moving)
        {
            moving = true;
            lid.transform.rotation = Quaternion.Euler(0, 0, lid.transform.rotation.eulerAngles.z - 1);
            while (lid.transform.rotation.eulerAngles.z > 330)
            {
                Debug.Log("rotation: " + lid.transform.rotation.eulerAngles.z);
                lid.transform.rotation = Quaternion.Euler(0, 0, lid.transform.rotation.eulerAngles.z - 1);
                yield return new WaitForFixedUpdate();
            }
            Quaternion.Euler(0, 0, 330);
            while (g != null)
            {
                yield return new WaitForFixedUpdate();
            }
            while (lid.transform.rotation.eulerAngles.z > 5)
            {
                lid.transform.rotation = Quaternion.Euler(0, 0, lid.transform.rotation.eulerAngles.z + 1);
                yield return new WaitForFixedUpdate();
                            }
            lid.transform.rotation = Quaternion.Euler(0, 0, 0);
            moving = false;
        }
    }
}
