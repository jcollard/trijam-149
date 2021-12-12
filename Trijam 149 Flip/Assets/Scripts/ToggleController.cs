using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleController : MonoBehaviour
{
    
    public void Toggle()
    {
        this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
    }
}
