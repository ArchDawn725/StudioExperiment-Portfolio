using SimpleWaypointIndicators;
using TMPro;
using UnityEngine;

public class Emergency : MonoBehaviour
{
    //private int type;
    //private int priority;
    //private int zone;

    [SerializeField] private int timer;
    private int unitTimer;
    public TextMeshPro textMeshPro;
    private SpriteRenderer spriteRenderer;
    [SerializeField]private SpriteRenderer barOutline;
    [SerializeField] private TextMeshPro _capacityText;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool paused;
    public EmergancyInfo info;
    private int maxTime;
    private EmergencyNode node;
    private EnemyPoint marker;
    [SerializeField] private Color[] colors;
    private int type;
    private int patients = 1;
    public int remainingPatients { private set; get; }
    [SerializeField] private int unitsOnScene;
    private Transform rotator;
    public void StartUp(int type, int priority, int zone, EmergencyNode node, int newpatients)
    {
        rotator = transform.GetChild(1);
        this.type = type;
        marker = GetComponent<EnemyPoint>();
        this.node = node;
        spriteRenderer = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        patients = newpatients;

        //this.type = type;
        //this.priority = priority;
        //this.zone = zone;

        int timeModifier = 0;//placeholder
        SpriteRenderer spriteRenderer2 = transform.GetChild(0).GetComponent<SpriteRenderer>();

        switch (type)
        {
            //EMS
            case 1:
                if (patients == -1)
                {
                    patients = 1;
                    if (Random.Range(0, 100) > 90) { patients -= 1; }
                    if (Random.Range(0, 100) > 75) { patients += 1; }
                }

                switch (priority)
                {
                    default: unitTimer = 600; timer = -1; spriteRenderer.sprite = sprites[10]; textMeshPro.color = colors[0]; barOutline.color = colors[0]; marker.arrowColor = colors[0]; spriteRenderer2.color = colors[0]; break;
                    case 1: unitTimer = 150; timer = 250 + timeModifier; spriteRenderer.sprite = sprites[0]; textMeshPro.color = colors[10]; barOutline.color = colors[1]; spriteRenderer2.color = colors[1]; marker.arrowColor = colors[1]; break;
                    case 2: unitTimer = 300; timer = 500 + timeModifier; spriteRenderer.sprite = sprites[1]; textMeshPro.color = colors[2]; barOutline.color = colors[2]; spriteRenderer2.color = colors[2]; marker.arrowColor = colors[2]; break;
                    case 3: unitTimer = 450; timer = 1000 + timeModifier; spriteRenderer.sprite = sprites[2]; textMeshPro.color = colors[4]; barOutline.color = colors[3]; spriteRenderer2.color = colors[3]; marker.arrowColor = colors[3]; break;
                }
                break;
                //Fire
            case 2:
                if (patients == -1)
                {
                    patients = 1;
                    if (Random.Range(0, 100) > 25) { patients -= 1; }
                    if (Random.Range(0, 100) > 75) { patients += 1; }
                }

                switch (priority)
                {
                    default: unitTimer = 600; timer = -1; spriteRenderer.sprite = sprites[11]; textMeshPro.color = colors[0]; barOutline.color = colors[0]; spriteRenderer2.color = colors[0]; marker.arrowColor = colors[0]; break;
                    case 1: unitTimer = 150; timer = 450 + timeModifier; spriteRenderer.sprite = sprites[3]; textMeshPro.color = colors[4]; barOutline.color = colors[4]; spriteRenderer2.color = colors[4]; marker.arrowColor = colors[4]; break;
                    case 2: unitTimer = 250; timer = 900 + timeModifier; spriteRenderer.sprite = sprites[4]; textMeshPro.color = colors[10]; barOutline.color = colors[10]; spriteRenderer2.color = colors[10]; marker.arrowColor = colors[10]; break;
                }
                break;
                //Police
            case 3:
                if (patients == -1)
                {
                    patients = 1;
                    if (Random.Range(0, 100) > 50) { patients -= 1; }
                    if (Random.Range(0, 100) > 75) { patients += 1; }
                }

                switch (priority)
                {
                    default: unitTimer = 600; timer = -1; spriteRenderer.sprite = sprites[12]; textMeshPro.color = colors[0]; barOutline.color = colors[0]; spriteRenderer2.color = colors[0]; marker.arrowColor = colors[0]; break;
                    case 1: unitTimer = 100; timer = 200 + timeModifier; spriteRenderer.sprite = sprites[5]; textMeshPro.color = colors[10]; barOutline.color = colors[1]; spriteRenderer2.color = colors[1]; marker.arrowColor = colors[1]; break;
                    case 2: unitTimer = 150; timer = 400 + timeModifier; spriteRenderer.sprite = sprites[6]; textMeshPro.color = colors[5]; barOutline.color = colors[5]; spriteRenderer2.color = colors[5]; marker.arrowColor = colors[5]; break;
                    case 3: unitTimer = 200; timer = 600 + timeModifier; spriteRenderer.sprite = sprites[7]; textMeshPro.color = colors[2]; barOutline.color = colors[2]; spriteRenderer2.color = colors[2]; marker.arrowColor = colors[2]; break;
                    case 4: unitTimer = 250; timer = 800 + timeModifier; spriteRenderer.sprite = sprites[8]; textMeshPro.color = colors[6]; barOutline.color = colors[6]; spriteRenderer2.color = colors[6]; marker.arrowColor = colors[6]; break;
                    case 5: unitTimer = 300; timer = 1000 + timeModifier; spriteRenderer.sprite = sprites[9]; textMeshPro.color = colors[4]; barOutline.color = colors[3]; spriteRenderer2.color = colors[3]; marker.arrowColor = colors[3]; break;
                }
                break;
            //Everything
            case -1:
                switch (priority)
                {
                    default: unitTimer = 600; timer = -1; spriteRenderer.sprite = sprites[14]; textMeshPro.color = colors[0]; barOutline.color = colors[0]; spriteRenderer2.color = colors[0]; marker.arrowColor = colors[0]; break;
                    case 1: unitTimer = 100; timer = 200 + timeModifier; spriteRenderer.sprite = sprites[13]; textMeshPro.color = colors[10]; barOutline.color = colors[1]; spriteRenderer2.color = colors[1]; marker.arrowColor = colors[1]; break;
                }
                break;
        }

        //patients = 3; //test
        remainingPatients = patients;
        _capacityText.text = remainingPatients.ToString() + "/" + patients.ToString();
        marker.StartUp(spriteRenderer.sprite);
        timer = GetTimer(timer);
        maxTime = timer;

        info = new EmergancyInfo
        {
            agency = type,
            priority = priority,
            zone = zone,
            unitTimer = unitTimer,
            node = node,
        };

        textMeshPro.text = (timer / 10).ToString("f0");
        if (timer != -1) { TickSystem.Instance.On1Tick += On1Tick; }
        else { }

        RotateVis(CameraController.instance, CameraController.instance.targetRotation);
        CameraController.instance.OnRotation += RotateVis;
    }

