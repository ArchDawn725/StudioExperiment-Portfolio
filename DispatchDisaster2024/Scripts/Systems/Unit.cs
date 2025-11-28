using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private TextMeshPro _levelText;
    [SerializeField] private TextMeshPro _capacityText;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private UnitPathfinder _pathFinder;

    [SerializeField] private bool onScene;
    [SerializeField] private bool atDestination;
    [SerializeField] private int timer;
    private Emergency emergency;
    public bool stationed;

    public UnitSO unitSO;
    [SerializeField] private int level;
    [SerializeField] private int maxFuel;
    private bool isMoving;
    [SerializeField] private Transform fuelBar;
    [SerializeField] private Transform ProgressBar;
    public string myName;
    public int type;
    public int capacity {  get; private set; }
    private List<EmergancyInfo> patients = new List<EmergancyInfo>();
    private Transform rotateVis;
    private SpriteRenderer myIcon;
    public void StartUp(UnitSO unit, int type)
    {
        rotateVis = transform.GetChild(2);
        this.capacity = unit.capacity;
        this.type = type;
        unitSO = unit;
        maxFuel = GetMaxFuelTicks();
        fuelTicks = maxFuel;
        _pathFinder.movementSpeed = GetSpeed();
        TickSystem.Instance.On1Tick += OperatingCosts;
        _levelText.text = $"Lv. {0}";
        _capacityText.text = patients.Count + "/" + capacity.ToString();
        GameObject spawnedObject = Instantiate(unit.unitVisual, transform.position, Quaternion.identity, transform);
        myIcon = spawnedObject.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();

        RotateVis(CameraController.instance, CameraController.instance.targetRotation);
        CameraController.instance.OnRotation += RotateVis;
    }

    public void Moving(bool isMoving)
    {
        this.isMoving = isMoving;
        if (!onScene && !atDestination)
        {
            if (isMoving)
            {
                if (patients.Count > 0)
                {
                    _textMeshPro.text = "Transporting";
                }
                else
                {
                    _textMeshPro.text = "En route";
                }
            }
            else
            {
                if (patients.Count > 0)
                {
                    _textMeshPro.text = "Waiting";
                }
                else
                {
                    _textMeshPro.text = "Available";
                }
            }
        }
    }
    private int maxTime;
    public void OnScene(Emergency emergency)
    {
        if (patients.Count < capacity)
        {
            //_pathFinder.ClearPathfinding();
            _pathFinder.ChangePathfinding(2);
            patients.Add(emergency.info);
            _pathFinder.movementSpeed = 0;
            this.emergency = emergency;
            emergency.Pause(this);
            _collider.enabled = false;
            timer = GetSceneTime(emergency.info.unitTimer);
            maxTime = timer;
            onScene = true;
            TickSystem.Instance.OnFPSTick += OnFPSTick;
        }
    }
    public void AtDestination(Building building)
    {
        if (patients.Count > 0)
        {
            if (building != null) { building.GetComponent<Transpatenter>().entities--; }
            //_pathFinder.ClearPathfinding();
            _pathFinder.ChangePathfinding(3);
            _pathFinder.movementSpeed = 0;
            _collider.enabled = false;
            timer = GetDestinationTime(patients[0].unitTimer);
            maxTime = timer;
            atDestination = true;
            TickSystem.Instance.OnFPSTick += OnFPSTick;
        }
    }

    private void OnFPSTick(object sender, int tick)
    {
        timer--;
        if (onScene)
        {
            _textMeshPro.text = "On Scene ";// + (timer / 10).ToString("f0");
            if (emergency != null) { emergency.textMeshPro.text = (timer / 30).ToString("f0"); emergency.UpdateBar((float)timer/maxTime); }
        }

        if (atDestination)
        {
            _textMeshPro.text = "At Destination ";// + (timer / 30).ToString("f0");
        }

        UpdateProgressBar((float)timer / maxTime);

        if (timer <= 0)
        {
            if (onScene)
            {
                Unsubscribe();

                if (emergency != null)
                {
                    if (!emergency.Finish(this))//if not to transport
                    {
                        Controller.instance.WinEmergency(patients[0]);
                        patients.RemoveAt(0);
                        if (level < 100) { level++; _levelText.text = $"Lv. {level}"; }
                        PlayerPrefs.SetInt(KeyHolder.experienceKey, PlayerPrefs.GetInt(KeyHolder.experienceKey) + 1);
                        onScene = false;
                        _collider.enabled = true;
                        _pathFinder.movementSpeed = GetSpeed();
                        emergency = null;
                        _capacityText.text = patients.Count + "/" + capacity.ToString();
                        _pathFinder.ChangePathfinding(0);
                    }
                    else
                    {
                        if (patients.Count < capacity && emergency.remainingPatients > 0)
                        {
                            _capacityText.text = patients.Count + "/" + capacity.ToString();
                            OnScene(emergency);
                        }
                        else
                        {
                            onScene = false;
                            _collider.enabled = true;
                            _pathFinder.movementSpeed = GetSpeed();
                            emergency = null;
                            _capacityText.text = patients.Count + "/" + capacity.ToString();
                            _pathFinder.ChangePathfinding(1);
                        }
                    }
                }
                else
                {
                    onScene = false;
                    _collider.enabled = true;
                    _pathFinder.movementSpeed = GetSpeed();
                    emergency = null;
                    _capacityText.text = patients.Count + "/" + capacity.ToString();
                    _pathFinder.ChangePathfinding(1);
                }
            }

            if (atDestination)
            {
                Controller.instance.WinEmergency(patients[0]);
                patients.RemoveAt(0);
                Unsubscribe();
                if (level < 100) { level++; _levelText.text = $"Lv. {level}"; }
                PlayerPrefs.SetInt(KeyHolder.experienceKey, PlayerPrefs.GetInt(KeyHolder.experienceKey) + 1);
                _capacityText.text = patients.Count + "/" + capacity.ToString();

                if (patients.Count == 0)
                {
                    atDestination = false;
                    _collider.enabled = true;
                    _pathFinder.movementSpeed = GetSpeed();
                    _pathFinder.ChangePathfinding(0);
                }
                else { AtDestination(null); }
            }
        }
    }

    private void Unsubscribe()
    {
        TickSystem.Instance.OnFPSTick -= OnFPSTick;
        if (emergency != null) { emergency.Resume(); }
    }

    private void OnDestroy()
    {
        Unsubscribe();
        TickSystem.Instance.On1Tick -= OperatingCosts;
        CameraController.instance.OnRotation -= RotateVis;
    }
    public float GetSpeed()
    {
        return 
            unitSO.speed 
            * (((float)PlayerPrefs.GetInt(KeyHolder.speedKey, 0) / KeyHolder.dividedBy2_MaxLevel100) + 1)
            * (((float)level / KeyHolder.dividedBy2_MaxLevel50) + 1)
            ;
    }
    public int GetSceneTime(int time)
    {
        /*
        return
    time
     - (int)(((double)PlayerPrefs.GetInt(KeyHolder.sceneTimeKey, 0) / KeyHolder.minusBy2_MaxLevel100) * time) == 0.5 max
     - (int)(((double)level / KeyHolder.dividedBy2_MaxLevel50) * time) == 0.5 max
    ;
        */

        /*
        return
            -PlayerPrefs.GetInt(KeyHolder.sceneTimeKey, 0) + 200
            ;
        */
        return
            (int)(time
            + ((-PlayerPrefs.GetInt(KeyHolder.sceneTimeKey, 0) / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * time)
            + ((-level / KeyHolder.minusBy2_MaxLevel50 + 0.5f) * time)
        );
        /*
        return
            (int)(time
            + ((-100/200f + 0.5f) * time) //== 0.5 to -0.5 
            + ((-50 / 100f + 0.5f) * time) //== 0.5 to -0.5
            ); //i.e. plus half or minus half
        //equaling out to 0 or double
        */
    }
    public int GetDestinationTime(int time)
    {
        /*
        return
    time
     - (int)(((double)PlayerPrefs.GetInt(KeyHolder.destinationTimeKey, 0) / KeyHolder.minusBy2_MaxLevel100) * time)
     - (int)(((double)level / KeyHolder.dividedBy2_MaxLevel50) * time)
    ;
        */
        return
    (int)(time
    + ((-PlayerPrefs.GetInt(KeyHolder.destinationTimeKey, 0) / KeyHolder.minusBy2_MaxLevel100 + 0.5f) * time)
    + ((-level / KeyHolder.minusBy2_MaxLevel50 + 0.5f) * time)
);
    }
    public int GetMaxFuelTicks()
    {
        return
(int)(unitSO.fuel
* (((float)PlayerPrefs.GetInt(KeyHolder.fuelCostsKey, 0) / KeyHolder.dividedBy2_MaxLevel100) + 1)
* (((float)level / KeyHolder.dividedBy2_MaxLevel50) + 1))
;
    }
    [SerializeField] private int fuelTicks;
    private void OperatingCosts(object sender, int tick)
    {
        if (stationed && patients.Count == 0 && !isMoving) { return; }

        if (atDestination || onScene) { fuelTicks -= 1; }
        else if (isMoving) { fuelTicks -= 3; }
        else { fuelTicks -= 2; }

        if (fuelTicks <= 0)
        {
            maxFuel = GetMaxFuelTicks();
            fuelTicks = maxFuel;
            Controller.instance.MoneyValueChange(-Controller.instance.fuelCost);
            Controller.instance.fuelCost += 0.01f;
        }

        UpdateFuelBar((float)fuelTicks/ maxFuel);
    }

    public void UpdateFuelBar(float value)
    {
        Vector3 newScale = fuelBar.localScale;
        newScale.x = value;
        fuelBar.localScale = newScale;
    }
    public void UpdateProgressBar(float value)
    {
        Vector3 newScale = ProgressBar.localScale;
        newScale.x = value;
        ProgressBar.localScale = newScale;
    }

    private void RotateVis(object sender, Quaternion targetRotation)
    {
        rotateVis.rotation = targetRotation;
    }
    private bool selected;
    public void Selected()
    {
        selected = true;
        myIcon.color = Color.red;
    }
    public void OnMouseEnter()
    {
        if (!selected) { myIcon.color = Color.yellow; }
    }
    public void OnMouseExit()
    {
        if (!selected) { myIcon.color = Color.white; }
    }
    public void DeSelected()
    {
        myIcon.color = Color.white;
    }
}
