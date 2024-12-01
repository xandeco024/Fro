using UnityEditor.Tilemaps;
using UnityEngine;

public class Tile
{
    private TilemapManager tilemapManager;

    private Vector3Int coordinate;
    private string tileName;
    private float health;
    private float currentHealth;
    private float humidity;
    private int luminosity;
    private int lifeSupport;
    private float temperature;
    private bool planted;
    private GameObject plowObject;
    private bool plowed;
    private float plowExpiration = 120;
    private float plowTimer;

    public Vector3Int Coordinate { get => coordinate; }
    public string Name { get => tileName; }
    public float Health { get => health; }
    public float CurrentHealth { get => currentHealth; }
    public float Humidity { get => humidity; }
    public int Luminosity { get => luminosity; }
    public int LifeSupport { get => lifeSupport; }
    public float Temperature { get => temperature; }
    public bool Plowed { get => plowed; }
    public bool Planted { get => planted; }

    private Tile[] neighbourTiles;

    public Tile(Vector3Int coordinate, TileData data, TilemapManager tilemapManager)
    {
        this.tilemapManager = tilemapManager;
        this.coordinate = coordinate;
        tileName = data.Name;
        health = data.Health;
        currentHealth = health;
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

        if (plowed && !planted){
            plowTimer += Time.deltaTime;
            if (plowTimer >= plowExpiration)
            {
                GameObject.Destroy(plowObject);
                plowed = false;
                plowTimer = 0;
            }
        }
    
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

    public void Plow(){
        Vector3 plowObjectPosition = new Vector3(coordinate.x + 0.5f, coordinate.y + 0.75f, 0);
        if (plowObject == null)
        {
            plowObject = GameObject.Instantiate(tilemapManager.PlowPrefab, plowObjectPosition, Quaternion.identity);
        }
        else
        {
            plowObject.transform.position = plowObjectPosition;
        }
        plowed = true;
    }

    public void Plant(GameObject plantPrefab){
        planted = true;

        bool flip = Random.Range(0, 2) == 0;

        GameObject.Instantiate(plantPrefab, new Vector3(coordinate.x + 0.5f, coordinate.y + 1f, 0), Quaternion.identity);
        if (flip)
        {
            plantPrefab.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
