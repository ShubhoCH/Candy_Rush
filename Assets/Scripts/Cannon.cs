using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject Cherry;
    GameObject gmm;
    public float timeSpawn;
    float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time > timeSpawn)
        {
            time = 0;
            if(gmm == null)
                gmm = Instantiate(Cherry);
            else
            {
                Destroy(gmm);
                gmm = Instantiate(Cherry);
            }
            gmm.transform.position = transform.position + new Vector3(0, .8f, 0) ;
            Rigidbody rb = gmm.transform.GetComponent<Rigidbody>();
            rb.velocity = transform.up * -50;
        }
    }
}
