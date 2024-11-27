using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
    private Player player;



    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        WirelessCharge();
    }

    void WirelessCharge()
    {
        // if (Vector3.Distance(player.transform.position, transform.position) <= wirelessChargeDistance && currBatteryKWh > 0)
        // {
        //     // Energia que o robô ainda precisa para completar a bateria
        //     float remainingWh = player.MaxBatteryWh - player.CurrentBatteryWh;

        //     // Calcula a quantidade a ser transferida (em Ws)
        //     float transfer = wirelessChargeRateWs * Time.deltaTime;

        //     // Converte a energia da base de kWh para Ws (1 kWh = 3600 Wh)
        //     float availableEnergyWs = currBatteryKWh * 3600f;

        //     // A quantidade a ser transferida não pode ser maior do que a energia disponível na base
        //     transfer = Mathf.Min(transfer, availableEnergyWs);
            
        //     // A quantidade a ser transferida também não pode ser maior do que a energia que o robô ainda precisa
        //     transfer = Mathf.Min(transfer, remainingWh * 3600f);

        //     // Carrega o robô com a energia calculada
        //     // player.ChargeWs(transfer);

        //     // Reduz a energia da base (converte Ws de volta para kWh)
        //     currBatteryKWh -= transfer / 3600f;
        }
    }
}
