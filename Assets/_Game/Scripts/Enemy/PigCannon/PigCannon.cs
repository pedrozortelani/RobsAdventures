using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigCannon : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject cannonBall = null;
    Animator anim;
    BoxCollider2D boxCollider;
    AudioSource audioSource;
    [SerializeField] AudioClip attackSound = null;
    [SerializeField] AudioClip hitSound = null;
    [SerializeField] AudioClip deathSound = null;

    public int damage { get; set; }
    public bool isAlive { get; set; }
    float reloadTime = 0;
    int health = 3;

    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        damage = 1;
        isAlive = true;
    }

    private void Update()
    {
        if (isAlive)
        {
            if (isAlive)
            {
                if (Time.time > reloadTime + 3)
                {
                    anim.Play("Attack");
                }
            }
        }
    }

    public void Shoot()
    {
        audioSource.PlayOneShot(attackSound, audioSource.volume);

        if (transform.rotation.y == 0)
        {
            GameObject cannonBallInstance = Instantiate(cannonBall, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.15f, 0), Quaternion.identity);
            cannonBallInstance.GetComponent<CannonBall>().direction = Vector3.right;
        }
        else
        {
            GameObject cannonBallInstance = Instantiate(cannonBall, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.15f, 0), Quaternion.identity);
            cannonBallInstance.GetComponent<CannonBall>().direction = Vector3.left;
        }
        
    }

    public void Reload()
    {
        reloadTime = Time.time;
        anim.Play("Idle");
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            audioSource.PlayOneShot(deathSound, audioSource.volume);
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
        {
            anim.Play("Idle");
        }
    }
}
