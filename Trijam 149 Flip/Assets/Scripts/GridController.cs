using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    public static GridController Instance;
    public Canvas GameOverScreen, ClearedScreen;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        RNG = new System.Random(42);
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid()
    {
        CaptainCoder.Unity.UnityEngineUtils.Instance.DestroyChildren(this.Container);
        MapObjects.Clear();
        RNG = new System.Random(PlayerController.Instance.level);
        Distance = 20 + PlayerController.Instance.level*5;
        for (int col = 0; col < Width; col++)
        {
            for (int row = 0; row <= Distance; row++)
            {
                int ix = (row + col) % Tiles.Length;
                GameObject Tile = UnityEngine.Object.Instantiate<GameObject>(Tiles[ix]);
                Tile.transform.parent = Container;
                Tile.transform.localPosition = new Vector3(col, 0, row);
                Tile.name = $"Tile: ({row}, {col})";

                if (row < 5 || row == Distance) continue;

                if (RNG.NextDouble() < ObstacleChance)
                {
                    GenerateObstacle(row, col);
                }
                else if (RNG.NextDouble() < EnemyChance)
                {
                    GenerateEnemy(row, col);
                }
                else if (RNG.NextDouble() < PowerupChance)
                {
                    GeneratePowerup(row, col);
                }

            }
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
        int ix = (row + col) % Enemies.Length;
        MapObject Tile = UnityEngine.Object.Instantiate<MapObject>(Enemies[ix]);
        MapObjects.Add((row, col), Tile);
        Tile.transform.parent = Container;
        Tile.transform.localPosition = new Vector3(col, 0, row);
        Tile.name = $"Enemy: ({row}, {col})";
    }

    public void GeneratePowerup(int row, int col)
    {
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
        GameOverScreen.gameObject.SetActive(false);
        ClearedScreen.gameObject.SetActive(false);
        GenerateGrid();
        PlayerController.Instance.Reset();

    }

}
