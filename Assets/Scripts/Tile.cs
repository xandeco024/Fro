using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Vector3Int coordinate;
    private string name;
    private float health;
    private float currentHealth;

    public Vector3Int Coordinate { get => coordinate; }
    public string Name { get => name; }
    public float Health { get => health; }
    public float CurrentHealth { get => currentHealth; }

    public Tile(Vector3Int coordinate, TileData data)
    {
        this.coordinate = coordinate;
        name = data.Name;
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

    public void SetHealth(float health)
    {
        this.health = health;
        currentHealth = health;
    }
}
