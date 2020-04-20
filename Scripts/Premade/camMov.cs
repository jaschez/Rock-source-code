using UnityEngine;
using System.Collections;

public class camMov : MonoBehaviour {

    bool shake = false,shakeLerp=false;
    float shakeTime = 0;
    float shakeAmount=0;

    public float currentFOV = 120;

    Vector3 originalCamPos;
    Vector3 distanceToPlayer;
    Vector3 toTargetPos;
    Vector3 originalObjPos;

    Quaternion originalRotation;

    GameObject target;
    GameObject camObj;

    Camera cam;

    float lastTargetPos = 0;
    float deltaX = 0;
    float sensibility = 5;

    public bool pause = false;

	void Start () {

        camObj = transform.GetChild(0).gameObject;
        cam = camObj.GetComponent<Camera>();

        originalCamPos = camObj.transform.localPosition;
        originalRotation = camObj.transform.rotation;

        toTargetPos = transform.position;
        originalObjPos = transform.position;

        target = GameObject.FindGameObjectWithTag("player");
        distanceToPlayer = target.transform.position - transform.position;
    }
	
	void Update () {
        lastTargetPos = toTargetPos.x;

        //PLAYER FOV ADJUSTMENT
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentFOV + Random.Range(-.2f, .2f), Time.deltaTime*3);


        //PLAYER FOLLOWING
        toTargetPos.x = Mathf.Lerp(toTargetPos.x, target.transform.position.x - distanceToPlayer.x, Time.deltaTime * 10);
        toTargetPos.y = originalObjPos.y;

        deltaX = toTargetPos.x - lastTargetPos;

        if (!pause)
        {
            transform.position = new Vector3(toTargetPos.x, toTargetPos.y, toTargetPos.z);
        }

        //CAMERA ROTATION
        if (!pause)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, deltaX * sensibility + Random.Range(-.3f, .3f));
        }

        //CAM SHAKING CALCULATIONS
        if (shake)
        {
            shakeTime = Time.time + 0.5f;
            shake = false;
            shakeLerp = true;

            float deltaX = Random.Range(-shakeAmount,shakeAmount);
            float deltaY = Random.Range(-shakeAmount, shakeAmount);

            camObj.transform.position += new Vector3(deltaX,deltaY,0);
        }

        if (Time.time < shakeTime && shakeLerp)
        {
            camObj.transform.localPosition = Vector3.Lerp(camObj.transform.localPosition,originalCamPos,Time.deltaTime*5);
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
            amt = Mathf.Lerp(amt,0,Time.deltaTime*30/duration);
            camObj.transform.Rotate(0, 0, Random.Range(-amt, amt),Space.Self);

            if (end) {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, origSize + 1f, Time.deltaTime*10);
            }

            Shake(amt);
            yield return new WaitForSeconds(.04f);

        }
    }

    public void ShakeQuake(float amt, float duration, bool end)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(amt,duration, end));
    }
}
