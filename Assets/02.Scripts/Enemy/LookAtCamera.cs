using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform CanvasTr;
    private Transform CamTr;
    void Start()
    {
        CanvasTr = GetComponent<Transform>();
        CamTr = Camera.main.transform;
    }

    void Update()
    {
        CanvasTr.LookAt(CamTr.transform);
        //캔버스가 메인카메라를 쳐다본다.
    }
}
