using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    private Player player;
    private PlayerTools playerTools;
    private Tilemap tilemap;
    public Tilemap Tilemap { get => tilemap; }
    private Tile selectedTile;
    [SerializeField] private GameObject selectedTileObject;
    [SerializeField] private GameObject crackObject;
    [SerializeField] private Sprite[] crackSprites = new Sprite[8];

    // Usando um dicionário para acessar dados dos tiles mais rapidamente
    private Dictionary<TileBase, TileData> tileDataDict = new Dictionary<TileBase, TileData>();
    public Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();
    [SerializeField] private List<TileData> tileData;
    private Camera mainCamera;
    [SerializeField] private GameObject plowPrefab;
    public GameObject PlowPrefab { get => plowPrefab; }

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        mainCamera = Camera.main;

        // Inicializar dicionário de TileData
        foreach (var data in tileData)
        {
            foreach (var tile in data.Tiles)
            {
                tileDataDict[tile] = data;
            }
        }

        // Inicializar dicionário de Tiles
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(position.x, position.y, position.z);
            if (!tilemap.HasTile(localPlace)) continue;

            TileBase tile = tilemap.GetTile(localPlace);
            if (tileDataDict.TryGetValue(tile, out TileData data))
            {
                tiles[localPlace] = new Tile(localPlace, data, this);
                tiles[localPlace].Reset();
            }
        }
    }

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        playerTools = player.GetComponent<PlayerTools>();
    }

    void Update()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = tilemap.WorldToCell(mouseWorldPos);
        coordinate.z = 0;

        selectedTile = GetTile(coordinate);

        if (selectedTile != null && playerTools.CanUseTool())
        {
            selectedTileObject.SetActive(true);
            selectedTileObject.transform.position = tilemap.GetCellCenterWorld(coordinate); 
        }   
        else
        {
            selectedTileObject.SetActive(false);
        }

        UpdateCrack(selectedTile);

        foreach (var tile in tiles.Values)
        {
            tile.Update();
        }
    }

    void UpdateCrack(Tile selectedTile)
    {
        if (selectedTile != null && selectedTile.CurrentHealth < selectedTile.Health && selectedTile.CurrentHealth > 0)
        {
            if (!crackObject.activeSelf)
            {
                crackObject.SetActive(true);
                crackObject.transform.position = tilemap.GetCellCenterWorld(selectedTile.Coordinate);
            }

            int index = (int)((1 - (selectedTile.CurrentHealth / selectedTile.Health)) * crackSprites.Length);
            crackObject.GetComponent<SpriteRenderer>().sprite = crackSprites[Mathf.Clamp(index, 0, crackSprites.Length - 1)];
        }
        else
        {
            crackObject.SetActive(false);
        }
    }

    public Tile GetTile(Vector3Int coordinate)
    {
        if (tiles.TryGetValue(coordinate, out Tile tile))
        {
            return tile;
        }

        return null;
    }

    public Tile GetSelectedTile()
    {
        if (selectedTile == null) return null;
        return GetTile(selectedTile.Coordinate);
    }

    public void DestroyTile(Vector3Int coordinate)
    {
        TileBase tile = tilemap.GetTile(coordinate);
        if (tile != null && tileDataDict.TryGetValue(tile, out TileData tileData))
        {
            if (tileData.Drop != null)
            {
                Vector3 dropPos = tilemap.GetCellCenterWorld(coordinate);
                Instantiate(tileData.Drop, dropPos, Quaternion.identity);
            }

            tilemap.SetTile(coordinate, null);
            tiles.Remove(coordinate);
        }
    }
}
