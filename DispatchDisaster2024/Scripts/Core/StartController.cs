using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public static StartController Instance;
    private void Awake() { Instance = this; }

    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    public EventHandler<int> experienceChanged;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        int xp = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0);
        //xpText.text = $"XP: {xp}";
        xpText.text = xp.ToString();
        levelText.text = $"Start level: {PlayerPrefs.GetInt(KeyHolder.levelKey, 0)}";
    }

    public void xpChanged()
    {
        xpText.text = PlayerPrefs.GetInt(KeyHolder.experienceKey, 0).ToString();
        experienceChanged?.Invoke(this, PlayerPrefs.GetInt(KeyHolder.experienceKey, 0));
    }
    public void StartGame()
    {
        //startButtonClickFX.Play();
        //loadingScreen.SetActive(true);
        //screen1.SetActive(false);
        Invoke(nameof(StartLoading), 1f);
    }
    private void StartLoading() { SceneManager.LoadSceneAsync(1); }
    public void Reset() { PlayerPrefs.DeleteAll(); SceneManager.LoadSceneAsync(0); }
}
