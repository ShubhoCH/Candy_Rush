using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Donut : MonoBehaviour
{
    public GameObject Donut;
    GameObject gmm;
    public float SpawnTime, Speed;
    public Vector3 SpawnAngleDonut;
    float time;
    void Update()
    {
        time += Time.deltaTime;
        if(time > SpawnTime)
        {
            time = 0;
            if(gmm == null)
                gmm = Instantiate(Donut);
            else
            {
                Destroy(gmm);
                gmm = Instantiate(Donut);
            }
            gmm.transform.localPosition = transform.position + new Vector3(0, 8, -Random.Range(-1, 2) * 3);
            gmm.transform.localRotation = Quaternion.Euler(SpawnAngleDonut);
            Rigidbody rb = gmm.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Speed;
        }
    }
}
