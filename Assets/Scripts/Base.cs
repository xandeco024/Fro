using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private Player player;

    [SerializeField] private int chargePerSecond;
    [SerializeField] private int wirelessChargeDistance;
    [SerializeField] private int baseEnergyGenPerSecond;
    private int currentEnergyGenPerSecond;
    private int currentEnergy;
    [SerializeField] private int maxEnergy;



    void Start()
    {
        player = FindObjectOfType<Player>();
        currentEnergy = maxEnergy;
        currentEnergyGenPerSecond = baseEnergyGenPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        TransferEnergy();
        GenerateEnergy();
    }

    void TransferEnergy()
    {

    }

    void GenerateEnergy()
    {

    }
}
