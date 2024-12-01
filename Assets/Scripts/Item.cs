using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        monsteraDeliciosaSeed,
        ficusSeed,
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
            case ItemType.monsteraDeliciosaSeed: return ItemAssets.Instance.monsteraDeliciosaPlantPrefab;
            case ItemType.ficusSeed: return ItemAssets.Instance.ficusPlantPrefab;
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
            case ItemType.monsteraDeliciosaSeed: return ItemAssets.Instance.monsteraDeliciosaSeedSprite;
            case ItemType.ficusSeed: return ItemAssets.Instance.ficusSeedSprite;
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
            case ItemType.monsteraDeliciosaSeed: return ItemAssets.Instance.monsteraDeliciosaSeedSprite;
            case ItemType.ficusSeed: return ItemAssets.Instance.ficusSeedSprite;
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
            case ItemType.monsteraDeliciosaSeed:
            case ItemType.ficusSeed:
            case ItemType.soil:
            case ItemType.smallEletronicScrap:
            case ItemType.smallPlasticScrap:
            case ItemType.smallMetalScrap:
            case ItemType.smallGlassScrap:
                return true;
        }
    }
}
