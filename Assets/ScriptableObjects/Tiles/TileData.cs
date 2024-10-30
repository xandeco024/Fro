using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    [SerializeField] private string tileName;
    [SerializeField] private int tileHealth;
    [SerializeField] private List<TileBase> tiles;
    [SerializeField] private GameObject tileParticles;
    [SerializeField] private GameObject tileDrop;

    public string TileName { get { return tileName; } }
    public int TileHealth { get { return tileHealth; } }
    public List<TileBase> Tiles { get { return tiles; } }
    public GameObject TileParticles { get { return tileParticles; } }
    public GameObject TileDrop { get { return tileDrop; } }
}
