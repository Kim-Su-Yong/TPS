using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public enum State //열거형 상수 가독성을 위해서 사용
    {
        PATROL = 0, TRACE = 1, ATTACK = 2, DIE = 3
    }
    public State state = State.PATROL;
    [SerializeField]
    private Transform playerTr;
    [SerializeField]
    private Transform monsterTr;
    [SerializeField]
    private Animator animator;
    public float attackDist = 5.5f;
    public float traceDist = 10.0f;
    public bool isDie = false;
    private WaitForSeconds ws;
    MoveAgent moveAgent;
    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 추출
    private readonly int hashMove = Animator.StringToHash("IsMove"); //hash:찾는다,변한다
    private readonly int hashSpeed = Animator.StringToHash("ForwardSpeed");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");


    void Awake() //Start()함수보다 먼저 호출
    {
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        playerTr = GameObject.FindWithTag("Player").transform; //Find들은 update에서 쓰면 안됨
        monsterTr = this.gameObject.transform;
        ws = new WaitForSeconds(0.2f);
        //업데이트 Update()를 쓰지 않는 이유
        //Update()는 60프레임 이상 소모 되기 때문에
        //수백 수천 마리의 적을 생성 할때 마다
        //각자가 60프레임 이상 소모한다면 게임은 매우
        //느려질 가능성이 크다.
    }
    private void OnEnable() //Awake()-> OnEnable()-> Start() 순으로 호출
    {
        //오브젝트가 활성화 되었을 때 저절로 호출
        StartCoroutine(CheckState());
        StartCoroutine(MonsterAction());
    }
    private void OnDisable() //오브젝트가 비활성화 되었을 때 호출
    {
                             //호출순서중 제일 느리다.
    }
    IEnumerator CheckState()
    {
        while(isDie == false)
        {
 
            float dist = (playerTr.position - monsterTr.position).magnitude;
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }
    IEnumerator MonsterAction()
    {
        while (isDie == false)
        {
            yield return ws;
            switch (state)
            {
                case State.PATROL:
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    monsterTr.LookAt(playerTr);

                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    moveAgent.Stop();

                    break;
            }
        }
    }
    private void Update()
    {
       animator.SetFloat(hashSpeed,moveAgent.speed);
    }

}
