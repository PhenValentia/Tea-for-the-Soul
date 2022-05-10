using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float walkSpeed = 5f;
    [SerializeField]
    float jumpInitialSpeed = 10f;
    [SerializeField]
    [Tooltip("In Seconds")]
    float timeBeforeJumpAgain = 0.3f;
    bool jumpInitiated = false;
    public bool dropInitiated = false;
    float groundedDetectionRange = 0.1f;
    Collider2D col;
    LayerMask ignorePlayerMask;
    Animator anim;
    GameObject playerModel;
    float pmScale;

    [SerializeField]
    bool allowMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        playerModel = transform.Find("PlayerModel").gameObject;
        pmScale = playerModel.transform.localScale.x;
        if(SceneManager.GetActiveScene().name == "HouseInterior")
        {
            playerModel.transform.localScale = new Vector3(-pmScale, pmScale, pmScale);
        }
        ignorePlayerMask = ~(1 << 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMovement)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * walkSpeed, rb.velocity.y);
            //Debug.Log("NewVel: "+ Input.GetAxis("Horizontal") * walkSpeed + " CurVel: "+rb.velocity.x);
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            if(rb.velocity.x > 0)
            {
                playerModel.transform.localScale = new Vector3(-pmScale, pmScale, pmScale);
            }
            else if (rb.velocity.x < 0)
            {
                playerModel.transform.localScale = new Vector3(pmScale, pmScale, pmScale);
            }
            if (SceneManager.GetActiveScene().name == "Cutscene1")
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else if(Input.GetAxis("Horizontal") != 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            if (Input.GetButtonDown("Jump") && Grounded() && !jumpInitiated)
            {
                //jumpInitiated = true;
                //StartCoroutine(resetJump());
                rb.AddForce(Vector2.up * jumpInitialSpeed, ForceMode2D.Impulse);
            }
            /*if (Input.GetAxis("Vertical") < 0)
            {
                //IgnoreCollision();
                dropInitiated = true;
                Debug.Log("Drop Initiated");
            }
            else
            {
                dropInitiated = false;
            }*/
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    bool Grounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(transform.position, new Vector2(col.bounds.size.x, groundedDetectionRange), 0f, Vector2.down, col.bounds.size.y / 2, ignorePlayerMask);
        if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return true;
        }
        else if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Dropdown"))
        {
            Physics2D.IgnoreCollision(col, ray.collider, true);
            return true;
        }
        return false;
    }
    
    void IgnoreCollision()
    {
        RaycastHit2D ray = Physics2D.BoxCast(transform.position, new Vector2(col.bounds.size.x, groundedDetectionRange), 0f, Vector2.down, col.bounds.size.y / 2, ignorePlayerMask);
        if(ray.collider.gameObject.layer == LayerMask.NameToLayer("Dropdown"))
        {
            //Physics2D.IgnoreCollision(col, ray.collider, true);
        }
    }

    IEnumerator resetJump()
    {
        yield return new WaitForSeconds(timeBeforeJumpAgain);
        jumpInitiated = false;
    }

    public void disableMovement()
    {
        allowMovement = false;
    }

    public void enableMovement()
    {
        allowMovement = true;
    }

    public void toggleMovement()
    {
        allowMovement = !allowMovement;
    }
}
