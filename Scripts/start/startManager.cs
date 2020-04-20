using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startManager : MonoBehaviour
{

    public camStart camScript;
    public GameObject logo, text, fadeWhite;

    public Animator fadeCts;
    public Animator camAnim;

    bool input = false;
    bool changeScene = false;

    public Rigidbody2D rock;
    public FAdeWite fade;
    public storyTeller teller;

    public AudioClip start,quake, jump;

    void Start()
    {
        Invoke("Shake",.3f);
        Invoke("inputIntro", 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (input)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = false;
                camAnim.SetTrigger("click");
                Invoke("StartTeller", 2.3f);
            }
        }

        if (changeScene)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                fade.fadeTime = .3f;
                fade.FadeOut();
                changeScene = false;
                Invoke("gameScene", 1f);
            }
        }
    }

    void Shake()
    {
        AudioSource.PlayClipAtPoint(start, camScript.gameObject.transform.position);
        camScript.gameObject.GetComponent<Animator>().enabled = true; 
        text.SetActive(true);
        logo.SetActive(true);
        camScript.ShakeQuake(2,1,true);
    }

    public void Event()
    {
        AudioSource.PlayClipAtPoint(quake, camScript.gameObject.transform.position);
        camScript.ShakeQuake(.1f, 2,false);
        Invoke("Impulse",3f);
    }

    void inputIntro()
    {
        fadeCts.SetTrigger("fade");
        input = true;
    }

    void inputScene()
    {
        changeScene = true;
    }

    void gameScene()
    {
        SceneManager.LoadScene(1);
    }

    void Impulse()
    {
        AudioSource.PlayClipAtPoint(jump, camScript.gameObject.transform.position);
        rock.AddForce(new Vector2(.5f,.5f),ForceMode2D.Impulse);
        rock.AddTorque(2);
        Invoke("inputScene", .8f);
    }

    void StartTeller()
    {
        teller.enabled = true;
    }
}
