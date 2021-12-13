using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{

    public static GridController Instance;
    public Canvas GameOverScreen, ClearedScreen, ReadyScreen, UpgradeScreen, StartScreen, MainUI;
    public UnityEngine.UI.Text StageText, StageShadow, ReportText, ReportShadow, BestText, BestShadow, StageReportText, StageReportShadow, ProtipText, ProtipShadow, BackflipsText, BackflipsShadow;

    public UnityEngine.UI.Button[] LastStageButtons, NextStageButtons;
    public GameObject PlayerModel;

    private readonly Dictionary<(int, int), MapObject> MapObjects = new Dictionary<(int, int), MapObject>();
    public int Width;
    public int Distance;

    public float EnemyChance, ObstacleChance, PowerupChance;

    public GameObject[] Tiles;
    public ObstacleController[] Obstacles;

    public EnemyController[] Enemies;

    public PowerupController[] Powerups;

    public Transform Container;

    public System.Random RNG = new System.Random();
    public int MaxCoins;
    public int MaxKills;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        RNG = new System.Random(42);
        PlayerController.Instance.CanMove = false;
    }

    public void GenerateGrid()
    {
        CaptainCoder.Unity.UnityEngineUtils.Instance.DestroyChildren(this.Container);
        MapObjects.Clear();
        if (PlayerController.Instance != null)
        {
            RNG = new System.Random(PlayerController.Instance.level);
            Distance = 20 + PlayerController.Instance.level * 3;
            MusicController.Instance.SetTrack(PlayerController.Instance.level);
        }
        MaxCoins = 0;
        MaxKills = 0;
        int LastSkeleton = 0;
        for (int row = 0; row <= Distance; row++)
        {
            
            for (int col = 0; col < Width; col++)
            {
                int ix = (row + col) % Tiles.Length;
                GameObject Tile = UnityEngine.Object.Instantiate<GameObject>(Tiles[ix]);
                Tile.transform.parent = Container;
                Tile.transform.localPosition = new Vector3(col, 0, row);
                Tile.name = $"Tile: ({row}, {col})";

                if (row < 5 || row == Distance) continue;
                // float skeletonBonus = (1f / PlayerController.Instance.level) * LastSkeleton;
                // skeletonBonus = Mathf.Min(.15f, skeletonBonus);
                // Debug.Log($"SkeltonBonus: {skeletonBonus}");
                ObstacleChance = 0.05f + (0.01f * PlayerController.Instance.level);
                ObstacleChance = Mathf.Min(0.33f, ObstacleChance);
                if (RNG.NextDouble() < ObstacleChance)
                {
                    GenerateObstacle(row, col);
                }
                else if (RNG.NextDouble() < EnemyChance)
                {
                    GenerateEnemy(row, col);
                    LastSkeleton = 0;
                }
                else if (RNG.NextDouble() < PowerupChance)
                {
                    GeneratePowerup(row, col);
                }

            }

            LastSkeleton++;
            if (LastSkeleton >= 5)
            {
                // Debug.Log($"Row: {row}, Last Skeleton: {LastSkeleton}");
                GenerateEnemy(row, RNG.Next(0,3));
                LastSkeleton = 0;
            }
        }

        EnsureBeatableLevel();

    }

    public void EnsureBeatableLevel()
    {
        int Col = 1;

        for (int Row = 0; Row <= Distance; Row++)
        {
            if (MapObjects.ContainsKey((Row, Col)))
            {
                MapObject mo = MapObjects[(Row, Col)];
                if (mo.GetComponent<ObstacleController>() != null)
                {
                    UnityEngine.Object.Destroy(MapObjects[(Row, Col)].gameObject);
                    MapObjects.Remove((Row, Col));
                }
            }
            int nextMove = RNG.Next(0, 2);
            if (nextMove == 0)
            {
                Col++;
            }
            else
            {
                Col--;
            }
            Col = ((Col % Width) + Width) % Width;
        }
    }

    public void GenerateObstacle(int row, int col)
    {
        int ix = (row + col) % Obstacles.Length;
        MapObject Tile = UnityEngine.Object.Instantiate<MapObject>(Obstacles[ix]);
        MapObjects.Add((row, col), Tile);
        Tile.transform.parent = Container;
        Tile.transform.localPosition = new Vector3(col, 0, row);
        Tile.name = $"Obstacles: ({row}, {col})";
    }

    public void GenerateEnemy(int row, int col)
    {
        if (MapObjects.ContainsKey((row, col)))
        {
            return;
        }
        MaxKills++;
        int ix = (row + col) % Enemies.Length;
        MapObject Tile = UnityEngine.Object.Instantiate<MapObject>(Enemies[ix]);
        MapObjects.Add((row, col), Tile);
        Tile.transform.parent = Container;
        Tile.transform.localPosition = new Vector3(col, 0, row);
        Tile.name = $"Enemy: ({row}, {col})";
    }

    public void GeneratePowerup(int row, int col)
    {
        MaxCoins++;
        int ix = (row + col) % Powerups.Length;
        MapObject Tile = UnityEngine.Object.Instantiate<MapObject>(Powerups[ix]);
        MapObjects.Add((row, col), Tile);
        Tile.transform.parent = Container;
        Tile.transform.localPosition = new Vector3(col, 0, row);
        Tile.name = $"PowerUp: ({row}, {col})";
    }

    public void CheckMove(int row, int col)
    {
        if (MapObjects.ContainsKey((row, col)))
        {
            MapObjects[(row, col)].Destroy(row, col);
        }
    }

    public void RemoveMapObject(int row, int col)
    {
        if (MapObjects.ContainsKey((row, col)))
        {
            MapObjects.Remove((row, col));
        }
    }

    public void Restart()
    {
        GenerateGrid();
        StageText.text = $"Stage {PlayerController.Instance.level}";
        StageShadow.text = $"Stage {PlayerController.Instance.level}";


        PlayerController p = PlayerController.Instance;
        LevelData ld = p.Levels[p.level - 1];
        string report = $"Coins: {ld.Coins}/{MaxCoins}    Skeletons {ld.Kills}/{MaxKills}";
        StageReportShadow.text = report;
        StageReportText.text = report;

        foreach (Button b in NextStageButtons)
        {
            if (p.Levels.Count > p.level)
            {
                b.gameObject.SetActive(true);
            }
            else
            {
                b.gameObject.SetActive(false);
            }
        }

        bool prevButton = p.level > 1;
        foreach (Button b in LastStageButtons)
        {
            b.gameObject.SetActive(prevButton);
        }

        GameOverScreen.gameObject.SetActive(false);
        ClearedScreen.gameObject.SetActive(false);
        UpgradeScreen.gameObject.SetActive(false);
        ReadyScreen.gameObject.SetActive(true);

        PlayerController.Instance.Reset();

    }

    public void StartGame()
    {
        MainUI.gameObject.SetActive(true);
        StartScreen.gameObject.SetActive(false);
        GameOverScreen.gameObject.SetActive(false);
        ClearedScreen.gameObject.SetActive(false);
        UpgradeScreen.gameObject.SetActive(false);
        ReadyScreen.gameObject.SetActive(false);
        PlayerModel.SetActive(true);
        Restart();
    }

}