using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Player player;  
    [SerializeField] private Image energyBar;
    [SerializeField] private TextMeshProUGUI consumptionText;
    [SerializeField] private TextMeshProUGUI batteryText;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {

        energyBar.fillAmount = player.CurrBatteryWh / player.MaxBatteryWh;
        consumptionText.text = player.CurrentConsumptionWs.ToString() + " Ws";
        batteryText.text = Mathf.RoundToInt(player.CurrBatteryWh).ToString() + " Wh / " + player.MaxBatteryWh.ToString() + " Wh";
    }
}
