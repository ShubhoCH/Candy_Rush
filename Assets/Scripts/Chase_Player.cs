using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_Player : MonoBehaviour
{
    Vector3 dir, lookHere;
    bool inAir;
    private void Start()
    {
        lookHere = GameObject.Find("Player").transform.position;
        dir = (GameObject.Find("Player").transform.position - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(dir * 900);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 900);
        inAir = true;
    }
    void Update()
    {
        transform.LookAt(lookHere);
        if (inAir != true)
            Jump();
    }

    private void Jump()
    {
        GetComponent<Rigidbody>().AddForce(dir * 500);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 500);
        inAir = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        inAir = false;
    }
}
