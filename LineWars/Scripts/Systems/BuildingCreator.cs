using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    private BuildingSO buildingSO;

    public struct BuildingCreatonData
    {
        public BuildingSO buildingSO;
        public GameObject location;
        public Vector3 spawnPosition;
        public Vector3 unitSpawnPosition;
        public int team;
        public int lane;
        public EnemyAIController ai;
        public int aiLevel;
        public bool isTower;
    }

    //before spawning
    public static BuildingCreator Create(BuildingCreatonData data)
    {
        Transform unitTransform = Instantiate(data.buildingSO.prefab.transform, data.spawnPosition, Quaternion.Euler(0, 0, 0));
        BuildingCreator unitSpawner = unitTransform.GetComponent<BuildingCreator>();
        unitSpawner.SetUp(data);
        return unitSpawner;
    }

    //after spawning
    private void SetUp(BuildingCreatonData data)
    {
        buildingSO = data.buildingSO;

        Spawner spawner = GetComponent<Spawner>();
        if (buildingSO.unitSO != null) { spawner.SetUp(data); }
        else { Destroy(spawner); }

        IncomeGenerator generator = GetComponent<IncomeGenerator>();
        if (buildingSO.defualtIncomeTime != -1) { generator.SetUp(data); }
        else { Destroy(generator); }

        AttackTower tower = GetComponent<AttackTower>();
        if (buildingSO.baseAttackSpeed != -1) { tower.SetUp(data); }
        else { Destroy(tower); Destroy(transform.GetComponent<CircleCollider2D>()); }


        Building building = GetComponent<Building>();
        if (building != null) { building.StartUp(data); }

        Destroy(this);
    }
}
