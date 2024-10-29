using UnityEngine;
using UnityEngine.UI;

public class CrosshairFollow : MonoBehaviour
{
    public RectTransform crosshair;  // O RectTransform do crosshair
    public Canvas canvas;            // O Canvas em que o crosshair está

    void Update()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,  // Referência ao Canvas
            Input.mousePosition,              // Posição do mouse
            canvas.worldCamera,               // Câmera do Canvas (ou null para Overlay)
            out mousePosition                 // Converte a posição do mouse
        );

        // Calcula o tamanho do canvas para limitar a posição do crosshair
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float halfWidth = canvasRect.rect.width / 2;
        float halfHeight = canvasRect.rect.height / 2;

        // Limita a posição do crosshair dentro dos limites do Canvas
        mousePosition.x = Mathf.Clamp(mousePosition.x, -halfWidth, halfWidth);
        mousePosition.y = Mathf.Clamp(mousePosition.y, -halfHeight, halfHeight);

        crosshair.localPosition = mousePosition;
    }
}
