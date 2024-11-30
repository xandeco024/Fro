using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite seedSprite;
    public Sprite soilSprite;
    public Sprite[] eletronicScrapSprite;
    public Sprite[] plasticScrapSprite;
    public Sprite[] metalScrapSprite;
    public Sprite[] glassScrapSprite;

    public Transform pfItemWorld;
}
