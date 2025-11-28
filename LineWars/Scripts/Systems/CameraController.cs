using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] public float max;
    [SerializeField] private float min;

    private bool active;
    private void Start()
    {
        //Invoke(nameof(Delay), 2);
        active = true;
    }
    private void Delay()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        active = true;
    }
    private void Update()
    {
        if (!active) { return; }
        if (EventSystem.current.IsPointerOverGameObject()) return;
#if UNITY_EDITOR


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * moveSpeed, -touchDeltaPosition.y * moveSpeed, 0);

            transform.position = new Vector3(
                0,
                Mathf.Clamp(transform.position.y, min, max),
                0);
        }
        //FlickTouch();

#elif PLATFORM_STANDALONE_WIN
        if (Input.GetMouseButton(0))
        {
            Vector2 touchDeltaPosition = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchDeltaPosition);
            if (worldPos.y >= transform.position.y)
            {
                transform.Translate(0, moveSpeed * 10, 0);
            }
            else
            {
                transform.Translate(0, -moveSpeed * 10, 0);
            }


            transform.position = new Vector3(
                0,
                Mathf.Clamp(transform.position.y, min, max),
                0);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * moveSpeed, -touchDeltaPosition.y * moveSpeed, 0);

            transform.position = new Vector3(
                0,
                Mathf.Clamp(transform.position.y, min, max),
                0);
        }
                //FlickTouch();
#endif
    }
    private Vector2 touchStartPos;
    [SerializeField] private float swipeThreshold = 50f; // Minimum distance for a swipe gesture to register
    [SerializeField] private float swipeTimeThreshold = 0.25f; // Minimum distance for a swipe gesture to register
    private float timer;
    private void FlickTouch()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            timer += Time.deltaTime;

            // Check for the beginning of the touch gesture
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                timer = 0;
            }

            // Check for the end of the touch gesture
            if (touch.phase == TouchPhase.Ended)
            {

                // Calculate the swipe direction based on the difference between start and end positions
                Vector2 swipeDirection = touch.position - touchStartPos;
                // Check if the swipe distance exceeds the threshold
                if (swipeDirection.magnitude <= swipeThreshold && timer <= swipeTimeThreshold)
                {
                    // Convert the swipe direction to 1 or -1
                    int swipeDirectionInt = (int)Mathf.Sign(swipeDirection.x);

                    if (swipeDirectionInt == -1) { transform.position = new Vector3(0, max, 0); }
                    else { transform.position = new Vector3(0, min, 0); }
                }
            }
        }
    }
}
