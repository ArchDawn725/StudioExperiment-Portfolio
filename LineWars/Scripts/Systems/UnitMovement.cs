using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnitCreator;

public class UnitMovement : MonoBehaviour
{
    private UnitSO unitSO;
    private Spawner spawner;
    private EnemyAIController ai;

    private double baseMovementSpeed;
    [SerializeField]private double movementSpeed;
    private Animator animator;
    private int team;

    public bool moving;

    //sealized for testing
    [SerializeField] private int lane;
    [SerializeField] private int currentNode;

    private Transform target;
    private Rigidbody2D rb;
    public void StartUP(UnitCreatonData data)
    {
        lane = data.lane;
        //get path
        animator = GetComponent<Animator>();
        this.ai = data.ai;
        this.unitSO = data.unitSO;
        this.team = data.team;
        spawner = data.spawner;
        switch(team)
        {
            default: Debug.LogError("No team assigned"); break;
            case 1:
                baseMovementSpeed =
                ((double)unitSO.baseSpeed / 2500f)
                     * (((double)PlayerPrefs.GetInt(KeyHolder.speedKey, 0) / KeyHolder.dividedBy2_MaxLevel200) + 1)
                     * (((double)spawner.unitSpeedBonus / KeyHolder.dividedBy2_MaxLevel100) + 1)
                     //any random modifiers?
                    //* team
                    ;
                break;

            case -1:
                baseMovementSpeed =
                ((double)unitSO.baseSpeed / 2500f)
                    * (((double)data.aiLevel / KeyHolder.dividedBy1_MaxLevel200) + 1)
                    //any random modifiers?
                    //* team
                    ;
                break;
        }
        movementSpeed = baseMovementSpeed;
        float number = (float)movementSpeed / 25f; movementSpeed += Random.Range(-number, number);

        animator.speed = (float)((movementSpeed / 100f) + 1) * TickSystem.Instance.timeMultiplier;
        if (team == -1 || spawner.isTower) { currentNode = GetStartingSection(); }
        rb = GetComponent<Rigidbody2D>();
        TickSystem.Instance.OnFPSTick += Tick;
        TickSystem.Instance.OnSpeedChange += OnTimeChangeTick;
    }
    public void Charging(bool value, Transform target, float speed)
    {
        //different charging multipliers
        if (value)
        {
            movementSpeed = baseMovementSpeed * speed;
        }
        else
        {
            movementSpeed = baseMovementSpeed;
        }
        animator.speed = (float)((movementSpeed / 100f) + 1) * TickSystem.Instance.timeMultiplier;

        if (target != null) { this.target = target; }
    }
    [SerializeField] private int movingFailsafe;
    [SerializeField] private int failsafeTeleportAtTicks;
    private int pastTarget;
    private void Tick(object sender, int tick)
    {
        if (moving)
        {
            pastTarget = currentNode;
            MoveTowardsTarget();

            if (pastTarget != currentNode) { movingFailsafe = 0; }
            if (movingFailsafe > failsafeTeleportAtTicks) { FailedToReachDestination(); }
            movingFailsafe++;
        }
        else { movingFailsafe = 0; }
    }
    private void OnTimeChangeTick(object sender, float speed)
    {
        animator.speed = (float)((movementSpeed / 100f) + 1) * speed;
    }
    /*
    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = GetTargetPosition(); // Ensure this method returns a valid Vector3

        if (target != null)
        {
            targetPosition = target.position;
        }

        Vector2 moveDir = (targetPosition - transform.position).normalized;

        // Move
        rb.velocity = moveDir * (float)movementSpeed * TickSystem.Instance.timeMultiplier;

        // Rotation
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle - 90); // Adjust the angle as per your game's orientation requirements

        // Check if target is reached (consider this part if it fits into the Rigidbody movement logic)
        if (target == null)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                currentNode += team; // Update path node logic here
            }
        }
    }
    */
    
    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = GetTargetPosition();
        if (target != null)
        {
            targetPosition = target.position;
        }


        Vector3 moveDir = (targetPosition - transform.position).normalized;

        //rotation
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Rotate(angle);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        //move
        transform.position = transform.position + moveDir * (float)movementSpeed * TickSystem.Instance.timeMultiplier;

