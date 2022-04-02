using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmLeafPowerUp : MonoBehaviour, IPowerUp
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UsePowerUP(PlayerController player)
    {
        audioSource.Play();
        player.onVine++;
        player.transform.Find("PalmLeaf").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void DeactivatePowerUP(PlayerController player)
    {
        if (player.transform.Find("PalmLeaf").GetComponent<SpriteRenderer>().enabled)
        {
            audioSource.Stop();
            player.onVine--;
            player.transform.Find("PalmLeaf").GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void PickPowerUP(PlayerController player)
    {
        player.health = 3;
        player.uiScript.AtualizeHealth();

        if (player.powerup)
        {
            player.powerup.GetComponent<IPowerUp>().LosePowerUP(player);
        }

        player.powerup = gameObject;
    }

    public void LosePowerUP(PlayerController player)
    {
        if (Input.GetKey(KeyCode.C))
        {
            player.onVine--;
        }
        player.transform.Find("PalmLeaf").GetComponent<SpriteRenderer>().enabled = false;

        player.powerup = null;
        Destroy(gameObject);
    }
}
