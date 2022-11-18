using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent2 : MonoBehaviour
{
    public List<Transform> wayPoints;
    private Transform enemyTr;
    int nexIdx = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private readonly float patrolSpeed2 = 1.5f;
    private readonly float traceSpeed2 = 4.0f;
    //회전할때 조절하는 계수 부드럽게 하는 정도의 값
    private float damping = 1.0f;
    private bool _patrolling;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed2;
                MoveWayPoint();
            }
        }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed2;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    void Awake()
    {
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        //자동으로 회전하는 기능을 비활성화
        var group = GameObject.Find("PatrolPoint");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0); 
        }
        nexIdx = Random.Range(0, wayPoints.Count);
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
        _patrolling = false;
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }
    void Update()
    {   //캐릭터가 이동중일때만 회전
        if(agent.isStopped == false)
        {   //네비메쉬 에이전트가 가야할 방향 벡터를 퀴터니언 타입의 각도로 변경
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
            //interpolation
        }
        
        if (!_patrolling) return;

        if (agent.remainingDistance <= 0.5f)
        {
            nexIdx = ++nexIdx % wayPoints.Count;
            MoveWayPoint();
        }
    }
}