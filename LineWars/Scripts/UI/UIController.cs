using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Transform instaSpawningZoneHolder;
    [SerializeField] private TextMeshProUGUI selectedBuildingText;

    [SerializeField] private GameObject instaSpawnButton;

    public static UIController Instance { get; private set; }
    private void Awake() { Instance = this; }

    public void SelectNewBuildingText(string message)
    {
        selectedBuildingText.text = message;
    }
    public InstaSpawnUnit CreateInstaUnit(int zone, Spawner building, int foodCost, Sprite sprite)
    {
        Debug.Log(zone);
        GameObject newObject = Instantiate(instaSpawnButton,instaSpawningZoneHolder.GetChild(zone));
        newObject.GetComponent<InstaSpawnUnit>().SetUp(building, foodCost, sprite);
        return newObject.GetComponent<InstaSpawnUnit>();
    }
    public void SpawnUnit(Spawner spawner)
    {
        spawner.SpawnUnit();
    }

}
