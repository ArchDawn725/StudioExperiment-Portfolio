using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Unit unit;
    public AttackTower tower;
    private void Start()
    {
        //needs something else for arrows
        if (transform.parent != null)
        {
            unit = transform.parent.parent.GetComponent<Unit>();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) { return; }
        if (!collision.gameObject.GetComponent<Health>()) { return; }

        if (unit != null) { unit.Attack(collision.gameObject.GetComponent<Health>()); }
        if (tower != null) { tower.Attack(collision.gameObject.GetComponent<Health>()); }
        //same team?
        //Hit(1000);
    }
}
