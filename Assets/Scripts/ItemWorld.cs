using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float pickupRadius;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);

        foreach (Collider2D collider in colliders)
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.Inventory.AddItem(item);
                Destroy(gameObject);
                break;
            }
        }
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();

        TMPro.TextMeshPro amountText = transform.Find("Amount").GetComponent<TMPro.TextMeshPro>();

        if (item.amount > 1){
            transform.Find("Amount").GetComponent<TMPro.TextMeshPro>().text = item.amount.ToString();
        }
        else {
            amountText.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
