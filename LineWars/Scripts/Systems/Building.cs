using UnityEngine;
using static BuildingCreator;
using static Cinemachine.DocumentationSortingAttribute;

public class Building : MonoBehaviour
{
    public BuildingSO buildingSO;
    [HideInInspector] public bool isTower;
    [HideInInspector] public int team;

    private SpriteRenderer myRend;
    private GameObject location;

    private BuildingCreatonData myData;

    public void StartUp(BuildingCreatonData data)
    {
        buildingSO = data.buildingSO;
        this.team = data.team;
        location = data.location;
        isTower = data.isTower;
        myData = data;

        if (data.ai != null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = data.ai.color;
            data.ai.spawnedBuildings.Add(this);
        }
        else
        {
            //Controller.Instance.moneyPerIncome += buildingSO.incomeAddor;
            //Controller.Instance.stonePerIncome += buildingSO.stoneAddor;
           // Controller.Instance.foodPerIncome += buildingSO.foodAddor;
        }
        myRend = transform.GetChild(0).GetComponent<SpriteRenderer>();

        gameObject.name = data.buildingSO.name;
    }

    private void OnDestroy()
    {
        // Controller.Instance.moneyPerIncome -= buildingSO.incomeAddor;
        // Controller.Instance.stonePerIncome -= buildingSO.stoneAddor;
        // Controller.Instance.foodPerIncome -= buildingSO.foodAddor;
        if (myData.ai != null) { myData.ai.spawnedBuildings.Remove(this); }   
    }
    public void Selected()
    {
        myRend.color = Color.red;
    }
    public void DeSelected()
    {
        myRend.color = Color.white;
    }
    public void UpgradeThisBuilding()
    {
        if (team == 1) { BuildingUpgradeController.Instance.Disable(); }

        BuildingCreatonData data = new BuildingCreatonData
        {
            buildingSO = buildingSO.this_Upgrades_To,
            location = this.location,
            spawnPosition = myData.spawnPosition,
            unitSpawnPosition = myData.unitSpawnPosition,
            lane = myData.lane,
            team = this.team,
            ai = myData.ai,
            aiLevel = myData.aiLevel,
            isTower = this.isTower,
        };

        BuildingCreator.Create(data);
        //Controller.Instance.SpawnBuildingTest(buildingSO.this_Upgrades_To, location, location.transform.position, location.transform.GetChild(1).transform.position, int.Parse(location.name), isTower);
        Destroy(gameObject, 0.1f);
    }
}
