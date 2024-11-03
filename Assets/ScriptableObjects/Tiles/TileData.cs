using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    [SerializeField] private string tileName;
    [SerializeField] private float health;
    [SerializeField] private List<TileBase> tiles;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject drop;
    [SerializeField] private bool isDestructible;

    public string Name { get { return tileName; } }
    public float Health { get { return health; } }
    public List<TileBase> Tiles { get { return tiles; } }
    public GameObject Particles { get { return particles; } }
    public GameObject Drop { get { return drop; } }
    public bool IsDestructible { get { return isDestructible; } }
}
