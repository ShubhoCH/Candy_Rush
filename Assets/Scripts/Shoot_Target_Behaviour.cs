using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Target_Behaviour : MonoBehaviour
{
    Animator anim;
    private void OnCollisionEnter(Collision collision)
    {
        //Name of the target object will decide the behaviour:
        // {1: Baloon, 2: Enemy_Crusher, 3: Humpty_Dumpty, 4: Wall_Break, 5: Carrot}
        if (collision.collider.tag == "Player")
            return;
        if (transform.name == "1")
        {
            anim = transform.GetChild(1).GetComponent<Animator>();
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).GetChild(0).transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetChild(2).transform.SetParent(transform.parent);
            transform.GetComponent<SphereCollider>().enabled = false;
            Invoke("Remove", 1.8f);
        }     //2: Chain
        if(transform.name == "2")
        {
            transform.parent.GetComponent<Animator>().enabled = true;
            transform.parent.parent.parent.GetChild(0).GetChild(0).GetComponent<Rigidbody>().AddForce(-500, 300, 0);
            transform.parent.parent.parent.GetChild(0).GetChild(0).GetComponent<Rigidbody>().freezeRotation = false;
            transform.parent.parent.parent.GetChild(0).GetChild(0).gameObject.AddComponent<Destroy>();
            transform.parent.parent.parent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Die");
            gameObject.AddComponent<Destroy>();
            Destroy(gameObject);
        }//3: Mushroom
        if (transform.name == "3")
        {
            Destroy(transform.parent.parent.gameObject);
        }
        if(transform.name == "4")
        {
            GameObject gmm;
            gmm = transform.parent.GetChild(0).gameObject;
            gmm.AddComponent<Rigidbody>().AddForce((gmm.transform.position - (collision.contacts[0].point + new Vector3(0,0,-30))).normalized * 3000);
            gmm.GetComponent<Animator>().SetTrigger("Die");
            gmm.transform.SetParent(null);
            gmm.AddComponent<Destroy>();
            int noOfChunks = transform.parent.GetChild(0).childCount;
            transform.parent.GetChild(0).gameObject.SetActive(true);
            for(int i = 0; i < noOfChunks; i++)
            {
                gmm = transform.parent.GetChild(0).GetChild(0).gameObject;
                gmm.GetComponent<Rigidbody>().AddForce((gmm.transform.position - (collision.contacts[0].point + new Vector3(0, 0, -10))).normalized * 1000);
                gmm.transform.SetParent(null);
                gmm.AddComponent<Destroy>();
            }
            transform.parent.Find("Collider").gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
        }
        if (transform.name == "5")
            Destroy(gameObject);
    }
    void Remove()
    {
        Destroy(gameObject);
    }
}
