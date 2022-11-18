using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform shakeCamera; //쉐이크 효과를 줄 카메라의 Transform 변수
    public bool shakeRotate = false; //카메라를 회전할 것인지 판단 변수
    private Vector3 originPos; //초기 위치값 저장
    private Quaternion originRot; //초기 회전값 저장
    void Start()
    {
        shakeCamera = GetComponent<Transform>();
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
    }
    public IEnumerator ShakeCamera(float duration = 0.05f, float magnitude = 0.03f, float magnitudeRot = 0.1f)
    {
        float passTime = 0.0f;
        while (passTime < duration)
        {   //불규칙한 위치를 산출되어 있다. 같은 확률이 덜 나오게 
            Vector3 shakePos = Random.insideUnitSphere;
            //카메라의 위치를 변경
            //insideUnitSphere은 반경이 1인 구체 내부에 3차원 좌표값을 불규칙하게 반환한다.
            shakeCamera.localPosition = shakePos * magnitude;
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0f));
                //펄린 노이즈는 0과 1사이의 난수를 발생 시키지만
                //일반 랜덤 발생기와 다르게 연속성이 있는 랜덤값을 생성
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);

            }
            passTime += Time.deltaTime;
            yield return null; //한프레임 돈다.
        }
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }
}
