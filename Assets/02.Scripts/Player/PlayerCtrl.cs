using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;
    float h = 0f, v = 0f, r = 0f; //Horizontal Vertical Mouse X 명령을 받는 변수
    public float moveSpeed = 3.5f; //이동 속도
    public float turnSpeed = 80f;  //회전 속도
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUP;
    }
    void UpdateSetUP()
    {
        moveSpeed = GameManager.gameManager.gameData.speed;
    }
    void Start()
    {
        _animation = GetComponent<Animation>();
        _animation.Play("Idle");
        moveSpeed = GameManager.gameManager.gameData.speed;
    }

    void Update()
    {
        Move();
    }
    private void Move()
    {
        h = Input.GetAxis("Horizontal"); //A,D를 눌렀을 때
        v = Input.GetAxis("Vertical"); //W,S를 눌렀을 때
                                       //게임에 맞는 부드러운 프레임으로 만들어준다
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        //정규화
        transform.Rotate(Vector3.up * r * turnSpeed * Time.deltaTime);

        UpdateAnimation();
        Sprint();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 6.5f;
            _animation.CrossFade("SprintF", 0.3f);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 3.5f;
        }
    }

    private void UpdateAnimation()
    {
        //애니메이션 연동
        //CrossFade : 직전동작과 지금하는 동작 애니메이션을
        //0.3초간 겹치게 해서 부드러운 애니메이션
        //즉 블렌드 애니메이션 효과가 나타나게 한다.
        if (h > 0.1f)
            _animation.CrossFade("RunR", 0.3f);
        else if (h < -0.1f)
            _animation.CrossFade("RunL", 0.3f);
        else if (v > 0.1f)
            _animation.CrossFade("RunF", 0.3f);
        else if (v < -0.1f)
            _animation.CrossFade("RunB", 0.3f);
        else
            _animation.CrossFade("Idle", 0.3f);
    }
}
