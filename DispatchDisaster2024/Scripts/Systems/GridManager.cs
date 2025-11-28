using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    private void Awake() { instance = this; }
    public List<PathNode> nodes = new List<PathNode>();

    public void StartUp()
    {
        StartCoroutine(SetupNeighbors());
    }
    private IEnumerator SetupNeighbors()
    {
        foreach (PathNode findernode in nodes)
        {
            yield return new WaitForSeconds(0.01f);
            foreach (Vector3 vector in findernode.neighborsPos)
            {
                Vector3 newVector = vector + findernode.position;
                foreach (PathNode node in nodes)
                {
                    if (node.transform.position == newVector) { findernode.AddNeighbor(node); break; }

                }
            }

        }
        UIController.instance.UpdateStartSlider();
        StartCoroutine(DeleteOtherNeighbors());
    }
    private IEnumerator DeleteOtherNeighbors()
    {

        foreach (PathNode findernode in nodes)
        {
            yield return new WaitForSeconds(0.01f);
            List<PathNode> newList = new List<PathNode>();
            foreach (PathNode neighbornode in findernode.neighbors)
            {
                if (neighbornode.neighbors.Contains(findernode)) { newList.Add(neighbornode); }
            }
            
            findernode.neighbors = newList;
        }

        UIController.instance.UpdateStartSlider();
        Debug.Log("Finished Navemesh");
        Controller.instance.FinishedGenerating();
    }

    [SerializeField] private PathNode startTest;
    [SerializeField] private PathNode endTest;
    [SerializeField] private List<PathNode> path;

    [SerializeField] private UnitPathfinder unitPathfinder;
    [SerializeField] private Vector3 endpoint;
    public void GetPathfinding(UnitPathfinder unit, Vector3 endPoint)
    {
        unitPathfinder = unit;
        this.endpoint = endPoint;

        startTest = FindClosestPoint(unit.transform.position);
        endTest = FindClosestPoint(endPoint);

        StartPathfinding();
    }
    public void AddToPathfinding(UnitPathfinder unit, Vector3 endPoint)
    {
        unitPathfinder = unit;
        this.endpoint = endPoint;

        if (unit.path.Count > 0) { startTest = FindClosestPoint(unit.path[unit.path.Count - 1]); }
        else { startTest = FindClosestPoint(unit.transform.position); }
        
        endTest = FindClosestPoint(endPoint);

        ContinuePathfinding();
    }
    private void StartPathfinding()
    {
        path = PathFinder.FindPath(startTest, endTest);

        List<Vector3> unitPath = new List<Vector3>();
        unitPath.Add(unitPathfinder.transform.position);
        Vector3 tempPos2 = startTest.transform.position;
        tempPos2.y = 1;
        unitPath.Add(tempPos2);
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 tempPos = path[i].transform.position;
            tempPos.y = 1;
            unitPath.Add(tempPos);
        }
        //disabled for now becuase it alows offroading.
        //Will need to be re-added if more nodes are placed that are not in the middle of the block
        //unitPath.Add(endpoint); 

        unitPathfinder.NewRoute(unitPath);
    }
    private void ContinuePathfinding()
    {
        path = PathFinder.FindPath(startTest, endTest);

        List<Vector3> unitPath = new List<Vector3>();
        Vector3 tempPos2 = startTest.transform.position;
        tempPos2.y = 1;
        unitPath.Add(tempPos2);
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 tempPos = path[i].transform.position;
            tempPos.y = 1;
            unitPath.Add(tempPos);
        }

        unitPathfinder.AddToRoute(unitPath);
    }
    private PathNode FindClosestPoint(Vector3 pos)
    {
        PathNode closestPoint = nodes[0];
        float closestDistanceSqr = (pos - closestPoint.transform.position).sqrMagnitude; // Using sqrMagnitude for performance

        foreach (PathNode node in nodes)
        {
            float distanceSqr = (pos - node.transform.position).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPoint = node;
            }
        }
        return closestPoint;
    }

}
