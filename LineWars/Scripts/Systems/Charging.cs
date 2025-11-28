
using UnityEngine;

public class Charging : StateMachineBehaviour
{
    private Unit unit;
    private UnitMovement movement;
    [SerializeField] private Transform target;
    [SerializeField] private float chargeSpeed;
    private Animator animator;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        unit = animator.transform.GetComponent<Unit>();
        movement = animator.transform.GetComponent<UnitMovement>();
        unit.EnemyNullChecker();
        target = FindNearestTarget();
        movement.Charging(true, target, chargeSpeed);
        //set aniation speed?
        //unit.RotateTowardsAttacke(target);
        TickSystem.Instance.OnFPSTick += OnTick;
    }

    private int ticks;
    private void OnTick(object sender, int tick)
    {
        ticks++;
        if (ticks % 10 == 0 || target == null)
        {
            unit.EnemyNullChecker();
            target = FindNearestTarget();
            if (target == null)
            {
                movement.Charging(false, null, chargeSpeed);
                movement.moving = true;
                animator.SetBool(KeyHolder.Target_Aquired, false);
                //TickSystem.Instance.OnFPSTick -= OnTick;
                return;
            }
        }

        if (Vector3.Distance(unit.transform.position, target.position) <= unit.attackRange)
        {
            movement.Charging(false, target, chargeSpeed);
            movement.moving = false;
            unit.target = target;
            animator.SetBool(KeyHolder.InAttackRange, true);
        }
    }
    private Transform FindNearestTarget()
    {
        Transform closest = null;
        float minDistance = float.MaxValue; // Initialize with the maximum possible distance

        if (unit.enemies.Count > 0)
        {
            foreach (Health enemy in unit.enemies)
            {
                if (enemy != null)
                {
                    foreach (Transform spot in enemy.attackPositions)
                    {
                        float distance = Vector3.Distance(unit.transform.position, spot.position);
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

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TickSystem.Instance.OnFPSTick -= OnTick;
    }
    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= OnTick;
    }
    /*

    private void OnTick(object sender, int tick)
    {
        //get distance from target and if distance is less than attack range, attack
        if (unit.target != null)
        {
            if (Vector3.Distance(unit.transform.position, unit.target.transform.position) <= unit.attackRange)
            {
                movement.Charging(false);
                movement.moving = false;
                animator.SetBool(KeyHolder.InAttackRange, true);
            }
        }
        else
        {
            movement.Charging(false);
            movement.moving = true;
            animator.SetBool(KeyHolder.InAttackRange, false);
        }
    }
    */
}
