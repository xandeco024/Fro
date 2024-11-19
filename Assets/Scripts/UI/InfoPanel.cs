using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private TextMeshProUGUI luminosityText;
    [SerializeField] private TextMeshProUGUI lifeSupportText;
    [SerializeField] private TextMeshProUGUI wetnessText;

    void Start()
    {
        panelRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePanel(string title, int temperature, int luminosity, int lifeSupport, int wetness)
    {
        titleText.text = title;
        temperatureText.text = "- temperature: " + temperature + "°C";
        luminosityText.text = "- luminosity: " + luminosity + " lumens";
        lifeSupportText.text = "- life Support: " + lifeSupport;
        wetnessText.text = "- wetness: " + wetness;   
    }
}