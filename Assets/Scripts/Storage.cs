using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public event EventHandler OnItemListChanged;
    private List<Item> items;

    public Storage()
    {
        items = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.seed, amount = 2 });
        AddItem(new Item { itemType = Item.ItemType.soil, amount = 7 });
        AddItem(new Item { itemType = Item.ItemType.smallEletronicScrap, amount = 3 });
        AddItem(new Item { itemType = Item.ItemType.smallPlasticScrap, amount = 5 });
    }

    public void AddItem(Item item)
    {
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

        if (item.IsStackable()){
            bool itemAlreadyInInventory = false;
            foreach(Item i in items){
                if(i.itemType == item.itemType){
                    itemAlreadyInInventory = true;
                    i.amount += item.amount;
                    return;
                }
            }
            if(!itemAlreadyInInventory){
                items.Add(item);
            }   
        }
        else {
            items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public List<Item> GetItems()
    {
        return items;
    }
}
