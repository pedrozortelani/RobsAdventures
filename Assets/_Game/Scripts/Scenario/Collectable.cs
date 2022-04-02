using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] TYPE itemType = new TYPE();

    [SerializeField] AudioClip pickupSound = null;
    AudioSource audioSource;
    Animator anim;

    enum TYPE
    {
        PowerUp,
        Marble
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(pickupSound, 0.6f);

            if(itemType == TYPE.Marble)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                collision.gameObject.GetComponent<PlayerController>().PickMarble();
                anim.Play("PickUp");
            }
            else if (itemType == TYPE.PowerUp)
            {
                gameObject.GetComponent<IPowerUp>().PickPowerUP(collision.gameObject.GetComponent<PlayerController>());
                GetComponent<CircleCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
