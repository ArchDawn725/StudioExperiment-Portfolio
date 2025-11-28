using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectionController : MonoBehaviour
{
    private GameObject location;
    private bool active;
    [SerializeField] private BuildingSO[] buildingSOs;
    public static BuildingSelectionController Instance { get; private set; }
    private void Awake() { Instance = this; }
    private void Start()
    {
        Controller.Instance.MoneyChanged += UpdateInteractable;
    }
    public void Enable(GameObject location, bool tower)
    {
        UIController.Instance.SelectNewBuildingText("Select a building to build");
        //Disable();
        BuildingUpgradeController.Instance.Disable();
        this.location = location;

        ActivateChildren(tower);
        active = true;
    }
    private void ActivateChildren(bool tower)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!tower)
            {
                if (i < 7)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = buildingSOs[i].cost.ToString();
                }
                else { transform.GetChild(i).gameObject.SetActive(false); }
            }
            else
            {
                if (i >= 7)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (buildingSOs[i].cost / 2).ToString();
                }
                else { transform.GetChild(i).gameObject.SetActive(false); }

            }
        }
        ChildrenInteractableCheck();
    }
    private void ChildrenInteractableCheck()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int value = 0; int valueRequired = 0;
            if (i < 7) { value = Controller.Instance.money; valueRequired = buildingSOs[i].cost; }
            else { value = Controller.Instance.stone; valueRequired = buildingSOs[i].cost / 2; }

            if (buildingSOs[i].researchRequired == "none")
            {
                if (value >= valueRequired)
                { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
            }
            else if (PlayerPrefs.GetInt(buildingSOs[i].researchRequired, 0) == 1)
            {
                if (value >= valueRequired)
                { transform.GetChild(i).GetComponent<Button>().interactable = true; }
                else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
            }
            else { transform.GetChild(i).GetComponent<Button>().interactable = false; }
        }
    }
    public void PurchaseBuilding(BuildingSO building)
    {
        if (Controller.Instance.SpawnBuildingTest(building, location, location.transform.position, location.transform.GetChild(1).transform.position, int.Parse(location.name), false))
        {
            location.tag = "Untagged";
            location.SetActive(false);
        }
        //UIController.Instance.SelectNewBuildingText("");
        UIController.Instance.SelectNewBuildingText("Units are now being created.");
        Disable();
    }
    public void PurchaseStoneBuilding(BuildingSO building)
    {
        if (Controller.Instance.SpawnBuildingTest(building, location, location.transform.position, location.transform.GetChild(1).transform.position, int.Parse(location.name), true))
        {
            location.tag = "Untagged";
            location.SetActive(false);
        }
        UIController.Instance.SelectNewBuildingText("");
        Disable();
    }
    public void Disable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        active = false;
    }
    private void UpdateInteractable(object sender, int tick)
    {
        if (active)
        {
            ChildrenInteractableCheck();
        }
    }
}
