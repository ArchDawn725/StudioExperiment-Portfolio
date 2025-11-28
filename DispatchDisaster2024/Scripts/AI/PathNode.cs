using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public Vector3 position;
    public List<PathNode> neighbors = new List<PathNode>();
    public Vector3[] neighborsPos;

    public float gCost; // Cost from start to this node
    public float hCost; // Heuristic cost to target
    [SerializeField] private float tileCost = 10;
    public float fCost => gCost + hCost + tileCost; // Total cost
    public PathNode parent; // For retracing the path

    private void Start()
    {
        position = transform.position;
        neighborsPos = GetComponent<CityBlock>().nodeSpawns;
        GridManager.instance.nodes.Add(this);
    }

    public void AddNeighbor(PathNode neighbor)
    {
        neighbors.Add(neighbor);
    }
}
