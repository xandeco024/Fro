using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
    private Player player;

    [Header("Energy")]
    [SerializeField] private float maxBatteryKWh;
    public float MaxBatteryKWh { get { return maxBatteryKWh; } }
    [SerializeField] private float currBatteryKWh;
    public float CurrBatteryKWh { get { return currBatteryKWh; } }

    //consumption
    [SerializeField] private float baseConsumptionKWs;
    public float BaseConsumptionKWs { get { return baseConsumptionKWs; } }
    [SerializeField] private float currentConsumptionKWs;
    public float CurrentConsumptionKWs { get { return currentConsumptionKWs; } }

    //generation
    [SerializeField] private float baseGenerationKWs;
    public float BaseGenerationKWs { get { return baseGenerationKWs; } }
    [SerializeField] private float currentGenerationKWs;
    public float CurrentGenerationKWs { get { return currentGenerationKWs; } }

    //charging
    [SerializeField] private float chargeRateWs;
    public float ChargeRateWs { get { return chargeRateWs; } }
    [SerializeField] private float wirelessChargeRateWs;
    public float WirelessChargeRateWs { get { return wirelessChargeRateWs; } }
    [SerializeField] private float wirelessChargeDistance;



    void Start()
    {
        player = FindFirstObjectByType<Player>();
        currBatteryKWh = maxBatteryKWh;
    }

    // Update is called once per frame
    void Update()
    {
        WirelessCharge();
    }

    void WirelessCharge()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= wirelessChargeDistance && currBatteryKWh > 0)
        {
            // Energia que o robô ainda precisa para completar a bateria
            float remainingWh = player.MaxBatteryWh - player.CurrBatteryWh;

            // Calcula a quantidade a ser transferida (em Ws)
            float transfer = wirelessChargeRateWs * Time.deltaTime;

            // Converte a energia da base de kWh para Ws (1 kWh = 3600 Wh)
            float availableEnergyWs = currBatteryKWh * 3600f;

            // A quantidade a ser transferida não pode ser maior do que a energia disponível na base
            transfer = Mathf.Min(transfer, availableEnergyWs);
            
            // A quantidade a ser transferida também não pode ser maior do que a energia que o robô ainda precisa
            transfer = Mathf.Min(transfer, remainingWh * 3600f);

            // Carrega o robô com a energia calculada
            // player.ChargeWs(transfer);

            // Reduz a energia da base (converte Ws de volta para kWh)
            currBatteryKWh -= transfer / 3600f;
        }
    }

    void HandleBattery()
    {
        currentConsumptionKWs = currentConsumptionKWs * Time.deltaTime / 3600 * 60;

        currBatteryKWh -= currentConsumptionKWs;

        if (currBatteryKWh < 0)
        {
            currBatteryKWh = 0;
        }
    }
}
