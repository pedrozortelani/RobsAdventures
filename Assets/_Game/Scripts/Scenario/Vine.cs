using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.audioSource.clip = player.onVineSound;
            if(!player.audioSource.isPlaying)
            { 
                player.audioSource.Play();
            }
            collision.gameObject.GetComponent<PlayerController>().canJump = true;
            collision.gameObject.GetComponent<PlayerController>().onVine += 1;
            collision.gameObject.GetComponent<Animator>().Play("WallSlide");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.onVine -= 1;
            if ((player.onVine == 0 && !player.transform.Find("PalmLeaf").GetComponent<SpriteRenderer>().enabled) || (player.onVine == 1 && player.transform.Find("PalmLeaf").GetComponent<SpriteRenderer>().enabled))
            {
                player.canJump = false;
                player.audioSource.Stop();
                player.audioSource.clip = null;
                collision.gameObject.GetComponent<Animator>().Play(player.nextState.ToString());
            }
        }
    }
}
