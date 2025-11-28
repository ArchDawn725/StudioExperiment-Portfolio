using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstaSpawnUnit : MonoBehaviour
{
    private Spawner spawner;
    private int cost;
    private Button button;
    public void SetUp(Spawner newSpawner, int foodCost, Sprite sprite)
    {
        spawner = newSpawner;
        cost = foodCost;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ButtonPress());
        Controller.Instance.FoodChanged += InteractableCheck;
        Controller.Instance.FoodChange(0);
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = cost.ToString();
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    private void ButtonPress()
    {
        //costs
        UIController.Instance.SpawnUnit(spawner);
        Controller.Instance.FoodChange(-cost);
    }
    private void InteractableCheck(object sender, int value)
    {
        Debug.Log(value);
        if (value >= cost) { button.interactable = true; }
        else { button.interactable = false; }
    }
}
