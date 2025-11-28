using UnityEngine;

[CreateAssetMenu()]
public class BuildingSO : ScriptableObject
{
    public GameObject prefab;
    public int cost;
    public string researchRequired;
    public BuildingSO this_Upgrades_To;
    public Sprite mySprite;

    [Space(10)]
    [Header("Spawners")]
    public UnitSO unitSO;
    public int defualtSpawnTime;
    public bool tower;

    [Space(10)]
    [Header("Producers")]
    public int defualtIncomeTime = -1;
    public int incomeAddor;
    public int stoneAddor;
    public int foodAddor;

    [Space(10)]
    [Header("Towers")]
    public GameObject rangedAttack;
    public int baseDamage;
    public int baseAttackSpeed = -1;
    public int baseAttackRange;
}
