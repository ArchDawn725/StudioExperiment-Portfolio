using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private EnemyAIController ai;
    [SerializeField] private int baseHealth;

    private void Start()
    {
        int team = 0;
        int health = 0;
        if (ai == null)
        {
            team = 1;
            health = (int)(baseHealth * (((double)PlayerPrefs.GetInt(KeyHolder.castleHealthKey, 0) / (KeyHolder.dividedBy1_MaxLevel200 / 2)) + 1));
        }
        else
        {
            team = -1;
            health = (int)(baseHealth * (((double)ai.level / KeyHolder.dividedBy1_MaxLevel200) + 1));
        }
        transform.GetComponent<Health>().StartUp(health, team);
        transform.GetComponent<Health>().death += OnKilled;

        if (ai != null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = ai.color;
        }

    }
    private void OnKilled(object sender, int tick)
    {
        if (ai != null)
        {
            int amount = 10 + 10 * PlayerPrefs.GetInt(KeyHolder.levelKey, 0);//(10 * AIController.Instance.xpBonus());
            int xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
            PlayerPrefs.SetInt(KeyHolder.experienceKey, xp + amount);
            Controller.Instance.MoneyChange(amount);
            Controller.Instance.addedXPSoFar += amount;
        }

        if (ai == null)
        {
            Controller.Instance.LoseGame();
        }
        else if (!Controller.Instance.gameOver) { ai.DefeatedMe(); }
    }

    private void OnDestroy()
    {
        transform.GetComponent<Health>().death -= OnKilled;
    }
}
