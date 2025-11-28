using UnityEngine;

[CreateAssetMenu()]
public class UnitSO : ScriptableObject
{
    public GameObject unitPrefab;

    public int baseHealth;
    public int baseSpeed;
    public int baseDamage;
    public int baseAttackSpeed;
    public int baseAttackRange;
    public int foodCost;

    public int xp;
    public RuntimeAnimatorController animatorController;
    public GameObject rangedAttack;
}
