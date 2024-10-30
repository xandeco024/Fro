using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private int itemID;

    public string ItemName { get { return itemName; } }
    public Sprite ItemSprite { get { return itemSprite; } }
    public int ItemID { get { return itemID; } }
}
