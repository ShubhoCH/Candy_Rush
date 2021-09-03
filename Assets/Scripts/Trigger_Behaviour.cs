using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Trigger_Behaviour : MonoBehaviour
{
    // Global Variables:
    #region
    Rigidbody rb;
    public Animator anim;
    Vector3 targetPosition;
    Move_Player move_Player;
    Manager_Boss manager_Boss;
    Transform targetTransform;
    float alignValue, alignSpeed;
    public GameObject pickUpTaken;
    Transform weaponPosition, Bones;
    GameObject currentPickUpItem;
    bool move, align, isInZipline, isSwitchingZipline, alignBones, lookAndShoot;
    #endregion

    //List of PickUpItems:
    public GameObject[] pickUpItems;

    // [0] = Gun_Bullet
    public GameObject[] pickUpChilds_Items;

    //This will define the current skill the player is having besed on the index and what index corresponds to what is written in thr on TriggerEnter Methord:
    int triggerIndex;

    private void Start()
    {
        Bones = transform.GetChild(0).GetChild(1);
        weaponPosition = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        move = align = isSwitchingZipline = isInZipline = alignBones = lookAndShoot = false;
        triggerIndex = 0;
        rb = GetComponent<Rigidbody>();
        move_Player = GetComponent<Move_Player>();
        //manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    private void Update()
    {
        if (move == true)
            MoveTo();
        if(align == true)
            Align();
        if (alignBones == true)
            Reset();
        if (lookAndShoot == true)
            Rotate_Shoot();
    }

    private void Reset()
    {
        transform.GetChild(0).transform.localPosition = -Bones.transform.localPosition + new Vector3(0, .5f, 0);
    }

    //Move the Player to a certain position automatically:
    void MoveTo()
    {
        int speedOfMove = 4;
        if(triggerIndex != 7)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedOfMove * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            move = false;
            isSwitchingZipline = false;
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            //Instantaiate the corresponding PickUp Item:
            if (triggerIndex == 1 || triggerIndex == 7)
            {
                //Instantite the Zip_Handle(at index -> 0) as it will be used for both 1 & 7:
                currentPickUpItem = Instantiate(pickUpItems[0]);
                currentPickUpItem.transform.SetParent(transform);
                currentPickUpItem.AddComponent<PickUpItem>();
                if (triggerIndex != 7)
                    currentPickUpItem.GetComponent<PickUpItem>().childItem = pickUpChilds_Items[triggerIndex - 1];
                transform.SetParent(targetTransform);
                currentPickUpItem.transform.localPosition = new Vector3(0, 2, .4f);
                transform.parent.GetComponent<ZipLine_Pivot>().enabled = true;
                anim.SetTrigger("isZipHanging");
                rb.velocity = Vector3.zero;
            }
        }
    }
    void Align() 
    {
        if (alignAxis == "x")
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(alignValue, transform.position.y, transform.position.z), alignSpeed * Time.deltaTime);
            if (transform.position.x == alignValue)
                align = false;
        }
        else if (alignAxis == "y")
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, alignValue, transform.position.z), alignSpeed * Time.deltaTime);
            if (transform.position.y == alignValue)
                align = false;
        }
        else if (alignAxis == "z")
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, alignValue), alignSpeed * Time.deltaTime);
            if (transform.position.z == alignValue)
                align = false;
        }
    }
    void Rotate_Shoot()
    {
        Transform Boss;
        Boss = GameObject.Find("Boss_Level").transform.GetChild(0).GetChild(2);
        transform.LookAt(Boss);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Portal")
        {
            Vector3 spawnPosition;
            if(collision.gameObject.name == "Right_Portal")
            {
                spawnPosition = collision.transform.parent.GetChild(3).GetChild(0).transform.position;
                transform.position = spawnPosition + Vector3.forward;
            }
            else if(collision.gameObject.name == "Wrong_Portal")
            {
                spawnPosition = collision.transform.parent.GetChild(3).GetChild(1).transform.position;
                transform.position = spawnPosition - Vector3.forward;
                transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 180, 0);
                move_Player.PlayerSpeedValue(0);
                move_Player.Dead(false);
                anim.SetBool("isFalling", true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {
            if (int.Parse(other.name) == 0 && triggerIndex == 0)
                return;
            else if (triggerIndex == 0)
            {
                triggerIndex = int.Parse(other.name);
                // 8: Only For Boss Level Instant Fire:
                if(triggerIndex == 8)
                {
                    triggerIndex = 0;
                    manager_Boss = GameObject.Find("Boss_Level").transform.GetChild(0).GetComponent<Manager_Boss>();
                    manager_Boss.Collected();
                    Shoot_Boss();
                }
                GameObject gmm = Instantiate(pickUpTaken);
                gmm.transform.position = other.gameObject.transform.position;
                Destroy(other.gameObject);
            }
            else
            {
                //Here remove every thing related to the current PickUp:
                if (triggerIndex == 1)
                {
                    triggerIndex = 0;
                    Destroy(currentPickUpItem);
                    transform.SetParent(null);
                    move_Player.PlayerSpeedValue(7);
                    anim.SetTrigger("isDropping");
                    rb.useGravity = true;
                }
                else if (triggerIndex == 2)
                {
                    triggerIndex = 0;
                    Destroy(currentPickUpItem);
                }
                else if (triggerIndex == 3)
                    triggerIndex = 0;
                else if (triggerIndex == 4)
                {
                    triggerIndex = 0;
                    anim.SetTrigger("isDropping");
                    Invoke("ResetPlayerSpeed", 2);
                    move_Player.PlayerSpeedValue(2);
                    Destroy(currentPickUpItem);
                    Destroy(transform.GetComponent<SpringJoint>());
                }
                else if (triggerIndex == 5)
                {
                    triggerIndex = 0;
                    Destroy(currentPickUpItem);
                }
                else if (triggerIndex == 6)
                {
                    triggerIndex = 0;
                    Destroy(currentPickUpItem);
                }
                else if (triggerIndex == 7)
                {
                    triggerIndex = 0;
                    Destroy(currentPickUpItem);
                    isInZipline = false;
                    isSwitchingZipline = false;
                    Destroy(transform.GetComponent<Swipe_Control>());
                    move_Player.PlayerSpeedValue(7);
                    anim.SetTrigger("isDropping");
                    anim.SetBool("isJumping", false);
                }
            }
        }
        else if (other.tag == "PickUpTrigger")
        {
                                             // 1 for Zipline_Hanger:
            if (triggerIndex == 1)
            {
                rb.useGravity = false;
                move_Player.PlayerSpeedValue(0);
                targetTransform = GameObject.Find("Obstacles").gameObject.transform.Find("Single_Zipline").GetChild(4).GetChild(0).GetChild(0).transform;
                targetPosition = targetTransform.position - new Vector3(0, 4, 0);
                move = true;
                anim.SetBool("isRunning", false);
                anim.SetTrigger("isInZipLine");
                Destroy(other.gameObject);
            }        // 2 Riffle Shoot Baloon:
            else if (triggerIndex == 2)
            {
                move_Player.PlayerSpeedValue(0);
                anim.SetBool("isRunning", false);
                anim.SetBool("isDucking", false);

                //Instantaiate the corresponding PickUp Item:
                currentPickUpItem = Instantiate(pickUpItems[triggerIndex - 1]);
                currentPickUpItem.transform.SetParent(transform);
                //Add the PickUpItem behaviour:
                currentPickUpItem.AddComponent<PickUpItem>();
                currentPickUpItem.GetComponent<PickUpItem>().childItem = pickUpChilds_Items[triggerIndex - 1];
                currentPickUpItem.transform.localPosition = new Vector3(0, 1, 1);
                Invoke("Shoot", 2f);
                Destroy(other.gameObject);
            }   // 3: Pebble Throw:
            else if (triggerIndex == 3)
            {
                Destroy(other.gameObject);
                anim.SetBool("isRunning", false);
                anim.SetTrigger("isThrowing");
                move_Player.PlayerSpeedValue(0);
                //Instantaiate the corresponding PickUp Item:
                currentPickUpItem = Instantiate(pickUpItems[triggerIndex - 1]);
                currentPickUpItem.transform.SetParent(transform);
                currentPickUpItem.transform.localPosition = Vector3.zero;
                currentPickUpItem.transform.SetParent(null);

                //Add the PickUpItem behaviour:
                currentPickUpItem.AddComponent<PickUpItem>();
                currentPickUpItem.GetComponent<PickUpItem>().childItem = pickUpChilds_Items[triggerIndex - 1];
                Invoke("Shoot", 2.4f);
            }   // 4: Grapple Gun:
            else if (triggerIndex == 4)
            {
                Destroy(other.gameObject);
                anim.SetTrigger("Grapple");
                move_Player.PlayerSpeedValue(0);

                //Instantaiate the corresponding PickUp Item:
                currentPickUpItem = Instantiate(pickUpItems[triggerIndex - 1]);
                currentPickUpItem.transform.SetParent(weaponPosition);
                currentPickUpItem.transform.localPosition = Vector3.zero;
                currentPickUpItem.transform.localRotation = Quaternion.Euler(-90, 24, -10);

                //Add the PickUpItem behaviour:
                currentPickUpItem.AddComponent<PickUpItem>();
                currentPickUpItem.GetComponent<PickUpItem>().childItem = pickUpChilds_Items[triggerIndex - 1];
                Invoke("Shoot", .9f);
                alignBones = true;
            }   // 5: Bazuka:
            else if (triggerIndex == 5)
            {
                move_Player.PlayerSpeedValue(0);
                anim.SetBool("isRunning", false);
                Destroy(other.gameObject);

                //Instantaiate the corresponding PickUp Item:
                currentPickUpItem = Instantiate(pickUpItems[triggerIndex - 1]);
                currentPickUpItem.transform.SetParent(weaponPosition);
                currentPickUpItem.transform.localPosition = Vector3.zero;
                currentPickUpItem.transform.localRotation = Quaternion.Euler(-90, 24, 10);

                //Add the PickUpItem behaviour:
                currentPickUpItem.AddComponent<PickUpItem>();
                currentPickUpItem.GetComponent<PickUpItem>().childItem = pickUpChilds_Items[triggerIndex - 1];
                Invoke("Shoot", 1);
            }   // 6: Sword:
            else if (triggerIndex == 6)
            {
                move_Player.PlayerSpeedValue(0);
                Destroy(other.gameObject);
                //anim.SetTrigger("SwordRun");

                //Instantaiate the corresponding PickUp Item:
                currentPickUpItem = Instantiate(pickUpItems[triggerIndex - 1]);
                currentPickUpItem.transform.SetParent(transform);
                currentPickUpItem.transform.localPosition = Vector3.one;

                //Add the PickUpItem behaviour:
                currentPickUpItem.AddComponent<PickUpItem>();
                Shoot();
            }   // 7: Consecutive ZipLine:
            else if (triggerIndex == 7 && isInZipline != true)
            {
                move = true;
                isInZipline = true;
                rb.useGravity = false;
                Destroy(other.gameObject);
                anim.SetTrigger("isInZipLine");
                move_Player.PlayerSpeedValue(0);
                anim.SetBool("isRunning", false);
                transform.gameObject.AddComponent<Swipe_Control>();
                targetTransform = other.transform.parent;
                targetPosition = other.transform.parent.position - new Vector3(0, 4, 0);
            }
            else if (isInZipline == true)
            {
                isSwitchingZipline = true;
                targetTransform = other.transform.parent;
                targetPosition = other.transform.parent.position - new Vector3(0, 4, 0);
                Destroy(other.gameObject);
            }
        }                                    // In Case Player is falling:
        else if (other.tag == "Falling_Die")
        {
            move_Player.Dead(false);
            anim.SetTrigger("isFalling");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PickUpTrigger" && triggerIndex == 7)
            isSwitchingZipline = false;
    }
    void ResetPlayerSpeed()
    {
        lookAndShoot = false;
        move_Player.PlayerSpeedValue(7);
    }
    void Shoot()
    {
        if(triggerIndex == 2 || triggerIndex == 5)
        {
            currentPickUpItem.GetComponent<PickUpItem>().Shoot();
        }
        else if (triggerIndex == 3)
        {
            currentPickUpItem.GetComponent<PickUpItem>().Throw();
        }
        else if (triggerIndex == 4)
        {
            currentPickUpItem.GetComponent<PickUpItem>().Grapple();
            Align("x", 0, 0.5f);
        }
        else if (triggerIndex == 6)
        {
            currentPickUpItem.GetComponent<PickUpItem>().Slash();
        }
    }
    void Shoot_Boss()
    {
        move_Player.PlayerSpeedValue(0);
        anim.SetBool("isRunning", false);

        //Instantaiate the corresponding PickUp Item:
        currentPickUpItem = Instantiate(pickUpItems[4]);
        currentPickUpItem.transform.SetParent(weaponPosition);
        currentPickUpItem.transform.localPosition = Vector3.zero;
        currentPickUpItem.transform.localRotation = Quaternion.Euler(-90, 24, 10);

        //Add the PickUpItem behaviour:
        currentPickUpItem.AddComponent<PickUpItem>();
        currentPickUpItem.GetComponent<PickUpItem>().childItem = pickUpChilds_Items[4];

        //Find the Shoot Target:
        GameObject nearestTarget = null;
        GameObject[] gmm = GameObject.FindGameObjectsWithTag("Shoot_Target");
        for (int i = 0; i < gmm.Length; i++)
        {
            if (gmm[i].transform.position.z > transform.position.z)
            {
                if (nearestTarget != null && gmm[i].transform.position.z < nearestTarget.transform.position.z)
                    nearestTarget = gmm[i];
                else if (nearestTarget == null)
                    nearestTarget = gmm[i];
            }
        }
        lookAndShoot = true;
        currentPickUpItem.GetComponent<PickUpItem>().Shoot_Boss(); 
        Invoke("ResetPlayerSpeed", 2f);
    }
    public int CurrentItem() {
        return triggerIndex; 
    }
    string alignAxis;
    public void Align(string aA, float aV, float aS)
    {
        alignAxis = aA;
        alignValue = aV;
        alignSpeed = aS;
        align = true;
    }
    public void SwitchZipLine(bool left)
    {
        Debug.Log(isSwitchingZipline);
        if (isSwitchingZipline == true && left == true)
        {
            transform.SetParent(targetTransform);
            targetPosition = targetTransform.position - new Vector3(0, 4, 0);
            anim.SetTrigger("isSwitchingZipline");
            transform.GetChild(0).localRotation = Quaternion.Euler(0,-15,0);
            move = true;
            isInZipline = true;
            Destroy(currentPickUpItem);
        }
        else
        {
            transform.GetComponent<Move_Player>().PlayerSpeedValue(7);
            rb.useGravity = true;
            rb.AddForce(new Vector3(1, 0, 1) * 100 * Time.deltaTime);
            Destroy(currentPickUpItem);
            //anim.SetTrigger("isSwitchingZipline");
        }
    }
}
