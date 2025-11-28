using System.Collections.Generic;
using UnityEngine;

public class UnitPathfinder : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public List<Vector3> path = new List<Vector3>();
    public float movementSpeed;
    [SerializeField] private Unit unit;

    private void Start()
    {
        TickSystem.Instance.OnFPSTick += Tick;
        ChangePathfinding(0);
    }

    public void NewRoute(List<Vector3> newPath)
    {
        path.Clear();
        path = newPath;
    }
    public void AddToRoute(List<Vector3> newPath)
    {
        for (int i = 0; i < newPath.Count; i++)
        {
            path.Add(newPath[i]);
        }
    }

    private void Tick(object sender, int tick)
    {
        if (path.Count > 0)
        {
            MoveTowardsTarget();
            UpdateLineRender();
            unit.Moving(true);
        }
        else { unit.Moving(false); }
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = path[0];

        Vector3 moveDir = (targetPosition - transform.position).normalized;

        //rotation
        Rotate(targetPosition);

        //move
        transform.position = transform.position + moveDir * (float)movementSpeed * TickSystem.Instance.timeMultiplier;

        if (Vector3.Distance(transform.position, targetPosition) < 1f) { path.Remove(targetPosition); }
    }

    private void Rotate(Vector3 target)
    {
        Vector3 lookPos = target - transform.GetChild(0).position;
        lookPos.y = 0; // Set the y-component of the look vector to zero to constrain rotation to the Y-axis.
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.GetChild(transform.childCount - 1).rotation = Quaternion.Slerp(transform.GetChild(transform.childCount - 1).rotation, rotation, Time.deltaTime * 25 * TickSystem.Instance.timeMultiplier); // Smoothly rotate
    }

    private void UpdateLineRender()
    {
        // Set the number of vertices in the LineRenderer
        lineRenderer.positionCount = path.Count + 1;
        List<Vector3> newPath = new List<Vector3>();
        Vector3 tempPos = transform.position;
        tempPos.y = 1;
        newPath.Add(tempPos);
        for (int i = 0; i < path.Count; i++)
        {
            newPath.Add(path[i]);
        }
        // Set the positions of the LineRenderer to the path points
        lineRenderer.SetPositions(newPath.ToArray());
    }
    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= Tick;
    }
    public void ClearPathfinding() { path.Clear(); lineRenderer.positionCount = 0; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            if (other.CompareTag("HighwayBorder"))
            {
                movementSpeed = unit.GetSpeed() * 2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            if (other.CompareTag("HighwayBorder"))
            {
                movementSpeed = unit.GetSpeed();
            }
        }
    }
    [SerializeField] private Material[] colors;
    public void ChangePathfinding(int type)
    {
        lineRenderer.material = colors[type];
    }
}
