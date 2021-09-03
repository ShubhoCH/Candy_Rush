using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Parent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            transform.GetChild(0).GetChild(0).GetComponent<Enemy>().enabled = false;
            collision.gameObject.GetComponent<Move_Player>().Dead(true);
        }
    }
}
