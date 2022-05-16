using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    float targetScale;

    CanvasGroup interact;
    IEnumerator lastCo;
    bool fading = false;

    [SerializeField]
    bool allowMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        interact = GameObject.Find("ButtonPrompt").GetComponent<CanvasGroup>();
        interact.alpha = 0;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        playerModel = transform.Find("PlayerModel").gameObject;
        pmScale = playerModel.transform.localScale.x;
        if(SceneManager.GetActiveScene().name == "HouseInterior" || SceneManager.GetActiveScene().name == "HouseExterior")
        {
            playerModel.transform.localScale = new Vector3(-pmScale, pmScale, pmScale);
            targetScale = -pmScale;
        }
        if (SceneManager.GetActiveScene().name == "HouseInterior" && PlayerPrefs.GetInt("StoryPoint") == 7)
        {
            playerModel.transform.localScale = new Vector3(pmScale, pmScale, pmScale);
            targetScale = pmScale;
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
                targetScale = -pmScale;
                playerModel.transform.localScale = new Vector3(-pmScale, pmScale, pmScale);
                playerModel.transform.localPosition = new Vector3(0.4f ,playerModel.transform.localPosition.y, playerModel.transform.localPosition.z);
            }
            else if (rb.velocity.x < 0)
            {
                targetScale = pmScale;
                playerModel.transform.localScale = new Vector3(pmScale, pmScale, pmScale);
                playerModel.transform.localPosition = new Vector3(-0.4f, playerModel.transform.localPosition.y, playerModel.transform.localPosition.z);
            }

            /*if(playerModel.transform.localScale.x < targetScale)
            {
                if (playerModel.transform.localScale.x > pmScale / 2)
                {
                    Debug.Log("FlipA");
                    playerModel.transform.localScale = new Vector3(-playerModel.transform.localScale.x, pmScale, pmScale);
                    playerModel.transform.localPosition = new Vector3(-0.4f, playerModel.transform.localPosition.y, playerModel.transform.localPosition.z);
                }
                else
                {
                    Debug.Log("ChangeA");
                    playerModel.transform.localScale = new Vector3(playerModel.transform.localScale.x + (Mathf.Abs(targetScale - playerModel.transform.localScale.x)), pmScale, pmScale);
                }
            }
            else if (playerModel.transform.localScale.x > targetScale)
            {
                if (playerModel.transform.localScale.x < -pmScale / 2)
                {
                    Debug.Log("FlipB");
                    playerModel.transform.localScale = new Vector3(-playerModel.transform.localScale.x, pmScale, pmScale);
                    playerModel.transform.localPosition = new Vector3(0.4f, playerModel.transform.localPosition.y, playerModel.transform.localPosition.z);
                }
                else
                {
                    Debug.Log("ChangeB");
                    playerModel.transform.localScale = new Vector3(playerModel.transform.localScale.x - (Mathf.Abs(targetScale - playerModel.transform.localScale.x)), pmScale, pmScale);
                }
            }*/

            if (SceneManager.GetActiveScene().name == "Cutscene1")
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                allowMovement = false;
            }
            else if(Input.GetAxis("Horizontal") != 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            /*if (Input.GetButtonDown("Jump") && Grounded() && !jumpInitiated)
            {
                //jumpInitiated = true;
                //StartCoroutine(resetJump());
                rb.AddForce(Vector2.up * jumpInitialSpeed, ForceMode2D.Impulse);
            }*/
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            if(SceneManager.GetActiveScene().name == "HouseInterior")
            {
                //interact.GetComponent<RectTransform>().anchorMin = new Vector2(0.1f, 0.55f);
                //interact.GetComponent<RectTransform>().anchorMax = new Vector2(0.2f, 0.65f);
                //interact.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            }
            if (fading)
            {
                StopCoroutine(lastCo);
            }
            fading = true;
            lastCo = fadeIn();
            StartCoroutine(lastCo);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            if (fading)
            {
                StopCoroutine(lastCo);
            }
            fading = true;
            lastCo = fadeOut();
            StartCoroutine(lastCo);
        }
    }

    IEnumerator fadeIn()
    {
        while(interact.alpha < 1 && fading)
        {
            interact.alpha += 0.05f;
            yield return new WaitForFixedUpdate();
        }
        fading = false;
    }

    IEnumerator fadeOut()
    {
        while (interact.alpha > 0 && fading)
        {
            interact.alpha -= 0.05f;
            yield return new WaitForFixedUpdate();
        }
        fading = false;
    }
}
