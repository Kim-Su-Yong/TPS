using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float Speed = 1500f;
    private Rigidbody rbody;
    private Transform tr;
    public float damage = 0f;
    TrailRenderer trail;
    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        //로컬 방향으로
        trail = GetComponent<TrailRenderer>();
        damage = GameManager.gameManager.gameData.damage;
        Invoke("bulletActive", 3.0f);
    }
    void bulletActive()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        rbody.AddForce(tr.forward * Speed);
        //Vector3.forward로 하면 안됨 절대좌표 사용X
    }
    private void OnDisable() //오브젝트가 비활성화 될때 호출
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rbody.Sleep(); //리지드 바디 정지
    }
}
