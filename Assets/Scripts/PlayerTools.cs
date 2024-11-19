using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    private Player player;
    private TilemapManager tilemapManager;
    private Tile selectedTile;

    [Header("Destroy")]
    [SerializeField] private bool hasDestroyTool;
    [SerializeField] private float destroyRange;
    [SerializeField] private float destroyDamage;

    [Header("Build")]
    [SerializeField] private bool hasBuildTool;

    [Header("Scanner")]
    [SerializeField] private bool hasScannerTool;
    private float scanTimeCounter;
    private float scanTime = .2f;
    private bool scanning;
    private Tile lastScannedTile;
    [SerializeField] private GameObject infoPanelPrefab;
    private InfoPanel currentInfoPanel;

    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindObjectOfType<TilemapManager>();
    }


    void Update()
    {
        if (hasDestroyTool) HandleDestroyTool(tilemapManager.GetSelectedTile());

        if (hasScannerTool) {
            //get player position relative to the canvas
            if (selectedTile != null && lastScannedTile != selectedTile)
            {
                if (currentInfoPanel != null)
                {
                    Destroy(currentInfoPanel.gameObject);
                }

                lastScannedTile = selectedTile;
                Vector2 playerPosition = Camera.main.WorldToScreenPoint(selectedTile.Coordinate);
                currentInfoPanel = Instantiate(infoPanelPrefab, playerPosition, Quaternion.identity).GetComponent<InfoPanel>();
                currentInfoPanel.GetComponent<InfoPanel>().UpdatePanel(
                    selectedTile.Name,
                    selectedTile.Temperature,
                    selectedTile.Luminosity,
                    selectedTile.LifeSupport,
                    selectedTile.Wetness);
                currentInfoPanel.transform.SetParent(GameObject.Find("HUD").transform);
            }
            // else if (GameObject.Find("Tile Info Panel(Clone)") != null)
            // {
            //     Destroy(GameObject.Find("Tile Info Panel(Clone)"));
            // }
        }

        selectedTile = tilemapManager.GetSelectedTile();
    }

    public void SetDestroyTool(bool value)
    {
        hasDestroyTool = value;
    }

    public void HandleDestroyTool(Tile tile)
    {
        if (Input.GetMouseButton(0))
        {
            if (tile != null && Vector3.Distance(player.transform.position, tile.Coordinate) <= destroyRange)
            {
                tile.Damage(destroyDamage * Time.deltaTime);
                if (tile.CurrentHealth <= 0)
                {
                    tilemapManager.DestroyTile(tile.Coordinate);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (tile != null)
            {
                tile.Reset();
            }
        }
    }
}
