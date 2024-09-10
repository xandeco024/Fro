using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private Player player;

    [SerializeField] private float wirelessEnergyTransferRate;
    [SerializeField] private float wirelessEnergyTransferDistance;
    [SerializeField] private float baseEnergyGenerationRate;
    private float currentEnergyGenerationRate;
    private float currentEnergy;
    private float maxEnergy;



    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        maxEnergy = 100;
        currentEnergy = maxEnergy;
        currentEnergyGenerationRate = baseEnergyGenerationRate;
    }

    // Update is called once per frame
    void Update()
    {
        TransferEnergy();
        GenerateEnergy();
    }

    void TransferEnergy()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < wirelessEnergyTransferDistance)
        {
            player.ReceiveEnergy(wirelessEnergyTransferRate * Time.deltaTime);
            currentEnergy -= wirelessEnergyTransferRate * Time.deltaTime;
        }

        //draw a line between the base and the player to visualize the energy transfer
        Debug.DrawLine(transform.position, player.transform.position, Color.blue);
    }

    void GenerateEnergy()
    {
        currentEnergy += currentEnergyGenerationRate * Time.deltaTime;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }
}
