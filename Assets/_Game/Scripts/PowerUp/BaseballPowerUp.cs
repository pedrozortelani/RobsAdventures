using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballPowerUp : MonoBehaviour, IPowerUp
{
    [SerializeField] GameObject baseballPrefab = null;
    public int ballCount = 0;

    public void UsePowerUP(PlayerController player)
    {
        if (ballCount < 3)
        {
            player.audioSource.PlayOneShot(player.throwSound, player.audioSource.volume);
            player.anim.Play("Throw");
            if (player.transform.rotation.y == 0)
            {
                ballCount++;
                GameObject baseballInstance = Instantiate(baseballPrefab, new Vector3(player.transform.position.x + 0.5f, player.transform.position.y, 0), Quaternion.identity);
                baseballInstance.GetComponent<Baseball>().direction = Vector3.right;
                baseballInstance.GetComponent<Baseball>().powerup = gameObject.GetComponent<BaseballPowerUp>();
            }
            else
            {
                ballCount++;
                GameObject baseballInstance = Instantiate(baseballPrefab, new Vector3(player.transform.position.x - 0.5f, player.transform.position.y, 0), Quaternion.identity);
                baseballInstance.GetComponent<Baseball>().direction = Vector3.left;
                baseballInstance.GetComponent<Baseball>().powerup = gameObject.GetComponent<BaseballPowerUp>();
            }
        }
    }

    public void DeactivatePowerUP(PlayerController player)
    {

    }

    public void PickPowerUP(PlayerController player)
    {
        player.health = 3;
        player.uiScript.AtualizeHealth();

        if(player.powerup)
        {
            player.powerup.GetComponent<IPowerUp>().LosePowerUP(player);
        }

        player.powerup = gameObject;
    }

    public void LosePowerUP(PlayerController player)
    {
        player.powerup = null;
        Destroy(gameObject);
    }

}
