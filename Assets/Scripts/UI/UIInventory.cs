using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class UIInventory : MonoBehaviour
{
    private Storage inventory;
    public Storage Inventory { get => inventory; }
    private RectTransform container;
    public RectTransform[] slots = new RectTransform[12];
    private bool active = false;

    void Start()
    {
        inventory = GameObject.FindFirstObjectByType<Player>().Inventory;
        container = transform.Find("Container").GetComponent<RectTransform>();

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            active = !active;
            container.gameObject.SetActive(active);
            if (active) RefreshInventoryItems();
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void SwapSlots(RectTransform slotA, RectTransform slotB)
    {
        int Aindex = 0, Bindex = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slotA)
            {
                Aindex = i;
            }
            else if (slots[i] == slotB)
            {
                Bindex = i;
            }
        }

        inventory.SwapItems(Aindex, Bindex);
    }

    private void RefreshInventoryItems()
    {
        Item[] items = inventory.GetItems();

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponent<ButtonUI>().onLeftClick.RemoveAllListeners();
            slots[i].GetComponent<ButtonUI>().onRightClick.RemoveAllListeners();

            if (items[i] != null)
            {
                int index = i;

                slots[i].transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = items[i].amount.ToString();
                slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                slots[i].GetComponent<Image>().sprite = items[i].GetUISprite();

                slots[i].GetComponent<ButtonUI>().onLeftClick.AddListener(() => {
                    Debug.Log("Left Clicked");
                });
                slots[i].GetComponent<ButtonUI>().onRightClick.AddListener(() => {
                    Debug.Log("Right Clicked");
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //drop all
                        Item duplicate = new Item { itemType = items[index].itemType, amount = items[index].amount };
                        inventory.RemoveItem(items[index], items[index].amount);
                        ItemWorld.DropItem(GameObject.FindFirstObjectByType<Player>().transform.position, duplicate);
                    }
                    else 
                    {
                        //drop one
                        Item duplicate = new Item { itemType = items[index].itemType, amount = 1 };
                        inventory.RemoveItem(items[index], 1);
                        ItemWorld.DropItem(GameObject.FindFirstObjectByType<Player>().transform.position, duplicate);
                    }
                });
            }
            else
            {
                slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                slots[i].transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
        }
    }
}
