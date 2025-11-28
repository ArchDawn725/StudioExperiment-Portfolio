using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource bossMusic;
    public static AIController Instance { get; private set; }
    private void Awake() { Instance = this; }

    public void StartUp(EnemyAIController ai)
    {
        //if random
        if (PlayerPrefs.GetInt(KeyHolder.levelKey, 0) > Controller.Instance.levels.Length) { }//add upgrades?

        //if boss
        if (PlayerPrefs.GetInt(KeyHolder.levelKey, 1) % 5 == 0 && PlayerPrefs.GetInt(KeyHolder.levelKey, 0) != 0) { music.Stop(); bossMusic.Play(); } //anything extra special?

        ai.StartUp();
    }
}
