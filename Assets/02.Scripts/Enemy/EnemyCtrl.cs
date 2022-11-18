using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //인공지능 관련 기능 즉 NavMeshAgent 쓸려면 선언해야함

public class EnemyCtrl : MonoBehaviour
{
    public Animator animator; //애니메이터
    public NavMeshAgent agent; //플레이어 추적 할려면
    public Transform playerTr;
    public Transform EnemyTr;
    public float traceDist = 15f; //추적 범위
    public float attackDist = 2.0f;
    EnemyDamage e_damage;
    

    void Start()
    {
        e_damage = GetComponent<EnemyDamage>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        // 하이라키에서 Player 태그를 가진 트랜스폼을 찾아서 playerTr에 대입

        AttackCollider(false);
    }
    void Update()
    {
        if (e_damage.IsDie == true) return; //조건 충족하면 안내려감

        float dist = Vector3.Distance(playerTr.position, EnemyTr.position);
        //플레이어와 자기자신(좀비)의 거리를 구한다.
        if (dist <= attackDist)
        {   //공격 사정거리에 들어오면
            animator.SetBool("IsAttack", true);
            //어택 실행 애니메이션 동작
            //agent.isStopped = true; //공격시 추적 중지
            AttackCollider(true);
        }
        else if (dist <= traceDist)
        {
            agent.isStopped = false; //추적시 추적 활성화
            agent.destination = playerTr.position;
            //추적 대상은 플레이어
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsTrace", true);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("IsTrace", false);
        }

    }

    public void AttackCollider(bool isEnable)
    {
        foreach (Collider Col in GetComponentsInChildren<SphereCollider>())
        {
            Col.enabled = isEnable;
            //콜라이더 비활성화
        }
    }
}
