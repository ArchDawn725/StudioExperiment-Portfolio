using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TickSystem : MonoBehaviour
{
    public static TickSystem Instance { get; private set; }
    private void Awake() { Instance = this; }

    public event EventHandler<int> OnFPSTick; //evey 16ms or 60 times per second
    public event EventHandler<int> On1Tick; //evey 100ms or 0.1 seconds
    public event EventHandler<int> On5Tick; //evey 500ms or 0.5 seconds
    public event EventHandler<int> On10Tick; //evey 1000ms or 1 second
    public event EventHandler<int> On25Tick; //evey 10000ms or 2.5 second
    public event EventHandler<int> On50Tick; //evey 5000ms or 5 second
    public event EventHandler<int> On100Tick; //evey 10000ms or 10 second
    public event EventHandler<int> On600Tick; //evey 60000ms or 1 min

    private int tick;
    public float timeMultiplier = 1f;
    public event EventHandler<float> OnSpeedChange;

    private float tickTimer;
    private float fpsTickTimer;
    private const float FPS_TICK_TIMER_MAX = 0.033333f;
    private const float TICK_TIMER_MAX = 0.1f;

    [SerializeField] private bool fps_Counter;

    [SerializeField] private TextMeshProUGUI currentFPS;
    [SerializeField] private TextMeshProUGUI lowestFPS;
    [SerializeField] private TextMeshProUGUI averageFPSText;

    private float deltaTime = 0.0f;
    private float minFPS = float.MaxValue;
    private float totalFPS = 0.0f;
    private int frameCount = 0;

    private void Update()
    {
        if (timeMultiplier > 0)
        {
            fpsTickTimer += Time.deltaTime;
            tickTimer += Time.deltaTime;

            if (fpsTickTimer >= (FPS_TICK_TIMER_MAX / timeMultiplier))
            {
                fpsTickTimer -= (FPS_TICK_TIMER_MAX / timeMultiplier);
                FPSTick();
            }
            if (tickTimer >= (TICK_TIMER_MAX / timeMultiplier))
            {
                tickTimer -= (TICK_TIMER_MAX / timeMultiplier);
                Tick();
            }

            if (fps_Counter) { FPSCounter(); }
        }
    }

    private void FPSTick() { OnFPSTick?.Invoke(this, 0); }

    private void Tick()
    {
        tick++;

        On1Tick?.Invoke(this, tick);
        if (tick % 5 == 0) { On5Tick?.Invoke(this, tick); }
        if (tick % 10 == 0) { On10Tick?.Invoke(this, tick); }
        if (tick % 25 == 0) { On25Tick?.Invoke(this, tick); }
        if (tick % 50 == 0) { On50Tick?.Invoke(this, tick); }
        if (tick % 100 == 0) { On100Tick?.Invoke(this, tick); }
        if (tick % 600 == 0) { On600Tick?.Invoke(this, tick); }
    }
    public void ChangeTime(float time)
    {
        timeMultiplier = time;
        OnSpeedChange?.Invoke(this, time);
    }
    private void FPSCounter()
    {
        // Update delta time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Update minimum FPS
        if (fps < minFPS && frameCount > 100)
        {
            minFPS = fps;
        }

        // Update average FPS
        totalFPS += fps;
        frameCount++;
        float averageFPS = totalFPS / frameCount;

        currentFPS.text = "FPS: " + fps.ToString("f0");
        lowestFPS.text = "Min: " + minFPS.ToString("f0");
        averageFPSText.text = "Avg: " + averageFPS.ToString("f0");
    }
    [SerializeField] private Transform timeButtonHolder;
    private int oldButton;
    private void OnApplicationPause(bool pause)
    {
        if (pause && timeMultiplier != 0) { oldButton = GetOldButton(); timeMultiplier = 0; timeButtonHolder.GetChild(0).GetComponent<Button>().onClick.Invoke(); }
        else if (oldButton != 0) { timeButtonHolder.GetChild(oldButton).GetComponent<Button>().onClick.Invoke(); }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus && timeMultiplier != 0) { oldButton = GetOldButton(); timeMultiplier = 0; }
        else if (oldButton != 0) { timeButtonHolder.GetChild(oldButton).GetComponent<Button>().onClick.Invoke(); }
    }
    private int GetOldButton()
    {
        if (timeMultiplier == 0) {  return 0; }
        if (timeMultiplier >= 2) {  return 2; }
        return 1;
    }
}
