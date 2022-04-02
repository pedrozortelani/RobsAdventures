using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretZone : MonoBehaviour
{
    Tilemap tm;

    private void Start()
    {
        tm = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, 0.13f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, 1);
        }
    }
}
