using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSpot : MonoBehaviour
{
    PlayerToken playerToken = null;
    public string scene = "";
    [SerializeField] bool isEnd = false;
    public bool isActive = true;

    [SerializeField] GameObject rightSpot = null;
    [SerializeField] GameObject upSpot = null;
    [SerializeField] GameObject leftSpot = null;
    [SerializeField] GameObject downSpot = null;

    public GameObject nextSpot = null;

    private void Update()
    {
        if (playerToken)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && rightSpot)
            {
                if (rightSpot.GetComponent<StageSpot>().isActive)
                {
                    playerToken.firstMove = false;
                    playerToken.destiny = rightSpot.transform.position;
                    nextSpot = rightSpot;
                }

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && upSpot)
            {
                if (upSpot.GetComponent<StageSpot>().isActive)
                {
                    playerToken.firstMove = false;
                    playerToken.destiny = upSpot.transform.position;
                    nextSpot = upSpot;
                }

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && leftSpot)
            {
                if (leftSpot.GetComponent<StageSpot>().isActive)
                {
                    playerToken.firstMove = false;
                    playerToken.destiny = leftSpot.transform.position;
                    nextSpot = leftSpot;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && downSpot)
            {
                if (downSpot.GetComponent<StageSpot>().isActive)
                {
                    playerToken.firstMove = false;
                    playerToken.destiny = downSpot.transform.position;
                    nextSpot = downSpot;
                }
            }

            if (Input.GetKeyDown(KeyCode.X) && !playerToken.firstMove)
            {
                if (scene != "" && scene != "empty")
                {
                    SceneManager.LoadScene(scene);
                }
            }
        }
        
    }

    void DefineCurveSpot(GameObject spot)
    {
        StageSpot curveSpot = spot.GetComponent<StageSpot>();

        if(curveSpot.rightSpot && curveSpot.rightSpot != gameObject)
        {
            curveSpot.nextSpot = curveSpot.rightSpot;
        }
        else if (curveSpot.upSpot && curveSpot.upSpot != gameObject)
        {
            curveSpot.nextSpot = curveSpot.upSpot;
        }
        else if (curveSpot.leftSpot && curveSpot.leftSpot != gameObject)
        {
            curveSpot.nextSpot = curveSpot.leftSpot;
        }
        else if (curveSpot.downSpot && curveSpot.downSpot != gameObject)
        {
            curveSpot.nextSpot = curveSpot.downSpot;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerToken = collision.gameObject.GetComponent<PlayerToken>();
            playerToken.spot = gameObject;

            if (isEnd && !playerToken.firstMove)
            {
                SceneManager.LoadScene(scene);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DefineCurveSpot(nextSpot);
            playerToken.spot = null;
            playerToken = null;
        }
    }
}
