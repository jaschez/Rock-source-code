using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    GameObject obstacleRef;
    GameObject managerObj;

    GameManager gameManager;

    float speed = 40;

    void Start()
    {
        obstacleRef = GameObject.FindGameObjectWithTag("obsRef");
        managerObj = GameObject.FindGameObjectWithTag("manager");

        gameManager = managerObj.GetComponent<GameManager>();
    }

    void Update()
    {
        speed = gameManager.GetSpeed()*8;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, obstacleRef.transform.position.y, Time.deltaTime*20), transform.position.z);
        transform.Translate(-transform.forward * speed * Time.deltaTime ,Space.Self);    
    }
}
