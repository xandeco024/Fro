using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Electrical : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    [SerializeField] private int timeScaleFactor = 60;

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

    public virtual void Start()
    {
        if (canWirelessCharge) circleCollider = GetComponent<CircleCollider2D>();

        currentBatteryWh = maxBatteryWh;

        consumptionWsDict["Base"] = baseConsumptionWs;
    }

    public virtual void Update()
    {
        currentConsumptionWs = 0;

        if (canWirelessCharge)
        {
            devicesToWirelessCharge.Clear();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, wirelessChargeDistance);

            foreach (Collider2D collider in colliders)
            {
                Electrical device = collider.GetComponent<Electrical>();
                if (device != null)
                {
                    devicesToWirelessCharge.Add(device);
                }
            }

            if (devicesToWirelessCharge.Count > 0)
            {
                foreach (Electrical device in devicesToWirelessCharge)
                {
                    if (device.canWirelessRecharge)
                    {
                        if (currentBatteryWh > 0)
                        {                            
                            device.RechargeWs(wirelessChargeRateWs * Time.deltaTime);
                            //Debug.Log("devia ter reccaregado " + device.gameObject.name + " com " + wirelessChargeRateWs);
                            ConsumeEnergyWs(wirelessChargeRateWs);
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<string, int> consumption in consumptionWsDict)
        {
            currentConsumptionWs += consumption.Value;
            Debug.Log(consumption.Key + " " + consumption.Value);
        }

        if (currentConsumptionWs > 0)
        {
            ConsumeEnergyWs(currentConsumptionWs);
        }
    }



    public virtual void ConsumeEnergyWs(float amountWs)
    {
        if (currentBatteryWh - amountWs >= 0)
        {
            currentBatteryWh -= amountWs / 3600 *  timeScaleFactor * Time.deltaTime;
            // Debug.Log("Consumed " + amountWs + " W");
        }
        else
        {
            currentBatteryWh = 0;
        }
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
        if (currentBatteryWh + amountWs <= maxBatteryWh)
        {
            currentBatteryWh += amountWs / 3600 * timeScaleFactor * Time.deltaTime;
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
