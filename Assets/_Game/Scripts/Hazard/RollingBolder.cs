using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBolder : MonoBehaviour
{

    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    AudioSource audioSource;
    [SerializeField] AudioClip hitGroundSound = null;

    int rotationSpeed = -5;
    Vector2 direction = Vector2.right;
    [SerializeField] float speed = 0;
    bool isMoving = false;
    bool isFalling = false;
    Vector2 lastPosition = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isFalling)
        { 
            rb.AddForce(Physics.gravity * (rb.mass * rb.mass));
        }

        if (isMoving)
        {
            if (transform.position.x == lastPosition.x)
            {
                rb.bodyType = RigidbodyType2D.Static;
                isMoving = false;
            }

            lastPosition = transform.position;
            transform.Rotate(0, 0, rotationSpeed);
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isFalling = true;
        }
     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isMoving)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(3);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            if(rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
            {
                audioSource.PlayOneShot(hitGroundSound, 0.8f);
            }
            
            isFalling = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            isMoving = true;
        }
    }
}
