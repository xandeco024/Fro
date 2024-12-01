using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Electrical : MonoBehaviour
{
    [SerializeField] protected int timeScaleFactor = 60;

    //about its battery
    [Header("Energy")]
    [SerializeField] protected float maxBatteryWh;
    public float MaxBatteryWh { get { return maxBatteryWh; } }
    [SerializeField] protected float currentBatteryWh;
    public float CurrentBatteryWh { get { return currentBatteryWh; }}



    //about its consumption
    protected Dictionary<string, int> consumptionWsDict = new Dictionary<string, int>{
        {"Base", 0}
    };
    protected float currentConsumptionWs;
    public float CurrentConsumptionWs { get { return currentConsumptionWs; } }
    [SerializeField] private int baseConsumptionWs;



    //about its RECHARGING (RECHARGE ITSELF) that is different from the GENERATION recharge is when it is connected to a power source, charging is when it is charging another device
    [SerializeField] protected bool canRecharge;
    [SerializeField] protected bool canWirelessRecharge;
    [SerializeField] protected bool recharging;



    //about its hability to charge other devices
    [SerializeField] protected bool canCharge;
    [SerializeField] protected int chargeRateWs;
    [SerializeField] protected bool canWirelessCharge;
    [SerializeField] protected int wirelessChargeRateWs;
    [SerializeField] protected float wirelessChargeDistance;
    [SerializeField] protected List<Electrical> devicesToWirelessCharge = new List<Electrical>();



    //about its generation
    [SerializeField] protected bool canGenerate;
    [SerializeField] protected int generationRateWs;

    public virtual void Start()
    {
        currentBatteryWh = maxBatteryWh;
        consumptionWsDict["Base"] = baseConsumptionWs;
    }

    public virtual void Update()
    {
        if (canWirelessCharge)
        {
            devicesToWirelessCharge.Clear();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, wirelessChargeDistance);

            foreach (Collider2D collider in colliders)
            {
                Electrical device = collider.GetComponent<Electrical>();
                if (device != null && device.canWirelessRecharge && device != this && device.currentBatteryWh < device.maxBatteryWh)
                {
                    devicesToWirelessCharge.Add(device);
                }
            }

            if (devicesToWirelessCharge.Count > 0)
            {
                foreach (Electrical device in devicesToWirelessCharge)
                {
                    if (currentBatteryWh > 0 && device.CurrentBatteryWh < device.MaxBatteryWh)
                    {                     
                        float remainingWh = device.MaxBatteryWh - device.CurrentBatteryWh;
                        float transfer = wirelessChargeRateWs;
                        float availableEnergyWs = currentBatteryWh * 3600f;
                        transfer = Mathf.Min(transfer, availableEnergyWs);
                        transfer = Mathf.Min(transfer, remainingWh * 3600f);

                        device.RechargeWs((int)transfer);

                        AddConsumptionWs("WirelessCharge", (int)transfer);
                    }
                }
            }

            else
            {
                RemoveConsumptionWs("WirelessCharge");
            }
        }

        if (canGenerate)
        {
            RechargeWs(generationRateWs);
        }

        HandleConsumption();
    }



    public virtual void HandleConsumption()
    {
        currentConsumptionWs = 0;

        foreach (var consumption in consumptionWsDict.Values)
        {
            currentConsumptionWs += consumption;
        }

        if (currentConsumptionWs > 0)
        {
            ConsumeEnergyWs(currentConsumptionWs);
        }
    }

    public virtual void ConsumeEnergyWs(float amountWs)
    {
        float amountW = amountWs / 3600 * timeScaleFactor * Time.deltaTime;

        if (currentBatteryWh - amountW >= 0)
        {
            currentBatteryWh -= amountW;
        }
        else
        {
            currentBatteryWh = 0;
        }
    }

    public virtual void ConsumeEnergyW(int amountW)
    {
        float currentBatteryWs = currentBatteryWh * 3600;
        amountW *= timeScaleFactor;

        if (currentBatteryWs - amountW >= 0)
        {
            currentBatteryWs -= amountW;
        }
        else
        {
            currentBatteryWs = 0;
        }

        currentBatteryWh = currentBatteryWs / 3600;
    }

    public virtual void AddConsumptionWs(string consumption ,int amountWs)
    {
        consumptionWsDict[consumption] = amountWs;
    }

    public virtual void RemoveConsumptionWs(string consumption)
    {
        consumptionWsDict.Remove(consumption);
    }


    public virtual void RechargeWs(float amountWs)
    {
        float amountW = amountWs / 3600 * timeScaleFactor * Time.deltaTime;

        if (currentBatteryWh + amountW <= maxBatteryWh)
        {
            currentBatteryWh += amountW;
        }
        else
        {
            currentBatteryWh = maxBatteryWh;
        }
    }




    void OnDrawGizmos()
    {
        if (canCharge)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, wirelessChargeDistance);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, wirelessChargeDistance);

            Gizmos.color = Color.blue;
            foreach (Collider2D collider in colliders)
            {
                Electrical device = collider.GetComponent<Electrical>();
                if (device != null)
                {
                    Gizmos.DrawLine(transform.position, device.transform.position);
                }
            }
        }
    }
}
