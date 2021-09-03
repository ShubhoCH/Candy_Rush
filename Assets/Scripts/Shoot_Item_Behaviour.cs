using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Item_Behaviour : MonoBehaviour
{
    public Transform targetTransform;
    public float speed;
    private void Start()
    {
        Invoke("Delete", 5);
    }
    void Update()
    {
        if(targetTransform != null)
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wall")
        {
            GameObject.Find("Player").GetComponent<Move_Player>().Blast(transform.position);
            GameObject gmm;
            if(collision.gameObject.transform.childCount > 1)
            {
                gmm = collision.gameObject.transform.GetChild(0).gameObject;
                gmm.transform.SetParent(null);
                gmm.gameObject.SetActive(true);
                gmm.AddComponent<Destroy>();
                int noOfChunks = gmm.transform.childCount;
                for (int i = 0; i < noOfChunks; i++)
                {
                    GameObject gm = gmm.transform.GetChild(i).gameObject;
                    gm.GetComponent<Rigidbody>().AddForce((gm.transform.position - (collision.contacts[0].point + new Vector3(0, 0, -20))).normalized * 1000);
                }
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Shoot_Target")
        {
            GameObject.Find("Player").GetComponent<Move_Player>().Blast(transform.position);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Boss")
        {
            GameObject.Find("Player").GetComponent<Move_Player>().Blast(transform.position);
            collision.gameObject.GetComponent<Boss>().Damage();
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Move_Player>().Dead(true);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 300);
            Destroy(gameObject);
        }
    }
    void Delete() => Destroy(gameObject);
}
