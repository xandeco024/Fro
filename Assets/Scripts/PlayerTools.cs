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
    private int currentToolIndex = 0;

    [Header("Destroy")]
    [SerializeField] private bool hasDestroyTool;
    [SerializeField] private float destroyDamage;

    [Header("Build")]
    [SerializeField] private bool hasBuildTool;

    [Header("Scanner")]
    [SerializeField] private bool hasScannerTool;
    private Tile lastScannedTile;
    [SerializeField] private GameObject infoPanelPrefab;
    private InfoPanel currentInfoPanel;

    [Header("Watering")]
    [SerializeField] private bool hasWateringTool;
    [SerializeField] private int tankCapacity;
    [SerializeField] private int tankLevel;

    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindObjectOfType<TilemapManager>();
        tankLevel = tankCapacity;
    }


    void Update()
    {
        currentToolIndex = UpdateSelectedTool();
        selectedTile = tilemapManager.GetSelectedTile();

        if (CanUseTool())
        {
            if (hasScannerTool) HandleScannerTool();

            switch (currentToolIndex)
            {
                case 1:
                    HandleDestroyTool();
                    break;

                case 2:
                    //HandleBuildTool(selectedTile);
                    break;

                case 3:
                    HandleWateringTool();
                    break;

                default:
                    break;
            }
        }
    }

    private bool CanUseTool()
    {
        return selectedTile != null && Vector3.Distance(player.transform.position, selectedTile.Coordinate) <= toolsRange;
    }

    private int UpdateSelectedTool()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) && hasDestroyTool) return 1;
        if (Input.GetKeyDown(KeyCode.Keypad2) && hasBuildTool) return 2;
        if (Input.GetKeyDown(KeyCode.Keypad3) && hasWateringTool) return 3;

        return currentToolIndex;
    }

    public void HandleDestroyTool()
    {
        if (Input.GetMouseButton(0))
        {
            selectedTile.Damage(destroyDamage * Time.deltaTime);
            if (selectedTile.CurrentHealth <= 0)
            {
                tilemapManager.DestroyTile(selectedTile.Coordinate);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedTile.Reset();
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
        if (Input.GetMouseButton(0))
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
