using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    private Tilemap tilemap;
    private Player player;
    private TileBase selectedTileBase;
    private Vector3Int lastSelectedTileCoordinate;
    private TileData selectedTileData;
    [SerializeField] private GameObject selectedTileUI;
    [SerializeField] private GameObject crackEffectUI;

    // Usando um dicionário para acessar dados dos tiles mais rapidamente
    private Dictionary<TileBase, TileData> tileDataDict = new Dictionary<TileBase, TileData>();
    public Dictionary<Vector3Int, TileData> tileDataMap = new Dictionary<Vector3Int, TileData>();
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

        InitializeTileData();
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = tilemap.WorldToCell(mouseWorldPos);
        coordinate.z = 0;

        if (coordinate != lastSelectedTileCoordinate)
        {
            UpdateSelectedTile(coordinate);
            lastSelectedTileCoordinate = coordinate;
        }
    }

    private void InitializeTileData()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                tileDataMap[pos] = GetTileDataAt(pos);
                tileDataMap[pos].ResetHealth();
            }
        }
    }

    private void UpdateSelectedTile(Vector3Int coordinate)
    {
        selectedTileBase = tilemap.GetTile(coordinate);
        Debug.Log(GetSelectedTileData().CurrentHealth);

        if (selectedTileBase != null && tileDataDict.TryGetValue(selectedTileBase, out TileData selectedTileData))
        {
            // Exibe a UI do tile selecionado e atualiza posição
            selectedTileUI.SetActive(true);
            selectedTileUI.transform.position = tilemap.GetCellCenterWorld(coordinate);
        }
        else
        {
            selectedTileUI.SetActive(false);
        }
    }

    public TileData GetTileDataAt(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        // Supondo que você tem um dicionário de TileData configurado anteriormente
        return tileDataDict.TryGetValue(tile, out TileData data) ? data : null;
    }

    public Vector3Int GetTileCoordinate(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    public TileData GetSelectedTileData()
    {
        if (selectedTileBase != null && tileDataDict.TryGetValue(selectedTileBase, out TileData selectedTileData))
        {
            return selectedTileData;
        }

        return null;
    }

    public Vector3Int GetSelectedTileCoordinate()
    {
        return lastSelectedTileCoordinate;
    }

    public TileBase GetSelectedTileBase()
    {
        return selectedTileBase;
    }

    public bool ApplyDamageToTile(Vector3Int coordinate, float damage)
    {
        if (tileDataMap.TryGetValue(coordinate, out TileData data))
        {
            data.TakeDamage(damage);
            if (data.CurrentHealth <= 0)
            {
                DestroyTile(coordinate);
                return true;
            }
        }
        return false;
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
            tileDataMap.Remove(coordinate);
        }
    }
}
