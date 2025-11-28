using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static BuildingCreator;
using static UnitCreator;

public class AttackTower : MonoBehaviour
{
    private BuildingSO buildingSO;
    private int team;

    private int damage;
    private int critChance;
    private int critDamage;
    private float attackRange;

    private int timer;
    private int time_Max;

    [SerializeField] private Transform target;
    [SerializeField] private List<Health> enemies = new List<Health>();

    [HideInInspector] public int level;

    [SerializeField] private AudioSource[] archerFX;
    private bool attacking;
    private ProgressBar progressBar;
    public void SetUp(BuildingCreatonData data)
    {
        this.buildingSO = data.buildingSO;
        this.team = data.team;
        if (team == -1) { transform.Rotate(0, 0, 180); }

        GetAttackRange();

        switch (team)
        {
            default: Debug.LogError("No team assigned"); break;
            case 1:
                damage =
                (int)(((double)buildingSO.baseDamage)
                     * (((double)PlayerPrefs.GetInt(KeyHolder.damageKey, 0) / KeyHolder.dividedBy2_MaxLevel200) + 1)
                     * (((double)level / KeyHolder.dividedBy2_MaxLevel100) + 1))
                    ;
                critChance =
                    PlayerPrefs.GetInt(KeyHolder.critChanceKey, 0)
                    ;
                critDamage =
                    (int)(damage
                    * ((PlayerPrefs.GetInt(KeyHolder.critDamageKey, 0) / 100f) + 1)
                    );
                break;

            case -1:
                damage =
                (int)(((double)buildingSO.baseDamage)
                    * (((double)data.aiLevel / KeyHolder.dividedBy1_MaxLevel200) + 1))
                    ;
                critChance =
                    data.aiLevel
                   ;
                critDamage =
                    (int)(damage
                    * ((data.aiLevel / 100f) + 1)
                    );
                break;
        }

        time_Max = GetAttackTime(data);

        progressBar = transform.GetChild(1).GetComponent<ProgressBar>();
        progressBar.StartUp();
        progressBar.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) { return; }
        if (!collision.gameObject.GetComponent<Health>()) { return; }

        if (collision.gameObject.GetComponent<Health>().team != team)
        {
            if (!enemies.Contains(collision.gameObject.GetComponent<Health>()))
            {
                enemies.Add(collision.gameObject.GetComponent<Health>());
                EnemyNullChecker();
                if (enemies.Count > 0 && !attacking) { PrepareAttack(); }
            }
        }
    }
    private void EnemyNullChecker()
    {
        List<Health> tempList = enemies;
        for (int i = tempList.Count - 1; i >= 0; i--)
        {
            if (tempList[i] == null)
            {
                tempList.RemoveAt(i);
            }
        }
        enemies = tempList;
    }
    private void PrepareAttack()
    {
        attacking = true;
        timer = 0;
        progressBar.gameObject.SetActive(true);
        TickSystem.Instance.OnFPSTick += Tick;
    }
    private void EndAttack()
    {
        attacking = false;
        progressBar.gameObject.SetActive(false);
        TickSystem.Instance.OnFPSTick -= Tick;
    }
    private void Tick(object sender, int tick)
    {
        timer++;
        progressBar.UpdateProgress(timer, time_Max);
        if (timer >= time_Max)
        {
            EnemyNullChecker();
            target = FindTarget();
            if (target != null) { BeginAttack(target); }
            //else if (enemies.Count > 0) { unitMovement.moving = true; animator.SetBool(KeyHolder.InAttackRange, false); }
            else { EndAttack(); }
        }
    }
    private Transform FindTarget()
    {
        List<Health> list = TargetsInRange();
        if (list.Count > 0)
        {
            target = FindNearestTarget(list);
            if (target == null) { return FindTarget(); }
            return target;
        }
        else { return null; }
    }
    private List<Health> TargetsInRange()
    {
        List<Health> preList = enemies;
        List<Health> list = new List<Health>();

        for (int i = 0; i < preList.Count; i++)
        {
            if (preList[i] == null) { continue; }
            foreach (Transform spot in enemies[i].attackPositions)
            {
                if (Vector3.Distance(transform.position, spot.position) <= attackRange) { list.Add(preList[i]); }
            }
        }
        return list;
    }
    private Transform FindNearestTarget(List<Health> enemies)
    {
        Transform closest = null;
        float minDistance = float.MaxValue; // Initialize with the maximum possible distance

        if (enemies.Count > 0)
        {
            foreach (Health enemy in enemies)
            {
                if (enemy != null)
                {
                    foreach (Transform spot in enemy.attackPositions)
                    {
                        float distance = Vector3.Distance(transform.position, spot.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closest = spot;
                        }
                    }
                }
            }

            return closest;
        }
        else { return null; }
    }
    //called when attacking
    private void BeginAttack(Transform rangedTarget)
    {
        timer = 0;
        progressBar.UpdateProgress(timer, time_Max);
        if (target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion thisrotation = new Quaternion(0f, 0f, angle, 0f);

            GameObject spawnedArrow = Instantiate(buildingSO.rangedAttack, transform.position, thisrotation);
            Projectile newProjectile = spawnedArrow.GetComponent<Projectile>();
            Weapon weapon = spawnedArrow.GetComponent<Weapon>();
            weapon.tower = this;
            newProjectile.StartUP(rangedTarget, Vector3.Distance(this.transform.position, rangedTarget.position) / 25f);
        }
    }
    public void Attack(Health targ)
    {
        if (targ.team != team)
        {
            int randomDamage = 0;
            if (Random.Range(0, 200) + critChance >= 200)
            {
                randomDamage = Random.Range(damage, critDamage);
            }
            else
            {
                randomDamage = Random.Range(damage / 2, damage);
            }

            targ?.Hit(randomDamage, damage);
            archerFX[Random.Range(0, archerFX.Length - 1)].Play();
        }
    }
    private int GetAttackTime(BuildingCreatonData data)
    {
        GetAttackRange();

        if (team == 1)
        {
            return
                (int)(buildingSO.baseAttackSpeed
                + ((-PlayerPrefs.GetInt(KeyHolder.attackSpeedKey, 0) / KeyHolder.minusBy2_MaxLevel200 + 0.5f) * buildingSO.baseAttackSpeed)
                + ((-level / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * buildingSO.baseAttackSpeed)
                );
        }
        else
            return
                (int)(buildingSO.baseAttackSpeed
                + ((-data.aiLevel / KeyHolder.minusBy1_MaxLevel200 + 1f) * buildingSO.baseAttackSpeed)
                );
    }
    private void GetAttackRange()
    {
        attackRange = buildingSO.baseAttackRange * ((level / 100) + 1);
        GetComponent<CircleCollider2D>().radius = buildingSO.baseAttackRange;
    }
}
