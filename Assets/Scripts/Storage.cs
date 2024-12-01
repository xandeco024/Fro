using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public event EventHandler OnItemListChanged;
    private Item[] items = new Item[12];

    public Storage()
    {
        AddItem(new Item { itemType = Item.ItemType.ficusSeed, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.monsteraDeliciosaSeed, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.clusiaSeed, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.cedarSeed, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.coconutSeed, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.jamboSeed, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.bromeliaSeed, amount = 1 });
    }

    public void SwapItems(int slotA, int slotB)
    {
        Item itemA = items[slotA];
        Item itemB = items[slotB];

        items[slotA] = itemB;
        items[slotB] = itemA;

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddItem(Item item)
    {
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

        if (item.IsStackable()){
            bool itemAlreadyInInventory = false;
            foreach(Item i in items){
                if(i != null && i.itemType == item.itemType){
                    itemAlreadyInInventory = true;
                    i.amount += item.amount;
                }
            }
            if(!itemAlreadyInInventory){
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = item;
                        break;
                    }
                }
            }   
        }
        else {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    break;
                }
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item, int amount)
    {
        if (amount == item.amount){
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    items[i] = null;
                    break;
                }
            }
        }
        else {
            item.amount -= amount;
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public Item[] GetItems()
    {
        return items;
    }

    public Item[] GetHotbarItems()
    {
        Item[] hotbarItems = new Item[3];
        for (int i = 0; i < 3; i++)
        {
            hotbarItems[i] = items[i];
        }
        return hotbarItems;
    }
}
