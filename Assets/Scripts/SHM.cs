using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHM : MonoBehaviour
{
    public int currenDir, currentDeviation;
    public float speedOfSHM;
    public Vector3 shmAngle;
    float maxAngle, currentAngle;
    private void Start()
    {
        if (shmAngle.x > 0)
            maxAngle = shmAngle.x;
        else if (shmAngle.y > 0)
            maxAngle = shmAngle.y;
        else
            maxAngle = shmAngle.z;
    }
    void Update()
    {
        currentAngle = maxAngle * Mathf.Sin(Time.time * speedOfSHM);
        transform.localRotation = Quaternion.Euler(0, 0, currenDir * currentAngle + currentDeviation);
    }
}