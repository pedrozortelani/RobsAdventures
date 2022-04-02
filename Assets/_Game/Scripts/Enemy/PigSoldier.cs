using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSoldier : MonoBehaviour, IEnemy
{
    [SerializeField] float speed = 0;
    float stateEntryTime = 0;
    bool isAttacking = false;
    public int damage { get; set; }
    float health = 3;
    public bool isAlive { get; set; }

    [SerializeField] STATE currentState = STATE.Idle;
    STATE nextState = STATE.Idle;

    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    AudioSource audioSource;
    [SerializeField] AudioClip attackSound = null;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip deathSound = null;
    PlayerController target = null;
    [SerializeField] LayerMask playerLayerMask = new LayerMask();

    enum STATE
    {
        Idle,
        Walk,
        Follow,
        Tired
    }

    private void Start()
    {
        isAlive = true;
        damage = 1;
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;

        if(isAlive)
        { 
            switch(currentState)
            {
                case STATE.Idle:
                    
                    if (Time.time > stateEntryTime + 4)
                    {
                        if (transform.rotation.y == -1)
                        {
                            transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(0, 180, 0);
                        }
                        anim.Play("Walk");
                        nextState = STATE.Walk;
                    }

                    if (target)
                    {
                        speed = 2;
                        anim.Play("Walk");
                        nextState = STATE.Follow;
                    }

                    break;
                case STATE.Walk:
                    if (Time.time > stateEntryTime + 2)
                    {
                        anim.Play("Idle");
                        nextState = STATE.Idle;
                    }

                    transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);

                    if (target)
                    {
                        speed = 2;
                        anim.Play("Walk");
                        nextState = STATE.Follow;
                    }
                    break;
                case STATE.Follow:
                    if(target)
                    { 
                        if(transform.rotation.y == -1)
                        {
                            if (target.transform.position.x - transform.position.x < -0.5f)
                            {
                                transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
                            }
                            else
                            {
                                if (!isAttacking)
                                {
                                    isAttacking = true;
                                    anim.Play("Attack");
                                }
                            }
                        }
                        else
                        {
                            if (transform.position.x - target.transform.position.x < -0.5f)
                            {
                                transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
                            }
                            else
                            {
                                if (!isAttacking)
                                {
                                    isAttacking = true;
                                    anim.Play("Attack");
                                }
                            }
                        }
                    }
                    else
                    {
                        speed = 1.5f;
                        anim.Play("Idle");
                        nextState = STATE.Idle;
                    }
                    break;
                case STATE.Tired:
                    if (Time.time > stateEntryTime + 4)
                    {
                        anim.Play("Idle");
                        nextState = STATE.Idle;
                    }
                    break;
            }

            if(nextState != currentState)
            {
                stateEntryTime = Time.time;
                currentState = nextState;
                isAttacking = false;
            }

            RaycastHit2D playerDetection;

            if (transform.rotation.y == -1)
            {
                playerDetection = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.15f), Vector2.left, 2, playerLayerMask);
            }
            else
            {
                playerDetection = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.15f), Vector2.right, 2, playerLayerMask);
            }

            if(playerDetection.collider)
            {
                target = playerDetection.collider.GetComponent<PlayerController>();
            }
            else
            {
                target = null;
            }
        }
    }

    void Attack()
    {
        audioSource.PlayOneShot(attackSound, audioSource.volume);

        if (isAlive)
        { 
            if (target)
            {
                target.TakeDamage(1);
            }
            anim.Play("Tired");
            nextState = STATE.Tired;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            audioSource.PlayOneShot(deathSound, audioSource.volume);
            rb.gravityScale = 0;
            boxCollider.enabled = false;
            anim.Play("Die");
            isAlive = false;
        }
        else
        {
            audioSource.PlayOneShot(hitSound, audioSource.volume);
            anim.Play("Hit");
        }
    }

    public void ResetAnimation()
    {
        if (isAlive)
            anim.Play(nextState.ToString());
    }

    public void EraseDeadBody()
    {
        Destroy(gameObject, 2);
    }
}
