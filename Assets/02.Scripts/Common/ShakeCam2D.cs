using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam2D : MonoBehaviour
{
    private Vector3 PosCam;
    private Quaternion RotCam;
    private Transform camTr;
    public bool isShake = false;
    private float timePrev; //시간지정
    private float duration = 0.5f; //몇초간 흔들 시간
    void Start()
    {
        camTr = GetComponent<Transform>();
        
    }

    void Update()
    {
        if(isShake == true)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.05f, 0.5f);
            Camera.main.transform.position += new Vector3(x, y, 0f);
            Camera.main.transform.localEulerAngles += new Vector3(x, y, 0f);
            if(Time.time - timePrev > duration)
            {
                isShake = false;
                Camera.main.transform.position = PosCam;
                Camera.main.transform.rotation = RotCam;
            }
        }
    }
    public void TurnOn()
    {
        if (isShake == false)
            isShake = true;
        timePrev = Time.time;
        PosCam = camTr.localPosition;
        RotCam = camTr.localRotation;
    }
}