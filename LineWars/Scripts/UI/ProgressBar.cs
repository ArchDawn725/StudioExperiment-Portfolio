using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private SpriteRenderer[] renderers = new SpriteRenderer[3];

    private Transform bar;
    private bool isDone = true;
    private float targetValue;

    private Vector3 currentScale;
    private Vector3 startScale;

    public void StartUp()
    {
        renderers[0] = transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderers[1] = transform.GetChild(1).GetComponent<SpriteRenderer>();
        bar = transform.GetChild(2);
        renderers[2] = transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();

        Activate(0);
    }
    public void UpdateProgress(int value, int maxValue)
    {
        float target = (float)value / (float)maxValue;
        Activate(target);
    }

    private void Activate(float target)
    {
        targetValue = target;

        startScale = bar.localScale;
        currentScale = bar.localScale;
        if (target == 0 || target == 1) { currentScale.x = 0; bar.localScale = currentScale; isDone = true; return; }
        isDone = false;
    }
    private void Update()
    {
        if (!isDone)
        {
            currentScale.x = Mathf.Lerp(currentScale.x, targetValue, (Time.deltaTime * 2.5f) * TickSystem.Instance.timeMultiplier);
            bar.localScale = currentScale;

            if (currentScale.x >= targetValue - 0.001) { Done(); }
        }
    }
    private void Done()
    {
        isDone = true;
    }
    public void ResetToZero()
    {
        startScale = bar.localScale;
        bar.localScale = new Vector3(0,startScale.y, startScale.z);
    }
}
