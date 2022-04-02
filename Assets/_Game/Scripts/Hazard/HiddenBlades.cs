using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlades : MonoBehaviour
{
    Animator anim;
    AudioSource audioSource;
    [SerializeField] AudioClip activateSound = null;
    STATE currentState = STATE.Idle;
    STATE nextState = STATE.Idle;
    float stateEntryTime = 0;

    enum STATE
    {
        Idle,
        Attack,
        Retract
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        switch(currentState)
        {
            case STATE.Idle:
                if(Time.time > stateEntryTime + 4)
                {
                    anim.Play("Attack");
                    nextState = STATE.Attack;
                }
                break;
            case STATE.Attack:
                if (Time.time > stateEntryTime + 3)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    anim.Play("Retract");
                    nextState = STATE.Retract;
                }
                break;
        }

        if(nextState != currentState)
        {
            stateEntryTime = Time.time;
            currentState = nextState;
        }
    }

    void ActivateBlades()
    {
        audioSource.PlayOneShot(activateSound, audioSource.volume);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void DeactivateBlades()
    {
        anim.Play("Idle");
        nextState = STATE.Idle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}
