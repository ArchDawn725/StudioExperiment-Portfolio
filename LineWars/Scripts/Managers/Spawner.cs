using UnityEngine;
using static BuildingCreator;
using static UnitCreator;

public class Spawner : MonoBehaviour
{
    private BuildingSO buildingSO;
    private EnemyAIController ai;

    private int team;
    private int lane;
    private int timer;
    [SerializeField] private int time_Max;

    //upgrades max out at 50
    [HideInInspector] public int unitHealthBonus;
    [HideInInspector] public int unitRangeBonus;

    [HideInInspector] public int unitSpeedBonus;
    [HideInInspector] public int spawnSpeedBonus;

    [HideInInspector] public int unitDamageBonus;
    [HideInInspector] public int unitAttackSpeedBonus;

    [HideInInspector] public bool isTower;

    private Vector3 unitSpawnPosition;
    private ProgressBar progressBar;
    private InstaSpawnUnit spawnUnitButton;
    public void SetUp(BuildingCreatonData data)
    {
        this.ai = data.ai;
        this.team = data.team;
        this.buildingSO = data.buildingSO;
        lane = data.lane;
        unitSpawnPosition = data.unitSpawnPosition;
        this.isTower = data.isTower;

        progressBar = transform.GetChild(1).GetComponent<ProgressBar>();
        progressBar.StartUp();

        time_Max = GetSpawnTime();
        time_Max += 150;

        TickSystem.Instance.OnFPSTick += Tick;

        if (team == 1 && !isTower) { spawnUnitButton = UIController.Instance.CreateInstaUnit(lane / 4, this, buildingSO.unitSO.foodCost, buildingSO.mySprite); }
    }

    private void Tick(object sender, int tick)
    {
        timer++;
        progressBar.UpdateProgress(timer, time_Max);
        if (timer >= time_Max)
        {
            SpawnUnit();
        }
    }
    public void SpawnUnit()
    {
        timer = 0;
        int level = 0;
        if (ai != null) { level = ai.level; }
        UnitCreatonData data = new UnitCreatonData
        {
            unitSO = buildingSO.unitSO,
            spawner = this,
            spawnPosition = unitSpawnPosition,
            lane = this.lane,
            team = this.team,
            ai = this.ai,
            aiLevel = level,
        };
        UnitCreator creator = UnitCreator.Create(data);
        if (team == 1) { time_Max = GetSpawnTime(); time_Max += 150; }
    }
    private void OnDestroy()
    {
        if (spawnUnitButton != null) { Destroy(spawnUnitButton.gameObject); }
        TickSystem.Instance.OnFPSTick -= Tick;
    }
    private int GetSpawnTime()
    {
        /*
                         time_Max = 
                    buildingSO.defualtSpawnTime
                     - (int)(((double)PlayerPrefs.GetInt(KeyHolder.spawnSpeedKey, 0) / KeyHolder.minusBy2_MaxLevel100) * buildingSO.defualtSpawnTime)
                     - (int)(((double)spawnSpeedBonus / KeyHolder.dividedBy2_MaxLevel50) * buildingSO.defualtSpawnTime)
                    ;
         */

        if (team == 1)
        {
            return
                (int)( buildingSO.defualtSpawnTime
                + ((-PlayerPrefs.GetInt(KeyHolder.spawnSpeedKey, 0) / KeyHolder.minusBy2_MaxLevel200 + 0.5f) * buildingSO.defualtSpawnTime)
                + ((-spawnSpeedBonus / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * buildingSO.defualtSpawnTime)
                );
        }
        else
            return 
                (int)(buildingSO.defualtSpawnTime
                + ((-ai.level / KeyHolder.minusBy1_MaxLevel200 + 1f) * buildingSO.defualtSpawnTime)
                //- (int)(((double)ai.building_SpawnSpeedBonus / KeyHolder.minusBy1_MaxLevel100) * buildingSO.defualtSpawnTime)
                );
        /*

(int) (time
+ ((-PlayerPrefs.GetInt(KeyHolder.destinationTimeKey, 0) / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * time)
+ ((-level / KeyHolder.dividedBy2_MaxLevel50 + 0.5f) * time)
);
*/
    }
}
