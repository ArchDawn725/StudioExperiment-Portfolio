using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BuildingCreator;
using Random = UnityEngine.Random;
public class Controller : MonoBehaviour
{
    [SerializeField] private int starting_Money;

    public int money { get; private set; }
    public int stone { get; private set; }
    public int food { get; private set; }
    public EventHandler<int> MoneyChanged;
    public EventHandler<int> StoneChanged;
    public EventHandler<int> FoodChanged;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI foodText;

    [SerializeField] private int defualtIncomeSpeed;
    private int timer;
    private int time_Max;
    [HideInInspector] public bool gameOver;
    public Transform[] levels;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private int currentLevel;

    public static Controller Instance { get; private set; }
    private void Awake() { Instance = this; }

    private void Start()
    {
        money = starting_Money;
        money = (int)(money * (((double)PlayerPrefs.GetInt(KeyHolder.startingGoldKey, 0) / (KeyHolder.dividedBy0_MaxLevel200 / 1.5f)) + 1));
        Debug.Log(PlayerPrefs.GetInt(KeyHolder.startingGoldKey, 0));
        time_Max =
                    (int)(defualtIncomeSpeed
                     + ((-PlayerPrefs.GetInt(KeyHolder.incomeKey, 0) / (KeyHolder.minusBy1_MaxLevel200 / 2) + 1f) * defualtIncomeSpeed)
                    );
        TickSystem.Instance.OnFPSTick += Tick;
        MoneyChange(0);

        currentLevel = PlayerPrefs.GetInt(KeyHolder.levelKey, 1) - 1;
        GetLevel();

        levels[currentLevel].gameObject.SetActive(true);
        //StoneChange(1000);
    }
    private int GetLevel() 
    {
        if (currentLevel >= levels.Length) { currentLevel -= levels.Length; return GetLevel(); }
        return currentLevel;
    }

    private void Tick(object sender, int tick)
    {
        timer++;
        if (timer >= time_Max)
        {
            timer = 0;
            MoneyChange(1);
        }
    }
    public void MoneyChange(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
        xpText.text = (PlayerPrefs.GetInt(KeyHolder.experienceKey, 0)).ToString();
        MoneyChanged?.Invoke(this, money);
    }
    public void StoneChange(int amount)
    {
        stone += amount;
        stoneText.text = stone.ToString();
        StoneChanged?.Invoke(this, stone);
    }
    public void FoodChange(int amount)
    {
        food += amount;
        foodText.text = food.ToString();
        FoodChanged?.Invoke(this, food);
    }

    public bool SpawnBuildingTest(BuildingSO building, GameObject location, Vector3 spawnPosition, Vector3 unitSpawnPosition, int lane, bool isStone)
    {
        if (!isStone)
        {
            if (money >= building.cost)
            {
                BuildingCreatonData data = new BuildingCreatonData
                {
                    buildingSO = building,
                    location = location,
                    spawnPosition = spawnPosition,
                    unitSpawnPosition = unitSpawnPosition,
                    team = 1,
                    lane = lane,
                    ai = null,
                    isTower = false,
                };

                BuildingCreator creator = BuildingCreator.Create(data);
                MoneyChange(-building.cost);
                return true;
            }
            return false;
        }
        else
        {
            if (stone >= building.cost/2)
            {
                BuildingCreatonData data = new BuildingCreatonData
                {
                    buildingSO = building,
                    location = location,
                    spawnPosition = spawnPosition,
                    unitSpawnPosition = unitSpawnPosition,
                    team = 1,
                    lane = lane,
                    ai = null,
                    isTower = true,
                };

                BuildingCreator creator = BuildingCreator.Create(data);
                StoneChange(-building.cost/2);
                return true;
            }
            return false;
        }

    }
    public void WinGame()
    {
        if (!gameOver)
        {
            TickSystem.Instance.ChangeTime(0);
            gameOver = true;
            gameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You won!";
            gameOverScreen.SetActive(true);

            PlayerPrefs.SetInt(KeyHolder.levelKey, PlayerPrefs.GetInt(KeyHolder.levelKey, 0) + 1);
        }

    }
    public void LoseGame()
    {
        if (!gameOver)
        {
            TickSystem.Instance.ChangeTime(0);
            gameOver = true;
            gameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You lost!";
            gameOverScreen.SetActive(true);
            PlayerPrefs.SetInt(KeyHolder.levelKey, PlayerPrefs.GetInt(KeyHolder.levelKey, 0) - 1);
        }
    }
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
    public void BoardWipe()
    {
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            //unit.GetComponent<Unit>().OnKilled(this, 0);
            Destroy(unit);
        }
        foreach (GameObject building in GameObject.FindGameObjectsWithTag("Building"))
        {
            if (building.GetComponent<Building>().team == -1)
            {
                Destroy(building);
            }
        }
    }

    public void StartWithDouble()
    {
        MoneyChange(money);
        StoneChange(stone);
        FoodChange(food);
    }
    [HideInInspector] public int addedXPSoFar;
    [SerializeField] private Button getDoubleButton;
    public void GetDoubleXP()
    {
        int xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
        PlayerPrefs.SetInt(KeyHolder.experienceKey, xp + addedXPSoFar);
        Controller.Instance.MoneyChange(0);

        getDoubleButton.interactable = false;
    }
    public void EnableDoubleSpeed()
    {

    }
}
