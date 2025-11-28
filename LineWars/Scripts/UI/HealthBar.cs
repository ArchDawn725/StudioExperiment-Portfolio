using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Health hp;
    private SpriteRenderer[] renderers = new SpriteRenderer[3];

    private Transform bar;
    private bool isDone = true;
    [SerializeField] private float autoFadeTime = 1;

    private float targetValue;

    private Vector3 currentScale;
    private Vector3 startScale;

    private bool fadeIn;
    private bool fadeOut;

    [SerializeField] private float fadeInTime = 0.2f; // How long the fade out will take
    [SerializeField] private float fadeOutTime = 0.4f; // How long the fade out will take
    private float timer = 0f; // Timer for keeping track of the elapsed time

    private int count;

    public void StartUp()
    {
        hp = transform.parent.GetComponent<Health>();

        renderers[0] = transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderers[1] = transform.GetChild(1).GetComponent<SpriteRenderer>();
        bar = transform.GetChild(2);
        renderers[2] = transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();

        hp.healthChanged += UpdateHealth;
        //Activate(1);
    }
    private void UpdateHealth(object sender, int health)
    {
        float target = (float)hp.health / (float)hp.maxHealth; //needs to send as a struct
        Activate(target);
    }

    private void Activate(float target)
    {
        targetValue = target;

        startScale = bar.localScale;
        currentScale = bar.localScale;
        fadeOut = false;
        timer = 0;
        fadeIn = true;
        isDone = false;
    }
    private void Update()
    {
        if (!isDone)
        {
            currentScale.x = Mathf.Lerp(currentScale.x, targetValue, (Time.deltaTime * 2.5f) * TickSystem.Instance.timeMultiplier);
            bar.localScale = currentScale;

            if (currentScale.x <= targetValue + 0.001) { Done(); }
        }

        if (fadeIn)
        {
            timer += Time.deltaTime;
            count = 0;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].gameObject.SetActive(true);
                float alpha = Mathf.Lerp(0f, 1f, timer);
                Color objectColor = renderers[i].material.color;
                objectColor.a = alpha;
                renderers[i].material.color = objectColor;
                if (alpha >= 1f)
                {
                    count++;
                }
            }

            if (count >= renderers.Length)
            {
                fadeIn = false; timer = 0;
                Invoke("AutoFade", autoFadeTime);
            }
            return;
        }
        else if (fadeOut && isDone)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < renderers.Length; i++)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer);
                Color objectColor = renderers[i].material.color;
                objectColor.a = alpha;
                renderers[i].material.color = objectColor;
                if (alpha <= 0f)
                {
                    count--;
                    renderers[i].gameObject.SetActive(false);
                }
            }
        }

        if (count <= 0) 
        { 
            count = 0; fadeOut = false; timer = 0;
        }
    }
    private void Done()
    {
        isDone = true;
    }
    private void AutoFade()
    {
        fadeOut = true;
    }
}
