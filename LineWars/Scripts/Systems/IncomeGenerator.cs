using UnityEngine;
using static BuildingCreator;
using static UnitCreator;

public class IncomeGenerator : MonoBehaviour
{
    private BuildingSO buildingSO;

    private int timer;
    private int time_Max;

    //upgrades max out at 100
    [HideInInspector] public int incomeSpeedBonus;

    private ProgressBar progressBar;
    public void SetUp(BuildingCreatonData data)
    {
        this.buildingSO = data.buildingSO;

        progressBar = transform.GetChild(1).GetComponent<ProgressBar>();
        progressBar.StartUp();

        time_Max = GetTime();

        if (time_Max <= 0) { time_Max = 1; }
        TickSystem.Instance.OnFPSTick += Tick;
    }

    private void Tick(object sender, int tick)
    {
        timer++;
        progressBar.UpdateProgress(timer, time_Max);
        if (timer >= time_Max)
        {
            GenerateIncome();
        }
    }
    private void GenerateIncome()
    {
        timer = 0;

        //generate income
        if (buildingSO.incomeAddor > 0) { Controller.Instance.MoneyChange(buildingSO.incomeAddor); }
        if (buildingSO.stoneAddor > 0) { Controller.Instance.StoneChange(buildingSO.stoneAddor); }
        if (buildingSO.foodAddor > 0) { Controller.Instance.FoodChange(buildingSO.foodAddor); }

        time_Max = GetTime(); if (time_Max <= 0) { time_Max = 1; }
    }
    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= Tick;
    }
    private int GetTime()
    {
        return
            (int)(buildingSO.defualtIncomeTime
            + ((-PlayerPrefs.GetInt(KeyHolder.generationSpeedKey, 0) / KeyHolder.minusBy2_MaxLevel200 + 0.5f) * buildingSO.defualtIncomeTime)
            + ((-incomeSpeedBonus / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * buildingSO.defualtIncomeTime)
            );
    }
}
