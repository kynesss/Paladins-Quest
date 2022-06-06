using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    PlayerController player;
    public Image staminaImage;

    public void RegisterPlayer(PlayerController p)
    {
        player = p;
    }

    private void Update()
    {
        staminaImage.fillAmount = player.stats.Stamina / player.stats.MaxStamina;
        
        if(player.isAlive)
            SetStamina();
    }

    public void SetStamina()
    {
        if (player.movement.isRunning)
        {
            player.stats.Stamina -= Time.deltaTime * 10.0f;
        }
        else
        {
            StartCoroutine(StaminaRegen());
        }

        if (player.stats.Stamina > player.stats.MaxStamina)
            player.stats.Stamina = player.stats.MaxStamina;
        else if (player.stats.Stamina < 0)
            player.stats.Stamina = 0f;
    }

    public IEnumerator StaminaRegen()
    {
        if(player.stats.Stamina <= 0f)
        {
            while(Input.GetKey(KeyCode.LeftShift))
            {
                yield return null;
            }

            RegenStaminaDuringWalk();
        }
        else if(player.stats.Stamina > 0 && player.stats.Stamina < player.stats.MaxStamina)
        {
            RegenStaminaDuringWalk();
        }

        yield return null;
    }
    public void RegenStaminaDuringWalk()
    {
        if (player.movement.isWalking)
            player.stats.Stamina += Time.deltaTime * 7.0f;
        else
            player.stats.Stamina += Time.deltaTime * 10.0f;
    }
}
