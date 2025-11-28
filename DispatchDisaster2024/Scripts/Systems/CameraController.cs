using Cinemachine;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private CinemachineVirtualCamera cameraCon;

    [SerializeField] private float speed = 5.0f; // Speed of the movement
    [SerializeField] private float verticalSpeed = 2.0f; // Speed of vertical movement via mouse wheel

    private Vector2 xBounds; // Min and max limits for x-axis
    private Vector2 zBounds; // Min and max limits for z-axis
    [SerializeField] private Vector2 yBounds = new Vector2(0.0f, 20.0f); // Min and max limits for y-axis

    private bool controlsEnabled;

    public Quaternion targetRotation;
    private float rotationSpeed = 5.0f; // Speed of the rotation

    public event EventHandler<Quaternion> OnRotation;

    public static CameraController instance;
    private void Awake() { instance = this; }

    private void Start()
    {
        //bounds
        int mapSize = MapGenerator.instance.mapSize * 10;
        xBounds = new Vector2(-mapSize, mapSize);
        zBounds = new Vector2(-mapSize, mapSize);
        yBounds = new Vector2(0, (mapSize / 2) + mapSize);

        //zoom out
        cameraLookAt.position = new Vector3(0, mapSize, -mapSize / 2);

        targetRotation = cameraLookAt.rotation; // Initialize targetRotation

        //event
        Controller.instance.OnFinishedGenerating += OnStart;
    }

    private void OnStart(object sender, bool tick)
    {
        //zoom in
        cameraLookAt.position = new Vector3(0, 0, 0);

        //enable controls
        controlsEnabled = true;
    }
    [SerializeField] private Quaternion e = Quaternion.Euler(10, 10, 10);
    [SerializeField] private Quaternion q = Quaternion.Euler(10, 10, 10);
    void Update()
    {
        if (controlsEnabled)
        {
            float xMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime; // Get horizontal movement
            float zMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;   // Get vertical movement
            float yMove = Input.GetAxis("Mouse ScrollWheel") * -verticalSpeed;  // Get scroll wheel movement

            // Calculate new position within bounds
            Vector3 newPosition = cameraLookAt.position + new Vector3(xMove, yMove, zMove);
            newPosition.x = Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y);
            newPosition.z = Mathf.Clamp(newPosition.z, zBounds.x, zBounds.y);
            newPosition.y = Mathf.Clamp(newPosition.y, yBounds.x, yBounds.y);

            // Update GameObject position
            Vector3 moveDirection = new Vector3(xMove, yMove, zMove);
            moveDirection = cameraLookAt.rotation * moveDirection; // Transform moveDirection by the camera's current rotation
            newPosition = cameraLookAt.position + moveDirection;
            cameraLookAt.position = newPosition;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRotation *= Quaternion.Euler(0, 90, 0); // Add 90 degrees to the current rotation
                OnRotation?.Invoke(this, targetRotation);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                targetRotation *= Quaternion.Euler(0, -90, 0); // Add 90 degrees to the current rotation
                OnRotation?.Invoke(this, targetRotation);
            }

            // Apply the rotation
            cameraLookAt.rotation = Quaternion.Lerp(cameraLookAt.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            /*

            else if (Input.GetKeyDown(KeyCode.Z))
            {
                targetRotation = Quaternion.Euler(0, 0, 00); // Subtract 90 degrees from the current rotation
            }


            */
        }
    }
    public void SetUpCam(float newSpeed, float newZoom)
    {
        speed = newSpeed;
        verticalSpeed = newZoom;
    }
}
