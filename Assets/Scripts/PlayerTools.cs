using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    private Player player;
    private TilemapManager tilemapManager;

    [Header("Destroy")]
    [SerializeField] private bool hasDestroyTool;
    [SerializeField] private float destroyRange;
    [SerializeField] private float destroyDamage;

    [Header("Build")]
    [SerializeField] private bool hasBuildTool;

    [Header("Scanner")]
    [SerializeField] private bool hasScannerTool;

    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindObjectOfType<TilemapManager>();
    }


    void Update()
    {
        if (hasDestroyTool) HandleDestroyTool(tilemapManager.GetSelectedTile());

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
