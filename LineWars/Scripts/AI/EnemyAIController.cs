using System.Collections.Generic;
using UnityEngine;
using static BuildingCreator;
public class EnemyAIController : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;

    private int totalBuildingsSpawned;
    [SerializeField] private int amountOfBuildingsToSpawn;
    [SerializeField] private BuildingSO[] availableBuildingSpawns;
    [SerializeField] private int ticksInBetweenBuildingSpawns = -1;

    private int totalTowersSpawned;
    [SerializeField] private int amountOfTowersToSpawn;
    [SerializeField] private BuildingSO[] availableTowerSpawns;
    [SerializeField] private int ticksInBetweenTowerSpawns = -1;

    private int totalUpgrades;
    [SerializeField] private int amountToUpgrade;
    [SerializeField] private BuildingSO[] availableUpgrades;
    [SerializeField] private int ticksInBetweenUpgrades = -1;

    public Color color;
    [HideInInspector] public int level;
    [HideInInspector] public List<Building> spawnedBuildings = new List<Building>();

    private int buildingTimer, towerTimer, upgradeTimer;
    private void Start()
    {
        AIController.Instance.StartUp(this);
        Camera.main.transform.parent.GetComponent<CameraController>().max = transform.parent.position.y;
        level = PlayerPrefs.GetInt(KeyHolder.levelKey, 1) - 1;
    }
    public void StartUp()
    {
        TickSystem.Instance.OnFPSTick += Tick;
    }
    private void Tick(object sender, int tick)
    {
        if (ticksInBetweenBuildingSpawns != -1) { buildingTimer++; if (buildingTimer == (ticksInBetweenBuildingSpawns * totalBuildingsSpawned) + ticksInBetweenBuildingSpawns) { buildingTimer = 0; SpawnBuilding(); } }
        if (ticksInBetweenTowerSpawns != -1) { towerTimer++; if (towerTimer == (ticksInBetweenTowerSpawns * totalTowersSpawned) + ticksInBetweenTowerSpawns) { towerTimer = 0; SpawnTower(); } }
        if (ticksInBetweenUpgrades != -1) { upgradeTimer++; if (upgradeTimer == (ticksInBetweenUpgrades * totalUpgrades) + ticksInBetweenUpgrades) { upgradeTimer = 0; UpgradeBuilding(); } }
    }
    private void SpawnBuilding()
    {
        if (spawnLocations.Length == 0 || availableBuildingSpawns.Length == 0) { Debug.LogError("AI not set up!"); ticksInBetweenBuildingSpawns = -1; return; }

        //gets availble spots
        List<Transform> availbaleSpawnPositions = new List<Transform>();
        foreach (Transform location in spawnLocations) { if (location.CompareTag("Spawner")) { availbaleSpawnPositions.Add(location); } }
        if (availbaleSpawnPositions.Count == 0) { Debug.Log("no open spots!"); ticksInBetweenBuildingSpawns = -1; return; }

        //spawns a random availble building on a random availble spot
        int i = Random.Range(0, availbaleSpawnPositions.Count);
        BuildingCreatonData data = new BuildingCreatonData
        {
            buildingSO = availableBuildingSpawns[Random.Range(0, availableBuildingSpawns.Length)],
            location = availbaleSpawnPositions[i].gameObject,
            spawnPosition = availbaleSpawnPositions[i].transform.position,
            unitSpawnPosition = availbaleSpawnPositions[i].transform.GetChild(1).transform.position,
            lane = int.Parse(availbaleSpawnPositions[i].name),
            team = -1,
            ai = this,
            aiLevel = level,
            isTower = false,
        };

        BuildingCreator creator = BuildingCreator.Create(data);
        availbaleSpawnPositions[i].tag = "Untagged";

        totalBuildingsSpawned++;
        if (totalBuildingsSpawned >= amountOfBuildingsToSpawn) { Debug.Log("finished Spawning!"); ticksInBetweenBuildingSpawns = -1; return; }
    }
    private void SpawnTower()
    {
        if (spawnLocations.Length == 0 || availableTowerSpawns.Length == 0) { Debug.Log("AI has not towers!"); ticksInBetweenTowerSpawns = -1; return; }

        //gets availble spots
        List<Transform> availbaleSpawnPositions = new List<Transform>();
        foreach (Transform location in spawnLocations) { if (location.CompareTag("Tower_Spawner")) { availbaleSpawnPositions.Add(location); } }
        if (availbaleSpawnPositions.Count == 0) { Debug.Log("no open spots!"); ticksInBetweenTowerSpawns = -1; return; }

        //spawns a random availble building on a random availble spot
        int i = Random.Range(0, availbaleSpawnPositions.Count);
        BuildingCreatonData data = new BuildingCreatonData
        {
            buildingSO = availableTowerSpawns[Random.Range(0, availableTowerSpawns.Length)],
            location = availbaleSpawnPositions[i].gameObject,
            spawnPosition = availbaleSpawnPositions[i].transform.position,
            unitSpawnPosition = availbaleSpawnPositions[i].transform.GetChild(1).transform.position,
            lane = int.Parse(availbaleSpawnPositions[i].name),
            team = -1,
            ai = this,
            aiLevel = level,
            isTower = true,
        };

        BuildingCreator creator = BuildingCreator.Create(data);
        availbaleSpawnPositions[i].tag = "Untagged";

        totalTowersSpawned++;
        if (totalTowersSpawned >= amountOfTowersToSpawn) { Debug.Log("finished Spawning Towers!"); ticksInBetweenTowerSpawns = -1; return; }
    }
    private void UpgradeBuilding()
    {
        if (availableUpgrades.Length == 0) { Debug.Log("AI has no Upgrades!"); ticksInBetweenUpgrades = -1; return; }

        //get available Upgrades
        List<Building> buildingsThatCanBeUpgraded = new List<Building>();

        foreach (var building in spawnedBuildings)
        {
            if (building.buildingSO.this_Upgrades_To == null) { continue; }

            foreach (var upgrade in availableUpgrades)
            {
                if (building.buildingSO.this_Upgrades_To == upgrade) { buildingsThatCanBeUpgraded.Add(building); break; }
            }
        }

        //upgrade Building
        if (buildingsThatCanBeUpgraded.Count == 0) { return; }

        buildingsThatCanBeUpgraded[Random.Range(0, buildingsThatCanBeUpgraded.Count)].UpgradeThisBuilding();

        totalUpgrades++;
        if (totalUpgrades >= amountToUpgrade) { Debug.Log("finished Upgrading!"); ticksInBetweenUpgrades = -1; return; }
    }

    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= Tick;
    }
    public void DefeatedMe()
    {
        Controller.Instance.WinGame();
    }
}
