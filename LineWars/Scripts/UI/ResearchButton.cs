using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private ResearchButton[] requiredResearches;
    [SerializeField] private string research_String;
    [SerializeField] private int cost;

    [HideInInspector] public bool researched;
    private Button button;
    private TextMeshProUGUI text;
    public event EventHandler<int> OnResearched; 

    private void Start()
    {
        button = GetComponent<Button>();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (requiredResearches.Length > 0)
        {
            for (int i = 0; i < requiredResearches.Length; i++)
            {
                GameObject newLinePrefab = Instantiate(linePrefab, this.transform.position, Quaternion.identity, this.transform);
                UILineRenderer lineRend = newLinePrefab.GetComponent<UILineRenderer>();
                lineRend.points[0] = new Vector2(0,0);
                lineRend.points[1] = -GetRelativePositionOfObject(this.GetComponent<RectTransform>(), requiredResearches[i].GetComponent<RectTransform>());
                lineRend.StartUp();
                requiredResearches[i].OnResearched += interactableCheck;
            }
        }

        Invoke(nameof(researchCheck), 0.1f);
        button.onClick.AddListener(BuyResearch);

        string suffix = "_Research";
        string newName = research_String.Substring(0, research_String.Length - suffix.Length);
        text.text =
    newName + System.Environment.NewLine +
    cost
    ;
        StartController.Instance.experienceChanged += interactableCheck;
    }

    private void researchCheck()
    {
        if (PlayerPrefs.GetInt(research_String, 0) == 1) { researched = true; button.interactable = false; GetComponent<Image>().color = Color.black; }
        else
        {
            Invoke(nameof(Delay), 0.1f);
        }
    }
    private void Delay()
    {
        interactableCheck(this, 0);
    }
    private void interactableCheck(object sender, int researched)
    {
        if (!this.researched)
        {
            if (PlayerPrefs.GetInt(KeyHolder.experienceKey, 0) >= cost && RequirementsMet())
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
        else { button.interactable = false; }

    }
    private bool RequirementsMet()
    {
        if (requiredResearches.Length == 0) { return true; }
        for (int i = 0; i < requiredResearches.Length; i++)
        {
            if (!requiredResearches[i].researched) { return false; }
        }
        return true;
    }
    private void BuyResearch()
    {
        PlayerPrefs.SetInt(research_String, 1);
        int xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
        PlayerPrefs.SetInt(KeyHolder.experienceKey, xp - cost);
        researched = true;
        GetComponent<Image>().color = Color.black;
        button.interactable = false;
        OnResearched?.Invoke(this, 0);
        StartController.Instance.xpChanged();
    }

    Vector2 GetRelativePositionOfObject(RectTransform object1, RectTransform object2)
    {
        // Get the screen position of object1
        Vector3 screenPosition = object1.position;

        // Convert the screen position of object1 to the local position of object2
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(object2, screenPosition, null, out localPosition);

        // Return the local position of object1 relative to object2
        return localPosition;
    }
}
