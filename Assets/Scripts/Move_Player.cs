using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Move_Player : MonoBehaviour
{
    // Global Variables:
    #region
    Rigidbody rb;
    public Animator anim, spring;
    bool jump, crouch, hanging, jumpedForHang, sneak, dead, stepedOnWaffle, won, inBossLevel, running;
    public float speed, currentAngle;
    float deltax, alignmentPoint;
    public Joystick joystick;
    Vector3 startTouch, pickUpPoint, velocity;
    Trigger_Behaviour triggerScript;
    Transform GFX, Stick, Camera;
    public GameObject vfx;
    #endregion
    private void Start()
    {
        GFX = transform.GetChild(0);
        Stick = GameObject.Find("Canvas").transform.GetChild(2);
        velocity = Vector3.zero;
        deltax = alignmentPoint =  0;
        pickUpPoint = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        sneak = jump = crouch = hanging = jumpedForHang = dead = stepedOnWaffle = false;
        triggerScript = GetComponent<Trigger_Behaviour>();
        inBossLevel = false;
        running = true;
    }
    void Update()
    {
        if (dead != true && won != true)
        {
            if(stepedOnWaffle == true)
            {
                if(rb.velocity.y < 2)
                {
                    anim.SetTrigger("isFalling");
                    StartCoroutine(Die(false));
                }
            }
            if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && !inBossLevel)
            {
                //Input by touch:
                //If Touch has just been started:
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    PreMove();

                //Call Each Frame if there is touch:
                Move();

                //Reset in case the Touch is released and other touch release logics:
                if ((Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled))
                    PostMove();
            }
            else if (inBossLevel)
            {
                Vector2 delta = new Vector2(joystick.Horizontal, joystick.Vertical);
                if (delta.magnitude > .5f && delta.magnitude > 0)
                    MoveBoss();
                else if (running == true)
                {
                    running = false;
                    anim.SetBool("isRunning", false);
                }
            }
            else if ((jump == true || sneak == true) && hanging != true)
            {
                transform.Translate(0, 0, 3 * Time.deltaTime);
            }
            else if (hanging == true)
                anim.SetBool("isHangMoving", false);
            else if (sneak != true)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isDucking", false);
                var collider = GetComponent<CapsuleCollider>();
                collider.height = 1.88f;
                collider.center = new Vector3(0, .85f, 0.1f);
                transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.touchCount <= 0)
                rb.velocity = Vector3.zero;
            if (hanging == true)
            {
                GameObject gm = transform.GetChild(0).gameObject;
                gm.transform.localRotation = Quaternion.Euler(0, -88, 0);
            }
            if (jumpedForHang && transform.position.x != 0)
            {
                float step = speed / 2 * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), step);
             }
        }
        if (won == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, pickUpPoint, 5 * Time.deltaTime);
            //transform.position += (transform.position - new Vector3(pickUpPoint.x, transform.position.y, pickUpPoint.z)).normalized * speed * Time.deltaTime;
            if (transform.position.z == pickUpPoint.z)
            {
                StartCoroutine(Win());
                anim.SetTrigger("Win");
            }
        }
    }
    private void LateUpdate()
    {
        if(Input.touchCount > 0)
        {
            Stick.rotation = Quaternion.Euler(0, 0, -currentAngle);
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                Stick.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void PreMove()
    {
        //Get Start Touch:
        startTouch = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, transform.position.z);

        //Trigger Suitable animation:
        if (crouch != true && hanging != true && sneak != true && speed != 0)
            anim.SetBool("isRunning", true);
        else if (hanging == true)
            anim.SetBool("isHangMoving", true);
        else if (sneak == true)
            anim.speed = 1;
        else if(crouch == true)
        {
            var collider = GetComponent<CapsuleCollider>();
            collider.height = .94f;
            collider.center = new Vector3(0, .425f, 0.1f);
            anim.SetBool("isDucking", true);
        }
    }
    void Move()
    {
        Vector3 moveDirection = Vector3.zero;
        //Apply logics according to the rotation of the Stick in the Canvas:
        //Get Delta X distance to Check for Left or Right movement:
        deltax = Input.GetTouch(0).position.x - startTouch.x;

        //Calculate the currentAngle of the stick Based on the deltax value:
        if(Mathf.Abs(deltax / 5) <= 30)
            currentAngle = deltax / 5;
        else
        {
            if (deltax < 0)
                currentAngle = -30;
            else
                currentAngle = 30;
        }
        if ((Mathf.Abs(currentAngle)) > 5 && hanging != true && sneak != true)
        {
                                                                        //Move Left:
            if (currentAngle < 5 && transform.position.x >= -5)
            {
                if (transform.position.x > -5)
                {
                    moveDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
                    GFX.transform.rotation = Quaternion.Euler(0, currentAngle, 0);
                }
                else
                {
                    moveDirection = Quaternion.Euler(0, 0, 0) * transform.forward;
                    transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }      //Move Right:
            else if (currentAngle > 5 && transform.position.x <= 5)
            {
                if (transform.position.x < 5)
                {
                    moveDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
                    GFX.transform.rotation = Quaternion.Euler(0, currentAngle, 0);
                }
                else
                {
                    moveDirection = Quaternion.Euler(0, 0, 0) * transform.forward;
                    transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }  //At Extreme X-axis Position:
            else if (transform.GetChild(0).transform.rotation.y != 0)
            {
                moveDirection = transform.forward;
                transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
                moveDirection = transform.forward;
        }
        else
        {
            moveDirection = transform.forward;
            if (transform.GetChild(0).transform.rotation.y != 0)
                transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position += moveDirection * speed * Time.deltaTime;

        //Change the start point X coordinate incase the current touch is much more distant than required:
        if(Mathf.Abs(deltax) > 155)
        {
            if (deltax < -155)
                startTouch = new Vector3(Input.GetTouch(0).position.x + 155, startTouch.y, startTouch.z);
            else
                startTouch = new Vector3(Input.GetTouch(0).position.x - 155, startTouch.y, startTouch.z);
        }
    }
    void MoveBoss()
    {
        //Rotate Player:
        Vector3 dir;
        dir = Camera.forward;
        dir.y = 0;
        dir = dir.normalized;
        float relativeAngle = Vector3.Angle(dir, Vector3.forward);
        dir = Vector3.forward - dir;
        float check = Vector3.SignedAngle(dir, Vector3.forward, Vector3.up);
        if (check < 0)
            relativeAngle = 360 - relativeAngle;
        Quaternion A = Quaternion.Euler(0, -(Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg + 270) + relativeAngle, 0);
        Quaternion B = Quaternion.Euler(transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Lerp(B, A, 0.5f);
        if (running == false)
        {
            running = true;
            anim.SetBool("isRunning", true);
        }
        //Move Player:
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void PostMove()
    {
        startTouch = Vector3.zero;
        if (transform.GetChild(0).transform.rotation.y != 0 && hanging != true && sneak != true)
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
        anim.SetBool("isRunning", false);
        if (crouch == true)
        {
            var collider = GetComponent<CapsuleCollider>();
            collider.height = 1.88f;
            collider.center = new Vector3(0, .85f, 0.1f);
            anim.SetBool("isDucking", false);
        }
    }
    void Jump()
    {
        jump = true;
        anim.SetBool("isJumping", true);
        anim.SetBool("isRunning", false);
        rb.velocity = new Vector3(0, 8, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(dead != true)
        {
            if (collision.gameObject.tag == "Platform")
            {
                jump = false;
                anim.SetBool("isJumping", false);
                anim.SetBool("isSneaking", false);
                anim.SetBool("isRunning", true);
            }
            else if (collision.gameObject.tag == "Drown")
            {
                StartCoroutine(Die(false));
                var child = transform.GetChild(0).gameObject;
                anim.SetBool("isRunning", false);
                //anim.SetBool("isJumping", false);
                anim.SetTrigger("isStuck");
                //Adjust rotaition and tranformation for the drownies:
                //child.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                //Now Slowly Drown him by giving a downward velocity of -.6
                var crb = child.AddComponent<Rigidbody>();
                crb.useGravity = false;
                crb.velocity = Vector3.down * .6f;
            }
            else if (collision.gameObject.tag == "Drownies")
            {
                StartCoroutine(Die(false));
                var child = transform.GetChild(0).gameObject;
                child.transform.SetParent(collision.transform);
                anim.SetBool("isRunning", false);
                anim.SetTrigger("isStuck");
                //Adjust rotaition and tranformation for the drownies:
                child.transform.localPosition = new Vector3(0, -1.5f, 0);
                child.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //Now Slowly Drown him by giving a downward velocity of -.6
                var crb = child.AddComponent<Rigidbody>();
                crb.useGravity = false;
                crb.velocity = Vector3.down * .6f;
            }
            else if (collision.gameObject.tag == "Slip")
            {
                speed = 1;
                //Write the logic for slip:
            }
            else if (collision.gameObject.tag == "Slicer")
            {
                //Write the logic for slicer:
                StartCoroutine(Die(true));
            }
            else if (collision.gameObject.tag == "Sneak")
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isSneaking", true);
                jump = false;
                sneak = true;
                speed = 2;
                rb.velocity = Vector3.zero;
                rb.useGravity = false;
                transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
                triggerScript.Align("x", collision.contacts[0].point.x, .6f);
            }
            else if (collision.gameObject.tag == "Stair")
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isRunning", false);
                anim.SetTrigger("Climbing_Up");
                alignmentPoint = collision.transform.position.x;
                rb.useGravity = false;
                velocity = new Vector3(0, .5f, 0);
                anim.speed = .3f;
                speed = 0;
            }
            else if (collision.gameObject.tag == "Moving")
            {
                transform.SetParent(collision.transform);
            }
            else if (collision.gameObject.tag == "Obstacle")
                StartCoroutine(Die(true));
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Moving")
        {
            transform.parent = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(dead != true)
        {
            if (other.tag == "Crouch")
            {
                if (crouch == false)
                {
                    crouch = true;
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isDucking", true);
                    var collider = GetComponent<CapsuleCollider>();
                    collider.height = .94f;
                    collider.center = new Vector3(0, .425f, 0.1f);
                }
                else
                {
                    crouch = false;
                    anim.SetBool("isDucking", false);
                    anim.SetBool("isRunning", true);
                    var collider = GetComponent<CapsuleCollider>();
                    collider.height = 1.88f;
                    collider.center = new Vector3(0, .85f, 0.1f);
                }
            }
            else if (other.tag == "Jump")
                Jump();
            else if (other.tag == "Mushroom")
            {
                anim.SetTrigger("Poisoned");
                StartCoroutine(Die(false));
            }
            else if (other.gameObject.tag == "Waffles")
            {
                //Activate the Chunks of Waffle and Deactivate the Bigger one:
                other.gameObject.transform.parent.transform.GetChild(1).gameObject.SetActive(false);
                other.gameObject.transform.parent.transform.GetChild(0).gameObject.SetActive(true);
                //Active the condition for the waffle break to check if the player is falling {If yes then we will play the fall animation and kill him}:
                stepedOnWaffle = true;
            }
            else if (other.gameObject.tag == "Spring")
            {
                jump = true;
                //Align
                alignmentPoint = 0;
                velocity = new Vector3(0, 0, 7);
                rb.velocity = new Vector3(0, 16, 0);
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);
                spring.SetTrigger("springTrigger");
                triggerScript.Align("x", other.transform.position.x, 4f);
                //spring.SetBool("spring", true);
            }
            else if (other.tag == "Sneak")
            {
                sneak = false;
                anim.speed = 1;
                anim.SetBool("isSneaking", false);
                speed = 7;
                jump = true;
                rb.useGravity = true;
                rb.velocity = new Vector3(0, 5, 0);
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);
            }
            else if (other.tag == "Hang")
            {
                if (hanging == false)
                {
                    Jump();
                    jumpedForHang = true;
                }
                else
                {
                    speed = 7;
                    anim.SetTrigger("isDropping");
                    GameObject gm = transform.GetChild(0).gameObject;
                    gm.transform.localPosition = new Vector3(.35f, gm.transform.localPosition.y, gm.transform.localPosition.z);
                    gm.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rb.useGravity = true;
                    hanging = false;
                }
            }
            else if (other.tag == "Jump_Up")
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);
                //Align
                rb.velocity = new Vector3(0, 12, 0);
                jumpedForHang = true;
            }
            else if (other.tag == "Bar")
            {
                hanging = true;
                jumpedForHang = false;
                speed = 4f;
                anim.SetBool("isHanging", true);
                if (Input.touchCount > 0)
                    anim.SetBool("isHangMoving", true);
                anim.SetBool("isJumping", false);
                GameObject gm = transform.GetChild(0).gameObject;
                if(jump == true)
                    gm.transform.localPosition = new Vector3(.35f, gm.transform.localPosition.y, gm.transform.localPosition.z);
                else
                    gm.transform.localPosition = new Vector3(.4f, gm.transform.localPosition.y, gm.transform.localPosition.z);
                gm.transform.localRotation = Quaternion.Euler(0, -88, 0);
                if(jump == true)
                    transform.position = new Vector3(0, transform.position.y + .4f, transform.position.z);
                else
                    transform.position = new Vector3(0, 13.2f, transform.position.z);
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }
            else if (other.tag == "Slip")
            {
                anim.SetTrigger("Slide");
                speed = 0;
                rb.AddForce(0, 0, 12, ForceMode.VelocityChange);
            }
            else if (other.tag == "FinishedRace")
            {
                won = true;
                pickUpPoint = other.gameObject.transform.parent.Find("PickUpPoint").position;
                pickUpPoint = new Vector3(pickUpPoint.x, transform.position.y, pickUpPoint.z);
                transform.GetChild(0).transform.rotation = Quaternion.Euler(pickUpPoint - transform.position);
                anim.SetBool("isRunning", true);
                speed = 0;
                //Assign Player Transform to the Cinemachine cameras:
                GameObject.Find("Platform").transform.GetChild(2).GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
                GameObject.Find("Platform").transform.GetChild(2).GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = transform;

                GameObject.Find("Platform").transform.GetChild(2).GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 2;
            }
        }
    }
    public void Dead(bool deadAnim)
    {
        StartCoroutine(Die(deadAnim));
    }
    public IEnumerator Die(bool playDeadAnim)
    {
        //Write Death Logic and after Death Logic:
        speed = 0;
        dead = true;
        Handheld.Vibrate();
        if (playDeadAnim)
            anim.SetTrigger("Dead");
        yield return new WaitForSeconds(3f);
        //Write the actions to be executed after 3 second of time lag:
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator Win()
    {
        //Write Win Race Logic:
        anim.SetTrigger("Win");
        yield return new WaitForSeconds(6f);
        //Write the actions to be executed after Win:
        PlayerPrefs.SetInt("NEWGAME", 0);
        if (SceneManager.GetActiveScene().name == "BOSS")
        {
            int x = PlayerPrefs.GetInt("BOSS", 0);
            x++;
            PlayerPrefs.SetInt("BOSS", x);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CURRENTLEVEL", 0));
        }
        else if ((SceneManager.GetActiveScene().buildIndex + 1) % 3 == 0)
        {
            PlayerPrefs.SetInt("CURRENTLEVEL", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene("BOSS");
        }
        else
        {
            PlayerPrefs.SetInt("CURRENTLEVEL", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CURRENTLEVEL", 0));
        }
        Debug.Log("CURRENTLEVEL: " + PlayerPrefs.GetInt("CURRENTLEVEL", 0));
        Debug.Log("BOSS: " + PlayerPrefs.GetInt("BOSS", 0));
    }
    public void PlayerSpeedValue(int s)
    {
        speed = s;
        Debug.Log(speed);
        if (Input.touchCount > 0)
            anim.SetBool("isRunning", true);
    }
    public void BossLevel()
    {
        inBossLevel = true;
        Camera = GameObject.Find("Boss_Level").transform.GetChild(0).GetChild(0);
        anim.SetBool("isRunning", false);
    }
    public void Won()
    {
        speed = 0;
        StartCoroutine(Win());
        anim.SetTrigger("Win");
    }
    public void Blast(Vector3 position)
    {
        if (inBossLevel || (triggerScript.CurrentItem() == 5))
        {
            GameObject gm = Instantiate(vfx);
            gm.transform.position = position;
        }
        else
            return;
    }
}