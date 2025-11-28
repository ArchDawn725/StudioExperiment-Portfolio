using UnityEngine;

public class UnitTest : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private float range;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    private UnitTest enemy;
    public int dir;
    [SerializeField] private GameObject arrow;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) { return; }
        if (!collision.gameObject.GetComponent<UnitTest>()) { return; }
        target = collision.gameObject.transform;
        animator.SetBool(KeyHolder.Target_Aquired, true);
    }
    private void Update()
    {
        if (animator.GetBool(KeyHolder.Target_Aquired))
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, range);
            foreach(Collider2D collider2D in collider2Ds)
            {
                if (collider2D.isTrigger) { continue; }

                if (collider2D.GetComponent<UnitTest>() != null)
                {
                    if (collider2D.GetComponent<UnitTest>() != this)
                    {
                        enemy = collider2D.GetComponent<UnitTest>();
                        animator.SetBool(KeyHolder.InAttackRange, true);
                        //collider2D.GetComponent<UnitTest>().Hit();
                    }
                }
            }
        }
    }
    public void Attack()
    {
        enemy.Hit();
        animator.SetBool(KeyHolder.Attacking, false);

        float rotationAmount = 0; if (dir == -1) { rotationAmount = 180; }
        Quaternion thisrotation = new Quaternion(0f, 0f, rotationAmount, 0f);
        GameObject spawnedArrow = Instantiate(arrow, transform.position, thisrotation);
        Projectile newProjectile = spawnedArrow.GetComponent<Projectile>();
        newProjectile.StartUP(enemy.transform, Vector3.Distance(this.transform.position, enemy.transform.position)/12.5f);
    }
    public void Hit()
    {
        Debug.Log("Hit");
    }
}
