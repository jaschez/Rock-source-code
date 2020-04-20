using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text goldText;
    public Slider levelSlider;
    public Text levelText;
    public AudioSource music;
    public GameObject pauseText;

    GameObject terrainObj;
    GameObject skyObj;
    GameObject camObj;
    GameObject playerObj;

    TerrainGenerator terrainGenerator;
    SkyGenerator skyGenerator;

    camMov camManager;
    Player player;

    float speed = 0;

    float levelGoal = 10;
    public float startLevel;
    float currentdistance;

    int levelNumber=1;

    bool pause = false;
    public bool gameOver = false;

    public AudioClip crash, nextLevel;
    public endTeller teller;

    void Start()
    {
        terrainObj = GameObject.FindGameObjectWithTag("terrain");
        skyObj = GameObject.FindGameObjectWithTag("sky");
        camObj = GameObject.FindGameObjectWithTag("MainCamera");
        playerObj = GameObject.FindGameObjectWithTag("player");

        terrainGenerator = terrainObj.GetComponent<TerrainGenerator>();
        skyGenerator = skyObj.GetComponent<SkyGenerator>();
        player = playerObj.GetComponent<Player>();

        camManager = camObj.GetComponent<camMov>();

        setSpeed(5);

        startLevel = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "Level: " + levelNumber;
        goldText.text = "Gold nuggets: " + player.GetGold();

        currentdistance = Time.time - startLevel;

        if (!pause) {
            levelSlider.value = currentdistance / levelGoal;

            if (currentdistance >= levelGoal)
            {
                NextLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Return) && !gameOver)
        {
            pause = !pause;
            pauseText.SetActive(pause);
            camManager.pause = pause;
            player.pause = pause;

            Time.timeScale = (pause?0:1);

            if (pause)
            {
                Time.timeScale = 0;
                music.Pause();
            }
            else
            {
                Time.timeScale = 1;
                music.UnPause();
            }
        }

    }

    void setSpeed(float spd)
    {
        terrainGenerator.speed = spd/2.5f;
        terrainGenerator.generationSpeed = spd/3f;

        skyGenerator.speed = spd*.01f;

        speed = spd/1.1f;
    }

    public void ShakeGround()
    {
        camManager.ShakeQuake(2f, 2f, false);
    }

    public void GameOver()
    {
        Invoke("Tell",.6f);
        gameOver = true;

        AudioSource.PlayClipAtPoint(crash, camObj.transform.position);
        setSpeed(0);
        pause = true;
        camManager.ShakeQuake(2, 2f, false);
        camManager.pause = true;
        terrainGenerator.enabled = false;
        player.enabled = false;
        playerObj.GetComponent<Rigidbody>().useGravity = true;
        camManager.currentFOV = 90;
        music.Stop();
    }

    public float GetSpeed()
    {
        return speed;
    }

    void Tell()
    {
        teller.enabled = true;
        teller.finalPhrase = "Rock collected " + player.GetGold() + " nuggets and reached " + (levelNumber - 1) + " levels.\n";

        if (levelNumber == 1)
        {
            teller.finalPhrase += "The rock felt like his journey was a little too brief.";
        }else if (levelNumber < 2)
        {
            teller.finalPhrase += "The rock was still asking for more bumps.";
        }
        else if(levelNumber < 4)
        {
            teller.finalPhrase += "For a moment, the rock felt pretty alive.";
        }
        else if (levelNumber < 6)
        {
            teller.finalPhrase += "The rock found itself really excited about its journey.";
        }
        else if (levelNumber < 7)
        {
            teller.finalPhrase += "The rock lived the moment of its life.";
        }else if(levelNumber < 9)
        {
            teller.finalPhrase += "Holy cow, now we can say the rock really ROCKS!";
        }else if(levelNumber >= 9)
        {
            teller.finalPhrase += "Okay, I'm certainly sure the rock isn't a rock.";
        }

teller.finalPhrase += "\nPress [R] to restart";
    }

    void NextLevel()
    {
        AudioSource.PlayClipAtPoint(nextLevel, camObj.transform.position);
        camObj.transform.GetChild(0).GetComponent<FAdeWite>().fadeTime = .2f;
        camObj.transform.GetChild(0).GetComponent<FAdeWite>().FadeIn();
        startLevel = Time.time;
        levelNumber++;
        setSpeed(speed + 1);
    }
}
