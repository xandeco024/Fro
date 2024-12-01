using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Module : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Down");
    }
}
