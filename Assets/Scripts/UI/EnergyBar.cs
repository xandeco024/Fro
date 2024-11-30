using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private Player player;
    [SerializeField] private Image[] fillTop;
    [SerializeField] private Image[] fillBottom;

    private float energyPerFill;
    private float energyInCurrentFill;
    private int currentFillIndex;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float swapTime;
    private float swapTimer;
    private int textIndex;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<Player>();


        text.text = (int)player.CurrentBatteryWh + "Wh / " + (int)player.MaxBatteryWh + "Wh";
        textIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        energyPerFill = player.MaxBatteryWh / 5;
        currentFillIndex = (int)(player.CurrentBatteryWh / energyPerFill);
        energyInCurrentFill = player.CurrentBatteryWh % energyPerFill;
        fillTop[currentFillIndex].fillAmount = energyInCurrentFill / energyPerFill;
        fillBottom[currentFillIndex].fillAmount = energyInCurrentFill / energyPerFill;

        swapTimer += Time.deltaTime;

        if (swapTimer >= swapTime)
        {
            swapTimer = 0;
            if (textIndex == 0)
            {
                text.text = (int)player.CurrentBatteryWh + "Wh / " + (int)player.MaxBatteryWh + "Wh";
                textIndex = 1;
            }
            else
            {
                text.text = (int)player.CurrentConsumptionWs + "Ws";
                textIndex = 0;
            }
        }
    }
}
