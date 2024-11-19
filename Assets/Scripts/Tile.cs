using UnityEngine;

public class Tile
{
    private Vector3Int coordinate;
    private string tileName;
    private float health;
    private float currentHealth;
    private int wetness;
    private int luminosity;
    private int lifeSupport;
    private int temperature;

    public Vector3Int Coordinate { get => coordinate; }
    public string Name { get => tileName; }
    public float Health { get => health; }
    public float CurrentHealth { get => currentHealth; }
    public int Wetness { get => wetness; }
    public int Luminosity { get => luminosity; }
    public int LifeSupport { get => lifeSupport; }
    public int Temperature { get => temperature; }



    public Tile(Vector3Int coordinate, TileData data)
    {
        this.coordinate = coordinate;
        tileName = data.Name;
        health = data.Health;
        currentHealth = health;
    }

    public void Reset()
    {
        currentHealth = health;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
    }

    public void Water(int amount)
    {
        wetness += amount;
    }
}
