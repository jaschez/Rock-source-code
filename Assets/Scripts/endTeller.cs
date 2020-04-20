using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endTeller : MonoBehaviour
{
    public Text teller;

    [TextArea]
    public string[] text;

    int index = 0;
    int stringIndex = 0;

    float timeBetweenWords;

    public float speed = 3;

    public AudioClip talk;

    GameObject camObj;

    bool resume = false;

    int level, nuggets;

    public string finalPhrase;
    string phrase;

    void Start()
    {
        timeBetweenWords = Time.time;
        camObj = GameObject.FindGameObjectWithTag("MainCamera");
        phrase = text[index];
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow)) && !resume)
        {
            stringIndex = 0;
            teller.text = text[0] + "\n\n";
            phrase = finalPhrase;
            resume = true;
        }

        if (stringIndex < phrase.Length)
        {
            char letter = phrase.ToCharArray()[stringIndex]; 

            if (letter == ',' || letter == '.')
            {
                teller.text += letter;
                timeBetweenWords = Time.time + 1f / (speed / 8);
                stringIndex++;
            }

            if (timeBetweenWords < Time.time)
            {
                if (letter != ' ')
                {
                    AudioSource.PlayClipAtPoint(talk, camObj.transform.position);
                }

                teller.text += phrase.ToCharArray()[stringIndex];

                stringIndex++;
                timeBetweenWords = Time.time + 1f / speed;
            }
        }
        else if(!resume)
        {
            teller.text += "\n\n";
            phrase = finalPhrase;
            resume = true;
            stringIndex = 0;
        }
    }
}
