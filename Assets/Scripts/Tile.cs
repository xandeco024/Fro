using UnityEngine;

public class Tile
{
    private Vector3Int coordinate;
    private string tileName;
    private float health;
    private float currentHealth;
    private float humidity;
    private int luminosity;
    private int lifeSupport;
    private float temperature;

    public Vector3Int Coordinate { get => coordinate; }
    public string Name { get => tileName; }
    public float Health { get => health; }
    public float CurrentHealth { get => currentHealth; }
    public float Humidity { get => humidity; }
    public int Luminosity { get => luminosity; }
    public int LifeSupport { get => lifeSupport; }
    public float Temperature { get => temperature; }

    private Tile[] neighbourTiles;

    public Tile(Vector3Int coordinate, TileData data)
    {
        TilemapManager tilemapManager = GameObject.FindObjectOfType<TilemapManager>();

        this.coordinate = coordinate;
        tileName = data.Name;
        health = data.Health;
        currentHealth = health;
        neighbourTiles = new Tile[4] {
            tilemapManager.GetTile(coordinate + Vector3Int.up),
            tilemapManager.GetTile(coordinate + Vector3Int.down),
            tilemapManager.GetTile(coordinate + Vector3Int.left),
            tilemapManager.GetTile(coordinate + Vector3Int.right)
        };
    }

    public void Update()
    {
        //chat gpt fez uns calculos muito doidos e concluimos que 1ml vai dar 0,03% de humidade
        humidity -= 0.003f;

        if (humidity < 0)
        {
            humidity = 0;
        }
        if (humidity > 100)
        {
            humidity = 100;
        }

        if (humidity > 10)
        {
            for (int i = 0; i < neighbourTiles.Length; i++)
            {
                if (neighbourTiles[i] != null)
                {
                    neighbourTiles[i].Water(0.25f);
                    Dry(0.25f);
                }
            }
        }

        Debug.Log("AAAAAAA");
    }

    public void Reset()
    {
        currentHealth = health;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
    }

    public void Water(float amount)
    {
        //chat gpt fez uns calculos muito doidos e concluimos que 1ml vai dar 0,03% de humidade
        humidity += amount * 0.03f;
        if (humidity > 100)
        {
            humidity = 100;
        }
    }

    public void Dry(float amount)
    {
        humidity -= amount * 0.03f;
        if (humidity < 0)
        {
            humidity = 0;
        }
    }

    public void SetTemperature(float temperature)
    {
        this.temperature = temperature;
    }

    public void Heat(float amount)
    {
        temperature += amount;
    }

    public void Cool(float amount)
    {
        temperature -= amount;
    }
}
