using System;
using UnityEngine;
public class Health : MonoBehaviour
{
    [HideInInspector] public int team;
    public int health;
    [HideInInspector] public int maxHealth;

    public EventHandler<int> healthChanged;
    public EventHandler<int> death;

    private bool attackable;
    public Transform[] attackPositions;

    [SerializeField] private Transform damagePopUp;

    public void StartUp(int maxHP, int team)
    {
        maxHealth = maxHP;
        health = maxHP;
        this.team = team;

        transform.GetChild(1).GetComponent<HealthBar>().StartUp();
        attackable = true;
    }

    public void Hit(int damage, int maxDamage)
    {
        if (!attackable) { return; }
        DamagePopup.Create(damagePopUp, transform.position, damage, maxDamage);
        health -= damage;
        healthChanged.Invoke(this, health);
        if (health <= 0)
        {
            attackable = false;
            death.Invoke(this, 0);
            Destroy(gameObject, 0.1f);
            ClearMe();
        }
    }

    private void ClearMe()
    {
        
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unitObj in units)
        {
            Unit unitComponent = unitObj.GetComponent<Unit>();

            if (unitComponent != null)
            {
                unitComponent.enemies.Remove(this);
            }
        }
        
    }
    public void Onclick() { healthChanged.Invoke(this, -1); }
}
