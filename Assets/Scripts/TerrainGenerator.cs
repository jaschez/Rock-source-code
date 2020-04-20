using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    [HideInInspector]
    public float speed = 5;

    public float generationSpeed = 5;
    public bool spawn = true;

    public GameObject[] obstaclePrefabs;
    public GameObject goldPrefab;

    Material terrainMat, boundsMat;

    Vector2 terrainPos;

    GameObject[] obstaclesPos;

    public GameObject nextLevelObj;

    float obstacleSpawnDelay;

    void Start()
    {
        obstaclesPos = GameObject.FindGameObjectsWithTag("obstaclesPos");

        terrainMat = GetComponent<MeshRenderer>().sharedMaterial;
        boundsMat = transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;

        terrainPos = Vector2.zero;

        speed = 0.7f;
    }

    void Update()
    {
        terrainPos = new Vector2(0,Time.time*speed* terrainMat.GetTextureScale("_MainTex").y);
        terrainMat.SetTextureOffset("_MainTex", terrainPos);
        boundsMat.SetTextureOffset("_MainTex", terrainPos);

        //EVERY SOME TIME A NEW OBSTACLE IS SPAWNED
        if (obstacleSpawnDelay < Time.time && spawn)
        {
            SpawnObstacle();

            obstacleSpawnDelay = Time.time + Random.Range(.5f,2)*.7f/generationSpeed;
        }
    }

    void SpawnObstacle()
    {
        float probability = Random.value;
        int posIndex = Random.Range(0, obstaclesPos.Length);
        Quaternion objectRotation = Quaternion.Euler(90 - transform.rotation.eulerAngles.x, 0, 0);

        if (probability > .1f) {
            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);

            GameObject newObstacle = Instantiate(obstaclePrefabs[obstacleIndex], obstaclesPos[posIndex].transform.position, objectRotation);
        }
        else
        {
            GameObject gold = Instantiate(goldPrefab, obstaclesPos[posIndex].transform.position, objectRotation);
        }
    }
}