        if (target == null)
        {             //change path node?
            if (Vector3.Distance(transform.position, targetPosition) < 0.75f) { currentNode += team; }
        }

    }
    
    private float rotationSpeed = 15f; // Speed of rotation
    private void Rotate(float angle)
    {
        // Desired rotation around the z-axis
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);

        // Smoothly interpolate to the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private Vector3 GetTargetPosition()
    {
        if (currentNode <= 0) { currentNode = 0; }
     
        switch(lane)
        {
            case 0:
                if (currentNode >= PathController.Instance.path_1_0.Count) { return PathController.Instance.path_1_0[PathController.Instance.path_1_0.Count - 1]; } else { return PathController.Instance.path_1_0[currentNode]; }
            case 1:
                if (currentNode >= PathController.Instance.path_1_1.Count) { return PathController.Instance.path_1_1[PathController.Instance.path_1_1.Count - 1]; } else { return PathController.Instance.path_1_1[currentNode]; }
            case 2:
                if (currentNode >= PathController.Instance.path_1_2.Count) { return PathController.Instance.path_1_2[PathController.Instance.path_1_2.Count - 1]; } else { return PathController.Instance.path_1_2[currentNode]; }
            case 3:
                if (currentNode >= PathController.Instance.path_1_3.Count) { return PathController.Instance.path_1_3[PathController.Instance.path_1_3.Count - 1]; } else { return PathController.Instance.path_1_3[currentNode]; }

            case 4:
                if (currentNode >= PathController.Instance.path_2_0.Count) { return PathController.Instance.path_2_0[PathController.Instance.path_2_0.Count - 1]; } else { return PathController.Instance.path_2_0[currentNode]; }
            case 5:
                if (currentNode >= PathController.Instance.path_2_1.Count) { return PathController.Instance.path_2_1[PathController.Instance.path_2_1.Count - 1]; } else { return PathController.Instance.path_2_1[currentNode]; }
            case 6:
                if (currentNode >= PathController.Instance.path_2_2.Count) { return PathController.Instance.path_2_2[PathController.Instance.path_2_2.Count - 1]; } else { return PathController.Instance.path_2_2[currentNode]; }
            case 7:
                if (currentNode >= PathController.Instance.path_2_3.Count) { return PathController.Instance.path_2_3[PathController.Instance.path_2_3.Count - 1]; } else { return PathController.Instance.path_2_3[currentNode]; }

            case 8:
                if (currentNode >= PathController.Instance.path_3_0.Count) { return PathController.Instance.path_3_0[PathController.Instance.path_3_0.Count - 1]; } else { return PathController.Instance.path_3_0[currentNode]; }
            case 9:
                if (currentNode >= PathController.Instance.path_3_1.Count) { return PathController.Instance.path_3_1[PathController.Instance.path_3_1.Count - 1]; } else { return PathController.Instance.path_3_1[currentNode]; }
            case 10:
                if (currentNode >= PathController.Instance.path_3_2.Count) { return PathController.Instance.path_3_2[PathController.Instance.path_3_2.Count - 1]; } else { return PathController.Instance.path_3_2[currentNode]; }
            case 11:
                if (currentNode >= PathController.Instance.path_3_3.Count) { return PathController.Instance.path_3_3[PathController.Instance.path_3_3.Count - 1]; } else { return PathController.Instance.path_3_3[currentNode]; }
        }

        return transform.position;
    }
    private int GetStartingSection()
    {
        int closestPointIndex = -1;
        float closestDistance = float.MaxValue;

        switch (lane)
        {
            case 0: 
                for (int i = 0; i < PathController.Instance.path_1_0.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_1_0[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 1:
                for (int i = 0; i < PathController.Instance.path_1_1.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_1_1[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 2:
                for (int i = 0; i < PathController.Instance.path_1_2.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_1_2[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 3:
                for (int i = 0; i < PathController.Instance.path_1_3.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_1_3[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;

            case 4:
                for (int i = 0; i < PathController.Instance.path_2_0.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_2_0[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 5:
                for (int i = 0; i < PathController.Instance.path_2_1.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_2_1[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 6: 
                for (int i = 0; i < PathController.Instance.path_2_2.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_2_2[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 7: 
                for (int i = 0; i < PathController.Instance.path_2_3.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_2_3[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;

            case 8:
                for (int i = 0; i < PathController.Instance.path_3_0.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_3_0[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 9: 
                for (int i = 0; i < PathController.Instance.path_3_1.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_3_1[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 10:
                for (int i = 0; i < PathController.Instance.path_3_2.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_3_2[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            case 11: 
                for (int i = 0; i < PathController.Instance.path_3_3.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, PathController.Instance.path_3_3[i]);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
        }
        return 0;
    }
    public void AfterAttackFindPoint()
    {
        if (Vector3.Distance(transform.position, GetTargetPosition()) > 5)
        {
            GetStartingSection();
        }
    }
    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= Tick;
        TickSystem.Instance.OnSpeedChange -= OnTimeChangeTick;
    }

    private void FailedToReachDestination()
    {
        currentNode += team;
        Debug.Log("Failed");
        movingFailsafe = 0;
    }
}
