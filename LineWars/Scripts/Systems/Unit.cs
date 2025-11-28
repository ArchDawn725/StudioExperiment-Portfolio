using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnitCreator;

public class Unit : MonoBehaviour
{
    private UnitSO unitSO;
    private Animator animator;
    private int team;

    private UnitMovement unitMovement;

    [HideInInspector] public int time_Max {  get; private set; }

    private int damage;
    private int critChance;
    private int critDamage;
    [HideInInspector] public float attackRange {  get; private set; }
    public List<Health> enemies = new List<Health>();
    public Transform target;

    [SerializeField] private AudioSource[] hitFX;
    [SerializeField] private AudioSource[] swordFX;
    [SerializeField] private AudioSource[] archerFX;

    public ProgressBar progressBar;

    public void StartUP(UnitCreatonData data)
    {
        attackRange = data.unitSO.baseAttackRange * ((data.spawner.unitRangeBonus / 100) + 1);


        this.unitSO = data.unitSO;
        this.team = data.team;
        if (team == -1) { transform.Rotate(0,0,180); }

        unitMovement = GetComponent<UnitMovement>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = unitSO.animatorController;
        unitMovement.moving = true;

        switch (team)
        {
            default: Debug.LogError("No team assigned"); break;
            case 1:
                damage =
                (int)(((double)unitSO.baseDamage)
                     * (((double)PlayerPrefs.GetInt(KeyHolder.damageKey, 0) / KeyHolder.dividedBy2_MaxLevel200) + 1)
                     * (((double)data.spawner.unitDamageBonus / KeyHolder.dividedBy2_MaxLevel100) + 1))
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
                (int)(((double)unitSO.baseDamage)
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

        transform.GetComponent<Health>().death += OnKilled;
        transform.GetComponent<Health>().healthChanged += Hit;

        if (data.ai != null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = data.ai.color;
            transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = data.ai.color;
        }

        progressBar.StartUp();
        progressBar.gameObject.SetActive(false);

        gameObject.name = data.unitSO.name;

        if (unitSO.rangedAttack != null)
        {
            gameObject.layer = 7;
        }
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
            }
            animator.SetBool(KeyHolder.Target_Aquired, true);
        }
    }
    public void EnemyNullChecker()
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
    public void RotateTowardsAttacke(Transform target)
    {
        if (target != null)
        {
            // Calculate the direction from the current object to the target
            Vector2 direction = target.transform.position - transform.position;

            // Calculate the angle in degrees from the current object's direction to the target direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation around the z-axis
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
        else//needs to face next path point, not just forward
        {
            Debug.LogError("Attacke null");
            transform.rotation = new Quaternion(0, 0, 0, 0);
            switch (team)
            {
                case 1:
                    transform.Rotate(0, 0, 0);
                    break;

                case -1:
                    transform.Rotate(0, 0, 180);
                    break;
            }
        }
    }
    public void BeginAttack(Transform rangedTarget)
    {
        if (unitSO.rangedAttack != null && target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion thisrotation = new Quaternion(0f, 0f, angle, 0f);

            GameObject spawnedArrow = Instantiate(unitSO.rangedAttack, transform.position, thisrotation);
            Projectile newProjectile = spawnedArrow.GetComponent<Projectile>();
            Weapon weapon = spawnedArrow.GetComponent<Weapon>();
            weapon.unit = this;
            newProjectile.StartUP(rangedTarget, Vector3.Distance(this.transform.position, rangedTarget.position)/25f);
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
            } else {
                randomDamage = Random.Range(damage / 2, damage);
            }

            targ?.Hit(randomDamage, damage);
            if (unitSO.rangedAttack != null) { archerFX[Random.Range(0, archerFX.Length - 1)].Play(); }
            else { swordFX[Random.Range(0, swordFX.Length - 1)].Play(); }
        }
    }
    private void Hit(object sender, int tick)
    {
        if ( tick != -1)
        {
            transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Play();
            hitFX[Random.Range(0, hitFX.Length - 1)].Play();
        }
    }
    public void OnKilled(object sender, int tick)
    {
        if (team != 1)
        {
            int amount = unitSO.xp;
            int xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
            PlayerPrefs.SetInt(KeyHolder.experienceKey, xp + amount);
            Controller.Instance.MoneyChange(amount);
            Controller.Instance.addedXPSoFar += amount;
        }
    }

    private void OnDestroy()
    {
        transform.GetComponent<Health>().death -= OnKilled;
        transform.GetComponent<Health>().healthChanged -= Hit;
    }

    public void DoneAttack() { animator.SetBool(KeyHolder.Attacking, false); }
    private int GetAttackTime(UnitCreatonData data)
    {
        if (team == 1)
        {
            return
                (int)(unitSO.baseAttackSpeed
                + ((-PlayerPrefs.GetInt(KeyHolder.attackSpeedKey, 0) / KeyHolder.minusBy2_MaxLevel200 + 0.5f) * unitSO.baseAttackSpeed)
                + ((-data.spawner.unitAttackSpeedBonus / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * unitSO.baseAttackSpeed)
                );
        }
        else
            return
                (int)(unitSO.baseAttackSpeed
                + ((-data.aiLevel / KeyHolder.minusBy1_MaxLevel200 + 1f) * unitSO.baseAttackSpeed)
                );
    }

}
