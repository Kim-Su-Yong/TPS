using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//이 오브젝트에는 NavMeshAgent 컴퍼넌트가 없으면
//안된다  삭제 할 시에 경고를 띄우는 애튜리뷰트(속성)
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints; //도착지점
    public int nexIdx = 0; //배열 인덱스 값 
    private NavMeshAgent agent;
    private Animator animator;
    // readonly const 
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    //순찰여부를 판단하는 변수 프로퍼티 
    private bool _parolling;
    public bool patrolling //프로퍼티 property;
    {
        get { return _parolling; } //read 읽기만 가능 수정은 불가능
        set
        {
            _parolling = value;
            agent.speed = patrolSpeed;
            MoveWayPoint();
        } // 수정 가능 즉 쓰기 가능
    }
    //추적 대상의 위치를 저장 하는 변수
    //프로퍼티 정의 (getter ,setter)
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }
    //NavMeshAgent의 이동 속도에 대한 프로퍼터 정의 getter
    public float speed
    {                //방향과 속도의 크기 즉 스피드
        get { return agent.velocity.magnitude; }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        var group = GameObject.Find("PatrolPoint");
        //유효성 검사 
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
            //첫번째로 잡힌 부모오브젝트 트랜스폼을 제외 한다. 
        }
        MoveWayPoint();
        animator.SetBool("IsMove", false);
    }
    void MoveWayPoint()
    {
        if (agent.isPathStale) return;
        // 경로계산이 안되거나 최단 경로가 잡히지 않으면 
        //이 함수를 빠져나간다. 
        agent.destination = wayPoints[nexIdx].position;
        // 첫번째 경로부터 이동 한다.
        agent.isStopped = false;
        animator.SetBool("IsMove", true);
    }
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;

    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _parolling = false;

    }
    void Update()
    {
        if (!_parolling) return;

        //목적지에 도착 했는 지를 판단
        if (agent.remainingDistance <= 0.5f)
        {
            nexIdx = ++nexIdx % wayPoints.Count;
            MoveWayPoint();
        }

    }
}