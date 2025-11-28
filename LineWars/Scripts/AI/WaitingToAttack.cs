using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaitingToAttack : StateMachineBehaviour
{
    private Unit unit;
    private Transform target;
    private Animator animator;
    private UnitMovement unitMovement;

    private int timer;
    private int time_Max;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unitMovement = animator.transform.GetComponent<UnitMovement>();
        unit = animator.transform.GetComponent<Unit>();
        this.animator = animator;
        time_Max = unit.time_Max;
        timer = 0;
        TickSystem.Instance.OnFPSTick += Tick;
        unit.progressBar.gameObject.SetActive(true);
    }
    private void Tick(object sender, int tick)
    {
        timer++;
        unit.progressBar.UpdateProgress(timer, time_Max);
        if (timer >= time_Max)
        {
            target = FindTarget();
            if (target != null) { unit.target = target; unit.RotateTowardsAttacke(target); animator.SetBool(KeyHolder.Attacking, true); unit.RotateTowardsAttacke(unit.target); }
            else if (unit.enemies.Count > 0)  { unitMovement.moving = true; animator.SetBool(KeyHolder.InAttackRange, false); }
            else { unitMovement.AfterAttackFindPoint(); unitMovement.moving = true; animator.SetBool(KeyHolder.Target_Aquired, false); animator.SetBool(KeyHolder.InAttackRange, false); }
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
        List<Health> preList = unit.enemies;
        List<Health> list = new List<Health>();

        for (int i = 0; i < preList.Count; i++)
        {
            if (preList[i] == null) { continue; }
            foreach (Transform spot in unit.enemies[i].attackPositions)
            {
                if (Vector3.Distance(unit.transform.position, spot.position) <= unit.attackRange) { list.Add(preList[i]); }
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
    /*


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit.RotateTowardsAttacke();
    }

    private void Tick(object sender, int tick)
    {
        if (animator != null)
        {
            timer++;
            if (timer >= time_Max)
            {
                target = FindTarget();
                if (target != null) { unitMovement.moving = false; FoundTarget(); animator?.SetBool(KeyHolder.Attacking, true); }
                else { unitMovement.moving = true; animator?.SetBool(KeyHolder.Target_Aquired, false); }
            }
        }
    }

    private void FoundTarget()
    {
        unit.target = target;
        TickSystem.Instance.OnFPSTick -= Tick;
    }
    */
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit.progressBar.UpdateProgress(0, time_Max);
        TickSystem.Instance.OnFPSTick -= Tick;
        unit.progressBar.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= Tick;
    }
}
