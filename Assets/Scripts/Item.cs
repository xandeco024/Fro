using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private Player player;
    [SerializeField] private float magnetRange;
    [SerializeField] private float magnetSpeed;	

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= magnetRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, magnetSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= 0.1f)
        {
            Debug.Log("Added " + itemData.ItemName + "(mentirakkkkkk)");
            Destroy(gameObject);
        }
    }
}
