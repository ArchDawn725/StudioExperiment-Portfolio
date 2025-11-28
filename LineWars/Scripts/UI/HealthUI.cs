using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider m_HealthSlider;
    [SerializeField] private Health health;
    private void Start()
    {
        health.healthChanged += UpdateSlider;
    }
    private void UpdateSlider(object sender, int tick)
    {
        m_HealthSlider.value = (float)health.health / health.maxHealth;
    }
}
