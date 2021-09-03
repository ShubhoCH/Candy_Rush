using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHM_LR : MonoBehaviour
{
    float currentPos;
    public float speedOfSHM;
    // Input shm axis as x, y, z:
    public string shmAxis;
    public float maxDeviation, currentDir;
    void Update()
    {
        currentPos = maxDeviation * Mathf.Sin(Time.time * speedOfSHM);
        if (shmAxis == "x")
            transform.localPosition = new Vector3(currentPos * currentDir, 0, 0);
        else if (shmAxis == "y")
            transform.localPosition = new Vector3(0, currentPos * currentDir, 0);
        else
            transform.localPosition = new Vector3(0, 0, currentPos * currentDir);
    }
}
