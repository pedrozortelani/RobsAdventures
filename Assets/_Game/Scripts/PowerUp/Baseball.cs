using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    Animator anim;
    CircleCollider2D circleCollider;
    AudioSource audioSource;
    [SerializeField] AudioClip bounceSound = null;
    Rigidbody2D rb;
    public BaseballPowerUp powerup;

    [SerializeField] LayerMask groundLayerMask = new LayerMask();
    [SerializeField] float speed = 0;
    [SerializeField] float bounces = 0;
    bool isActive = true;
    [SerializeField] float destroyTime = 0;
    float startTime;
    public Vector3 direction;

    private void Start()
    {
        startTime = Time.time;
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            if(Time.time > destroyTime + startTime)
            {
                anim.Play("BaseballOut");
            }
            transform.Translate(direction * speed * Time.fixedDeltaTime);
        }       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            audioSource.PlayOneShot(bounceSound, audioSource.volume);
            bounces += 1;

            if(bounces < 4)
            {

                RaycastHit2D raycastHitDown = Physics2D.Raycast(circleCollider.bounds.center, Vector2.down, circleCollider.bounds.extents.y + 0.01f, groundLayerMask);
                RaycastHit2D raycastHitUp = Physics2D.Raycast(circleCollider.bounds.center, Vector2.up, circleCollider.bounds.extents.y + 0.01f, groundLayerMask);
                if (raycastHitDown.collider != null)
                {
                    rb.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                }
                else if (raycastHitUp.collider == null)
                {
                    if (direction == Vector3.left)
                    {
                        direction = Vector3.right;
                    }
                    else
                    {
                        direction = Vector3.left;
                    }
                }

            }
            else{
                isActive = false;
                anim.Play("BaseballOut");
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IEnemy>().TakeDamage(1);
            isActive = false;
            anim.Play("BaseballOut");
        }
    }

    void DestroyObject()
    {
        powerup.ballCount--;
        Destroy(gameObject);
    }
}
