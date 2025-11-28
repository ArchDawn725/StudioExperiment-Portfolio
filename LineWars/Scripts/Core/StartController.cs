using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TouchPhase = UnityEngine.TouchPhase;

public class StartController : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject screen1;

    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    public EventHandler<int> experienceChanged;

    [SerializeField] private GameObject[] screens;
    [SerializeField] private int activeScreen;
    [SerializeField] private Button[] screenToggles;
    [SerializeField] private AudioSource startButtonClickFX;
    [SerializeField] private AudioSource buttonClickFX;

    public static StartController Instance { get; private set; }
    private void Awake() { if (Instance != null && Instance != this) { Destroy(this); } else { Instance = this;} }
    private void Start()
    {
        int xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
        //xpText.text = $"XP: {xp}";
        xpText.text = xp.ToString();
        PlayerPrefs.SetInt(KeyHolder.levelKey, PlayerPrefs.GetInt(KeyHolder.levelKey, 1));
        int level = PlayerPrefs.GetInt(KeyHolder.levelKey, 1);
        if (level < 1) { PlayerPrefs.SetInt(KeyHolder.levelKey, 1); level = 1; }
        levelText.text = $"Start Level: {level}";
        ScreenChange(0);
    }
    public void StartGame()
    {
        startButtonClickFX.Play();
        loadingScreen.SetActive(true);
        screen1.SetActive(false);
        Invoke(nameof(StartLoading), 1f);
    }
    private void StartLoading() { SceneManager.LoadSceneAsync(1); }
    public void xpChanged() 
    {
        buttonClickFX.Play();
        //xpText.text = $"XP: {PlayerPrefs.GetInt(KeyHolder.experienceKey, 0)}"; 
        xpText.text = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0).ToString();
        experienceChanged?.Invoke(this, PlayerPrefs.GetInt(KeyHolder.experienceKey, 0)); 
    }

    public void Reset() { PlayerPrefs.DeleteAll(); SceneManager.LoadSceneAsync(0); }
    public void Debug_GiveXP() { PlayerPrefs.SetInt(KeyHolder.experienceKey, 100000); xpChanged(); }

    public void ScreenChange(int value)
    {
        Debug.Log(value);
        buttonClickFX.Play();
        screens[activeScreen].SetActive(false);
        activeScreen += value;

        if (activeScreen < 0) { activeScreen = screens.Length - 1; }
        if (activeScreen >= screens.Length) { activeScreen = 0; }

        //if (activeScreen <= 0) { screenToggles[0].interactable = false; sceneCanGoBack = false; }
        //else { screenToggles[0].interactable = true; sceneCanGoBack = true; }

        //if (activeScreen >= screens.Length - 1) { screenToggles[1].interactable = false; sceneCanGoForward = false; }
        //else { screenToggles[1].interactable = true; sceneCanGoForward = true; }

        screens[activeScreen].SetActive(true);
    }
    [SerializeField] private float zoomSpeed = 0.1f;  // Speed of the zoom
    private void Update()
    {
        if (activeScreen != 2)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            ProcessScreenSwiping();
        }
        else
        {
            if (Input.touchCount == 2)  // Make sure there are exactly two touches on the screen
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Check the touch phase of each touch
                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    // Find the position in the previous frame of each touch.
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                    Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                    // Find the magnitude of the vector (distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                    float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    // If the pinch movement is large enough, scale the object
                    if (Mathf.Abs(deltaMagnitudeDiff) >= zoomSpeed)
                    {
                        // Scale the object, you can replace this logic with your zoom logic
                        // This could be modifying the camera's field of view, the scale of a map, etc.
                        float pinchAmount = -deltaMagnitudeDiff * zoomSpeed;
                        ProcessResearchZoom(pinchAmount);
                    }
                }
            }
            /*
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Debug.Log(scroll);
            ProcessResearchZoom(scroll);
            */
        }

    }

    private Vector2 touchStartPos;
    [SerializeField] private float swipeThreshold = 50f; // Minimum distance for a swipe gesture to register
    private bool sceneCanGoForward = true;
    private bool sceneCanGoBack = false;

    private void ProcessScreenSwiping()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check for the beginning of the touch gesture
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }

            // Check for the end of the touch gesture
            if (touch.phase == TouchPhase.Ended)
            {
                // Calculate the swipe direction based on the difference between start and end positions
                Vector2 swipeDirection = touch.position - touchStartPos;
                // Check if the swipe distance exceeds the threshold
                if (swipeDirection.magnitude >= swipeThreshold)
                {
                    // Convert the swipe direction to 1 or -1
                    int swipeDirectionInt = (int)Mathf.Sign(swipeDirection.x);

                    // Use the swipe direction (1 or -1) for further processing

                    if (swipeDirectionInt > 0)
                    {
                        if (sceneCanGoForward) { ScreenChange(swipeDirectionInt); }
                    }
                    else
                    {
                        if (sceneCanGoBack) { ScreenChange(swipeDirectionInt); }
                    }
                }
            }
        }
    }
    [SerializeField] private RectTransform researchRectTransform;
    [SerializeField] private float resizeSpeed = 100f; // Control how fast the UI element resizes
    private void ProcessResearchZoom(float scroll)
    {
        //researchRectTransform.sizeDelta += new Vector2(scroll * resizeSpeed, scroll * resizeSpeed);
        Vector3 newSize = researchRectTransform.localScale + new Vector3(scroll * resizeSpeed, scroll * resizeSpeed);
        newSize.x = Mathf.Clamp(newSize.x, 0.4f, 1);
        newSize.y = Mathf.Clamp(newSize.y, 0.4f, 1);
        researchRectTransform.localScale = newSize;
    }
}
