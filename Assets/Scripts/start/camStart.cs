using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camStart : MonoBehaviour
{

    bool shake = false, shakeLerp = false;
    float shakeTime = 0;
    float shakeAmount = 0;
    float orSize;

    Vector3 originalCamPos;

    GameObject camObj;
    Quaternion originalRotation;

    Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        camObj = transform.GetChild(0).gameObject;
        cam = camObj.GetComponent<Camera>();

        originalCamPos = camObj.transform.localPosition;
        originalRotation = camObj.transform.rotation;

        orSize = cam.orthographicSize;
    }

    void Update()
    {
 
        //CAM SHAKING CALCULATIONS
        if (shake)
        {
            shakeTime = Time.time + 0.5f;
            shake = false;
            shakeLerp = true;

            float deltaX = Random.Range(-shakeAmount, shakeAmount);
            float deltaY = Random.Range(-shakeAmount, shakeAmount);

            camObj.transform.position += new Vector3(deltaX, deltaY, 0);
        }

        if (Time.time < shakeTime && shakeLerp)
        {
            camObj.transform.localPosition = Vector3.Lerp(camObj.transform.localPosition, originalCamPos, Time.deltaTime * 5);
            camObj.transform.localRotation = Quaternion.Lerp(camObj.transform.localRotation, originalRotation, Time.deltaTime);
        }
        else
        {
            shakeLerp = false;
        }
    }

    public void Shake(float amt)
    {
        camObj.transform.localPosition = originalCamPos;
        shakeAmount = amt;
        shake = true;
    }

    IEnumerator Shake(float amount, float duration, bool end)
    {
        float endTime = Time.time + duration;
        float amt = amount;

        float origSize = cam.orthographicSize;

        while (Time.time < endTime)
        {
            amt = Mathf.Lerp(amt, 0, Time.deltaTime * 30 / duration);
            //camObj.transform.Rotate(0, 0, Random.Range(-amt, amt), Space.Self);

            if (end)
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, orSize + 1f, Time.deltaTime*10);
            }

            Shake(amt);
            yield return new WaitForSeconds(.04f);

        }
    }

    public void ShakeQuake(float amt, float duration, bool end)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(amt, duration, end));
    }
}
