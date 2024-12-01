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



    private int currentToolIndex = 1;
    public int CurrentToolIndex { get { return currentToolIndex; } }
    private bool selectingTool;
    public bool SelectingTool { get { return selectingTool; } }

    [Header("Hand")]
    [SerializeField] private float handToolRange;



    [Header("Terrain")]
    [SerializeField] private bool hasTerrainTool;
    [SerializeField] private float terrainToolRange;
    [SerializeField] private float destroyDamage;
    [SerializeField] private int destroyConsumptionWs;
    [SerializeField] private int buildConsumptionWs;



    [Header("Scanner")]
    [SerializeField] private bool hasScannerTool;
    [SerializeField] private int scannerConsumptionWs;
    [SerializeField] private float scannerToolRange;
    private Tile lastScannedTile;
    [SerializeField] private GameObject infoPanelPrefab;
    private InfoPanel currentInfoPanel;



    [Header("Watering")]
    [SerializeField] private bool hasWateringTool;
    [SerializeField] private float wateringToolRange;
    [SerializeField] private int tankCapacity;
    [SerializeField] private int tankLevel;
    [SerializeField] private int wateringConsumptionWs;



    [Header("Plow")]
    [SerializeField] private bool hasPlowTool;
    [SerializeField] private float plowToolRange;
    [SerializeField] private int plowToolConsumptionW;



    [Header("dabaguaga")]
    [SerializeField] private SpriteRenderer holdingItem;
    private UIHotbar hotbar;

    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindFirstObjectByType<TilemapManager>();
        tankLevel = tankCapacity;
        hotbar = FindFirstObjectByType<UIHotbar>();
    }

    void Update()
    {
        selectedTile = tilemapManager.GetSelectedTile();

        HandleToolsPanel();

        holdingItem.gameObject.SetActive(currentToolIndex == 1);

        switch (currentToolIndex)
        {
            case 1:
                if (hotbar.SelectedItem != null)
                {
                    holdingItem.color = new Color(1, 1, 1, 1);
                    holdingItem.sprite = hotbar.SelectedItem.GetUISprite();

                    if (hotbar.SelectedItem.itemType == Item.ItemType.seed)
                    {
                        if (Input.GetMouseButtonDown(0) && CanUseTool() && selectedTile.Plowed)
                        {
                            selectedTile.Plant(hotbar.SelectedItem.GetPlantPrefab());
                            player.Inventory.RemoveItem(hotbar.SelectedItem, 1);
                        }
                    }
                }
                else
                {
                    holdingItem.color = new Color(1, 1, 1, 0);
                }

                break;

            case 2:
                PlowTool();
                break;

            case 3:
                HandleWateringTool();
                break;

            case 4:
                HandleTerrainTool();
                break;

            default:
                break;
        }

        if (hasScannerTool && selectedTile != null && Vector3.Distance(transform.position, selectedTile.Coordinate) <= scannerToolRange)
        {
            HandleScannerTool();
        }

        else 
        {
            DestroyinfoPanel();
        }
    }

    public bool CanUseTool()
    {
        switch (currentToolIndex)
        {
            case 1:
                return selectedTile != null && Vector3.Distance(transform.position, selectedTile.Coordinate) <= handToolRange;

            case 2:
                return hasPlowTool && selectedTile != null && Vector3.Distance(transform.position, selectedTile.Coordinate) <= plowToolRange;

            case 3:
                return hasWateringTool && selectedTile != null && Vector3.Distance(transform.position, selectedTile.Coordinate) <= wateringToolRange;

            case 4:
                return hasTerrainTool && selectedTile != null && Vector3.Distance(transform.position, selectedTile.Coordinate) <= terrainToolRange;

            default:
                return false;
        }
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
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasPlowTool) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasWateringTool) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasTerrainTool) return 4;

        return currentToolIndex;
    }

    public void HandleTerrainTool()
    {
        if (CanUseTool()){
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
        else 
        {
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
                (int)selectedTile.Humidity);
            currentInfoPanel.transform.SetParent(GameObject.Find("HUD").transform);
        }
        else if (lastScannedTile == selectedTile && currentInfoPanel != null)
        {
            currentInfoPanel.UpdatePanel(
                selectedTile.Name,
                (int)selectedTile.Temperature,
                (int)selectedTile.Luminosity,
                (int)selectedTile.LifeSupport,
                (int)selectedTile.Humidity);
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
        if (CanUseTool())
        {
            if (Input.GetMouseButtonDown(0)){
                player.AddConsumptionWs("WateringTool", wateringConsumptionWs);
            }

            if (Input.GetMouseButtonUp(0)){
                player.RemoveConsumptionWs("WateringTool");
            }

            if (Input.GetMouseButton(0) && tankLevel > 0){
                if (tankLevel > 0){
                    selectedTile.Water(1);
                    tankLevel--;
                }
                else{
                    Debug.Log("Tank is empty!");
                }
            }
        }
        else
        {
            player.RemoveConsumptionWs("WateringTool");
        }   
    }

    private void PlowTool(){
        if (CanUseTool())
        {
            if (Input.GetMouseButton(0))
            {
                if (!selectedTile.Plowed)
                {
                    selectedTile.Plow();
                    player.ConsumeEnergyW(plowToolConsumptionW);
                }
            }
        }
    }
    
}
