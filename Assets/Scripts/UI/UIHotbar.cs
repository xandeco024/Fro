using UnityEngine;

public class UIHotbar : MonoBehaviour
{
    private Storage inventory;

    private RectTransform container;
    private RectTransform slotTemplate;
    private RectTransform selection;
    private int selectionIndex;
    public int SelectionIndex { get => selectionIndex; }
    public Item SelectedItem { get => inventory.GetHotbarItems()[selectionIndex]; }

    void Start()
    {
        inventory = GameObject.FindFirstObjectByType<Player>().Inventory;
        container = transform.Find("Container").GetComponent<RectTransform>();
        slotTemplate = container.Find("Slot Template").GetComponent<RectTransform>();
        selection = container.Find("Inventory Selection").GetComponent<RectTransform>();

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshHotbarItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectionIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectionIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectionIndex = 2;
        }

        switch (selectionIndex)
        {
            case 0:
                selection.anchoredPosition = new Vector2(0, 109.5f);
                break;
            case 1:
                selection.anchoredPosition = new Vector2(0, 6.5f);
                break;
            case 2:
                selection.anchoredPosition = new Vector2(0, -96.5f);
                break;
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshHotbarItems();
    }

    void RefreshHotbarItems()
    {
        foreach (Transform child in container)
        {
            if (child == slotTemplate || child.name == "Inventory Selection") continue;
            Destroy(child.gameObject);
        }

        int y = 0;
        float slotSize = slotTemplate.sizeDelta.y;

        foreach(Item item in inventory.GetHotbarItems())
        {
            RectTransform slot = Instantiate(slotTemplate, container).GetComponent<RectTransform>();
            
            if (item != null){
                slot.gameObject.SetActive(true);
                slot.anchoredPosition = new Vector2(0, -y * slotSize);
                slot.transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item.amount.ToString();
                slot.GetComponent<UnityEngine.UI.Image>().sprite = item.GetUISprite();
            }

            y++;
        }

        //make the selection child be the last one in the hierarchy
        selection.SetAsLastSibling();
    }
}
