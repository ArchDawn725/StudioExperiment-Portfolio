using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    private UnitSO unitSO;
    private Unit unit;
    private UnitMovement unitMovement;
    private Health unitHealth;
    public struct UnitCreatonData
    {
        public UnitSO unitSO;
        public Spawner spawner;
        public Vector3 spawnPosition;
        public int team;
        public int lane;
        public EnemyAIController ai;
        public int aiLevel;
    }

    public static UnitCreator Create(UnitCreatonData data)
    {
        Transform unitTransform = Instantiate(data.unitSO.unitPrefab.transform, data.spawnPosition, Quaternion.Euler(0, 0, 0));
        UnitCreator unitSpawner = unitTransform.GetComponent<UnitCreator>();
        unitSpawner.SetUp(data);
        return unitSpawner;
    }
    private void SetUp(UnitCreatonData data)
    {
        this.unitSO = data.unitSO;
        unit = GetComponent<Unit>();
        unitMovement = GetComponent<UnitMovement>();
        unitHealth = GetComponent<Health>();


        double rangeModifier = 0;
        int health = 0;
        switch (data.team)
        {
            default: Debug.LogError("No team assigned"); break;
            case 1:
                health =
                (int)(((double)unitSO.baseHealth)
                     * (((double)PlayerPrefs.GetInt(KeyHolder.healthKey, 0) / KeyHolder.dividedBy2_MaxLevel200) + 1)
                     * (((double)data.spawner.unitHealthBonus / KeyHolder.dividedBy2_MaxLevel100) + 1))
                    ;
                rangeModifier = 
                    unitSO.baseAttackRange
                    * ((PlayerPrefs.GetInt(KeyHolder.attackRangeKey, 0) / KeyHolder.dividedBy2_MaxLevel200) + 1)
                    * ((data.spawner.unitRangeBonus / KeyHolder.dividedBy2_MaxLevel100) + 1)
                    ;
                break;

            case -1:
                health =
                (int)(((double)unitSO.baseHealth)
                    * (((double)data.aiLevel / KeyHolder.dividedBy1_MaxLevel200) + 1))
                    ;
                rangeModifier = 
                    unitSO.baseAttackRange
                    * ((data.aiLevel / KeyHolder.dividedBy1_MaxLevel200) + 1)
                    ;
                break;
        }

        Vector2 attackRange = new Vector2(GetComponent<CapsuleCollider2D>().size.x, (float)(rangeModifier));
        GetComponent<CapsuleCollider2D>().size = attackRange;

        unitMovement.StartUP(data);
        unitHealth.StartUp(health, data.team);
        unit.StartUP(data);

        Destroy(this);
    }
}
