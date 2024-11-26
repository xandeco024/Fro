using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    private Player player;
    private InputActions inputActions;
    private TilemapManager tilemapManager;
    private Tile selectedTile;



    [SerializeField] private float toolsRange;
    private int currentToolIndex = 1;
    public int CurrentToolIndex { get { return currentToolIndex; } }
    private bool selectingTool;
    public bool SelectingTool { get { return selectingTool; } }



    private bool rightClick;
    private bool lastRightClick;
    private bool leftClick;
    private bool lastLeftClick;



    [Header("Terrain")]
    [SerializeField] private bool hasTerrainTool;
    [SerializeField] private float destroyDamage;
    [SerializeField] private int destroyConsumptionWs;
    [SerializeField] private int buildConsumptionWs;



    [Header("Scanner")]
    [SerializeField] private bool hasScannerTool;
    private Tile lastScannedTile;
    [SerializeField] private GameObject infoPanelPrefab;
    private InfoPanel currentInfoPanel;



    [Header("Watering")]
    [SerializeField] private bool hasWateringTool;
    [SerializeField] private int tankCapacity;
    [SerializeField] private int tankLevel;
    [SerializeField] private int wateringConsumptionWs;



    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputActions();

            inputActions.Player.RightClick.started += ctx => rightClick = true;
            inputActions.Player.RightClick.canceled += ctx => rightClick = false;

            inputActions.Player.LeftClick.started += ctx => leftClick = true;
            inputActions.Player.LeftClick.canceled += ctx => leftClick = false;
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

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
            if (hasScannerTool) HandleScannerTool();

            switch (currentToolIndex)
            {
                case 1:
                    //hand tool
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

        lastRightClick = rightClick;
        lastLeftClick = leftClick;
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
        if (leftClick)
        {
            selectedTile.Damage(destroyDamage * Time.deltaTime);
            if (selectedTile.CurrentHealth <= 0)
            {
                tilemapManager.DestroyTile(selectedTile.Coordinate);
            }

            player.AddConsumption("TerrainTool", destroyConsumptionWs);
        }

        if (!leftClick && lastLeftClick)
        {
            selectedTile.Reset();
            player.RemoveConsumption("TerrainTool");
        }
    }

    public void HandleScannerTool()
    {
        if (lastScannedTile != selectedTile)
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
        else if (lastScannedTile == selectedTile && currentInfoPanel != null)
        {
            currentInfoPanel.UpdatePanel(
                selectedTile.Name,
                selectedTile.Temperature,
                selectedTile.Luminosity,
                selectedTile.LifeSupport,
                selectedTile.Wetness);
        }
    }

    private void HandleWateringTool()
    {
        if (leftClick)
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

            player.AddConsumption("WateringTool", wateringConsumptionWs);
        }

        if (!leftClick && lastLeftClick)
        {
            player.RemoveConsumption("WateringTool");
        }
    }

}
