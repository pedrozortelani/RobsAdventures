using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject Player = null;
    PlayerController playerScript = null;
    PlayerData playerData = null;

    [SerializeField] Text Lives = null;
    [SerializeField] Text Marbles = null;
    [SerializeField] Text Timer = null;

    [SerializeField] Image Health1 = null;
    [SerializeField] Image Health2 = null;
    [SerializeField] Image Health3 = null;

    [SerializeField] Sprite heartOn = null;
    [SerializeField] Sprite heartOff = null;

    [SerializeField] GameObject mainMenuPanel = null;
    [SerializeField] GameObject saveSlotPanel = null;

    [SerializeField] Slider ProgressBar = null;
    [SerializeField] GameObject tilemapObject = null;
    Tilemap stageTilemap;

    [SerializeField] SceneType sceneType;
    float timeLeft = 180.0f;

    enum SceneType
    {
        Stage,
        Map,
        Screen
    }

    private void Start()
    {
        switch(sceneType)
        {
            case SceneType.Stage:
                playerScript = Player.GetComponent<PlayerController>();
                stageTilemap = tilemapObject.GetComponent<Tilemap>();
                break;
            case SceneType.Map:
                playerData = SaveSystem.LoadPlayer();
                InitializeLives();
                MapHUDAtualize();
                break;
        }
       
    }

    private void Update()
    {
        if (sceneType == SceneType.Stage)
        {
            UpdateTimer();
            UpdateProgressBar();
        }

    }

    public void AtualizeHealth()
    {
        if (playerScript.health == 3)
        {
            Health1.sprite = heartOn;
            Health2.sprite = heartOn;
            Health3.sprite = heartOn;
        }
        else if (playerScript.health == 2)
        {
            Health1.sprite = heartOn;
            Health2.sprite = heartOn;
            Health3.sprite = heartOff;
        }
        else if (playerScript.health == 1)
        {
            Health1.sprite = heartOn;
            Health2.sprite = heartOff;
            Health3.sprite = heartOff;
        }
        else
        {
            Health1.sprite = heartOff;
            Health2.sprite = heartOff;
            Health3.sprite = heartOff;
        }
    }

    public void AtualizeMarbles()
    {
        Marbles.text = playerScript.marbles.ToString("00");
    }

    public void AtualizeLives()
    {
        Lives.text = playerScript.lives.ToString("00");
    }

    void InitializeLives()
    {
        if(playerData != null)
        {
            if (playerData.lives == 0)
            {
                playerData.lives = 5;
            }
        }
    }

    void MapHUDAtualize()
    {
        if (playerData != null)
        {
            Lives.text = playerData.lives.ToString("00");
            Marbles.text = playerData.marbles.ToString("00");
        }
    }

    void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;
        Timer.text = Mathf.Floor(timeLeft / 60).ToString("00") + ":" + Mathf.Floor(timeLeft % 60).ToString("00");
    }

    void UpdateProgressBar()
    {
        ProgressBar.value = playerScript.transform.position.x / (stageTilemap.cellBounds.xMax / 2);
    }

    public void NewGame()
    {
        SaveSystem.RestartSaveFiles();
        SceneManager.LoadScene("WorldMap");
    }

    public void ContinueGame()
    {
        mainMenuPanel.SetActive(false);
        saveSlotPanel.SetActive(true);
    }

}
