using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] float speed = 0;
    public Vector3 direction = new Vector3();
    Animator anim;
    bool isActive = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isActive)
            transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isActive = false;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }

        anim.Play("CannonBallOut");
    }

    public void Shoot()
    {
        print("Shoot");
    }

    public void DestroyCannonBall()
    {
        Destroy(gameObject);
    }
}
