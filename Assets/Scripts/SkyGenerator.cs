using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGenerator : MonoBehaviour
{
    
    [HideInInspector]
    public float speed = .1f;

    Material skyMat;

    Vector2 skyPos;

    void Start()
    {
        skyMat = GetComponent<MeshRenderer>().sharedMaterial;

        skyPos = Vector2.zero;
    }

    void Update()
    {
        skyPos = new Vector2(0, -Time.time * speed);
        skyMat.SetTextureOffset("_MainTex", skyPos);
    }
}
