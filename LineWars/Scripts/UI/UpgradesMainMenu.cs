using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradesMainMenu : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private int multiplier;

    private int xp;
    private int value;
    private int cost;

    private Button button;
    private TextMeshProUGUI text;

    private void Start()
    {
        xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
        value = PlayerPrefs.GetInt(key, 0);

        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonPress);
        button.interactable = InteractableCheck();
        ChangeDisplay();

        StartController.Instance.experienceChanged += XPChanged;
    }
    public void ButtonPress()
    {
        value++;
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.SetInt(KeyHolder.experienceKey, xp - cost);
        StartController.Instance.xpChanged();
    }
    private void ChangeDisplay()
    {
        text.text =
            key + System.Environment.NewLine +
            "x" + value + System.Environment.NewLine +
            "$" + cost
            ;
    }
    private bool InteractableCheck()
    {
        if (value < 200)
        {
            cost = value * multiplier;
            if (cost == 0) { cost++; }

            ChangeDisplay();
            if (xp >= cost) { return true; }
            else { return false; }
        }
        else { ChangeDisplay(); return false; }
    }
    private void XPChanged(object sender, int value) { xp = value; button.interactable = InteractableCheck(); }
}
