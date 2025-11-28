using UnityEngine;

public class CityBlock : MonoBehaviour
{
    public int zone;
    [SerializeField] private GameObject node;
    public Vector3[] nodeSpawns;
    [SerializeField] private Vector3[] fillerNodeSpawns;
    private Renderer[] renderers;
    public EmergencyNode emergencyNode;
    private void Start()
    {
        foreach (Vector3 pos in nodeSpawns)
        {
            Vector3 spawnPosition = transform.position + pos;
            GameObject newNode = Instantiate(node, spawnPosition, Quaternion.identity);
            Node nodeS = newNode.GetComponent<Node>();
            nodeS.zone = zone;
            nodeS.dir = getDir(pos);
            nodeS.StartUp();
        }

        foreach (Vector3 pos in fillerNodeSpawns)
        {
            Vector3 spawnPosition = transform.position + pos;
            GameObject newNode = Instantiate(node, spawnPosition, Quaternion.identity);
            Node nodeS = newNode.GetComponent<Node>();
            nodeS.zone = zone;
            nodeS.dir = Node.Direction.filler;
            nodeS.StartUp();
        }
        //MapGenerator.instance.Next();

        renderers = transform.GetChild(0).GetComponentsInChildren<Renderer>();
        TickSystem.Instance.OnFPSTick += CheckVisablility;
        if (transform.childCount > 3)
        {
            if (transform.GetChild(3).GetChild(0).GetComponent<EmergencyNode>() != null) { emergencyNode = transform.GetChild(3).GetChild(0).GetComponent<EmergencyNode>(); }
        }

    }

    private Node.Direction getDir(Vector3 pos)
    {
        if (zone != 0)
        {
            string positionKey = $"{pos.x}_{pos.y}_{pos.z}";
            switch (positionKey)
            {
                default: return Node.Direction.north;
                case "-10_0_0": return Node.Direction.west;
                case "10_0_0": return Node.Direction.east;
                case "0_0_10": return Node.Direction.north;
                case "0_0_-10": return Node.Direction.south;
            }
        }
        else
        {
            string positionKey = $"{pos.x}_{pos.y}_{pos.z}";
            switch (positionKey)
            {
                default: return Node.Direction.north;
                case "-10_0_0": return Node.Direction.river_west;
                case "10_0_0": return Node.Direction.river_east;
                case "0_0_10": return Node.Direction.river_north;
                case "0_0_-10": return Node.Direction.river_south;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.transform.parent != transform && !other.CompareTag("HighwayBorder"))
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= CheckVisablility;
    }
    private bool visable = true;
    private void CheckVisablility(object sender, int tick)
    {
        if (visable)
        {
            if (tick % 2 == 0)
            {
                if (!IsInView(Camera.main))
                {
                    foreach (Renderer rend in renderers) { rend.enabled = false; } visable = false;
                }
            }
        }
        else
        {
            if (tick % 1 == 0)
            {
                if (IsInView(Camera.main))
                {
                    foreach (Renderer rend in renderers) { rend.enabled = true; }
                    visable = true;
                }
            }
        }
    }
    bool IsInView(Camera camera)
    {
        Vector3 pointOnScreen = camera.WorldToViewportPoint(transform.position);

        // Check if the point is within the camera's view
        return pointOnScreen.z > -0.1 && pointOnScreen.x > -0.1 && pointOnScreen.x < 1.1 && pointOnScreen.y > -0.1 && pointOnScreen.y < 1.1;
    }
}
