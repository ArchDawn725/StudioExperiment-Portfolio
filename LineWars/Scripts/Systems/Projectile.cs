using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // Assign the target position in the inspector
    private Vector3 originalScale; // To store the original scale of the GameObject
    private Vector3 originalPosition; // To store the starting position
    private float duration; // Total duration of the move and scale
    private float timeElapsed;
    private Vector3 targetPos;
    private CircleCollider2D circleCollider;
    [SerializeField] private float speed;

    public void StartUP(Transform newTarget, float time)
    {
        target = newTarget;
        duration = time / speed;

        originalScale = transform.localScale; // Save the original scale
        originalPosition = transform.position; // Save the original position
        targetPos = target.position;
        circleCollider = transform.GetComponent<CircleCollider2D>();

        // Calculate the direction from the current object to the target
        Vector2 direction = target.transform.position - transform.position;

        // Calculate the angle in degrees from the current object's direction to the target direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation around the z-axis
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        TickSystem.Instance.OnFPSTick += OnTick;
    }

    private void OnTick(object sender, int tick)
    {
        if (timeElapsed < duration)
        {
            // Update the elapsed time
            timeElapsed += Time.deltaTime;

            // Calculate the interpolation factor
            float t = timeElapsed / duration;

            // Move the object towards the target
            transform.position = Vector3.Lerp(originalPosition, targetPos, t);

            // Scale the object
            // Calculate the scale factor, where it peaks at 2x the original scale at t = 0.5
            float scaleFactor = 1 + Mathf.PingPong(t * 3, 1); // Peaks at 2x size, then reduces back to 1x
            transform.localScale = originalScale * scaleFactor;

            if (Vector3.Distance(transform.position, targetPos) < 1) { circleCollider.enabled = true; } else { circleCollider.enabled = false; }
        }
        else { Destroy(gameObject); }
    }

    private void OnDestroy()
    {
        TickSystem.Instance.OnFPSTick -= OnTick;
    }
}
