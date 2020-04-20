using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject dirtPart, nuggetPart;
    public ParticleSystem speedPart;
    public GameObject particlePos;
    public AudioClip jump, dash, score, hitBound, ground;

    public float gravity = .06f;
    public float resistance = .03f;
    public float jumpHeight = .1f;
    public float torqueSpd = 150;

    public SpriteRenderer sprite;

    int gold = 0;

    float jumpForce = 0;
    float height = 1;
    float velY;
    float xMousePos;
    float xPosition;
    float globalGravity = -9.81f;
    float gravityScale = 6;

    int positionIndex = 1;

    bool changeHop = true;
    bool canJump = true;
    bool control = true;
    public bool pause = false;

    public GameObject[] positions;
    GameObject camObj;
    GameObject managerObj;

    Animator anim;

    Vector3 playerPos, originalPos;

    camMov camManager;
    GameManager gameManager;
    Rigidbody rb;

    

    void Start()
    {
        playerPos = transform.position;
        originalPos = transform.position;

        camObj = GameObject.FindGameObjectWithTag("MainCamera");
        managerObj = GameObject.FindGameObjectWithTag("manager");

        camManager = camObj.GetComponent<camMov>();
        gameManager = managerObj.GetComponent<GameManager>();

        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration); 
    }

    void Update()
    {
        //PLAYER CONTROL
        //PLAYER X AXIS TRASLATES TO NEAREST POINT ON THE WAY
        xPosition = Mathf.Lerp(xPosition, positions[positionIndex].transform.position.x,Time.deltaTime*10);

        if (control)
        {
            //PLAYER JUMP
            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.UpArrow)) && canJump)
            {
                Jump();
            }

            //ALTERNATIVES TO MOUSE CONTROL
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                DashLeft();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                DashRight();
            }

            if (Input.GetMouseButtonDown(0))    //IF PLAYER CLICKS IT WILL MOVE TO ONE OF THE SIDES
            {
                xMousePos = Input.mousePosition.x;

                if (xMousePos > Screen.width / 2)
                {
                    DashRight();    //TRASLATES TO NEAREST POINT TO THE RIGHT
                }
                else
                {
                    DashLeft();     //TRASLATES TO NEAREST POINT TO THE LEFT
                }
            }
        }

        //JUMP CALCULATIONS

        if (!pause)
        {

            if (canJump)
            {
                velY -= gravity;
                velY += jumpForce;
                height += velY;
            }

            if (jumpForce > 0)
            {
                jumpForce -= resistance;
            }
            else
            {
                jumpForce = 0;
            }

            if (height < 0 && changeHop)
            {
                height = 0;
                jumpHeight = Random.Range(0.06f, 0.09f);
                torqueSpd = Random.Range(500, 1200);
                velY = 0;
                jumpForce += jumpHeight;

                changeHop = false;
                canJump = true;
            }
            else if (height > 0)
            {
                changeHop = true;
            }

            playerPos = new Vector3(xPosition, transform.position.y, transform.position.z);

            transform.position = playerPos;
            transform.GetChild(0).transform.Rotate(0, 0, torqueSpd * Time.deltaTime, Space.Self);
            transform.GetChild(0).transform.localPosition = new Vector3(0, height, 0);
        }

        sprite.color = Color.Lerp(sprite.color, Color.white, Time.deltaTime*5);
    }

    void DashRight()
    {
        Instantiate(dirtPart,transform);

        if (positionIndex < positions.Length-1)
        {
            AudioSource.PlayClipAtPoint(dash, camObj.transform.position);
            positionIndex++;
        }
        else
        {
            AudioSource.PlayClipAtPoint(hitBound, camObj.transform.position);
            camManager.Shake(1.4f);
        }
    }

    void Jump()
    {
        AudioSource.PlayClipAtPoint(jump,camObj.transform.position);

        gameManager.ShakeGround();
        height = 0;
        canJump = false;
        rb.AddForce(Vector2.up * 80, ForceMode.Impulse);
        torqueSpd = 0;

        anim.SetTrigger("jump");
    }

    void DashLeft()
    {
        Instantiate(dirtPart,transform);

        if (positionIndex > 0)
        {
            AudioSource.PlayClipAtPoint(dash, camObj.transform.position);
            positionIndex--;
        }
        else
        {
            AudioSource.PlayClipAtPoint(hitBound, camObj.transform.position);
            camManager.Shake(1.4f);
        }
    }

    void Die()
    {
        canJump = false;
        torqueSpd = 0;
        gameManager.GameOver();
        speedPart.Stop();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gold")
        {
            Destroy(other.gameObject);
            Gain(1);
            Instantiate(nuggetPart, other.transform.position, Quaternion.identity);
            camManager.ShakeQuake(3, 2, false);
            AudioSource.PlayClipAtPoint(score, camObj.transform.position);
            sprite.color = new Color(1, 1, 0);

        } else if (other.tag == "floor") {
            anim.ResetTrigger("jump");
            anim.SetTrigger("jump");
            AudioSource.PlayClipAtPoint(ground, camObj.transform.position);
            camManager.ShakeQuake(2, 1, false);
            canJump = true;
            changeHop = true;
            velY = 0;

            anim.ResetTrigger("jump");
        } else if (other.tag == "obstacle")
        {
            Die();
        }
    }

    public void Gain(int amt) //PLAYER GAINS GOLD
    {
        gold += amt;
        managerObj.GetComponent<GameManager>().startLevel -= 2; //REACHES LEVEL FASTER
    }

    public void Spend(int amt) //PLAYER LOOSES/SPEND GOLD
    {
        if (amt < gold)
        {
            gold -= amt;
        }
    }

    public int GetGold()
    {
        return gold;
    }
}
