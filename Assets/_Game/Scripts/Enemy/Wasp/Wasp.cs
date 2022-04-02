using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasp : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject target = null;

    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;
    AudioSource audioSource;
    [SerializeField] AudioClip attackSound = null;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] GameObject sting = null;
    public GameObject hive;

    Vector3 targetPosition;
    [SerializeField] STATE currentState = STATE.None;
    STATE nextState = STATE.None;
    float stateEntryTime = 0;
    [SerializeField] float speed = 0;
    public bool isAlive { get; set; }
    public int damage { get; set; }
    bool isActive = false;
    int health = 2;
    [SerializeField] float walkX;
    [SerializeField] float walkY;

    enum STATE
    {
        None,
        Idle,
        Walk,
        Attack
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        isAlive = true;
        damage = 1;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        anim.Play("Born");
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;

        if (isActive)
        { 
            if (target)
            {
                if (target.transform.position.x > transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }

            switch(currentState)
            {
                case STATE.Idle:
                    if(Time.time > stateEntryTime + 3)
                    {
                        if(target)
                        {
                            targetPosition = target.transform.position;
                            nextState = STATE.Attack;
                            anim.Play("Attack");
                        }
                        else
                        {
                            DecideWalkPosition();
                            nextState = STATE.Walk;
                            anim.Play("Walk");
                        }
                    }
                    break;
                case STATE.Walk:

                    transform.position = Vector2.MoveTowards(transform.position, new Vector3(walkX, walkY), speed * Time.fixedDeltaTime);
                    //transform.Translate((new Vector3(walkX, walkY) - transform.position) * speed * Time.fixedDeltaTime);
                    if(Vector3.Distance(new Vector3(walkX, walkY), transform.position) < 0.1f)
                    {
                        nextState = STATE.Idle;
                        anim.Play("Idle");
                    }
                    break;
            }
        }
        if (currentState != nextState)
        {
            stateEntryTime = Time.time;
            currentState = nextState;
        }
    }

    void MakeWaspActive()
    {
        nextState = STATE.Idle;
        anim.Play("Idle");
        isActive = true;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            boxCollider.enabled = false;
            anim.Play("Die");
            isAlive = false;
            audioSource.PlayOneShot(deathSound, 1);
        }
        else
        {
            audioSource.Stop();
            audioSource.PlayOneShot(hitSound, 1);
            anim.Play("Hit");
        }
    }

    void DecideWalkPosition()
    {
        walkX = Random.Range(transform.position.x - 1.5f, transform.position.x + 1.5f);
        walkY = Random.Range(transform.position.y - 1.5f, transform.position.y + 1.5f);
    }

    void Attack()
    {
        audioSource.PlayOneShot(attackSound, 1);
        GameObject stingInstance = Instantiate(sting, transform.GetChild(0).position, Quaternion.identity) as GameObject;
        Sting stingScript = stingInstance.GetComponent<Sting>();
        stingScript.target = targetPosition;
        Vector3 moveDirection = targetPosition - stingScript.transform.position;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        stingScript.direction = targetPosition - stingScript.transform.position;
        stingScript.transform.rotation = Quaternion.AngleAxis(angle + 135, Vector3.forward);
        nextState = STATE.Idle;
        anim.Play("Idle");
    }

    public void ResetAnimation()
    {
        if (isAlive)
            anim.Play(nextState.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        nextState = STATE.Idle;
        anim.Play("Idle");
    }

    void DestroyWasp()
    {
        hive.GetComponent<Hive>().waspCount--;
        Destroy(gameObject);
    }
}
