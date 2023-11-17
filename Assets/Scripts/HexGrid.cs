using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour {
    [SerializeField] private uint width, height;

    [SerializeField] private HexTile tilePrefab;
    [SerializeField] private Transform tilesParent;

    [SerializeField] private LayerMask terrain;

    public readonly float innerRadius = Mathf.Sqrt(3) / 2f;

    private HexTile[,] tiles;

    [SerializeField] private Player playerPrefab;
    public Player Player { get; private set; }

    private List<Enemy> enemies;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private uint enemyCount = 3;

    private void Awake() {
        Generate();

        Player = Instantiate(playerPrefab);
        Player.x = Random.Range(0, (int)width);
        Player.y = Random.Range(0, (int)height);

        enemies = new List<Enemy>();
        for (var i = 0; i < enemyCount; i++) {
            var enemy = Instantiate(enemyPrefab);

            int x, y;
            do {
                x = Random.Range(0, (int)width);
                y = Random.Range(0, (int)height);
            } while (!IsFree(x, y));

            enemy.x = x;
            enemy.y = y;

            enemies.Add(enemy);
        }
    }

    private void Generate() {
        tiles = new HexTile[width, height];

        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var tile = Instantiate(tilePrefab, tilesParent);
                tile.Initialize(x, y);
                tiles[x, y] = tile;
            }
        }
    }

    private void Update() {
        foreach (var tile in tiles)
            tile.clickable = false;

        tiles[Player.x, Player.y].clickable = true;
        foreach (var neighbor in GetNeighbors(Player.x, Player.y))
            neighbor.clickable = true;


        foreach (var tile in tiles)
            tile.selected = false;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000f, terrain)) {
            var tile = hit.collider.GetComponentInParent<HexTile>();
            if (tile) {
                tile.selected = true;

                if (Input.GetMouseButtonDown(0) && tile.clickable)
                    MovePlayer(tile);
            }
        }
    }

    private void MovePlayer(HexTile tile) {
        Player.x = tile.X;
        Player.y = tile.Y;

        foreach (var enemy in enemies)
            enemy.TakeTurn();
    }

    public Vector3 HexPosition(int x, int y) => tiles[x, y].IdealPosition;

    public HexTile TileAt(int x, int y) => tiles[x, y];

    public bool IsFree(int x, int y) {
        if (x < 0 || x >= width || y < 0 || y >= width)
            return false;

        if (Player.x == x && Player.y == y)
            return false;

        foreach (var enemy in enemies)
            if (enemy.x == x && enemy.y == y)
                return false;

        return true;
    }

    public IEnumerable<HexTile> GetNeighbors(int x, int y, bool freeOnly = true) {
        var neighbors = new List<HexTile>();

        if (x >= 1)
            neighbors.Add(tiles[x - 1, y]);

        if (x < width - 1)
            neighbors.Add(tiles[x + 1, y]);

        if (y >= 1) {
            neighbors.Add(tiles[x, y - 1]);

            if (y % 2 == 0) {
                if (x >= 1)
                    neighbors.Add(tiles[x - 1, y - 1]);
            } else {
                if (x < width - 1)
                    neighbors.Add(tiles[x + 1, y - 1]);
            }
        }

        if (y < height - 1) {
            neighbors.Add(tiles[x, y + 1]);

            if (y % 2 == 0) {
                if (x >= 1)
                    neighbors.Add(tiles[x - 1, y + 1]);
            } else {
                if (x < width - 1)
                    neighbors.Add(tiles[x + 1, y + 1]);
            }
        }

        return neighbors.Where(t => IsFree(t.X, t.Y) || !freeOnly);
    }
}
