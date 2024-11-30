using UnityEngine;

public class Plant2 : MonoBehaviour
{
    protected TilemapManager tilemapManager;
    protected Tile tile;

    [SerializeField] protected string plantName;
    [SerializeField] protected float growthRate;
    private float growth;
    [SerializeField] protected int waterConsumptionS;

    [System.Serializable]
    class PlantStage 
    {
        public Sprite sprite;
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
        if (tile.Wetness > 0)
        {
            growth += growthRate * Time.deltaTime;
            tile.Dry(waterConsumptionS * Time.deltaTime);

            if (growth >= stages[stages.Length - 1].growthNeeded)
            {
                //plant is fully grown
            }
            else
            {
                for (int i = 0; i < stages.Length; i++)
                {
                    if (growth < stages[i].growthNeeded)
                    {
                        GetComponent<SpriteRenderer>().sprite = stages[i].sprite;
                        break;
                    }
                }
            }
        }
    }
}
