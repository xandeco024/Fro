using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Player player;  
    private PlayerTools playerTools;

    [SerializeField] private Image energyBar;
    [SerializeField] private TextMeshProUGUI consumptionText;
    [SerializeField] private TextMeshProUGUI batteryText;



    [Header("Tools")]
    private bool toolsPanelActive = true;
    [SerializeField] private Image currentToolImage;
    [SerializeField] private Image handToolImage;
    [SerializeField] private Image terrainToolImage;
    [SerializeField] private Image wateringToolImage;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        playerTools = player.GetComponent<PlayerTools>();
    }

    void Update()
    {
        HandleEnergyBar();
        HandleTools();
    }

    void HandleEnergyBar()
    {
        energyBar.fillAmount = player.CurrBatteryWh / player.MaxBatteryWh;
        consumptionText.text = player.CurrentConsumptionWs.ToString() + " Ws";
        batteryText.text = Mathf.RoundToInt(player.CurrBatteryWh).ToString() + " Wh / " + player.MaxBatteryWh.ToString() + " Wh";
    }

    void HandleTools()
    {
        switch (playerTools.CurrentToolIndex)
        {
            case 1:
                //currentToolImage.sprite = handToolImage.sprite;
                currentToolImage.color = handToolImage.color;
                break;

            case 2:
                // currentToolImage.sprite = terrainToolImage.sprite;
                currentToolImage.color = terrainToolImage.color;
                break;

            case 3:
                // currentToolImage.sprite = wateringToolImage.sprite;
                currentToolImage.color = wateringToolImage.color;
                break;
        }

        if (playerTools.SelectingTool && !toolsPanelActive) ToggleToolsPanel(true);
        else if (!playerTools.SelectingTool && toolsPanelActive) ToggleToolsPanel(false);
    }

    void ToggleToolsPanel(bool state)
    {
        toolsPanelActive = state;
        handToolImage.gameObject.SetActive(state);
        terrainToolImage.gameObject.SetActive(state);
        wateringToolImage.gameObject.SetActive(state);
    }
}
