using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    private Tilemap tilemap;
    public Tilemap Tilemap { get => tilemap; }
    private Player player;
    private Tile selectedTile;
    [SerializeField] private GameObject selectedTileUI;
    [SerializeField] private GameObject crackEffectUI;

    // Usando um dicionário para acessar dados dos tiles mais rapidamente
    private Dictionary<TileBase, TileData> tileDataDict = new Dictionary<TileBase, TileData>();
    public Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();
    [SerializeField] private List<TileData> tileData;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        player = FindObjectOfType<Player>();

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
                tiles[localPlace] = new Tile(localPlace, data);
                tiles[localPlace].Reset();

            Debug.Log("CRIADO TILE EM " + localPlace + "\n" +
                    "Nome: " + tiles[localPlace].Name + "\n" +
                    "Vida: " + tiles[localPlace].Health + "\n" +
                    "Vida Atual: " + tiles[localPlace].CurrentHealth);
            }
        }
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = tilemap.WorldToCell(mouseWorldPos);
        coordinate.z = 0;

        if (selectedTile != null && coordinate != selectedTile.Coordinate)
        {
            UpdateSelectedTile(coordinate);
        }
        else if (selectedTile == null)
        {
            UpdateSelectedTile(coordinate);
        }
    }

    private void UpdateSelectedTile(Vector3Int coordinate)
    {
        selectedTile = GetTile(coordinate);
        if (selectedTile != null)
        {
            selectedTileUI.SetActive(true);
            selectedTileUI.transform.position = tilemap.GetCellCenterWorld(coordinate); 

            Debug.Log(selectedTile.Name);
            Debug.Log(selectedTile.CurrentHealth);
            Debug.Log(selectedTile.Health);       
        }   
        else
        {
            selectedTileUI.SetActive(false);
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
