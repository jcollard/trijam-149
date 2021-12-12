using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    
    public RectTransform rect;
    public float Wobble = 10;
    public float Speed = 5;

    void Start()
    {
        if (rect == null)
        {
            rect = this.gameObject.GetComponent<RectTransform>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        rect.localPosition = new Vector2(0, Mathf.Sin(Time.time*Speed)*Wobble);
    }
}
