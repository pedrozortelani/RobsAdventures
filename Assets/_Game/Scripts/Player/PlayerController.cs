using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;
    public Animator anim;
    [SerializeField] GameObject UIController = null;
    public UIController uiScript;

    //Audio Mechanic
    public AudioSource audioSource;
    public AudioClip celebrateSound = null;
    [SerializeField] AudioClip deathSound = null;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip jumpSound = null;
    public AudioClip onVineSound = null;
    public AudioClip runSound = null;
    public AudioClip throwSound = null;
    float runAudioTimer = 0;

    //Movement Mechanic
    [SerializeField] float speed = 0;
    Vector2 direction;
    [SerializeField] float jumpForce = 0;
    bool shouldJump = false;
    public bool canJump = true;
    public int onVine = 0;
    [SerializeField] LayerMask groundLayerMask = new LayerMask();
    [SerializeField] LayerMask enemyLayerMask = new LayerMask();
    STATE currentState;
    public STATE nextState;
    public Component interactionObject = null;

    //Player Stats
    public bool isAlive = true;
    public int lives = 5;
    public int health = 3;
    public int marbles = 0;
    public int checkpoint = 0;
    float lastHit = -3;
    public GameObject powerup = null;

    public enum STATE
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dead
    }

    void Start()
    {
        currentState = STATE.Idle;
        nextState = STATE.Idle;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        uiScript = UIController.GetComponent<UIController>();

        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        { 
            lives = data.lives;
            if(lives == 0)
            {
                lives = 5;
            }
            marbles = data.marbles;

            if(data.checkpoint == 1)
            {
                transform.position = GameObject.Find("CheckPoint").transform.position;
            }
        }
        uiScript.AtualizeLives();
        uiScript.AtualizeMarbles();
    }

    private void Update()
    {
        UpdateState();

        if (isAlive)
        {
            UpdateDirection();

            if (Input.GetKeyDown(KeyCode.X) && canJump)
            {
                shouldJump = true;
            }
           
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (interactionObject)
                {
                    interactionObject.GetComponent<IInteractable>().Interact();
                }
                else if (powerup)
                {
                    powerup.GetComponent<IPowerUp>().UsePowerUP(gameObject.GetComponent<PlayerController>());
                }
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                if (powerup)
                {
                    powerup.GetComponent<IPowerUp>().DeactivatePowerUP(gameObject.GetComponent<PlayerController>());
                }
            }

            if (nextState != currentState)
            {
                anim.Play(nextState.ToString());
                currentState = nextState;
            }
               
        }
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            if (onVine != 0)
            {
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -1, 0));
                }
            }

            Move();

            if(shouldJump)
            {
                shouldJump = false;
                Jump();
            }
            
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y);
        }
    }

    void UpdateDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        direction = new Vector2(x, 0);

        if(direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if(direction.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    
    void UpdateState()
    {
        if (isAlive)
        {

            if(rb.velocity.y < 0.01f && rb.velocity.y > -0.01f)
            {
                if (direction.x != 0)
                {
                    nextState = STATE.Walk;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = runSound;
                        audioSource.time = runAudioTimer;
                        audioSource.Play();
                    }
                }
                else
                {
                    nextState = STATE.Idle;
                }
            }
            else
            {
                if(onVine == 0)
                {
                    canJump = false;
                }
                
                if (rb.velocity.y < -1)
                {
                    nextState = STATE.Fall;
                }
                else
                {
                    nextState = STATE.Jump;
                }
            }
        }
        else
        {
            nextState = STATE.Dead;
        }

        if (nextState != STATE.Walk)
        {
            if(audioSource.clip == runSound)
            {
                runAudioTimer = audioSource.time;
                audioSource.clip = null;
            }
        }
        
    }

    void Move()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
       
        if(rb.velocity.y < -3)
        {
            rb.velocity += new Vector2(direction.x * speed, 0) * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity += new Vector2(direction.x * speed, rb.velocity.y) * Time.fixedDeltaTime;
        }
    }

    void Jump()
    {
        audioSource.PlayOneShot(jumpSound, audioSource.volume);
        canJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void ResetAnimation()
    {
        if(isAlive)
            anim.Play(nextState.ToString());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!canJump)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {

                RaycastHit2D raycastHitRight = Physics2D.Raycast(new Vector3(boxCollider.bounds.max.x - 0.01f, boxCollider.bounds.max.y), Vector3.down, 2 * boxCollider.bounds.extents.y + 0.1f, groundLayerMask);
                RaycastHit2D raycastHitLeft = Physics2D.Raycast(new Vector3(boxCollider.bounds.min.x + 0.01f, boxCollider.bounds.max.y), Vector3.down, 2 * boxCollider.bounds.extents.y + 0.1f, groundLayerMask);

                if (raycastHitRight.collider != null || raycastHitLeft.collider != null)
                {
                    canJump = true;
                    nextState = STATE.Idle;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.queriesHitTriggers = false;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, boxCollider.bounds.extents.y + 0.1f, enemyLayerMask);
            Physics2D.queriesHitTriggers = true;

            if (raycastHit.collider != null)
            {
                IEnemy enemyInstance = collision.gameObject.GetComponent<IEnemy>();
                enemyInstance.TakeDamage(1);
                Jump();
            }
            else
            {
                IEnemy enemyInstance = collision.gameObject.GetComponent<IEnemy>();
                if (enemyInstance.isAlive)
                {
                    TakeDamage(enemyInstance.damage);
                }
                
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        if(isAlive)
        {
            if (Time.time > lastHit + 3)
            {
                lastHit = Time.time;

                if (powerup)
                {
                    powerup.GetComponent<IPowerUp>().LosePowerUP(gameObject.GetComponent<PlayerController>());
                }

                health -= dmg;
                uiScript.AtualizeHealth();

                if (health <= 0)
                {
                    uiScript.AtualizeLives();
                    Die("normal");
                }
                else
                {
                    audioSource.PlayOneShot(hitSound, audioSource.volume);
                    anim.Play("Hit");
                }
            }
        }
    }

    public void PickMarble()
    {
        marbles++;
        if (marbles == 50)
        {
            lives++;
            marbles = 0;
            uiScript.AtualizeLives();
        }
        uiScript.AtualizeMarbles();
    }

    public void Die(string type)
    {
        lives--;
        SaveSystem.SavePlayer(this);
        nextState = STATE.Dead;
        isAlive = false;

        switch (type)
        {
            case "normal":
                audioSource.PlayOneShot(deathSound, audioSource.volume);
                anim.Play("Die");
                break;
            case "time":
                break;
        }

        if (lives > 0)
        {
            Invoke("ReloadScene", 2);
        }
        else
        {
            //implement gameover;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
