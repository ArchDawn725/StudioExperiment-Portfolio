using UnityEngine;

public class Node : MonoBehaviour
{
    public int zone;
    public Direction dir;
    private bool finished;
    private float timer;
    public enum Direction
    {
        starter,
        north,
        south,
        west,
        east,
        bridge_north,
        bridge_west,
        river_north,
        river_south,
        river_west,
        river_east,
        filler
    }

    private void Start()
    {
        if (zone == -1)
        {
            switch (dir)
            {
                case Direction.starter: MapGenerator.instance.starterNodes.Add(this); break;

                case Direction.river_north:
                case Direction.river_west: MapGenerator.instance.riverStarterNodes.Add(this); break;
            }
        }
    }

    public void StartUp()
    {
        if (dir != Direction.filler)
        {
            switch (zone)
            {
                case -2: MapGenerator.instance.highwayNodes.Add(this); break;
                case 0: MapGenerator.instance.riverNodes.Add(this); break;
                case 1: MapGenerator.instance.zone1Nodes.Add(this); break;
                case 2: MapGenerator.instance.zone2Nodes.Add(this); break;
                case 3: MapGenerator.instance.zone3Nodes.Add(this); break;
                case 4: MapGenerator.instance.zone4Nodes.Add(this); break;
                case 5: MapGenerator.instance.zone5Nodes.Add(this); break;
            }
        }
        else
        {
            MapGenerator.instance.fillerNodes.Add(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.CompareTag("River") && zone != 0 && zone != -1 && dir != Direction.filler)
            {
                int thisZone = zone;
                if (thisZone < 0) { thisZone = 0; }
                if (Random.Range(0, 100) > MapGenerator.instance.bridgeChances[thisZone]) { Destroy(gameObject); return; }
                switch (dir)
                {
                    case Direction.south:
                    case Direction.north: dir = Direction.bridge_north; break;

                    case Direction.east:
                    case Direction.west: dir = Direction.bridge_west; break;

                    case Direction.starter: Destroy(gameObject); return;
                }
            }
            else if (other.CompareTag("HighwayBorder") && zone == 0)
            {
                return;
            }
            else if (other.CompareTag("HighwayBorder") && zone != 0 && dir != Direction.filler)
            {
                if (zone != -2)
                {
                    OnDestroy();
                    zone = -2;
                    Vector3 spawnPosition = transform.position;
                    GameObject newNode = Instantiate(gameObject, spawnPosition, Quaternion.identity);
                    Node nodeS = newNode.GetComponent<Node>();
                    nodeS.zone = zone;
                    nodeS.dir = dir;
                    nodeS.StartUp();
                    MapGenerator.instance.starterHighwayNodes.Add(nodeS);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        if (dir != Direction.filler)
        {
            switch (zone)
            {
                case -2: MapGenerator.instance.highwayNodes.Remove(this); MapGenerator.instance.starterHighwayNodes.Remove(this); break;
                case -1:
                    switch (dir)
                    {
                        case Direction.starter: MapGenerator.instance.starterNodes.Remove(this); break;

                        case Direction.river_north:
                        case Direction.river_west: MapGenerator.instance.riverStarterNodes.Remove(this); break;
                    }
                    break;
                case 0: MapGenerator.instance.riverNodes.Remove(this); break;
                case 1: MapGenerator.instance.zone1Nodes.Remove(this); break;
                case 2: MapGenerator.instance.zone2Nodes.Remove(this); break;
                case 3: MapGenerator.instance.zone3Nodes.Remove(this); break;
                case 4: MapGenerator.instance.zone4Nodes.Remove(this); break;
                case 5: MapGenerator.instance.zone5Nodes.Remove(this); break;
            }
        }
        else
        {
            MapGenerator.instance.fillerNodes.Remove(this);
        }
    }
}
