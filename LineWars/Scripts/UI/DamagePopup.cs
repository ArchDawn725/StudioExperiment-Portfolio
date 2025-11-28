using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private Color _color;
    private float timerMax;
    private float timer;
    private static int sortingOrder;
    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }
    private void StartUp(int amount, int maxAmount)
    {
        if (amount > maxAmount)
        {
            _textMeshPro.fontSize = 7;
            _textMeshPro.color = Color.red;
            _textMeshPro.sortingOrder = sortingOrder + 10;
            timerMax = 1.5f;
        }
        else if (amount < 0)
        {
            _textMeshPro.fontSize = 6;
            _textMeshPro.color = Color.green;
            _textMeshPro.sortingOrder = sortingOrder + 5;
            timerMax = 1.25f;
        }
        else
        {
            _textMeshPro.fontSize = 5;
            _textMeshPro.color = Color.yellow;
            _textMeshPro.sortingOrder = sortingOrder + 0;
            timerMax = 1f;
        }

        sortingOrder++;
        timer = timerMax;
        _textMeshPro.text = amount.ToString();
        _color = _textMeshPro.color;
    }

    private Vector3 moveVector = new Vector3(0.25f,0.25f) * 30;
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8 * Time.deltaTime;
        if (timer > timerMax * 0.5f)
        {
            transform.localScale += Vector3.one * 1 * Time.deltaTime;

        } else {
            transform.localScale -= Vector3.one * 1 * Time.deltaTime;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            float disappearSpeed = timerMax * 3;
            _color.a -= disappearSpeed * Time.deltaTime;
            _textMeshPro.color = _color;

            if (_color.a < 0) { Destroy(gameObject); }
        }
    }
    ////
    public static DamagePopup Create(Transform popup, Vector3 position, int damageAmount, int maxDamage)
    {
        Transform newDamagePopup = Instantiate(popup, position, Quaternion.identity);

        DamagePopup damagePopup = newDamagePopup.GetComponent<DamagePopup>();
        damagePopup.StartUp(damageAmount, maxDamage);

        return damagePopup;
    }
}
