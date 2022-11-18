using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL, TRACE, ATTACK, DIE
    }
    public State state = State.PATROL;
    [SerializeField]
    private Transform playerTr;
    [SerializeField]
    private Transform enemyTr;
    private Animator anim;
    private NavMeshAgent agent;
    private EnemyFire enemyFire;
    //시야각 및 추적 반경을 제어하는 EnemyFOV
    private EnemyFOV enemyFOV;
    MoveAgent2 moveAgent2;

    public float attackDist = 5.0f;
    public float traceDist = 10f;
    bool isDie = false;
    private WaitForSeconds ws;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashexpDie = Animator.StringToHash("expDie");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    EnemyDamage1 enemydamage;
    BarrelCtrl barrelctrl;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemydamage = GetComponent<EnemyDamage1>();
        enemyFire = GetComponent<EnemyFire>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        moveAgent2 = GetComponent<MoveAgent2>();
        enemyFOV = GetComponent<EnemyFOV>();
        ws = new WaitForSeconds(0.3f);
        //cycle Offset값을 불규칙하게
        anim.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        anim.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
    }
    private void OnEnable() //오브젝트가 활성화 되었을때
    {                       //Start()함수보다 먼저 호출
        Damage.OnPlayerDie += this.OnPlayerDie;
        BarrelCtrl.OnEnemyDie += this.expDie;
        agent.isStopped = false;
        StartCoroutine(CheckState());
        StartCoroutine(EnemyAction());
    }
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            yield return ws;
            //float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            //float dist = (playerTr.position - enemyTr.position).magnitude; //3D 개체 크기 = magnitude
            Vector3 dist = (playerTr.position - enemyTr.position);
            if (dist.magnitude <= attackDist)
            {   //주인공과의 거리에 장애물 여부 판단
                if (enemyFOV.isViewPlayer())
                {
                    state = State.ATTACK;
                }
            } //추적반경 및 시야각에 들어왔는지 판단
            else if (enemyFOV.isTracePlayer())
            {
                state = State.TRACE;
            }
            else
                state = State.PATROL;
        }
    }
    IEnumerator EnemyAction()
    {
        while(!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent2.patrolling = true;
                    anim.SetBool(hashMove,true);
                    break;

                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent2.traceTarget = playerTr.position;
                    anim.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    if(enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    enemyTr.LookAt(playerTr);
                    moveAgent2.Stop();
                    anim.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    Die();
                    
                    break;
            }
           
        }
    }
    public void Update()
    {
        anim.SetFloat(hashSpeed, moveAgent2.speed);
    }
    public void Die()
    {
        enemyFire.isFire = false;
        isDie = true;
        
        moveAgent2.Stop();
        anim.SetInteger(hashDieIdx, Random.Range(0, 2));
        anim.SetTrigger(hashDie);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StopAllCoroutines();
        //Destroy(this.gameObject, 5.0f);
        //this.gameObject.SetActive(false);
        StartCoroutine(PushPool());
        //UIManager.uimanager.IncKillCount();
        GameManager.gameManager.IncKillCount();
        //Invoke("DieAnim", 2f);//2초후 실행
    }
    public void expDie()
    {
        if (isDie) return;

        enemyFire.isFire = false;
        isDie = true;
        moveAgent2.Stop();
        anim.SetTrigger(hashexpDie);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StopAllCoroutines();
        //Destroy(this.gameObject, 5.0f);
        //this.gameObject.SetActive(false);
        StartCoroutine(PushPool());
        //UIManager.uimanager.IncKillCount();
        GameManager.gameManager.IncKillCount();
    }
    IEnumerator PushPool()
    {
        yield return new WaitForSeconds(3.0f);

        this.gameObject.SetActive(false);
        moveAgent2.patrolling = true;
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        enemydamage.hp = 100f;
        enemydamage.hpBar.color = Color.green;
        enemydamage.hpBar.fillAmount = 1.0f;
    }
    public void OnPlayerDie()
    {
        moveAgent2.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines(); //모든 코루틴 중지
        anim.SetTrigger(hashPlayerDie);
    }
    private void OnDisable() //오브젝트가 비활성화 되었을때 호출
    {                        //제일 느리게 호출
        //이벤트 연결 해제
        Damage.OnPlayerDie -= this.OnPlayerDie;
        BarrelCtrl.OnEnemyDie -= this.expDie;
    }
    //void DieAnim()
    //{
    //    anim.enabled = false; //애니메이션 멈추게 함
    //}
}