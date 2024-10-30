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
    [SerializeField] private int destroyDamage;
    private float destroyProgress;
    private float tileHealth;

    void Start()
    {
        player = GetComponent<Player>();
        tilemapManager = FindObjectOfType<TilemapManager>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (tilemapManager.GetSelectedTileBase() != null)
            {
                tileHealth = tilemapManager.GetSelectedTileData().TileHealth;
                destroyProgress = 0;
            }
        }

        if (Input.GetMouseButton(0))
        {
            float distance = Vector3.Distance(player.transform.position, tilemapManager.GetSelectedTileCoordinate());
            if (distance <= destroyRange)
            {
                destroyProgress += destroyDamage * Time.deltaTime;
                if (destroyProgress >= tileHealth)
                {
                    tilemapManager.DestroyTile(tilemapManager.GetSelectedTileCoordinate());
                    destroyProgress = 0;
                }
            }
        }
        else 
        {
            destroyProgress = 0;
        }
    }
}
