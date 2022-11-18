using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatCtrl : MonoBehaviour
{
    [SerializeField]
    Transform tr;
    [SerializeField]
    Animator animator;
    float h, v, r;
    public float smoothBlend = 0.1f; //부드럽게 움직이기 위한 타임델타 값과 곱할 값
    public float moveSpeed = 3.5f;
    public float turnSpeed = 90f;
    void Start()
    {
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");
        tr.Translate(Vector3.right * h *moveSpeed * Time.deltaTime);
        {
            animator.SetFloat("SpeedX", h, smoothBlend, Time.deltaTime);
        }
        tr.Translate(Vector3.forward.normalized * v * moveSpeed * Time.deltaTime);
        {
            animator.SetFloat("SpeedY", v, smoothBlend, Time.deltaTime);
        }
        tr.Rotate(Vector3.up * r * Time.deltaTime * turnSpeed);

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 7.5f;
            animator.SetBool("isSprint", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 4.5f;
            animator.SetBool("isSprint", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && h == 0 && v == 0)
        {
            animator.SetTrigger("IsJump");
        }

        if (Input.GetKeyDown(KeyCode.Space) && v > 0)
        {
            animator.SetTrigger("IsJumpForward");
        }

        if (Input.GetMouseButtonDown(1) && v == 0 && h == 0)
        {
            animator.SetTrigger("IsAttack");
        }

        if (Input.GetMouseButtonDown(0) && v == 0 && h == 0)
        {
            animator.SetTrigger("IsKick");
        }
    }

}