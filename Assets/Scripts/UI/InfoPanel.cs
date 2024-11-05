using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI[] infoTexts;

    [SerializeField] private float marginTop, marginBottom, marginLeft, marginRight;
    [SerializeField] private float titleHeight, descriptionHeight, infoHeight;
    [SerializeField] private float titleFontSize, descriptionFontSize, infoFontSize;
    private string title;
    private string description;
    private string[] infos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTitle(string title)
    {
        this.title = title;
        titleText.text = title;
    }


    public void SetInfos(string[] infos)
    {
        this.infos = infos;
    }

    public void UpdatePanel(string title, string description = null, string[] infos = null)
    {
        SetTitle(title);
        SetInfos(infos);
    }
}
