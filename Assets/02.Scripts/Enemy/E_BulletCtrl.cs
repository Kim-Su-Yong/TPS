using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_BulletCtrl : MonoBehaviour
{
    public float Speed = 800f;
    private Rigidbody rbody;
    private Transform tr;
    private TrailRenderer trail;
    Transform player;
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        //로컬 방향으로
        trail = GetComponent<TrailRenderer>();
    }
    private void OnEnable()
    {
        rbody.AddForce(tr.forward * Speed);
        //Vector3.forward로 하면 안됨 절대좌표 사용X
        Invoke("e_BulletDeActive", 3.0f);
    }
    private void OnDisable() //오브젝트가 비활성화 될때 호출
    {        
        tr.position = player.position + Vector3.up * 1.8f;
        tr.rotation = Quaternion.identity;
        trail.Clear();
        rbody.Sleep(); //리지드 바디 정지
    }
}
