using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public List<LevelData> Levels = new List<LevelData>();
    public string[] Protips;

    public UnityEngine.UI.Text TimeRemaining;

    public int CoinsCollected;
    public int MaxBackflips;
    public int Backflips;

    public int Kills;
    public int level = 1;
    public float MaxTime = 8;
    public float StartTime = -1;
    public float EndTime;
    public float BonusTime = 1f;

    public int Row, Col, LastRow, LastCol;
    public float MovedAt, MoveDuration;
    public float JumpHeight;

    // public bool CanBackFlip = true;
    public bool CanMove = true;

    public GameObject PlayerModel;
    public GameObject[] Models;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        this.Levels.Add(new LevelData());
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        AnimateMove();
        AnimateJump();
        AnimateFlip();
        CheckSwitchModel();
        CheckTime();
        GridController.Instance.BackflipsText.text = $"Backflips: {Backflips}";
        GridController.Instance.BackflipsShadow.text = $"Backflips: {Backflips}";
    }

    public void CheckTime()
    {
        if (!CanMove)
        {
            return;
        }
        if (EndTime <= Time.time && StartTime > 0)
        {
            Die();
            TimeRemaining.text = "Times up!";
        }
        else if (StartTime < 0)
        {
            TimeRemaining.text = $"{MaxTime:0.00}";
        }
        else
        {
            float tr = EndTime - Time.time;
            System.Math.Max(0, tr);
            TimeRemaining.text = $"{tr:0.00}";
        }
    }

    public void AnimateMove()
    {
        float EndMoveAt = MovedAt + MoveDuration;
        Vector3 endPosition = new Vector3(Col, transform.position.y, Row);
        Vector3 newPosition = endPosition;
        if (EndMoveAt > Time.time)
        {
            float percent = (Time.time - MovedAt) / MoveDuration;
            Vector3 startPosition = new Vector3(LastCol, transform.position.y, LastRow);
            newPosition = Vector3.Lerp(startPosition, endPosition, percent);

        }
        this.transform.position = newPosition;
    }

    public void AnimateJump()
    {
        float EndMoveAt = MovedAt + MoveDuration;
        float MidpointAt = MovedAt + (MoveDuration / 2);
        Vector3 newPosition = new Vector3();
        if (MidpointAt > Time.time)
        {
            float percent = (Time.time - MovedAt) / (MoveDuration / 2);
            Vector3 startPosition = new Vector3();
            Vector3 endPosition = new Vector3(0, JumpHeight, 0);
            newPosition = Vector3.Lerp(startPosition, endPosition, percent);

        }
        else if (EndMoveAt > Time.time)
        {
            float percent = (Time.time - MidpointAt) / (MoveDuration / 2);
            Vector3 endPosition = new Vector3();
            Vector3 startPosition = new Vector3(0, JumpHeight, 0);
            newPosition = Vector3.Lerp(startPosition, endPosition, percent);
        }
        this.PlayerModel.transform.localPosition = newPosition;
    }

    public void AnimateFlip()
    {
        float EndMoveAt = MovedAt + MoveDuration;
        float endAngle = Row * 180;
        float newAngle = endAngle;
        if (EndMoveAt > Time.time)
        {
            float percent = (Time.time - MovedAt) / MoveDuration;
            float startAngle = LastRow * 180;
            newAngle = Mathf.LerpAngle(startAngle, endAngle, percent);
        }
        PlayerModel.transform.eulerAngles = new Vector3(newAngle, 0, 0);
    }

    public void CheckSwitchModel()
    {
        float EndMoveAt = MovedAt + MoveDuration;
        float MidpointAt = MovedAt + (MoveDuration / 2);
        GameObject newModel = Models[Row % Models.Length];
        if (MidpointAt > Time.time)
        {
            newModel = Models[LastRow % Models.Length];
        }

        foreach (GameObject obj in Models)
        {
            if (obj == newModel)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    public void HandleInput()
    {
        if (Input.GetButtonDown("Left"))
        {
            this.Move(1, -1);
        }

        if (Input.GetButtonDown("Right"))
        {
            this.Move(1, 1);
        }

        if (Input.GetButtonDown("Down") && Backflips > 0 && Row > 0)
        {
            Backflips--;
            this.Move(-1, 0);
        }
    }

    public void Move(int Rows, int Cols)
    {
        if (!CanMove)
            return;
        if (StartTime < 0)
        {
            StartTime = Time.time;
            EndTime = Time.time + MaxTime;
            GridController.Instance.UpgradeScreen.gameObject.SetActive(false);
            GridController.Instance.ReadyScreen.gameObject.SetActive(false);
        }
        this.LastRow = Row;
        this.LastCol = Col;
        Row += Rows;
        int width = GridController.Instance.Width;
        Col = (((Col + Cols) % width) + width) % width;
        GridController.Instance.CheckMove(Row, Col);
        MovedAt = Time.time;
        if (Row >= GridController.Instance.Distance)
        {
            Win();
        }
    }

    public void IncreaseTime()
    {
        EndTime += BonusTime;
        if ((EndTime - Time.time) > MaxTime)
        {
            EndTime = Time.time + MaxTime;
        }
    }

    public void Die()
    {
        CanMove = false;
        string tip = Protips[UnityEngine.Random.Range(0, Protips.Length)];
        string protip = $"Flippin' Tip: {tip}";
        GridController.Instance.ProtipText.text = protip;
        GridController.Instance.ProtipShadow.text = protip;
        GridController.Instance.GameOverScreen.gameObject.SetActive(true);
    }

    public void Win()
    {
        CanMove = false;
        LevelData data = Levels[level - 1];
        if (Levels.Count < level + 1)
        {
            Levels.Add(new LevelData());
        }
        data.SetCoins(CoinsCollected);
        data.SetKills(Kills);

        int MaxCoins = GridController.Instance.MaxCoins;
        int MaxKills = GridController.Instance.MaxKills;

        string report = $"Coins: {CoinsCollected}/{MaxCoins}    Skeletons {Kills}/{MaxKills}";
        GridController.Instance.ReportShadow.text = report;
        GridController.Instance.ReportText.text = report;

        string best = $"Best: {data.Coins}/{MaxCoins}       Best: {data.Kills}/{MaxKills}";
        GridController.Instance.BestShadow.text = best;
        GridController.Instance.BestText.text = best;

        foreach (UnityEngine.UI.Button b in GridController.Instance.NextStageButtons)
        {
            if (Levels.Count > level)
            {
                b.gameObject.SetActive(true);
            }
            else
            {
                b.gameObject.SetActive(false);
            }
        }

        bool prevButton = level > 1;
        foreach (UnityEngine.UI.Button b in GridController.Instance.LastStageButtons)
        {
            b.gameObject.SetActive(prevButton);
        }

        GridController.Instance.ClearedScreen.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        level++;
        GridController.Instance.Restart();

    }

    public void PrevLevel()
    {
        level--;
        level = System.Math.Max(1, level);
        GridController.Instance.Restart();
    }

    public void Reset()
    {
        CoinsCollected = 0;
        Kills = 0;
        StartTime = -1;
        Row = 1;
        Col = 1;
        CanMove = true;
        Backflips = MaxBackflips;
    }


    public void RestoreBackFlip()
    {
        if (Backflips < MaxBackflips)
        {
            Backflips++;
        }
    }
}
