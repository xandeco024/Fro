using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    public TileBase dirtTile;

    private Player player;
    private float destroyProgress = 0;
    private TileBase selectedTile;
    private TileBase lastSelectedTile;
    [SerializeField] private GameObject selectedTileUI;
    [SerializeField] private GameObject crackEffectUI;



    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = tilemap.WorldToCell(mouseWorldPos);
        coordinate.z = 0;

        if (Input.GetMouseButton(0) && tilemap.GetTile(coordinate) != null)
        {
            selectedTile = tilemap.GetTile(coordinate);

            if (selectedTile == lastSelectedTile)
            {
                destroyProgress += player.DestroySpeed;
                if (!crackEffectUI.activeSelf)
                {
                    crackEffectUI.SetActive(true);
                    crackEffectUI.transform.position = tilemap.GetCellCenterWorld(coordinate);
                }
            }
            else
            {
                destroyProgress = 0;
                crackEffectUI.SetActive(true);
            }

            if (destroyProgress >= 100)
            {
                tilemap.SetTile(coordinate, null);
                destroyProgress = 0;
                crackEffectUI.SetActive(false);
            }

            lastSelectedTile = selectedTile;
        }

        if (Input.GetMouseButton(1) && tilemap.GetTile(coordinate) == null)
        {
            tilemap.SetTile(coordinate, dirtTile);
        }

        if (tilemap.GetTile(coordinate) != null)
        {
            selectedTileUI.SetActive(true);
            selectedTileUI.transform.position = tilemap.GetCellCenterWorld(coordinate);
        }
        else
        {
            selectedTileUI.SetActive(false);
        }
    }
}
