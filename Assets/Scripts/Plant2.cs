using UnityEngine;

public class Plant2 : MonoBehaviour
{
    protected TilemapManager tilemapManager;
    protected Tile tile;

    [SerializeField] protected string plantName;
    [SerializeField] protected string cientificName;
    [SerializeField] protected float growthRate;
    protected int currentStage = 0;
    private float health = 100;
    private float growth= 0;
    private bool dead = false;

    [Header("Preferences")]
    [SerializeField] protected IntRange preferredHumidity;
    [SerializeField] protected IntRange tolerableHumidity;
    [SerializeField] protected IntRange preferredTemperature;
    [SerializeField] protected IntRange tolerableTemperature;
    [SerializeField] protected IntRange preferredLight;
    [SerializeField] protected IntRange tolerableLight;

    [Header("Consumption")]
    [SerializeField] protected float waterConsumptionS;



    [System.Serializable]
    public struct IntRange
    {
        public int min;
        public int max;

        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Contains(int value)
        {
            return value >= min && value <= max;
        }
    }

    [System.Serializable]
    class PlantStage 
    {
        public Sprite healthySprite;
        public Sprite sickSprite;
        public Sprite deadSprite;
        public int growthNeeded;
    }

    [SerializeField] private PlantStage[] stages;

    void Start()
    {
        //find the tile position of the plant, so we can get the tile
        tilemapManager = FindFirstObjectByType<TilemapManager>();
        Vector3 roundedPosition = new Vector3(transform.position.x - 0.5f, transform.position.y - 1, 0);
        tile = tilemapManager.GetTile(tilemapManager.Tilemap.WorldToCell(roundedPosition));
    }

    // Update is called once per frame
    void Update()
    {
        Grow();
    }

    void Grow()
    {
        if (tolerableHumidity.Contains((int)tile.Humidity)){
            if (preferredHumidity.Contains((int)tile.Humidity)){
                growth += growthRate * 2 * Time.deltaTime;
                if (health < 100){
                    health += Time.deltaTime * 2;
                }
            }
            else{
                if (health < 100){
                    health += Time.deltaTime;
                }
                growth += growthRate * Time.deltaTime;
            }
            if (growth >= stages[currentStage].growthNeeded){
                currentStage++;
            }
        }

        else {
            if (health > 0){
                health -= Time.deltaTime;
            }

            if (health <= 0){
                dead = true;
            }
        }

        if (health >= 75){
            GetComponent<SpriteRenderer>().sprite = stages[currentStage].healthySprite;
        }
        else if (health > 0){
            GetComponent<SpriteRenderer>().sprite = stages[currentStage].sickSprite;
        }
        else{
            GetComponent<SpriteRenderer>().sprite = stages[currentStage].deadSprite;
        }

        tile.Dry(waterConsumptionS * Time.deltaTime);
    }
}
