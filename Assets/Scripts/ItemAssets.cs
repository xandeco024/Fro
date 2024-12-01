using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    [Header("Plants & Seeds")]
    public Sprite monsteraDeliciosaSeedSprite;
    public GameObject monsteraDeliciosaPlantPrefab;
    public Sprite ficusSeedSprite;
    public GameObject ficusPlantPrefab;

    [Header("Items")]
    public Sprite soilSprite;

    [Header("Scraps")]
    public Sprite eletronicScrapUISprite;
    public Sprite[] eletronicScrapWorldSprites;
    public Sprite plasticScrapUISprite;
    public Sprite[] plasticScrapWorldSprites;
    public Sprite metalScrapUISprite;
    public Sprite[] metalScrapWorldSprites;
    public Sprite glassScrapUISprite;
    public Sprite[] glassScrapWorldSprites;

    [Header("Item World")]
    public Transform pfItemWorld;
}
