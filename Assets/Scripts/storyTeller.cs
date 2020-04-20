using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storyTeller : MonoBehaviour
{
    public Text teller;

    [TextArea]
    public string[] text;

    int index = 0;
    int stringIndex = 0;

    float timeBetweenWords;

    public float speed = 3;

    public AudioClip talk;


    public startManager manager; //EVENTS

    GameObject camObj;

    void Start()
    {
        timeBetweenWords = Time.time;
        camObj = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow)) && stringIndex >= text[index].Length)
        {
            if (index < text.Length - 1)
            {
                teller.text = "";
                stringIndex = 0;
                index++;

                timeBetweenWords = Time.time + 1f / (speed/10);
            }
        }

        if (stringIndex < text[index].Length) {
            char letter = text[index].ToCharArray()[stringIndex];

            if (letter == '~')
            {

                manager.Event();

                timeBetweenWords = Time.time + 5f;
                stringIndex++;
            }

            if (letter == ',')
            {
                teller.text += ',';
                timeBetweenWords = Time.time + 1f / (speed / 8);
                stringIndex++;
            }

            if (letter != '\n')
            {
                if (timeBetweenWords < Time.time)
                {
                    if (letter != ' ')
                    {
                        AudioSource.PlayClipAtPoint(talk,camObj.transform.position);
                    }

                    teller.text += text[index].ToCharArray()[stringIndex];

                    stringIndex++;
                    timeBetweenWords = Time.time + 1f / speed;

                }
            } else {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    teller.text += '\n';
                    stringIndex++;
                }
            }
        }
    }
}
