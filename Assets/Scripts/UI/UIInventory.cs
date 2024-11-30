using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private Storage inventory;
    private RectTransform container;
    private RectTransform slotTemplate;
    private List<RectTransform> slots;
    [SerializeField] private float gap;
    private bool active = false;

    void Start()
    {
        inventory = GameObject.FindFirstObjectByType<Player>().Inventory;
        container = transform.Find("Container").GetComponent<RectTransform>();
        slotTemplate = container.Find("Slot Template").GetComponent<RectTransform>();

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
            // if (!active) ClearSlots();
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in container)
        {
            if (child == slotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float slotSize = slotTemplate.sizeDelta.x;

        foreach(Item item in inventory.GetItems())
        {
            RectTransform slot = Instantiate(slotTemplate, container).GetComponent<RectTransform>();
            slot.gameObject.SetActive(true);
            slot.anchoredPosition = new Vector2(x * slotSize + gap, -y * slotSize);
            slot.transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item.amount.ToString();
            slot.GetComponent<Image>().sprite = item.GetSprite();
            // slots.Add(slot);
            x++;
            if (x > 2)
            {
                x = 0;
                y++;
            }
        }
    }

    private void ClearSlots()
    {
        foreach (RectTransform slot in slots)
        {
            Destroy(slot.gameObject);
        }

        slots.Clear();
    }
}
