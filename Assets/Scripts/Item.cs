using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        seed,
        soil,
        smallEletronicScrap,
        smallPlasticScrap,
        smallMetalScrap,
        smallGlassScrap,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.seed: return ItemAssets.Instance.seedSprite;
            case ItemType.soil: return ItemAssets.Instance.soilSprite;
            case ItemType.smallEletronicScrap: return ItemAssets.Instance.eletronicScrapSprite[Random.Range(0, ItemAssets.Instance.eletronicScrapSprite.Length)];
            case ItemType.smallPlasticScrap: return ItemAssets.Instance.plasticScrapSprite[Random.Range(0, ItemAssets.Instance.plasticScrapSprite.Length)];
            case ItemType.smallMetalScrap: return ItemAssets.Instance.metalScrapSprite[Random.Range(0, ItemAssets.Instance.metalScrapSprite.Length)];
            case ItemType.smallGlassScrap: return ItemAssets.Instance.glassScrapSprite[Random.Range(0, ItemAssets.Instance.glassScrapSprite.Length)];
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.seed:
            case ItemType.soil:
            case ItemType.smallEletronicScrap:
            case ItemType.smallPlasticScrap:
            case ItemType.smallMetalScrap:
            case ItemType.smallGlassScrap:
                return true;
        }
    }
}
