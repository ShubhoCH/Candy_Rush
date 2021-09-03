using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCam_Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(transform.name == "000")
            {
                transform.parent.Find("CM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
                Destroy(gameObject);
            }
            else if (transform.name == "0")
            {

            }
            else if (transform.name == "1")
            {
                transform.parent.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = other.transform;
                transform.parent.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = other.transform;
                transform.parent.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 2;
                Destroy(gameObject);
            }
            else if (transform.name == "2")
            {
                transform.parent.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = other.transform;
                transform.parent.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 2;
                Destroy(gameObject);
            }
        }
    }
}
