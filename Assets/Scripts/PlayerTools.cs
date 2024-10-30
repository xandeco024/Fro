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
            Vector3Int tilePosition = tilemapManager.GetTileCoordinate(mouseWorldPos);
            tilePosition.z = 0;

            float distance = Vector3.Distance(player.transform.position, tilePosition);

            if (distance <= destroyRange)
            {
                bool tileDestroyed = tilemapManager.ApplyDamageToTile(tilePosition, destroyDamage * Time.deltaTime);
                if (tileDestroyed)
                {
                    Debug.Log("Tile destruído!");
                }
            }
        }
    }
}
