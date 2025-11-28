using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    public List<EmergencyNode> e_Nodes;
    [SerializeField] private GameObject emergency;

    public event EventHandler<float> OnMoneyValueChanged;
    public event EventHandler<List<float>> OnZoneValueChanged;
    public event EventHandler<List<float>> OnAgencyValueChanged;
    public event EventHandler<bool> OnFinishedGenerating;
    public float money; //{ get; private set; }

    [SerializeField] private float startTime;
    public int ticksBetweenEmergencies;
    public int downTime;//dynamic? randomized?
    [SerializeField] private int ticks;

    public bool[] agencies;
    [SerializeField] private float[] agencyLoyalties;
    [SerializeField] private float[] zoneLoyalties;

    public BuildingSO placingPrefab;
    public GameObject selectedBuilding;
    public GameObject selectedUnit;
    public float fuelCost;

    public static Controller instance;
    private void Awake() { instance = this; }

    public void FinishedGenerating()
    {
        OnFinishedGenerating?.Invoke(this, true);
        Invoke(nameof(StartUp), startTime);
    }

    private void StartUp() { downTime = ticksBetweenEmergencies; TickSystem.Instance.On1Tick += On1Tick; }

    private void On1Tick(object sender, int tick)
    {
        if (isHoldingShift && !Input.GetKey(KeyCode.LeftShift)) { isHoldingShift = false; selectedUnit.GetComponent<Unit>().DeSelected(); selectedUnit = null; }
        ticks++;
        if (ticks >= downTime) { 
            ticks = 0; 
            if (Random.Range(0,1000) < PlayerPrefs.GetInt(KeyHolder.levelKey, 0)) { SpawnTragedy(); }
            else { SpawnEmergency(); }
        }
    }

    private void SpawnEmergency()
    {
        List<int> availableAgencies = new List<int>();
        for(int i = 0; i < agencies.Length; i++) { if (agencies[i]) { availableAgencies.Add(i); } }
        int agencie = availableAgencies[Random.Range(0, availableAgencies.Count)];

        int type = 0;
        switch (agencie)
        {
            case 1: type = Random.Range(0, 3) + 1; break;
            case 2: type = Random.Range(0, 2) + 1; break;
            case 3: type = Random.Range(0, 5) + 1; break;
        }

        EmergencyNode node = e_Nodes[Random.Range(0, e_Nodes.Count)];
        if (node.claimed) { SpawnEmergency(); return; }
        int zone = node.zone;
        node.claimed = true;
        GameObject spawned = Instantiate(emergency, node.transform.position, Quaternion.identity);
        Emergency spawnedEmergency = spawned.GetComponent<Emergency>();
        spawnedEmergency.StartUp(agencie, type, zone, node, -1);

        if (ticksBetweenEmergencies > 50) { ticksBetweenEmergencies--; downTime = Random.Range(1, ticksBetweenEmergencies * 2); }
    }
    private void ReSpawnEmergency(EmergancyInfo info)
    {
        int agencie = info.agency;
        int type = info.priority - 1;
        int zone = info.zone;
        EmergencyNode node = info.node;
        node.claimed = true;

        GameObject spawned = Instantiate(emergency, node.transform.position, Quaternion.identity);
        Emergency spawnedEmergency = spawned.GetComponent<Emergency>();
        spawnedEmergency.StartUp(agencie, type, zone, node, info.patients);
    }
    private void SpawnTragedy()
    {
        EmergencyNode node = e_Nodes[Random.Range(0, e_Nodes.Count)];
        if (node.claimed) { SpawnEmergency(); return; }
        int zone = node.zone;
        node.claimed = true;
        GameObject spawned = Instantiate(emergency, node.transform.position, Quaternion.identity);
        Emergency spawnedEmergency = spawned.GetComponent<Emergency>();
        spawnedEmergency.StartUp(-1, 1, zone, node, Random.Range(3, 11));

        if (ticksBetweenEmergencies > 50) { downTime = Random.Range(1, ticksBetweenEmergencies * 2); }
    }
    public void LoseEmergency(EmergancyInfo info)
    {
        float difficulty = 1;
        float value = 0;
        int priority = info.priority;

        switch (info.agency)
        {
            case -1: value = 10 * difficulty; break;
            case 1: value = (9f / priority) * difficulty; break;
            case 2: value = (8f / priority) * difficulty; break;
            case 3: value = (12f / priority) * difficulty; break;
        }

        if (info.zone >= 0) { zoneLoyalties[info.zone] -= value * info.patients; }
        if (info.agency > 0) { agencyLoyalties[info.agency] -= value * info.patients; }
        else { agencyLoyalties[1] -= (value * info.patients) / 3; agencyLoyalties[2] -= (value * info.patients) / 3; agencyLoyalties[3] -= (value * info.patients) / 3; }

        UpdateZones();
        ReSpawnEmergency(info);
    }
    public void WinEmergency(EmergancyInfo info)
    {
        float difficulty = 1;
        float value = 0;
        int priority = info.priority;

        if (priority != 0)
        {
            value = (10f / priority) / difficulty;
            /*
            switch (info.agency)
            {
                case 1: value = (9f / priority) / difficulty; break;
                case 2: value = (8f / priority) / difficulty; break;
                case 3: value = (12f / priority) / difficulty; break;
            }
            */
        }
        else { value = 0; }

        if (info.failedTimes < 2)
        {
            value /= (info.failedTimes + 1);
        }
        else { value = 0; }

        if (info.zone >= 0) { zoneLoyalties[info.zone] += value; }
        if (info.agency > 0) { agencyLoyalties[info.agency] += value; }
        else { agencyLoyalties[1] += value / 3; agencyLoyalties[2] += value / 3; agencyLoyalties[3] += value / 3; }


        if (info.zone >= 0) { if (zoneLoyalties[info.zone] > 100) { zoneLoyalties[info.zone] = 100; } }

        value *= 10;
        value *= (((float)PlayerPrefs.GetInt(KeyHolder.incomeKey, 0) / KeyHolder.dividedBy1_MaxLevel100) + 1);

        MoneyValueChange(value);

        UpdateZones();
    }
    private void OnDestroy()
    {
        TickSystem.Instance.On1Tick -= On1Tick;
    }

    public void MoneyValueChange(float value)
    {
        /*
        if (value < 0)
        {
            float newValue = value;
            if (!taxFree) { newValue = value * ((TransitionController.Instance.tax) + 1); }
            value = newValue;

            if (referance != null)
            {
                UtilsClass.CreateWorldTextPopup(value.ToString("f2") + "$", referance, Color.red);
            }
        }
        else if (value > 0)
        {
            float newValue = value;
            if (!taxFree) { newValue = (value * ((TransitionController.Instance.tax) + 1)); }
            newValue -= value;
            value = value - newValue;

            if (referance != null)
            {
                UtilsClass.CreateWorldTextPopup("+" + value.ToString("f2") + "$", referance, Color.green);
            }
        }

        if (natural)
        {
            if (value > 0) { UIController.Instance.MoneyGained += value; }
            else { UIController.Instance.MoneyLost -= value; }
        }
        */
        money += value;
        OnMoneyValueChanged?.Invoke(this, money);
    }
    public void Build(Vector3 pos)
    {
        GameObject newObject = Instantiate(placingPrefab.BuildingPrefab, pos, Quaternion.identity);
        newObject.GetComponent<Building>().StartUp(placingPrefab);
        MoneyValueChange(-placingPrefab.cost);
        placingPrefab = null;
        foreach(EmergencyNode node in e_Nodes)
        {
            if (node.transform.position == new Vector3(pos.x, 0.25f, pos.z)) { node.claimed = true; break; }
        }

        UIController.instance.DismissMenus();
    }
    public void BuyUnit(int value)
    {
        if (selectedBuilding != null)
        {
            Vector3 spawnPos = selectedBuilding.transform.position;
            spawnPos.y = 2;
            GameObject newObject = Instantiate(units[value].unitPrefab, spawnPos, Quaternion.identity);
            Unit newUnit = newObject.GetComponent<Unit>();
            newUnit.StartUp(units[value], selectedBuilding.GetComponent<Building>().type);

            MoneyValueChange(-units[value].cost);
            selectedBuilding = null;
            UIController.instance.DismissMenus();
        }
    }
    public BuildingSO[] buildings;
    public UnitSO[] units;
    public void SelectBuildingToBuild(int value)
    {
        placingPrefab = buildings[value];
    }
    public void NewSelectUnit(GameObject newUnit)
    {
        if (selectedUnit != null) { selectedUnit.GetComponent<Unit>().DeSelected(); }
        selectedUnit = newUnit;
        selectedUnit.GetComponent<Unit>().Selected();
    }
    public void MoveUnitTONewPos(Vector3 pos, bool keepPathfinding)
    {
        if (keepPathfinding) { GridManager.instance.AddToPathfinding(selectedUnit.GetComponent<UnitPathfinder>(), pos); isHoldingShift = true; }
        else 
        {
            GridManager.instance.GetPathfinding(selectedUnit.GetComponent<UnitPathfinder>(), pos);
            selectedUnit.GetComponent<Unit>().DeSelected();
            selectedUnit = null;
        }

    }
    private bool isHoldingShift;

    private void UpdateZones()
    {
        OnZoneValueChanged?.Invoke(this, zoneLoyalties.ToList());
        OnAgencyValueChanged?.Invoke(this, agencyLoyalties.ToList());

        foreach (float zone in zoneLoyalties)
        {
            if (zone <= 0) { Debug.Log("You Lose!"); Debug.Log(fuelCost); SceneManager.LoadSceneAsync(0); }
        }

        foreach (float agency in agencyLoyalties)
        {
            if (agency >= 100) { PlayerPrefs.SetInt(KeyHolder.levelKey, PlayerPrefs.GetInt(KeyHolder.levelKey, 0) + 1); Debug.Log("You Win!"); Debug.Log(fuelCost); SceneManager.LoadSceneAsync(0); }
            if (agency <= 0) { Debug.Log("You Lose!"); Debug.Log(fuelCost); SceneManager.LoadSceneAsync(0); }
        }
    }
}
