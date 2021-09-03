using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Start()
    {
        Invoke("Delete", 10f);
    }
    void Delete()
    {
        Destroy(gameObject);
    }
}
