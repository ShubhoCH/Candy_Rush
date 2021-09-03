using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Spawner : MonoBehaviour
{
    public GameObject Rock;
    float maxTime, currentTime;
    private void Start()
    {
        maxTime = 5;
        currentTime = 0;
    }
    void Update()
    {
        if (currentTime > maxTime)
        {
            GameObject gmm = Instantiate(Rock);
            gmm.transform.SetParent(transform);
            gmm.transform.localPosition = Vector3.zero;
            gmm.AddComponent<Destroy>();
            currentTime = 0;
        }
        else
            currentTime += Time.deltaTime;
    }
}
