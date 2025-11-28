using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradeController : MonoBehaviour
{
    private Building building;
    //[SerializeField] private int weight;
    [SerializeField] private int reverseWeight;
    private bool active;
    /* Key
     * 0 = health
     * 1 = speed
     * 2 = attack speed
     * 3 = damage
     * 4 = spawn speed
     * 
     * 0 = health + range
     * 1 = speed + spawn speed
     * 2 = damage + attackspeed
     * 3 = building level
     * 4 = Upgrade to new building
    */
    public static BuildingUpgradeController Instance { get; private set; }
    private void Awake() { Instance = this; }
    private void Start()
    {
        Controller.Instance.MoneyChanged += UpdateInteractable;
    }
    public void Enable(Building building)
    {
        UIController.Instance.SelectNewBuildingText(building.buildingSO.name + " Upgrades");
        BuildingSelectionController.Instance.Disable();
        Disable();
        this.building = building;

        ActivateChildren(building.isTower);
        active = true;
    }
    private void ActivateChildren(bool tower)
    {
        //NEEDS BUILDING TYPE
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!tower)
            {
                if (i < 3)
                {
                    if (building.transform.GetComponent<Spawner>() != null) { transform.GetChild(i).gameObject.SetActive(true); }
                    else { transform.GetChild(i).gameObject.SetActive(false); }
                }
                else if (i == 3)
                {
                    if (building.transform.GetComponent<Spawner>() != null) { transform.GetChild(i).gameObject.SetActive(false); }
                    else if (building.transform.GetComponent<IncomeGenerator>() != null) { transform.GetChild(i).gameObject.SetActive(true); }
                    else { transform.GetChild(i).gameObject.SetActive(false); }
                }
                else if (i == 4)
                {
                    if (building.buildingSO.this_Upgrades_To != null) { transform.GetChild(i).gameObject.SetActive(true); transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = building.buildingSO.this_Upgrades_To.mySprite; }
                    else { transform.GetChild(i).gameObject.SetActive(false); }
                }
                else { transform.GetChild(i).gameObject.SetActive(false); }
            }
            else
            {
                if (i < 8 && i > 4)
                {
                    if (building.transform.GetComponent<Spawner>() != null) { transform.GetChild(i).gameObject.SetActive(true); }
                    else { transform.GetChild(i).gameObject.SetActive(false); }
                }
                else if (i == 8)
                {
                    if (building.transform.GetComponent<AttackTower>() != null) { transform.GetChild(i).gameObject.SetActive(true); }
                    else { transform.GetChild(i).gameObject.SetActive(false); }
                }
                else if(i == 9)
                {
                    if (building.buildingSO.this_Upgrades_To != null) { transform.GetChild(i).gameObject.SetActive(true); transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = building.buildingSO.this_Upgrades_To.mySprite; }
                    else { transform.GetChild(i).gameObject.SetActive(false); }
                }
                else { transform.GetChild(i).gameObject.SetActive(false); }
            }

        }
        SetChildrenValues();
    }
    private void SetChildrenValues()
    {
        if (building.transform.GetComponent<Spawner>() != null) 
        {
            Spawner spawner = building.transform.GetComponent<Spawner>();
            for (int i = 0; i < transform.childCount; i++)
            {
                switch(i)
                {
                    case 0: 
                        transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = (spawner.unitHealthBonus / 4).ToString();
                        transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitHealthBonus + 1) * (building.buildingSO.cost / reverseWeight)).ToString();
                        break;
                    case 1: 
                        transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = (spawner.unitSpeedBonus / 4).ToString();
                        transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight)).ToString();
                        break;
                    case 2:
                        transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = (spawner.unitAttackSpeedBonus / 4).ToString();
                        transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitAttackSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight)).ToString();
                        break;
                    /*
                case 3: 
                    transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = spawner.unitDamageBonus.ToString();
                    transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitDamageBonus + 1) * weight).ToString();
                    break;
                case 4: 
                    transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = spawner.spawnSpeedBonus.ToString();
                    transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.spawnSpeedBonus + 1) * weight).ToString();
                    break;
                    */
                    case 5:
                        transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = (spawner.unitHealthBonus / 4).ToString();
                        transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitHealthBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2))).ToString();
                        break;
                    case 6:
                        transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = (spawner.unitSpeedBonus / 4).ToString();
                        transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitSpeedBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2))).ToString();
                        break;
                    case 7:
                        transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = (spawner.unitAttackSpeedBonus / 4).ToString();
                        transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((spawner.unitAttackSpeedBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2))).ToString();
                        break;
                }
            }
        }
        else if (building.transform.GetComponent<IncomeGenerator>() != null || building.transform.GetComponent<AttackTower>() != null)
        {
            IncomeGenerator generator = building.transform.GetComponent<IncomeGenerator>();
            AttackTower tower = building.transform.GetComponent<AttackTower>();

            if (generator != null)
            {
                transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = (generator.incomeSpeedBonus / 4).ToString();
                transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((generator.incomeSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight)).ToString();
            }
            if (tower != null)
            {
                transform.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>().text = (tower.level / 4).ToString();
                transform.GetChild(8).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ((tower.level + 1) * (building.buildingSO.cost / (reverseWeight * 2))).ToString();
            }
        }

        if (building.buildingSO.this_Upgrades_To != null)
        {
            transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (building.buildingSO.this_Upgrades_To.cost).ToString();

            transform.GetChild(9).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(9).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (building.buildingSO.this_Upgrades_To.cost / 2).ToString();
        }

        ChildrenInteractableCheck();
    }
    private void ChildrenInteractableCheck()
    {
        if (building.transform.GetComponent<Spawner>() != null)
        {
            Spawner spawner = building.transform.GetComponent<Spawner>();

            //HAVE ENOUGH MONEY AND LEVEL IS LESS THAN 100
            for (int i = 0; i < transform.childCount; i++)
            {
                switch (i)
                {
                    case 0:
                        if (spawner.unitHealthBonus < 100 && Controller.Instance.money >= (spawner.unitHealthBonus + 1) * (building.buildingSO.cost / reverseWeight)) 
                        { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                        else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                        break;
                    case 1:
                        if (spawner.unitSpeedBonus < 100 && Controller.Instance.money >= (spawner.unitSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight))
                        { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                        else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                        break;
                    case 2:
                        if (spawner.unitAttackSpeedBonus < 100 && Controller.Instance.money >= (spawner.unitAttackSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight))
                        { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                        else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                        break;
                    /*
                case 3:
                    if (spawner.unitDamageBonus < 100 && Controller.Instance.money >= (spawner.unitDamageBonus + 1) * weight)
                    { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                    else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                    break;
                case 4:
                    if (spawner.spawnSpeedBonus < 100 && Controller.Instance.money >= (spawner.spawnSpeedBonus + 1) * weight)
                    { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                    else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                    break;
                    */
                    case 5:
                        if (spawner.unitHealthBonus < 100 && Controller.Instance.stone >= (spawner.unitHealthBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2)))
                        { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                        else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                        break;
                    case 6:
                        if (spawner.unitSpeedBonus < 100 && Controller.Instance.stone >= (spawner.unitSpeedBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2)))
                        { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                        else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                        break;
                    case 7:
                        if (spawner.unitAttackSpeedBonus < 100 && Controller.Instance.stone >= (spawner.unitAttackSpeedBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2)))
                        { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                        else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
                        break;
                }
            }
        }
        else if (building.transform.GetComponent<IncomeGenerator>() != null || building.transform.GetComponent<AttackTower>() != null)
        {
            if (building.transform.TryGetComponent(out IncomeGenerator generator))
            {
                if (generator.incomeSpeedBonus < 100 && Controller.Instance.money >= (generator.incomeSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight))
                { transform.GetChild(3).GetComponent<Button>().interactable = true; }
                else { transform.GetChild(3).GetComponent<Button>().interactable = false; }
            }
            if (building.transform.TryGetComponent(out AttackTower tower))
            {
                if (tower.level < 100 && Controller.Instance.stone >= (tower.level + 1) * (building.buildingSO.cost / (reverseWeight * 2)))
                { transform.GetChild(8).GetComponent<Button>().interactable = true; }
                else { transform.GetChild(8).GetComponent<Button>().interactable = false; }
            }
        }

        if (building.buildingSO.this_Upgrades_To != null)
        {
            if (Controller.Instance.money >= building.buildingSO.this_Upgrades_To.cost && (building.buildingSO.this_Upgrades_To.researchRequired == "none" || PlayerPrefs.GetInt(building.buildingSO.this_Upgrades_To.researchRequired, 0) == 1))
            { transform.GetChild(4).GetComponent<Button>().interactable = true; }
            else { transform.GetChild(4).GetComponent<Button>().interactable = false; }

            if (Controller.Instance.stone >= building.buildingSO.this_Upgrades_To.cost / 2 && (building.buildingSO.this_Upgrades_To.researchRequired == "none" || PlayerPrefs.GetInt(building.buildingSO.this_Upgrades_To.researchRequired, 0) == 1))
            { transform.GetChild(9).GetComponent<Button>().interactable = true; }
            else { transform.GetChild(9).GetComponent<Button>().interactable = false; }
        }
    }

    public void Disable()
    {
        for(int  i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        active = false;
    }
    public void PurchaseUpgrade(int type)
    {
        Spawner spawner = null;
        switch (type)
        {
            case 0: spawner = building.transform.GetComponent<Spawner>(); Controller.Instance.MoneyChange(-(spawner.unitHealthBonus + 1) * (building.buildingSO.cost / reverseWeight)); spawner.unitHealthBonus += 4; spawner.unitRangeBonus += 4; break;
            case 1: spawner = building.transform.GetComponent<Spawner>(); Controller.Instance.MoneyChange(-(spawner.unitSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight)); spawner.unitSpeedBonus += 4; spawner.spawnSpeedBonus += 4; break;
            case 2: spawner = building.transform.GetComponent<Spawner>(); Controller.Instance.MoneyChange(-(spawner.unitAttackSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight)); spawner.unitAttackSpeedBonus += 4; spawner.unitDamageBonus += 4; break;

            case 3: IncomeGenerator generator = building.transform.GetComponent<IncomeGenerator>(); Controller.Instance.MoneyChange(-((generator.incomeSpeedBonus + 1) * (building.buildingSO.cost / reverseWeight))); generator.incomeSpeedBonus += 4; break;

            case 4: Controller.Instance.MoneyChange(-building.buildingSO.this_Upgrades_To.cost); building.UpgradeThisBuilding(); break;

            case 5: spawner = building.transform.GetComponent<Spawner>(); Controller.Instance.StoneChange(-(spawner.unitHealthBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2))); spawner.unitHealthBonus += 4; spawner.unitRangeBonus += 4; break;
            case 6: spawner = building.transform.GetComponent<Spawner>(); Controller.Instance.StoneChange(-(spawner.unitSpeedBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2))); spawner.unitSpeedBonus += 4; spawner.spawnSpeedBonus += 4; break;
            case 7: spawner = building.transform.GetComponent<Spawner>(); Controller.Instance.StoneChange(-(spawner.unitAttackSpeedBonus + 1) * (building.buildingSO.cost / (reverseWeight * 2))); spawner.unitAttackSpeedBonus += 4; spawner.unitDamageBonus += 4; break;

            case 8: AttackTower tower = building.transform.GetComponent<AttackTower>(); Controller.Instance.StoneChange(-((tower.level + 1) * (building.buildingSO.cost / (reverseWeight * 2)))); tower.level += 4; break;

            case 9: Controller.Instance.StoneChange(-building.buildingSO.this_Upgrades_To.cost / 2); building.UpgradeThisBuilding(); break;
        }

        SetChildrenValues();
    }

    private void UpdateInteractable(object sender, int tick)
    {
        if (active)
        {
            ChildrenInteractableCheck();
        }
    }
}
