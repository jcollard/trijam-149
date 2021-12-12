using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MapObject
{
    private float DestroyAt = float.PositiveInfinity;
    public override void Destroy(int row, int col)
    {

        PlayerController.Instance.IncreaseTime();
        PlayerController.Instance.CanBackFlip = true;
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
