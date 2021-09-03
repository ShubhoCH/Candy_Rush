using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float rotationSpeed;
    public string rotationAxis;
    void Update()
    {
        if (rotationAxis == "x")
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0, Space.World);
        else if (rotationAxis == "y")
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        else
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.World);
    }
}
