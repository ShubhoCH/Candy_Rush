using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// IT CONTAINS THE BEHAVIOUR OF EACH PICK_UP_ITEMS:
/// THE METHORDS ARE CALLED WHEN THE PLAYER COLLIDES WITH PICKUPTRIGGER FROM Trigger_Behaviour Script:
/// </summary>
public class PickUpItem : MonoBehaviour
{
    // It is assigned from the Trigger_Behaviour Script(i.e. which item to instantiate):
    public GameObject childItem;
    Animator anim;
    Transform HandTransform;
    private void Awake()
    {
        anim = GameObject.Find("Player").GetComponentInChildren<Animator>(); 
        HandTransform = GameObject.Find("Player").transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
    }
    public void Shoot()
    {
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
        Debug.Log(nearestTarget.name);
        if (nearestTarget != null)
        {
            anim.SetTrigger("Shoot");
            childItem = Instantiate(childItem);
            childItem.transform.SetParent(transform.Find("Pivot").transform);
            childItem.transform.localPosition = Vector3.zero;
            childItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
            childItem.GetComponent<Shoot_Item_Behaviour>().targetTransform = nearestTarget.transform;
            if (GameObject.Find("Player").GetComponent<Trigger_Behaviour>().CurrentItem() == 5)
                childItem.GetComponent<Shoot_Item_Behaviour>().speed = 15;
            else
                childItem.GetComponent<Shoot_Item_Behaviour>().speed = 50;
            childItem.GetComponent<Shoot_Item_Behaviour>().enabled = false;
            Invoke("Activate_Shoot_Item_Behaviour", .3f);
            Invoke("ResetPlayerSpeed", 2f);
        }
    }
    void Activate_Shoot_Item_Behaviour()
    {
        childItem.GetComponent<Shoot_Item_Behaviour>().enabled = true;
        childItem.transform.SetParent(null);
    }
    //After Firing or Some other kind of behaviour reset the player speed here:
    void ResetPlayerSpeed()
    {
        GameObject.Find("Player").GetComponent<Move_Player>().PlayerSpeedValue(7);
        Destroy(gameObject);
    }
    public void Throw()
    {
        GameObject nearestTarget = null;
        GameObject[] gmm = GameObject.FindGameObjectsWithTag("Shoot_Target");
        for (int i = 0; i < gmm.Length; i++)
        {
            if(gmm[i].transform.position.z > transform.position.z)
            {
                if (nearestTarget != null && gmm[i].transform.position.z < nearestTarget.transform.position.z)
                    nearestTarget = gmm[i];
                else if(nearestTarget == null)
                    nearestTarget = gmm[i];
            }
        }
        if (nearestTarget != null)
        {
            GameObject shoot_Item = Instantiate(childItem);
            shoot_Item.transform.position = HandTransform.position;
            shoot_Item.AddComponent<Shoot_Item_Behaviour>();
            shoot_Item.GetComponent<Shoot_Item_Behaviour>().targetTransform = nearestTarget.transform;
            shoot_Item.GetComponent<Shoot_Item_Behaviour>().speed = 50;
            Invoke("ResetPlayerSpeed", 2.1f);
        }
        else
            Debug.Log("null");
    }
    public void Grapple()
    {
        GameObject shoot_Item = Instantiate(childItem);
        shoot_Item.transform.SetParent(transform);
        shoot_Item.transform.localPosition = new Vector3(0, 2, 0);
        gameObject.AddComponent<Grapple_Gun>();
    }
    public void Slash()
    {
        //Find the Enemy:
        //Rotate towards him:
        //If not enough close go near him:

        //Play the Slash Animation:
        Animator parent_Anim = transform.parent.GetComponent<Animator>();
        //parent_Anim.SetTrigger("Slash");

        //Call the Respective kill methord on the enemy:
        //After 2 sec of the Death of Enemy Reset Player Speed:
        Invoke("ResetPlayerSpeed", 2);
    }
    public void Shoot_Boss()
    {
        GameObject nearestTarget = null;
        GameObject[] gmm = GameObject.FindGameObjectsWithTag("Boss");
        for (int i = 0; i < gmm.Length; i++)
        {
            if (nearestTarget != null && gmm[i].transform.position.z < nearestTarget.transform.position.z)
                nearestTarget = gmm[i];
            else if (nearestTarget == null)
                nearestTarget = gmm[i];
        }
        if (nearestTarget != null)
        {
            anim.SetTrigger("Shoot");
            childItem = Instantiate(childItem);
            childItem.transform.SetParent(transform.Find("Pivot").transform);
            childItem.transform.localPosition = Vector3.zero;
            childItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
            childItem.AddComponent<Shoot_Item_Behaviour>();
            childItem.GetComponent<Shoot_Item_Behaviour>().targetTransform = nearestTarget.transform;
            childItem.GetComponent<Shoot_Item_Behaviour>().speed = 15;
            childItem.GetComponent<Shoot_Item_Behaviour>().enabled = false;
            Invoke("Activate_Shoot_Item_Behaviour", .3f);
            Invoke("ResetPlayerSpeed", 2f);
        }
    }
}
