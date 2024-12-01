using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Player player;  
    private PlayerTools playerTools;



    [Header("Bars")]
    [SerializeField] GameObject waterTank;
    private Image waterTankFiller;
    private TextMeshProUGUI waterTankText;
    [SerializeField] GameObject soilTank;
    private Image soilTankFiller;
    private TextMeshProUGUI soilTankText;
    [SerializeField] GameObject hotbar;



    [Header("Tools")]
    private bool toolsPanelActive = true;
    [SerializeField] private GameObject toolsPanel;
    [SerializeField] private RectTransform selection;
    [SerializeField] private Sprite[] toolSprites;
    [SerializeField] private Image currentToolImage;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        playerTools = player.GetComponent<PlayerTools>();

        waterTankFiller = waterTank.transform.Find("Filler").GetComponent<Image>();
        waterTankText = waterTank.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        soilTankFiller = soilTank.transform.Find("Filler").GetComponent<Image>();
        soilTankText = soilTank.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        currentToolImage.sprite = toolSprites[0];
    }

    void Update()
    {
        HandleTools();

        switch (playerTools.CurrentToolIndex)
        {
            case 1:
                waterTank.SetActive(false);
                soilTank.SetActive(false);
                hotbar.SetActive(true);
                break;

            case 2:
            case 4:
                waterTank.SetActive(false);
                soilTank.SetActive(true);
                hotbar.SetActive(false);

                soilTankFiller.fillAmount = (float)playerTools.SoilTankLevel / (float)playerTools.SoilTankCapacity;
                soilTankText.text = playerTools.SoilTankLevel * 100 / playerTools.SoilTankCapacity + "'/.";
                break;

            case 3:
                waterTank.SetActive(true);
                soilTank.SetActive(false);
                hotbar.SetActive(false);

                waterTankFiller.fillAmount = (float)playerTools.WaterTankLevel / (float)playerTools.WaterTankCapacity;
                waterTankText.text = playerTools.WaterTankLevel * 100 / playerTools.WaterTankCapacity + "'/.";
                break;
        }
    }



    void HandleTools()
    {
        if (playerTools.SelectingTool){
            toolsPanelActive = true;
            toolsPanel.SetActive(true);
        }
        else {
            toolsPanelActive = false;
            toolsPanel.SetActive(false);
        }

        if (toolsPanelActive){

            switch (playerTools.CurrentToolIndex){
                case 1:
                    selection.anchoredPosition = new Vector2(-418.2f, -44.8f);
                    currentToolImage.sprite = toolSprites[0];
                    break;

                case 2:
                    selection.anchoredPosition = new Vector2(-319.2f, -44.8f);
                    currentToolImage.sprite = toolSprites[1];
                    break;

                case 3:
                    selection.anchoredPosition = new Vector2(-219.8f, -44.8f);
                    currentToolImage.sprite = toolSprites[2];
                    break;
                case 4:
                    selection.anchoredPosition = new Vector2(-120.3f, -44.8f);
                    currentToolImage.sprite = toolSprites[3];
                    break;
            }
        }
    }
}
