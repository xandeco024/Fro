using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    RectTransform rectTransform;
    Vector2 initialPosition;
    [SerializeField] Canvas canvas;
    CanvasGroup canvasGroup;

    public UIInventory UIinventory;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        //find uiinventory in parents
        UIinventory = GetComponentInParent<UIInventory>();
        if (UIinventory != null) Debug.Log("UIinventory found");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .75f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta * canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = initialPosition;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragAndDrop>() != null)     
        {
            if (UIinventory == eventData.pointerDrag.GetComponent<DragAndDrop>().UIinventory){
                UIinventory.SwapSlots(eventData.pointerDrag.GetComponent<RectTransform>(), rectTransform);
            }
        }
    }
}
