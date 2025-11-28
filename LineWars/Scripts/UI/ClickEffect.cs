using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public float duration = 3.0f; // Total duration of the scaling in seconds
    private Vector3 originalScale; // To store the original scale of the GameObject
    private float timeElapsed = 0; // Time elapsed since the start of the scaling

    private bool started;
    private bool started2;
    private void Start() { Invoke(nameof(Delay), 1); }
    private void Delay() { started2 = true; }
    public void StartUp()
    {
        originalScale = transform.localScale; // Save the original scale
        started = true;
    }
    private void Update()
    {
        if (started)
        {
            if (timeElapsed < duration)
            {
                // Update the elapsed time
                timeElapsed += Time.deltaTime;

                // Calculate the interpolation factor
                float t = timeElapsed / duration;

                // Smoothly interpolate the scale from the original scale to 3 times the original scale
                transform.localScale = Vector3.Lerp(originalScale, originalScale * 3, t);
            }
            else
            {
                // Once the target scale is reached and duration has elapsed, destroy the GameObject
                Destroy(gameObject);
            }
        }

        if (started2)
        {
            if (InputController.Instance.SpawnedEffect != gameObject) { started = true; }
        }
    }
}
