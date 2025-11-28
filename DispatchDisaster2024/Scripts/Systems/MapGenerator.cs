using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private float waitTime = 0.02f;
    [SerializeField] private Transform borders;
    [SerializeField] private Transform highwayBorders;
    public int mapSize;

    [HideInInspector] public List<Node> riverStarterNodes = new List<Node>();
    [HideInInspector] public List<Node> riverNodes = new List<Node>();
    [HideInInspector] public List<Node> starterNodes = new List<Node>();
    [HideInInspector] public List<Node> starterHighwayNodes = new List<Node>();
    [SerializeField] public List<Node> starterHighwayTurnNodes = new List<Node>();
    [HideInInspector] public List<Node> highwayNodes = new List<Node>();

    [HideInInspector] public List<Node> zone1Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone2Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone3Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone4Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone5Nodes = new List<Node>();
    [HideInInspector] public List<Node> fillerNodes = new List<Node>();

    [SerializeField] private int[] zoneSpawns = {0,0,0,0,0,0 };
    [SerializeField] public int[] zoneMinSizes;
    [SerializeField] public int[] bridgeChances;

    [SerializeField] private GameObject[] zone_0_NorthSpawns;
    [SerializeField] private GameObject[] zone_0_SouthSpawns;
    [SerializeField] private GameObject[] zone_0_EastSpawns;
    [SerializeField] private GameObject[] zone_0_WestSpawns;

    [SerializeField] private GameObject[] zone_1_NorthSpawns;
    [SerializeField] private GameObject[] zone_1_SouthSpawns;
    [SerializeField] private GameObject[] zone_1_EastSpawns;
    [SerializeField] private GameObject[] zone_1_WestSpawns;

    [SerializeField] private GameObject[] zone_2_NorthSpawns;
    [SerializeField] private GameObject[] zone_2_SouthSpawns;
    [SerializeField] private GameObject[] zone_2_EastSpawns;
    [SerializeField] private GameObject[] zone_2_WestSpawns;

    [SerializeField] private GameObject[] zone_3_NorthSpawns;
    [SerializeField] private GameObject[] zone_3_SouthSpawns;
    [SerializeField] private GameObject[] zone_3_EastSpawns;
    [SerializeField] private GameObject[] zone_3_WestSpawns;

    [SerializeField] private GameObject[] zone_4_NorthSpawns;
    [SerializeField] private GameObject[] zone_4_SouthSpawns;
    [SerializeField] private GameObject[] zone_4_EastSpawns;
    [SerializeField] private GameObject[] zone_4_WestSpawns;

    [SerializeField] private GameObject[] zone_5_NorthSpawns;
    [SerializeField] private GameObject[] zone_5_SouthSpawns;
    [SerializeField] private GameObject[] zone_5_EastSpawns;
    [SerializeField] private GameObject[] zone_5_WestSpawns;

    [SerializeField] private GameObject[] zone_Highway_NorthSpawns;
    [SerializeField] private GameObject[] zone_Highway_SouthSpawns;
    [SerializeField] private GameObject[] zone_Highway_EastSpawns;
    [SerializeField] private GameObject[] zone_Highway_WestSpawns;
    [SerializeField] private GameObject[] zone_Highway_TurnSpawns;

    [SerializeField] private GameObject[] bridgeSpawns;
    [SerializeField] private GameObject[] fillerSpawns;

    public static MapGenerator instance;
    private void Awake() { instance = this; }
    private void Start() 
    {
        int level = PlayerPrefs.GetInt(KeyHolder.levelKey, 0);
        //Controller.instance.MoneyValueChange(12500);//test

        if (level < 16)
        {
            switch (level)
            {
                case 0: //introduction to game. 1 zone, ambulance, easy, 5x5
                    Controller.instance.MoneyValueChange(1250);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.ticksBetweenEmergencies = 300;
                    mapSize = 5;
                    zoneMinSizes = new int[] { -1, mapSize * 3, 0, 0, 0, 0};
                    break;
                case 1: //added a second zone
                    Controller.instance.MoneyValueChange(1250);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.ticksBetweenEmergencies = 290;
                    mapSize = 6;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, 0, 0, 0 };

                    break;
                case 2: //new agency
                    Controller.instance.MoneyValueChange(800);
                    Controller.instance.agencies[2] = true;
                    Controller.instance.ticksBetweenEmergencies = 285;
                    mapSize = 6;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, 0, 0, 0 };

                    break;
                case 3: //3rd zone
                    Controller.instance.MoneyValueChange(800);
                    Controller.instance.agencies[2] = true;
                    Controller.instance.ticksBetweenEmergencies = 280;
                    mapSize = 7;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, 0, 0 };

                    break;
                case 4: //police
                    Controller.instance.MoneyValueChange(2200);
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 275;
                    mapSize = 8;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, 0, 0 };

                    break;
                case 5: //4th zone
                    Controller.instance.MoneyValueChange(2200);
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 270;
                    mapSize = 9;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, 0 };

                    break;
                case 6: //two agencies
                    Controller.instance.MoneyValueChange(2000);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.agencies[2] = true;
                    Controller.instance.ticksBetweenEmergencies = 265;
                    mapSize = 9;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, 0 };

                    break;
                case 7: //other two
                    Controller.instance.MoneyValueChange(3000);
                    Controller.instance.agencies[2] = true;
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 260;
                    mapSize = 9;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, 0 };

                    break;
                case 8://other two
                    Controller.instance.MoneyValueChange(3500);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 255;
                    mapSize = 9;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, 0 };

                    break;
                case 9://all three
                    Controller.instance.MoneyValueChange(4250);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.agencies[2] = true;
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 250;
                    mapSize = 9;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, 0 };

                    break;

                case 10://all zones
                    Controller.instance.MoneyValueChange(4250);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.agencies[2] = true;
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 250;
                    mapSize = 10;
                    zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, mapSize };

                    break;

                case 11: //downtown Police
                    Controller.instance.MoneyValueChange(2200);
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 250;
                    mapSize = 11;
                    zoneMinSizes = new int[] { -1, 0, mapSize, 0, 0, 0 };
                    break;

                case 12: //industrail fire
                    Controller.instance.MoneyValueChange(800);
                    Controller.instance.agencies[2] = true;
                    Controller.instance.ticksBetweenEmergencies = 250;
                    mapSize = 11;
                    zoneMinSizes = new int[] { -1, mapSize, 0, 0, 0, 0 };
                    break;

                case 13: //rural ambulance
                    Controller.instance.MoneyValueChange(1250);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.ticksBetweenEmergencies = 500;
                    mapSize = 20;
                    zoneMinSizes = new int[] { -1, 0, 0, 0, 0, mapSize };
                    break;

                case 14: //wealthy city
                    Controller.instance.MoneyValueChange(2000);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.agencies[2] = true;
                    Controller.instance.ticksBetweenEmergencies = 250;
                    mapSize = 12;
                    zoneMinSizes = new int[] { -1, mapSize, 0, 0, 0, 0 };
                    break;

                case 15: //poor city
                    Controller.instance.MoneyValueChange(3500);
                    Controller.instance.agencies[1] = true;
                    Controller.instance.agencies[3] = true;
                    Controller.instance.ticksBetweenEmergencies = 200;
                    mapSize = 11;
                    zoneMinSizes = new int[] { -1, mapSize, 0, 0, 0, 0 };
                    break;

            }
        }
        else
        {
            Controller.instance.MoneyValueChange(4250);
            Controller.instance.agencies[1] = true;
            Controller.instance.agencies[2] = true;
            Controller.instance.agencies[3] = true;

            Controller.instance.ticksBetweenEmergencies = 250 - level;
            mapSize = 10 + (int)(level / 10);
            zoneMinSizes = new int[] { -1, mapSize * 3, mapSize, mapSize, mapSize, mapSize };
        }

        Controller.instance.MoneyValueChange((Controller.instance.money * (((float)PlayerPrefs.GetInt(KeyHolder.startingMoneyKey, 0) / KeyHolder.dividedBy1_MaxLevel100) + 1)) - Controller.instance.money);

        StartUp();
        CameraController.instance.SetUpCam(mapSize * 2.5f, mapSize);
    }
    public void StartUp()
    {
        highwayBorders.GetChild(0).position = new Vector3(mapSize * 10, 0, 0);
        highwayBorders.GetChild(1).position = new Vector3(-mapSize * 10, 0, 0);
        highwayBorders.GetChild(2).position = new Vector3(0,0, mapSize * 10);
        highwayBorders.GetChild(3).position = new Vector3(0, 0, -mapSize * 10);

        highwayBorders.GetChild(0).localScale = new Vector3(10,1,mapSize * 20);
        highwayBorders.GetChild(1).localScale = new Vector3(10, 1, mapSize * 20);
        highwayBorders.GetChild(2).localScale = new Vector3(10, 1, mapSize * 20);
        highwayBorders.GetChild(3).localScale = new Vector3(10, 1, mapSize * 20);

        borders.GetChild(0).position = new Vector3((mapSize + 1) * 10, 0, 0);
        borders.GetChild(1).position = new Vector3(-(mapSize + 1) * 10, 0, 0);
        borders.GetChild(2).position = new Vector3(0, 0, (mapSize + 1) * 10);
        borders.GetChild(3).position = new Vector3(0, 0, -(mapSize + 1) * 10);

        borders.GetChild(0).localScale = new Vector3(10, 1, (mapSize + 1) * 20);
        borders.GetChild(1).localScale = new Vector3(10, 1, (mapSize + 1) * 20);
        borders.GetChild(2).localScale = new Vector3(10, 1, (mapSize + 1) * 20);
        borders.GetChild(3).localScale = new Vector3(10, 1, (mapSize + 1) * 20);

        StartCoroutine(Zone0());
    }
    private IEnumerator Zone0()
    {
        yield return new WaitForSeconds(waitTime);

        Node starter_node = riverStarterNodes[Random.Range(0, riverStarterNodes.Count)];
        GameObject starter_toSpawn = GetToSpawn(0, starter_node, 0);
        GameObject starter_spawn = Instantiate(starter_toSpawn, starter_node.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(waitTime);

        while (riverNodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (riverNodes.Count == 0) { break; }
            Node node = riverNodes[Random.Range(0, riverNodes.Count)];
            GameObject toSpawn = GetToSpawn(0, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
        StartCoroutine(Zone1());
    }
    private IEnumerator Zone1()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);

        if (zoneMinSizes[1] != 0)
        {
            if (starterNodes.Count == 0) { FailedGeneration(); yield break; }
            Node starter_node = starterNodes[Random.Range(0, starterNodes.Count)];
            GameObject starter_toSpawn = GetToSpawn(1, starter_node, 0);
            GameObject starter_spawn = Instantiate(starter_toSpawn, starter_node.transform.position, Quaternion.identity);
        }


        yield return new WaitForSeconds(waitTime);

        while (zoneSpawns[1] < zoneMinSizes[1] && zone1Nodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (zone1Nodes.Count == 0) { break; }
            Node node = zone1Nodes[Random.Range(0, zone1Nodes.Count)];
            GameObject toSpawn = GetToSpawn(1, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
            zoneSpawns[1]++;
        }

        //failsafe 1
        if (zoneSpawns[1] < zoneMinSizes[1] && zone1Nodes.Count == 0 && starterNodes.Count > 0)
        {
            StartCoroutine(Zone1()); 
            yield break;
        }

        //failsafe 2
        if (zoneSpawns[1] < zoneMinSizes[1] && zone1Nodes.Count == 0 && starterNodes.Count == 0)
        {
            FailedGeneration();
            yield break;
        }

        StartCoroutine(Zone2());
    }
    private IEnumerator Zone2()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);

        if (zoneMinSizes[2] != 0)
        {
            Node starter_node = null;
            if (zone1Nodes.Count == 0) 
            {
                if (starterNodes.Count == 0) { FailedGeneration(); yield break; }
                starter_node = starterNodes[Random.Range(0, starterNodes.Count)];
            }
            else { starter_node = zone1Nodes[Random.Range(0, zone1Nodes.Count)]; }
            
            GameObject starter_toSpawn = GetToSpawn(2, starter_node, 0);
            GameObject starter_spawn = Instantiate(starter_toSpawn, starter_node.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(waitTime);

        while (zoneSpawns[2] < zoneMinSizes[2] && zone2Nodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (zone2Nodes.Count == 0) { break; }
            Node node = zone2Nodes[Random.Range(0, zone2Nodes.Count)];
            GameObject toSpawn = GetToSpawn(2, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
            zoneSpawns[2]++;
        }

        //failsafe 1
        if (zoneSpawns[2] < zoneMinSizes[2] && zone2Nodes.Count == 0 && zone1Nodes.Count > 0)
        {
            StartCoroutine(Zone2());
            yield break;
        }

        //failsafe 2
        if (zoneSpawns[2] < zoneMinSizes[2] && zone2Nodes.Count == 0 && zone1Nodes.Count == 0)
        {
            FailedGeneration();
            yield break;
        }
        StartCoroutine(Zone3());
    }
    private IEnumerator Zone3()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);

        if (zoneMinSizes[3] != 0)
        {
            Node starter_node = null;
            if (zone1Nodes.Count == 0)
            {
                if (starterNodes.Count == 0) { FailedGeneration(); yield break; }
                starter_node = starterNodes[Random.Range(0, starterNodes.Count)];
            }
            else { starter_node = zone1Nodes[Random.Range(0, zone1Nodes.Count)]; }

            GameObject starter_toSpawn = GetToSpawn(3, starter_node, 0);
            GameObject starter_spawn = Instantiate(starter_toSpawn, starter_node.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(waitTime);

        while (zoneSpawns[3] < zoneMinSizes[3] && zone3Nodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (zone3Nodes.Count == 0) { break; }
            Node node = zone3Nodes[Random.Range(0, zone3Nodes.Count)];
            GameObject toSpawn = GetToSpawn(3, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
            zoneSpawns[3]++;
        }

        //failsafe 1
        if (zoneSpawns[3] < zoneMinSizes[3] && zone3Nodes.Count == 0 && zone1Nodes.Count > 0)
        {
            StartCoroutine(Zone3());
            yield break;
        }

        //failsafe 2
        if (zoneSpawns[3] < zoneMinSizes[3] && zone3Nodes.Count == 0 && zone1Nodes.Count == 0)
        {
            FailedGeneration();
            yield break;
        }
        StartCoroutine(Zone4());
    }
    private IEnumerator Zone4()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);

        if (zoneMinSizes[4] != 0)
        {
            Node starter_node = null;
            if (zone1Nodes.Count == 0)
            {
                if (starterNodes.Count == 0) { FailedGeneration(); yield break; }
                starter_node = starterNodes[Random.Range(0, starterNodes.Count)];
            }
            else { starter_node = zone1Nodes[Random.Range(0, zone1Nodes.Count)]; }

            GameObject starter_toSpawn = GetToSpawn(4, starter_node, 0);
            GameObject starter_spawn = Instantiate(starter_toSpawn, starter_node.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(waitTime);

        while (zoneSpawns[4] < zoneMinSizes[4] && zone4Nodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (zone4Nodes.Count == 0) { break; }
            Node node = zone4Nodes[Random.Range(0, zone4Nodes.Count)];
            GameObject toSpawn = GetToSpawn(4, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
            zoneSpawns[4]++;
        }

        //failsafe 1
        if (zoneSpawns[4] < zoneMinSizes[4] && zone4Nodes.Count == 0 && zone1Nodes.Count > 0)
        {
            StartCoroutine(Zone4());
            yield break;
        }

        //failsafe 2
        if (zoneSpawns[4] < zoneMinSizes[4] && zone4Nodes.Count == 0 && zone1Nodes.Count == 0)
        {
            FailedGeneration();
            yield break;
        }
        StartCoroutine(Zone5());
    }
    private IEnumerator Zone5()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);

        if (zoneMinSizes[5] != 0)
        {
            Node starter_node = null;
            if (zone1Nodes.Count == 0)
            {
                if (starterNodes.Count == 0) { FailedGeneration(); yield break; }
                starter_node = starterNodes[Random.Range(0, starterNodes.Count)];
            }
            else { starter_node = zone1Nodes[Random.Range(0, zone1Nodes.Count)]; }

            GameObject starter_toSpawn = GetToSpawn(5, starter_node, 0);
            GameObject starter_spawn = Instantiate(starter_toSpawn, starter_node.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(waitTime);

        while (zoneSpawns[5] < zoneMinSizes[5] && zone5Nodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (zone5Nodes.Count == 0) { break; }
            Node node = zone5Nodes[Random.Range(0, zone5Nodes.Count)];
            GameObject toSpawn = GetToSpawn(5, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
            zoneSpawns[5]++;
        }

        //failsafe 1
        if (zoneSpawns[5] < zoneMinSizes[5] && zone5Nodes.Count == 0 && zone1Nodes.Count > 0)
        {
            StartCoroutine(Zone5());
            yield break;
        }

        //failsafe 2
        if (zoneSpawns[5] < zoneMinSizes[5] && zone5Nodes.Count == 0 && zone1Nodes.Count == 0)
        {
            FailedGeneration();
            yield break;
        }
        StartCoroutine(All());
    }
    private IEnumerator All()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);
        while (zone1Nodes.Count > 0 || zone2Nodes.Count > 0 || zone3Nodes.Count > 0 || zone4Nodes.Count > 0 || zone5Nodes.Count > 0)
        {
            if (zone1Nodes.Count > 0)
            {
                yield return new WaitForSeconds(waitTime);

                if (zone1Nodes.Count == 0) { break; }
                Node node = zone1Nodes[Random.Range(0, zone1Nodes.Count)];
                GameObject toSpawn = GetToSpawn(1, node, 0);
                GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(waitTime);
            }

            if (zone2Nodes.Count > 0)
            {
                yield return new WaitForSeconds(waitTime);

                if (zone2Nodes.Count == 0) { break; }
                Node node = zone2Nodes[Random.Range(0, zone2Nodes.Count)];
                GameObject toSpawn = GetToSpawn(2, node, 0);
                GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(waitTime);
            }

            if (zone3Nodes.Count > 0)
            {
                yield return new WaitForSeconds(waitTime);

                if (zone3Nodes.Count == 0) { break; }
                Node node = zone3Nodes[Random.Range(0, zone3Nodes.Count)];
                GameObject toSpawn = GetToSpawn(3, node, 0);
                GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(waitTime);
            }

            if (zone4Nodes.Count > 0)
            {
                yield return new WaitForSeconds(waitTime);

                if (zone4Nodes.Count == 0) { break; }
                Node node = zone4Nodes[Random.Range(0, zone4Nodes.Count)];
                GameObject toSpawn = GetToSpawn(4, node, 0);
                GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(waitTime);
            }

            if (zone5Nodes.Count > 0)
            {
                yield return new WaitForSeconds(waitTime);

                if (zone5Nodes.Count == 0) { break; }
                Node node = zone5Nodes[Random.Range(0, zone5Nodes.Count)];
                GameObject toSpawn = GetToSpawn(5, node, 0);
                GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(waitTime);
            }
        }

        StartCoroutine(Filler());
    }
    private IEnumerator Filler()
    {
        UIController.instance.UpdateStartSlider();
        yield return new WaitForSeconds(waitTime);
        while (fillerNodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (fillerNodes.Count == 0) { break; }
            Node node = fillerNodes[Random.Range(0, fillerNodes.Count)];
            GameObject toSpawn = fillerSpawns[node.zone];
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
        StartCoroutine(Highways());
    }
    private IEnumerator Highways()
    {
        UIController.instance.UpdateStartSlider();

        highwayBorders.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
        highwayBorders.GetChild(1).GetComponent<BoxCollider>().isTrigger = true;
        highwayBorders.GetChild(2).GetComponent<BoxCollider>().isTrigger = true;
        highwayBorders.GetChild(3).GetComponent<BoxCollider>().isTrigger = true;

        yield return new WaitForSeconds(waitTime);

        starterHighwayTurnNodes[0].transform.position = new Vector3(mapSize * 10, 0, mapSize * 10);
        starterHighwayTurnNodes[1].transform.position = new Vector3(-mapSize * 10, 0, mapSize * 10);
        starterHighwayTurnNodes[2].transform.position = new Vector3(mapSize * 10, 0, -mapSize * 10);
        starterHighwayTurnNodes[3].transform.position = new Vector3(-mapSize * 10, 0, -mapSize * 10);

        yield return new WaitForSeconds(waitTime);

        Node highwayTurn = starterHighwayTurnNodes[3];
        GameObject highwayTurnToSpawn = zone_Highway_TurnSpawns[3]; ;
        GameObject hiughwayTurn = Instantiate(highwayTurnToSpawn, highwayTurn.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);

        highwayTurn = starterHighwayTurnNodes[2];
        highwayTurnToSpawn = zone_Highway_TurnSpawns[2]; ;
        hiughwayTurn = Instantiate(highwayTurnToSpawn, highwayTurn.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);

        highwayTurn = starterHighwayTurnNodes[1];
        highwayTurnToSpawn = zone_Highway_TurnSpawns[1]; ;
        hiughwayTurn = Instantiate(highwayTurnToSpawn, highwayTurn.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);

        highwayTurn = starterHighwayTurnNodes[0];
        highwayTurnToSpawn = zone_Highway_TurnSpawns[0]; ;
        hiughwayTurn = Instantiate(highwayTurnToSpawn, highwayTurn.transform.position, Quaternion.identity);


        yield return new WaitForSeconds(waitTime);

        while (starterHighwayNodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (starterHighwayNodes.Count == 0) { break; }
            Node node = starterHighwayNodes[Random.Range(0, starterHighwayNodes.Count)];
            GameObject toSpawn = GetToSpawn(-2, node, 0);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(waitTime);

        while (highwayNodes.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);

            if (highwayNodes.Count == 0) { break; }
            Node node = highwayNodes[Random.Range(0, highwayNodes.Count)];
            GameObject toSpawn = GetToSpawn(-2, node, 1);
            GameObject spawn = Instantiate(toSpawn, node.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }

        Finish();
    }

    private void Finish()
    {
        UIController.instance.UpdateStartSlider();
        DeleteAllNodes();
        Debug.Log("Done");
        //FailedGeneration();
        GridManager.instance.StartUp();

        highwayBorders.GetChild(0).localScale = new Vector3(10, 1, mapSize * 18.5f);
        highwayBorders.GetChild(1).localScale = new Vector3(10, 1, mapSize * 18.5f);
        highwayBorders.GetChild(2).localScale = new Vector3(10, 1, mapSize * 18.5f);
        highwayBorders.GetChild(3).localScale = new Vector3(10, 1, mapSize * 18.5f);

    }
    private GameObject GetToSpawn(int zone, Node node, int set)
    {
        switch(zone)
        {
            default: Debug.LogError(""); return null;
            case -2:
                if (node.zone == zone)
                {
                    //0 == initial
                    //1 == straight
                    //2 == turn
                    switch (node.dir)
                    {
                        default: Debug.LogError(""); return null;
                        case Node.Direction.north: return zone_Highway_NorthSpawns[set];
                        case Node.Direction.south: return zone_Highway_SouthSpawns[set];
                        case Node.Direction.east: return zone_Highway_EastSpawns[set];
                        case Node.Direction.west: return zone_Highway_WestSpawns[set];

                        case Node.Direction.bridge_north: return bridgeSpawns[10];
                        case Node.Direction.bridge_west: return bridgeSpawns[11];
                    }
                }
                else
                {
                    switch (node.dir)
                    {
                        default: return zone_5_NorthSpawns[0];

                        case Node.Direction.bridge_north: return bridgeSpawns[8];
                        case Node.Direction.bridge_west: return bridgeSpawns[9];
                    }
                }

            case 0:
                switch (node.dir)
                {
                    default: Debug.LogError(""); return null;
                    case Node.Direction.river_north: return zone_0_NorthSpawns[Random.Range(0, zone_0_NorthSpawns.Length)];
                    case Node.Direction.river_south: return zone_0_SouthSpawns[Random.Range(0, zone_0_SouthSpawns.Length)];
                    case Node.Direction.river_east: return zone_0_EastSpawns[Random.Range(0, zone_0_EastSpawns.Length)];
                    case Node.Direction.river_west: return zone_0_WestSpawns[Random.Range(0, zone_0_WestSpawns.Length)];
                }
            case 1:
                if (node.zone == zone)
                {
                    switch (node.dir)
                    {
                        default: return zone_1_NorthSpawns[0];
                        case Node.Direction.north: return zone_1_NorthSpawns[Random.Range(0, zone_1_NorthSpawns.Length)];
                        case Node.Direction.south: return zone_1_SouthSpawns[Random.Range(0, zone_1_SouthSpawns.Length)];
                        case Node.Direction.east: return zone_1_EastSpawns[Random.Range(0, zone_1_EastSpawns.Length)];
                        case Node.Direction.west: return zone_1_WestSpawns[Random.Range(0, zone_1_WestSpawns.Length)];

                        case Node.Direction.bridge_north: return bridgeSpawns[0];
                        case Node.Direction.bridge_west: return bridgeSpawns[1];
                    }
                }
                else
                {
                    switch (node.dir)
                    {
                        default: return zone_1_NorthSpawns[0];

                        case Node.Direction.bridge_north: return bridgeSpawns[0];
                        case Node.Direction.bridge_west: return bridgeSpawns[1];
                    }
                }

            case 2:
                if (node.zone == zone)
                {
                    switch (node.dir)
                    {
                        default: Debug.LogError(""); return null;
                        case Node.Direction.north: return zone_2_NorthSpawns[Random.Range(0, zone_2_NorthSpawns.Length)];
                        case Node.Direction.south: return zone_2_SouthSpawns[Random.Range(0, zone_2_SouthSpawns.Length)];
                        case Node.Direction.east: return zone_2_EastSpawns[Random.Range(0, zone_2_EastSpawns.Length)];
                        case Node.Direction.west: return zone_2_WestSpawns[Random.Range(0, zone_2_WestSpawns.Length)];

                        case Node.Direction.bridge_north: return bridgeSpawns[2];
                        case Node.Direction.bridge_west: return bridgeSpawns[3];
                    }
                }
                else
                {
                    switch (node.dir)
                    {
                        default: return zone_2_NorthSpawns[0];

                        case Node.Direction.bridge_north: return bridgeSpawns[2];
                        case Node.Direction.bridge_west: return bridgeSpawns[3];
                    }
                }

            case 3:
                if (node.zone == zone)
                {
                    switch (node.dir)
                    {
                        default: Debug.LogError(""); return null;
                        case Node.Direction.north: return zone_3_NorthSpawns[Random.Range(0, zone_3_NorthSpawns.Length)];
                        case Node.Direction.south: return zone_3_SouthSpawns[Random.Range(0, zone_3_SouthSpawns.Length)];
                        case Node.Direction.east: return zone_3_EastSpawns[Random.Range(0, zone_3_EastSpawns.Length)];
                        case Node.Direction.west: return zone_3_WestSpawns[Random.Range(0, zone_3_WestSpawns.Length)];

                        case Node.Direction.bridge_north: return bridgeSpawns[4];
                        case Node.Direction.bridge_west: return bridgeSpawns[5];
                    }
                }
                else
                {
                    switch (node.dir)
                    {
                        default: return zone_3_NorthSpawns[0];

                        case Node.Direction.bridge_north: return bridgeSpawns[4];
                        case Node.Direction.bridge_west: return bridgeSpawns[5];
                    }
                }

            case 4:
                if (node.zone == zone)
                {
                    switch (node.dir)
                    {
                        default: Debug.LogError(""); return null;
                        case Node.Direction.north: return zone_4_NorthSpawns[Random.Range(0, zone_4_NorthSpawns.Length)];
                        case Node.Direction.south: return zone_4_SouthSpawns[Random.Range(0, zone_4_SouthSpawns.Length)];
                        case Node.Direction.east: return zone_4_EastSpawns[Random.Range(0, zone_4_EastSpawns.Length)];
                        case Node.Direction.west: return zone_4_WestSpawns[Random.Range(0, zone_4_WestSpawns.Length)];

                        case Node.Direction.bridge_north: return bridgeSpawns[6];
                        case Node.Direction.bridge_west: return bridgeSpawns[7];
                    }
                }
                else
                {
                    switch (node.dir)
                    {
                        default: return zone_4_NorthSpawns[0];
  

                        case Node.Direction.bridge_north: return bridgeSpawns[6];
                        case Node.Direction.bridge_west: return bridgeSpawns[7];
                    }
                }

            case 5:
                if (node.zone == zone)
                {
                    switch (node.dir)
                    {
                        default: Debug.LogError(""); return null;
                        case Node.Direction.north: return zone_5_NorthSpawns[Random.Range(0, zone_5_NorthSpawns.Length)];
                        case Node.Direction.south: return zone_5_SouthSpawns[Random.Range(0, zone_5_SouthSpawns.Length)];
                        case Node.Direction.east: return zone_5_EastSpawns[Random.Range(0, zone_5_EastSpawns.Length)];
                        case Node.Direction.west: return zone_5_WestSpawns[Random.Range(0, zone_5_WestSpawns.Length)];

                        case Node.Direction.bridge_north: return bridgeSpawns[8];
                        case Node.Direction.bridge_west: return bridgeSpawns[9];
                    }
                }
                else
                {
                    switch (node.dir)
                    {
                        default: return zone_5_NorthSpawns[0];

                        case Node.Direction.bridge_north: return bridgeSpawns[8];
                        case Node.Direction.bridge_west: return bridgeSpawns[9];
                    }
                }


        }
    }
    /*
         [HideInInspector] public List<Node> riverStarterNodes = new List<Node>();
    [HideInInspector] public List<Node> riverNodes = new List<Node>();
    [HideInInspector] public List<Node> starterNodes = new List<Node>();

    [HideInInspector] public List<Node> zone1Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone2Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone3Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone4Nodes = new List<Node>();
    [HideInInspector] public List<Node> zone5Nodes = new List<Node>();
    [HideInInspector] public List<Node> fillerNodes = new List<Node>();
     */
    private void DeleteAllNodes()
    {
        List<Node> nodes = new List<Node>();

        nodes = riverStarterNodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = riverNodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = starterNodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = zone1Nodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = zone2Nodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = zone3Nodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = zone4Nodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = zone5Nodes;
        if (nodes.Count > 0) { DeletingList(nodes); }

        nodes = fillerNodes;
        if (nodes.Count > 0) { DeletingList(nodes); }
    }
    private void DeletingList(List<Node> nodes) { for (int i = nodes.Count - 1; i >= 0; i--) { if (nodes[i] != null) { Destroy(nodes[i].gameObject); } } }

    private void FailedGeneration()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
