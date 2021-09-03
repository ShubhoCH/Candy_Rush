using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemSpot : MonoBehaviour
{
    public GameObject[] pickUpItems;
    private void Start()
    {
        if(transform.name != "8")
            transform.localPosition = new Vector3(transform.localPosition.x, -1, 0);
        GameObject gmm;
        if (transform.name != "7" && transform.name != "8")
            gmm = Instantiate(pickUpItems[int.Parse(transform.name) - 1]);
        else if (transform.name == "8")
            gmm = Instantiate(pickUpItems[4]);
        else
            gmm = Instantiate(pickUpItems[0]);
        gmm.transform.SetParent(transform);
        gmm.transform.localPosition = new Vector3(0, 1, 0);
    }
    private void Update()
    {
        transform.GetChild(0).transform.Rotate(0, 45 * Time.deltaTime, 0);
    }
}
