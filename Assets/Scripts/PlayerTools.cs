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
    [SerializeField] private float destroyRange;
    [SerializeField] private float destroyDamage;

    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindObjectOfType<TilemapManager>();
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = tilemapManager.Tilemap.WorldToCell(mouseWorldPos);
            coordinate.z = 0;
            Tile tile = tilemapManager.GetTile(coordinate);

            if (tile != null && Vector3.Distance(player.transform.position, tile.Coordinate) <= destroyRange)
            {
                tile.Damage(destroyDamage * Time.deltaTime);
                if (tile.CurrentHealth <= 0)
                {
                    tilemapManager.DestroyTile(tile.Coordinate);
                }
            }
        }
    }
}
