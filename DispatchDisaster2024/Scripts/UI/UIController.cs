using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider startSlider;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Slider[] zoneLoyalties;
    [SerializeField] private Image[] agencyLoyalties;

    [SerializeField] private BuildingSO hospitalSO;

    [SerializeField] private UnitSO ambulanceSO;

    [SerializeField] private GameObject mainUI;

    public static UIController instance;
    private void Awake() { instance = this; }

    private void Start()
    {
        Controller.instance.OnFinishedGenerating += FinishedGenerating;
        Controller.instance.OnMoneyValueChanged += InteractableCheck;

        Controller.instance.OnZoneValueChanged += ZoneValueChanged;
        Controller.instance.OnAgencyValueChanged += AgencyValueChanged;
    }

    public void UpdateStartSlider()
    {
        startSlider.value += 9;
    }
    private void FinishedGenerating(object sender, bool tick)
    {
        startSlider.gameObject.SetActive(false);
        InteractableCheck(this, Controller.instance.money);

        mainUI.SetActive(true);
        ActivateCheck();
    }
    private void ActivateCheck()
    {
        for (int i = 1; i < zoneLoyalties.Length + 1; i++)
        {
            if (MapGenerator.instance.zoneMinSizes[i] > 0) { zoneLoyalties[i-1].gameObject.SetActive(true); }
        }

        for (int i = 1; i < agencyLoyalties.Length + 1; i++)
        {
            if (Controller.instance.agencies[i]) { agencyLoyalties[i-1].transform.parent.gameObject.SetActive(true); }
        }
    }
    private void InteractableCheck(object sender, float money)
    {
        moneyText.text = money.ToString("f2") + "$";
    }
    private void ZoneValueChanged(object sender, List<float> values)
    {
        for (int i = 1; i < zoneLoyalties.Length + 1; i++)
        {
            zoneLoyalties[i - 1].value = values[i];
        }
    }

    private void AgencyValueChanged(object sender, List<float> values)
    {
        for (int i = 1; i < agencyLoyalties.Length + 1; i++)
        {
            agencyLoyalties[i - 1].fillAmount = (values[i] / 100f);
        }
    }
    private void OnDestroy()
    {
        Controller.instance.OnFinishedGenerating -= FinishedGenerating;
        Controller.instance.OnMoneyValueChanged -= InteractableCheck;

        Controller.instance.OnZoneValueChanged -= ZoneValueChanged;
        Controller.instance.OnAgencyValueChanged -= AgencyValueChanged;
    }

    public void BuildMenu()
    {
        newUnitMenu.gameObject.SetActive(false);
        if (!buildMenu.gameObject.activeInHierarchy)
        {
            buildMenu.gameObject.SetActive(true);

            //text
            if (!buildingNameText)
            { 
                buildingNameText = buildMenu.GetChild(1).GetComponent<TextMeshProUGUI>();
                //foreach
                for (int i = 1; i < Controller.instance.agencies.Length; i++)
                {
                    buildMenu.GetChild(2).GetChild(i - 1).gameObject.SetActive(Controller.instance.agencies[i]);
                }
            }
            buildingNameText.text = "";

            //image

            //interactables
            for (int i = 0; i < buildMenu.GetChild(2).childCount; i++)
            {
                if (Controller.instance.buildings[i + 1].cost <= Controller.instance.money) { buildMenu.GetChild(2).GetChild(i).GetComponent<Button>().interactable = true; }
                else { buildMenu.GetChild(2).GetChild(i).GetComponent<Button>().interactable = false; }
            }
        }
        else { buildMenu.gameObject.SetActive(false); }
    }
    [SerializeField] private Transform buildMenu;
    [SerializeField] private Transform selectMenu;
    [SerializeField] private Transform newUnitMenu;

    private TextMeshProUGUI buildingNameText;
    private TextMeshProUGUI selectedBuildingNameText;

    private Button buyUnitButton;
    private Building selectedBuilding;

    public void DismissMenus()
    {
        buildMenu.gameObject.SetActive(false);
        selectMenu.gameObject.SetActive(false);
        newUnitMenu.gameObject.SetActive(false);
    }
    public void SelectedBuilding(Building building)
    {
        selectedBuilding = building;
        DismissMenus();
        selectMenu.gameObject.SetActive(true);

        //text
        if (!selectedBuildingNameText)
        {
            selectedBuildingNameText = selectMenu.GetChild(2).GetComponent<TextMeshProUGUI>();
            buyUnitButton = selectMenu.GetChild(4).GetComponent<Button>();
        }

        //name
        selectedBuildingNameText.text = building.myName;

        //image

        //info

        buyUnitButton.gameObject.SetActive(true);
    }

    public void NewUnitButtonPressed()
    {
        buildMenu.gameObject.SetActive(false);
        if (!newUnitMenu.gameObject.activeInHierarchy)
        {
            newUnitMenu.gameObject.SetActive(true);

            //text
            newUnitMenu.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            //image

            //interactables
            for (int i = 0; i < newUnitMenu.GetChild(2).childCount; i++)
            {
                if (i < 3) 
                {
                    if (selectedBuilding.type == 1) { newUnitMenu.GetChild(2).GetChild(i).gameObject.SetActive(true); }
                    else { newUnitMenu.GetChild(2).GetChild(i).gameObject.SetActive(false); continue; }
                }

                if (i > 2 && i < 6)
                {
                    if (selectedBuilding.type == 2) { newUnitMenu.GetChild(2).GetChild(i).gameObject.SetActive(true); }
                    else { newUnitMenu.GetChild(2).GetChild(i).gameObject.SetActive(false); continue; }
                }

                if (i > 5)
                {
                    if (selectedBuilding.type == 3) { newUnitMenu.GetChild(2).GetChild(i).gameObject.SetActive(true); }
                    else { newUnitMenu.GetChild(2).GetChild(i).gameObject.SetActive(false); continue; }
                }

                if (Controller.instance.units[i + 1].cost <= Controller.instance.money) { newUnitMenu.GetChild(2).GetChild(i).GetComponent<Button>().interactable = true; }
                else { newUnitMenu.GetChild(2).GetChild(i).GetComponent<Button>().interactable = false; }
            }
        }
        else { newUnitMenu.gameObject.SetActive(false); }
    }
    public void SelectUnit(Unit unit)
    {
        DismissMenus();
        selectMenu.gameObject.SetActive(true);

        //text
        if (!selectedBuildingNameText)
        {
            selectedBuildingNameText = selectMenu.GetChild(2).GetComponent<TextMeshProUGUI>();
            buyUnitButton = selectMenu.GetChild(4).GetComponent<Button>();
        }

        //name
        selectedBuildingNameText.text = unit.myName;

        //image

        //info

        buyUnitButton.gameObject.SetActive(false);
    }
}
