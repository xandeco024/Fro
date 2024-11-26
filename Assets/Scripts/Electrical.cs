using System.Collections.Generic;
using UnityEngine;

public class Electrical : MonoBehaviour
{
    //about its battery
    [SerializeField] protected float maxBatteryWh;
    public float MaxBatteryWh { get { return maxBatteryWh; } }
    [SerializeField] protected float currentBatteryWh;
    public float CurrentBatteryWh { get { return currentBatteryWh; }}



    //about its consumption
    protected Dictionary<string, int> consumptionWsDict = new Dictionary<string, int>{
        {"Base", 0}
    };
    protected float currentConsumptionWs;
    [SerializeField] private int baseConsumptionWs;



    //about its RECHARGING (RECHARGE ITSELF) that is different from the GENERATION recharge is when it is connected to a power source, charging is when it is charging another device
    protected bool recharging;



    //about its hability to charge other devices
    [SerializeField] protected bool canCharge;
    [SerializeField] protected int chargeRateWs;
    [SerializeField] protected bool canWirelessCharge;
    [SerializeField] protected int wirelessChargeRateWs;
    [SerializeField] protected float wirelessChargeDistance;

    public virtual void Start()
    {
        currentBatteryWh = maxBatteryWh;
        consumptionWsDict["Base"] = baseConsumptionWs;
    }

    public virtual void Update()
    {
        
    }

    public virtual void ConsumeEnergyW(float amountW)
    {
        currentBatteryWh -= amountW;
    }

    public virtual void AddConsumption(string key, int value)
    {
        consumptionWsDict[key] = value;
    }

    public virtual void RemoveConsumption(string key)
    {
        consumptionWsDict.Remove(key);
    }

    public virtual void HandleBattery()
    {
        currentConsumptionWs = 0;

        foreach (var item in consumptionWsDict)
        {
            currentConsumptionWs += item.Value;
        }

        if (currentBatteryWh > 0)
        {
            ConsumeEnergyW(currentConsumptionWs / 3600 * 60);
        }

        else
        {
            Debug.Log("dead");
            CancelInvoke("HandleBattery");
        }
    }
}
