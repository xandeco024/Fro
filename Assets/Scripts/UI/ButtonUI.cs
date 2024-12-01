using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Eventos")]
    public UnityEvent onLeftClick;  // Ação para clique esquerdo
    public UnityEvent onRightClick; // Ação para clique direito

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeftClick?.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick?.Invoke();
        }
    }
}