    private void On1Tick(object sender, int tick)
    {
        if (!paused)
        {
            timer--;
            textMeshPro.text = (timer / 10).ToString("f0");
            UpdateBar((float)timer / maxTime);
            info.patients = remainingPatients;
            if (timer <= 0) { info.failedTimes++; Controller.instance.LoseEmergency(info); Destroy(gameObject); }
        }
    }
    private void OnDestroy()
    {
        TickSystem.Instance.On1Tick -= On1Tick;
        CameraController.instance.OnRotation -= RotateVis;
        node.claimed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>() != null)
        {
            if (other.GetComponent<Unit>().type == type || type == -1)
            {
                if (unitsOnScene < remainingPatients || (patients == 0 && unitsOnScene == 0))
                {
                    other.GetComponent<Unit>().OnScene(this);
                }
            }
        }
    }
    public void Pause(Unit unit)
    {
        unitsOnScene += unit.capacity;
        paused = true;
        bar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void Resume()
    {
        if (unitsOnScene < patients)
        {
            paused = false;
            bar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public bool Finish(Unit unit)
    {
        remainingPatients--;
        unitsOnScene -= unit.capacity;
        _capacityText.text = remainingPatients.ToString() + "/" + patients.ToString();
        if (remainingPatients < 0) { Destroy(gameObject, 0.01f); return  false; }
        if (remainingPatients == 0) { Destroy(gameObject, 0.01f); return true; }
        if (remainingPatients > 0) { Resume(); return true; }
        return false;//failsafe
    }

    private int GetTimer(int time)
    {
        return
            (int)(time
            * (((float)PlayerPrefs.GetInt(KeyHolder.callTimesKey, 0) / KeyHolder.dividedBy1_MaxLevel100) + 1))
            ;
    }

    [SerializeField] Transform bar;
    public void UpdateBar(float value)
    {
        Vector3 newScale = bar.localScale;
        newScale.x = value;
        bar.localScale = newScale;
    }

    private void RotateVis(object sender, Quaternion targetRotation)
    {
        rotator.rotation = targetRotation;
    }
}

public struct EmergancyInfo
{
    public int agency;
    public int priority;
    public int zone;
    public int unitTimer;
    public EmergencyNode node;
    public int patients;
    public int failedTimes;
}
