using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollower : MonoBehaviour
{

    public Transform Target;
    

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            Target = PlayerController.Instance.transform;
        }
        else 
        {
            Vector3 lookPos = Target.position - transform.position;
            lookPos.y = 0;
            this.transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
