using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Player player;  
    private PlayerTools playerTools;

    [SerializeField] private Image energyBar;
    [SerializeField] private TextMeshProUGUI energyBarText;



    [Header("Tools")]
    private bool toolsPanelActive = true;
    [SerializeField] private Image currentToolImage;
    [SerializeField] private Image handToolImage;
    [SerializeField] private Image terrainToolImage;
    [SerializeField] private Image wateringToolImage;
    [SerializeField] private Image toolHighlight;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        playerTools = player.GetComponent<PlayerTools>();
    }

    void Update()
    {
        HandleTools();
    }



    void HandleTools()
    {
        switch (playerTools.CurrentToolIndex)
        {
            case 1:
                currentToolImage.sprite = handToolImage.sprite;
                break;

            case 2:
                currentToolImage.sprite = terrainToolImage.sprite;
                break;

            case 3:
                currentToolImage.sprite = wateringToolImage.sprite;
                break;
        }

        if (playerTools.SelectingTool && !toolsPanelActive) ToggleToolsPanel(true);
        else if (!playerTools.SelectingTool && toolsPanelActive) ToggleToolsPanel(false);
        if (playerTools.SelectingTool) 
        {
            switch (playerTools.CurrentToolIndex)
            {
                case 1:
                    toolHighlight.rectTransform.anchoredPosition = handToolImage.rectTransform.anchoredPosition;
                    break;

                case 2:
                    toolHighlight.rectTransform.anchoredPosition = terrainToolImage.rectTransform.anchoredPosition;
                    break;

                case 3:
                    toolHighlight.rectTransform.anchoredPosition = wateringToolImage.rectTransform.anchoredPosition;
                    break;
            }
        }
    }

    void ToggleToolsPanel(bool state)
    {
        toolsPanelActive = state;
        handToolImage.gameObject.SetActive(state);
        terrainToolImage.gameObject.SetActive(state);
        wateringToolImage.gameObject.SetActive(state);
        toolHighlight.gameObject.SetActive(state);
    }
}
