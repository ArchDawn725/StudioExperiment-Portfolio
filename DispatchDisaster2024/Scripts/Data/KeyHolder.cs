using UnityEngine;

public class KeyHolder
{
    public const string experienceKey = "experience";
    public const string levelKey = "level";

    public const string incomeKey = "income";
    public const string startingMoneyKey = "starting money";
    public const string fuelCostsKey = "fuel costs";

    public const string unitCostsKey = "unit costs";
    public const string buildingCostsKey = "building costs";

    public const string speedKey = "speed";

    public const string destinationTimeKey = "Destination time";
    public const string sceneTimeKey = "Scene time";
    public const string callTimesKey = "Caller times";

    //objective is to get a variable to x5 or 6 at max values
    public const float dividedBy2_MaxLevel100 = 100;
    public const float dividedBy2_MaxLevel50 = 50;
    public const float dividedBy1_MaxLevel100 = 25;
    public const float dividedBy0_MaxLevel100 = 10;

    //objective is to get a variable to equal a percentage of its value down to 0 at max value
    public const float minusBy2_MaxLevel100 = 200;
    public const float minusBy2_MaxLevel50 = 100;
    public const float minusBy1_MaxLevel100 = 100;

    /*
    [Space(10)]
    [Header("Research")]
    public const string archer_Research = "archer_Research";
    public const string market_Research = "market_Research";

    [Space(10)]
    [Header("Animator")]
    public const string Target_Aquired = "Target_Aquired";
    public const string Attacking = "Attacking";
    public const string InAttackRange = "InAttackRange";

    [Space(10)]
    [Header("Tags")]
    public const string weapon_Tag = "Weapon";
    */
}
