using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Die("normal");
        }
        else if(collision.gameObject.CompareTag("Destructible"))
        {
            Destroy(collision.gameObject);
        }
    }
}
