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

    public GameObject GetPlantPrefab()
    {
        switch (itemType)
        {
            default:
            case ItemType.seed: return ItemAssets.Instance.seedPlantPrefab;
            case ItemType.soil:
            case ItemType.smallEletronicScrap:
            case ItemType.smallPlasticScrap:
            case ItemType.smallMetalScrap:
            case ItemType.smallGlassScrap:
                return null;
        }
    }

    public Sprite GetWorldSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.seed: return ItemAssets.Instance.seedSprite;
            case ItemType.soil: return ItemAssets.Instance.soilSprite;
            case ItemType.smallEletronicScrap: return ItemAssets.Instance.eletronicScrapWorldSprites[Random.Range(0, ItemAssets.Instance.eletronicScrapWorldSprites.Length)];
            case ItemType.smallPlasticScrap: return ItemAssets.Instance.plasticScrapWorldSprites[Random.Range(0, ItemAssets.Instance.plasticScrapWorldSprites.Length)];
            case ItemType.smallMetalScrap: return ItemAssets.Instance.metalScrapWorldSprites[Random.Range(0, ItemAssets.Instance.metalScrapWorldSprites.Length)];
            case ItemType.smallGlassScrap: return ItemAssets.Instance.glassScrapWorldSprites[Random.Range(0, ItemAssets.Instance.glassScrapWorldSprites.Length)];
        }
    }

    public Sprite GetUISprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.seed: return ItemAssets.Instance.seedSprite;
            case ItemType.soil: return ItemAssets.Instance.soilSprite;
            case ItemType.smallEletronicScrap: return ItemAssets.Instance.eletronicScrapUISprite;
            case ItemType.smallPlasticScrap: return ItemAssets.Instance.plasticScrapUISprite;
            case ItemType.smallMetalScrap: return ItemAssets.Instance.metalScrapUISprite;
            case ItemType.smallGlassScrap: return ItemAssets.Instance.glassScrapUISprite;
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
