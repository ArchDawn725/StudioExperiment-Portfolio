using UnityEngine;

public class Attacking : StateMachineBehaviour
{
    private Unit unit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit = animator.transform.GetComponent<Unit>();
        unit.RotateTowardsAttacke(unit.target);
        unit.RotateTowardsAttacke(unit.target);
        unit.BeginAttack(unit.target);
    }
    /*

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
        unit = animator.transform.GetComponent<Unit>();
        //unit.Attack();
        //animator.speed = (float)((unit. / 100f) + 1);
        unit.RotateTowardsAttacke();
    }
    */
}
