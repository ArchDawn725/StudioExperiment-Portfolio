using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private AudioSource ground_Tap;
    [SerializeField] private GameObject clickEffect;
    public GameObject SpawnedEffect;

    public static InputController Instance { get; private set; }
    private void Awake() { Instance = this; }

    private void Start()
    {
        ground_Tap = GetComponent<AudioSource>();
    }
    private void Update()
    {
        ProcessInput();
    }
    private void ProcessInput()
    {
        Vector3 worldPos = new Vector3(0, 0, 0);
#if PLATFORM_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Vector3 touchPosition = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(touchPosition);
            worldPos.y -= 2.5f; //why do we need this? IDK
            SpawnedEffect = Instantiate(clickEffect, worldPos, Quaternion.identity);
                        worldPos.z = 0;
            SpawnedEffect.transform.position = worldPos;

        }

                if (Input.GetMouseButtonUp(0))
        {
            if (SpawnedEffect != null) { SpawnedEffect.GetComponent<ClickEffect>().StartUp(); }
        }

                if (worldPos != new Vector3(0,0,0))
        {
            HitObject(worldPos);
        }
#else
        if (Touchscreen.current.primaryTouch != null)
        {
            if (Input.touchCount > 0) {Touch touch = Input.GetTouch(0); if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { return; } if (IsPointerOverUIObject()) { return; } }
            if (Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the pinch movement is large enough, change the zoom value
                zoomValue += deltaMagnitudeDiff * zoomSpeed;

                Zoom();
                return;
            }
            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                worldPos = Camera.main.ScreenToWorldPoint(touchPosition);
                worldPos.y -= 2.5f; //why do we need this? IDK

                SpawnedEffect = Instantiate(clickEffect, worldPos, Quaternion.identity);
            }
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                Vector3 touchPosition = Input.mousePosition;
                worldPos = Camera.main.ScreenToWorldPoint(touchPosition);
                worldPos.y -= 2.5f; //why do we need this? IDK
                worldPos.z = 0;
                if (SpawnedEffect != null) { SpawnedEffect.transform.position = worldPos; }
            }
            if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame) 
            {
                if (SpawnedEffect != null) { SpawnedEffect.GetComponent<ClickEffect>().StartUp();

                    Vector3 touchPosition = Input.mousePosition;
                    worldPos = Camera.main.ScreenToWorldPoint(touchPosition);
                    worldPos.y -= 2.5f; //why do we need this? IDK
                    worldPos.z = 0;
                    HitObject(worldPos); }
            }
        }
#endif


    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private Building selectedBuilding;
    private SpriteRenderer selectedSpawner;
    private void HitObject(Vector3 worldPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.down, 0);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "UI") { Debug.Log("UI"); return; }

            if (hit.collider != null && !hit.collider.isTrigger)
            {
                if (hit.collider.tag == "Spawner")
                {
                    if (selectedBuilding != null) { selectedBuilding.DeSelected(); }
                    if (selectedSpawner != null) { selectedSpawner.color = Color.white; }
                    BuildingSelectionController.Instance.Enable(hit.collider.gameObject, false);
                    selectedSpawner = hit.collider.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    selectedSpawner.color = Color.red;
                    break;
                }

                if (hit.collider.tag == "Tower_Spawner")
                {
                    if (selectedBuilding != null) { selectedBuilding.DeSelected(); }
                    if (selectedSpawner != null) { selectedSpawner.color = Color.white; }
                    BuildingSelectionController.Instance.Enable(hit.collider.gameObject, true);
                    selectedSpawner = hit.collider.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    selectedSpawner.color = Color.red;
                    break;
                }

                if (hit.collider.tag == "Building")
                {
                    if (hit.collider.GetComponent<Building>().team == 1)
                    {
                        if (selectedBuilding != null) { selectedBuilding.DeSelected(); }
                        if (selectedSpawner != null) { selectedSpawner.color = Color.white; }
                        BuildingUpgradeController.Instance.Enable(hit.collider.GetComponent<Building>());
                        selectedBuilding = hit.collider.GetComponent<Building>();
                        selectedBuilding.Selected();
                        break;
                    }
                }

                if (hit.collider.GetComponent<Health>() != null) { hit.collider.GetComponent<Health>().Onclick(); }
            }
        }

        ground_Tap.Play();
    }

    private float zoomSpeed = 0.1f; // Control how fast the zoom changes
    private float zoomValue = 4.8f; // Initial zoom value, set this as needed
    private float minZoom = 4.8f;
    private float maxZoom = 9.6f;
    private void Zoom()
    {
        zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);

        Vector3 negPosition = transform.GetChild(3).transform.position;
        Vector3 posPosition = transform.GetChild(4).transform.position;

        posPosition.x = zoomValue;
        negPosition.x = -zoomValue;

        transform.GetChild(3).transform.position = negPosition;
        transform.GetChild(4).transform.position = posPosition;
    }
}
