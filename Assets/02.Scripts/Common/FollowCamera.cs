using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    private Transform tr;
    public float Height = 4.0f;
    public float Distance = 5f;
    public float moveDamping = 15f; //이동 계수
    public float rotateDamping = 10f; //회전 계수
    public float targetOffset = 2.0f; //추적 좌표의 오프셋
    private float originHeight; //최초의 높이를 저장
    private float overDamping = 5.0f; //이동속도 계수
    [Header("Etc Obstacle Setting")]
    public float heightobstacle = 15.0f; //카메라가 장애물에 부딪칠때 카메라가 올라갈 높이
    public float castOffset = 1.0f; //주인공에 투사할 레이캐스트의 높이 오프셋
    void Start()
    {
        tr = GetComponent<Transform>();
        originHeight = Height;
    }
    private void Update()
    {
        //주인공 장애물에 가려 졌는지를 판단할 레이캐스트의 높낮이를 설정
        Vector3 castTarget = target.position + (target.up * castOffset);
        //castTarget 좌표로의 방향 벡터를 계산
        Vector3 castDir = (castTarget - tr.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(tr.position, castDir, out hit, Mathf.Infinity))
        {       //주인공이 레이캐스트에 맞지 않았을 때
            if(!hit.collider.CompareTag("Player"))
            {
                Height = Mathf.Lerp(Height, heightobstacle, Time.deltaTime * overDamping);
                //보간 함수를 이용해서 카메라높이를 부드럽게 상승시킴
            }
            else
            {   //보간 함수를 이용해서 카메라 높이를 부드럽게 하강시킴
                Height = Mathf.Lerp(Height, originHeight, Time.deltaTime * overDamping);
            }
        }
    }

    void LateUpdate()
    {   //카메라 위치를 타켓 뒤에 타겟 위에 배치
        Vector3 camPos = target.position - (target.forward * Distance) + (target.up * Height);
        tr.position = Vector3.Lerp(tr.position, camPos, Time.deltaTime * moveDamping);
        //보간함수              직선 혹은 이동보간
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        //회전 보간
        tr.LookAt(target.position + (target.up * targetOffset));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 1.0f);
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
