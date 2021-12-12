using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    Vector3 Speed = new Vector3(0, 180, 0);
    void Update()
    {
        this.transform.Rotate(Speed * Time.deltaTime, Space.World);
    }
}
