using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    public const string experienceKey = "experience";
    public const string levelKey = "level";

    public const string castleHealthKey = "Castle Health";
    public const string incomeKey = "Income";
    public const string startingGoldKey = "Starting Gold";

    //temp
    public const string attackSpeedKey = "Attack Speed";
    public const string damageKey = "Damage";
    public const string healthKey = "Health";
    public const string speedKey = "Speed";
    public const string spawnSpeedKey = "Spawn Time";
    public const string attackRangeKey = "Attack Range";
    public const string generationSpeedKey = "Generation Speed";
    public const string critChanceKey = "Crit Chance";
    public const string critDamageKey = "Crit Damage";

    //objective is to get a variable to x5 or 6 at max values
    public const float dividedBy2_MaxLevel200 = 100; //player
    public const float dividedBy2_MaxLevel100 = 50; //player
    public const float dividedBy1_MaxLevel200 = 50; //ai and castle health
    public const float dividedBy0_MaxLevel200 = 10; //player

    //objective is to get a variable to equal a percentage of its value down to 0 at max value
    public const float minusBy2_MaxLevel200 = 200; //player
    public const float minusBy2_MaxLevel100 = 100; //player
    public const float minusBy1_MaxLevel200 = 200; //ai + income


    [Space(10)]
    [Header("Research")]
    public const string warrior_2_Research = "warrior_2_Research";
    public const string warrior_3_Research = "warrior_3_Research";
    public const string warrior_4_Research = "warrior_4_Research";
    public const string warrior_5_Research = "warrior_5_Research";
    public const string archer_1_Research = "archer_1_Research";
    public const string archer_2_Research = "archer_2_Research";
    public const string market_Research = "market_Research";
    public const string quarry_Research = "quarry_Research";
    public const string farm_Research = "farm_Research";
    public const string tower_Research = "tower_Research";
    public const string spear_1_Research = "spear_1_Research";
    public const string spear_2_Research = "spear_2_Research";
    public const string spear_3_Research = "spear_3_Research";
    public const string calvery_Research = "cavalry_Research";

    public const string bronzeAge_Research = "bronzeAge_Research";

    [Space(10)]
    [Header("Animator")]
    public const string Target_Aquired = "Target_Aquired";
    public const string Attacking = "Attacking";
    public const string InAttackRange = "InAttackRange";

    [Space(10)]
    [Header("Tags")]
    public const string weapon_Tag = "Weapon";
}
