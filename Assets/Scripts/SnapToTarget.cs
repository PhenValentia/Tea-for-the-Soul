using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToTarget : MonoBehaviour
{
    [SerializeField]
    string nameOfTarget = "Not Set";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TargetLocation" && collision.name == nameOfTarget)
        {
            tag = "Untagged";
            StartCoroutine(moveToTarget(collision.transform.position));
        }
    }

    IEnumerator moveToTarget(Vector3 target)
    {
        while ((target - transform.position).magnitude > 0.1f)
        {
            transform.position = Vector3.Normalize(target - transform.position) * 50f * Time.deltaTime * Mathf.Clamp((target - transform.position).magnitude, 0.01f, 1f) + transform.position;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
