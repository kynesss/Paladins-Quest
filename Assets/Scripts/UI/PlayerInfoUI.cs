using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public PlayerController player;

    public Image HealthBarImage;
    public Image ManaBarImage;
    public Text LevelTxt;
    public Text PlayerName;

    public void RegisterPlayer(PlayerController p)
    {
        player = p;
    }

    private void Update()
    {
        HealthBarImage.fillAmount = player.stats.Health / player.stats.MaxHealth;
        ManaBarImage.fillAmount = player.stats.Mana / player.stats.MaxMana;
        LevelTxt.text = player.stats.Level.ToString();
        PlayerName.text = player.stats.Name;
    }
}
