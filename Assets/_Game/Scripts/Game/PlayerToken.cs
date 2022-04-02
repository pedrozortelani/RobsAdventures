using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{
    public Vector3 destiny;
    public float speed = 0;
    public GameObject spot = null;
    public bool firstMove = true;
    Animator anim;
    SpriteRenderer sprite;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        destiny = transform.position;
    }

    private void Update()
    {
        if(destiny.x < transform.position.x)
        {
            sprite.flipX = true;
        }
        else if (destiny.x > transform.position.x)
        {
            sprite.flipX = false;
        }

        float step = speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, destiny, step);

        if(Vector3.Distance(transform.position, destiny) != 0)
        {
            anim.Play("TokenWalk");
        }
        else
        {
            anim.Play("TokenIdle");
        }

        if(spot)
        {
            if(spot.GetComponent<StageSpot>().scene.Length == 0)
            {
                destiny = spot.GetComponent<StageSpot>().nextSpot.transform.position;
            }
            
        }
    }

}
