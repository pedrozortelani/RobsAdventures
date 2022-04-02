using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedTable : MonoBehaviour
{
    [SerializeField] int worldIndex = 0;
    [SerializeField] int index = 0;
    [SerializeField] string worldmap = null;
    [SerializeField] Sprite bedTableOff = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().sprite = bedTableOff;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.checkpoint = 0;
            SaveSystem.SavePlayer(player);
            SaveSystem.SaveWorld(worldIndex, index);
            player.isAlive = false;
            player.anim.Play("Celebrate");
            player.audioSource.PlayOneShot(player.celebrateSound, 0.6f);
            Invoke("EndStage", 2);
        }
    }

    void EndStage()
    {
        SceneManager.LoadScene(worldmap);
    }
}
