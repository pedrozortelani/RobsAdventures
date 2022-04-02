using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    bool isSpawning = false;
    float startSpawnTimer = 0;
    float spawnTimer = 0;

    Animator anim;
    [SerializeField] GameObject wasp = null;
    public int waspCount = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isActive)
        {
            if (!isSpawning)
            { 
                if ((Time.time > startSpawnTimer + 4 && waspCount < 3) || waspCount == 0)
                {
                    anim.Play("Spawn");
                    spawnTimer = Time.time;
                    isSpawning = true;
                }
            }

            if (isSpawning)
            {
                if(Time.time > spawnTimer + 3)
                {
                    if(waspCount == 0)
                    {
                        Instantiate(wasp, new Vector3(transform.position.x, transform.position.y - 0.8f), Quaternion.identity);
                    }
                    else if (waspCount == 1)
                    {
                        Instantiate(wasp, new Vector3(transform.position.x - 0.7f, transform.position.y - 0.4f), Quaternion.identity);
                    }
                    else if (waspCount == 2)
                    {
                        Instantiate(wasp, new Vector3(transform.position.x + 0.7f, transform.position.y - 0.4f), Quaternion.identity);
                    }
                    waspCount++;
                    startSpawnTimer = Time.time;
                    isSpawning = false;
                    anim.Play("Idle");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isActive = false;
        }
    }
}
