using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform Target;
    public float Distance;

    public float Speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;
        float newZ = Target.position.z - Distance;
        if (this.transform.position.z < newZ)
        { 
            float d = Target.position.z - this.transform.position.z;
            float bonus = d;
            newZ = this.transform.position.z + ((Speed + bonus) * Time.deltaTime);
        }
        this.transform.position = new Vector3(newX, newY, newZ);
        
    }
}
