using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyChest : MonoBehaviour, IInteractable
{
    [SerializeField] Sprite openChest = null;
    [SerializeField] GameObject powerUP = null;
    AudioSource audioSource;

    bool isOpened = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().interactionObject = gameObject.GetComponent<ToyChest>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().interactionObject = null;
        }
    }

    public void Interact()
    {
        if(!isOpened)
        {
            audioSource.Play();
            isOpened = true;
            GetComponent<SpriteRenderer>().sprite = openChest;
            Instantiate(powerUP, new Vector3(transform.position.x, transform.position.y + 0.7f, 0), Quaternion.identity);
        }
        
    }
}
