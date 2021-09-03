using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipLine_Pivot : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount > 0)
        {
            transform.localPosition -= new Vector3(0, .5f * Time.deltaTime, 0);
            if (transform.localPosition.y < -1)
            {
                transform.Find("Player").GetComponent<Rigidbody>().useGravity = true;
                transform.Find("Player").SetParent(null);
            }
        }
        else
            GetComponent<ZipLine_Pivot>().enabled = false;
    }
}
