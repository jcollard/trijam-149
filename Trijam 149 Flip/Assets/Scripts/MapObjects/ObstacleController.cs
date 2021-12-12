using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MapObject
{
    public override void Destroy(int row, int col)
    {
        PlayerController.Instance.Die();
        SoundController.Instance.Death.Play();
    }
}
