using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator anim;
    bool isActive = false;
    [SerializeField] AudioClip activateSound = null;
    AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isActive)
        {
            isActive = true;
            audioSource.PlayOneShot(activateSound, audioSource.volume);
            collision.gameObject.GetComponent<PlayerController>().checkpoint = 1;
            anim.Play("Activate");
        }
    }

    public void CheckPointOn()
    {
        anim.Play("CheckPointOn");
    }
}
