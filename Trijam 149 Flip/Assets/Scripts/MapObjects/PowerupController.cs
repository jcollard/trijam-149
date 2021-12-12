using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MapObject
{

    private float DestroyAt = float.PositiveInfinity;
    public override void Destroy(int row, int col)
    {

        PlayerController.Instance.CoinsCollected++;
        GridController.Instance.RemoveMapObject(row, col);
        DestroyAt = Time.time + PlayerController.Instance.MoveDuration;
    }

    public void Update()
    {
        if (this.gameObject != null && this.DestroyAt <= Time.time)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
