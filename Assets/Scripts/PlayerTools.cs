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



    [SerializeField] private float toolsRange;
    private int currentToolIndex = 1;
    public int CurrentToolIndex { get { return currentToolIndex; } }
    private bool selectingTool;
    public bool SelectingTool { get { return selectingTool; } }



    [Header("Terrain")]
    [SerializeField] private bool hasTerrainTool;
    [SerializeField] private float destroyDamage;
    [SerializeField] private int destroyConsumptionWs;
    [SerializeField] private int buildConsumptionWs;



    [Header("Scanner")]
    [SerializeField] private bool hasScannerTool;
    [SerializeField] private int scannerConsumptionWs;
    [SerializeField] private float scannerRange;
    private Tile lastScannedTile;
    [SerializeField] private GameObject infoPanelPrefab;
    private InfoPanel currentInfoPanel;



    [Header("Watering")]
    [SerializeField] private bool hasWateringTool;
    [SerializeField] private int tankCapacity;
    [SerializeField] private int tankLevel;
    [SerializeField] private int wateringConsumptionWs;



    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindFirstObjectByType<TilemapManager>();
        tankLevel = tankCapacity;
    }

    void Update()
    {
        selectedTile = tilemapManager.GetSelectedTile();

        HandleToolsPanel();

        if (CanUseTool())
        {
            switch (currentToolIndex)
            {
                case 1:
                    //hand tool
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log(tilemapManager.GetSelectedTile().Coordinate);
                    }
                    break;

                case 2:
                    HandleTerrainTool();
                    break;

                case 3:
                    HandleWateringTool();
                    break;

                default:
                    break;
            }
        }

        else 

        {
            switch (currentToolIndex)
            {
                case 1:
                    //hand tool
                    break;

                case 2:
                    player.RemoveConsumptionWs("DestroyTool");
                    break;

                case 3:
                    player.RemoveConsumptionWs("WateringTool");
                    break;

                default:
                    break;
            }
        }

        if (hasScannerTool && selectedTile != null && Vector3.Distance(transform.position, selectedTile.Coordinate) <= scannerRange)
        {
            HandleScannerTool();
        }

        else 
        {
            DestroyinfoPanel();
        }
    }

    private bool CanUseTool()
    {
        return selectedTile != null && Vector3.Distance(player.transform.position, selectedTile.Coordinate) <= toolsRange;
    }

    void HandleToolsPanel()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            selectingTool = true;
            currentToolIndex = UpdateSelectedTool();
        }

        else 
        {
            selectingTool = false;
        }
    }

    private int UpdateSelectedTool()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasTerrainTool) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasWateringTool) return 3;

        return currentToolIndex;
    }

    public void HandleTerrainTool()
    {

        if (Input.GetMouseButtonDown(0))
        {
            player.AddConsumptionWs("DestroyTool", destroyConsumptionWs);
        }



        if (Input.GetMouseButton(0))
        {
            player.AddConsumptionWs("DestroyTool", destroyConsumptionWs);

            selectedTile.Damage(destroyDamage * Time.deltaTime);
            if (selectedTile.CurrentHealth <= 0)
            {
                tilemapManager.DestroyTile(selectedTile.Coordinate);
                player.RemoveConsumptionWs("DestroyTool");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedTile.Reset();
            player.RemoveConsumptionWs("DestroyTool");
        }
    }

    #region scanner

    public void HandleScannerTool()
    {
        if (lastScannedTile != selectedTile)
        {
            DestroyinfoPanel();

            lastScannedTile = selectedTile;
            Vector2 playerPosition = Camera.main.WorldToScreenPoint(selectedTile.Coordinate);
            currentInfoPanel = Instantiate(infoPanelPrefab, playerPosition, Quaternion.identity).GetComponent<InfoPanel>();
            currentInfoPanel.UpdatePanel(
                selectedTile.Name,
                (int)selectedTile.Temperature,
                (int)selectedTile.Luminosity,
                (int)selectedTile.LifeSupport,
                (int)selectedTile.Wetness);
            currentInfoPanel.transform.SetParent(GameObject.Find("HUD").transform);
        }
        else if (lastScannedTile == selectedTile && currentInfoPanel != null)
        {
            currentInfoPanel.UpdatePanel(
                selectedTile.Name,
                (int)selectedTile.Temperature,
                (int)selectedTile.Luminosity,
                (int)selectedTile.LifeSupport,
                (int)selectedTile.Wetness);
        }
        else if (selectedTile == null)
        {
            DestroyinfoPanel();
        }
    }

    void DestroyinfoPanel()
    {
        if (currentInfoPanel != null)
        {
            Destroy(currentInfoPanel.gameObject);
        }
    }

    #endregion

    private void HandleWateringTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.AddConsumptionWs("WateringTool", wateringConsumptionWs);
        }

        if (Input.GetMouseButtonUp(0))
        {
            player.RemoveConsumptionWs("WateringTool");
        }

        if (Input.GetMouseButton(0) && tankLevel > 0)
        {
            if (tankLevel > 0)
            {
                selectedTile.Water(1);
                tankLevel--;
            }
            else
            {
                Debug.Log("Tank is empty!");
            }

        }
    }

}
