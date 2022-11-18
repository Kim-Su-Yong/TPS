using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayserBeam : MonoBehaviour
{
    private Transform tr;
    [SerializeField]
    private LineRenderer line;
    RaycastHit hit; //광선에 충돌한 게임오브젝트의 맞은 위치 정보를 가져올 변수
    void Start()
    {
        tr = this.transform;
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false; //로컬좌표로
        line.enabled = false; //초기에 비활성화
        //line.SetWidth(0.3f, 0.01f); //라인의 시작폭과 종료폭 설정
    }


    void Update()
    {
        Ray ray = new Ray(tr.position + (Vector3.up * 0.02f), tr.forward);
        //광선 위치와 방향을 설정
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {   
            //라인 렌더러의 첫번째 점의 위치
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin)); 
                                    //월드좌표를 로컬좌표로 변환
            //어떤 물체에 맞았을 때의 위치를 끝점으로 설정
            if(Physics.Raycast(ray,out hit, 100f))
            {
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else //맞지 않았을 때는 끝점을 100으로 잡는다.
            {
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));
            }
            StartCoroutine(ShowLaserBeam());
        }
    }
    IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        line.enabled = false;
    }
}
