using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //애트리뷰트 : 클래스를 인스펙터 하위에 보여지게 한다.
public class _Animation
{
    public AnimationClip idle;
    public AnimationClip Forward;
    public AnimationClip Backward;
    public AnimationClip Right;
    public AnimationClip Left;
    public AnimationClip Sprint;
    public AnimationClip RunJump;
    public AnimationClip idleJump;
    public AnimationClip Kick;
    public AnimationClip Attack;
}

public class MariaCtrl : MonoBehaviour
{
    private float h, v, r;
    [SerializeField] //인스펙터 컴퍼넌트에 변수가 보이게 하는 애트리뷰트
    private Transform tr;

    public _Animation ani;
    [SerializeField]
    private Animation _animation;
    public float moveSpeed = 4.5f;
    public float rotSpeed = 85f;
    void Start()
    {
        tr = GetComponent<Transform>();
        _animation = GetComponent<Animation>(); //컴퍼넌트나 변수를 초기화하는 공간
        _animation.Play(ani.idle.name);
                        //.name 문자열 스트링이라는 의미
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);

        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * r * rotSpeed * Time.deltaTime);
        UpdateAnimation();
        Sprint();
        Jump();
        MariaAttack();
    }
    void MariaAttack()
    {
        #region kickAttack Mouse 오른쪽
        if(Input.GetMouseButton(1) && v == 0 && h==0)
        {
            _animation.Play(ani.Kick.name);
        }

        #endregion
        #region swordAttack Mouse 왼쪽
        if(Input.GetMouseButton(0) && v == 0 && h == 0)
        {
            _animation.Play(ani.Attack.name);
        }

        #endregion
    }
    void Jump()
    {
        #region idle jump
        if (Input.GetKey(KeyCode.Space) && h == 0 && v == 0)
        {
            _animation.Play(ani.idleJump.name);
        }
        #endregion
        #region moveJump
        if(Input.GetKey(KeyCode.Space) && v > 0)
        {
            _animation.Play(ani.RunJump.name);
        }

        #endregion
    }
    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 7.5f;
            _animation.CrossFade(ani.Sprint.name);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 4.5f;
        }
    }

    private void UpdateAnimation()
    {
        //애니메이션 연동
        if (h > 0.1f)
            _animation.CrossFade(ani.Right.name);
        else if (h < -0.1f)
            _animation.CrossFade(ani.Left.name);
        else if (v > 0.1f)
            _animation.CrossFade(ani.Forward.name);
        else if (v < -0.1f)
            _animation.CrossFade(ani.Backward.name);
        else
            _animation.CrossFade(ani.idle.name);
    }
}
