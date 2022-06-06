using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    PlayerController player;

    public Text dataText;
    public Text statsText;

    public void RegisterPlayer(PlayerController p)
    {
        player = p;
    }

    private void Update()
    {
        dataText.text = player.stats.Name + "\n\n" + player.stats.Level + "\n\n" + player.stats.Experience + "\n\n" + "Paladyni";
        statsText.text = player.stats.Health + "/" + player.stats.MaxHealth + "\n\n" + player.stats.Mana + "/" + player.stats.MaxMana + "\n\n" +
        +player.stats.Stamina + "/" + player.stats.MaxStamina + "\n\n" + player.stats.baseAttack + "/" + player.stats.Attack + "\n\n" +
        +player.stats.baseMagicAttack + "/" + player.stats.MagicAttack + "\n\n" + player.stats.baseDefence + "/" + player.stats.Defence;
    }
}
