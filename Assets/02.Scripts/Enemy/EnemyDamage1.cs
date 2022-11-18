using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage1 : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    public float hp = 0f;
    public float hpMax = 100f;
    [SerializeField]
    private GameObject BloodEffect;
    //피격시 사용할 혈흔 효과
    EnemyAI enemyAI;
    public Image hpBar;
    public Canvas hpCanvas;
    void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        BloodEffect = Resources.Load("BloodSprayEffect") as GameObject;
        hpBar.color = Color.green;
    }
    private void OnDisable()
    {
        
    }
    void OnDamage(object[] _params)
    {
        //col.gameObject.SetActive(false);
        //Vector3 pos = (Vector3)_params[1];
        ShowBloodEffect((Vector3)_params[1]);

        hp -= 10f;
        //hp -= (float)_params[0];
        //Debug.Log("hp: " + hp.ToString());

        hpBar.fillAmount = (float)hp / (float)hpMax;

        if (hpBar.fillAmount <= 0.25f)
            hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow;

        if (hp <= 0)
        {
            //enemyAI.state = EnemyAI.State.DIE;
            enemyAI.Die();
        }
    }
    #region 오브젝트 풀 방식의 충돌함수 OnCollision
    //private void OnCollisionEnter(Collision col)
    //{
    //    if (/*col.collider.CompareTag(bulletTag)*/col.collider.CompareTag(bulletTag))
    //    {
    //        //Destroy(col.gameObject);
    //        col.gameObject.SetActive(false);
    //        ShowBloodEffect(col);     
    //        hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
    //        Debug.Log("hp: "+hp.ToString());

    //        hpBar.fillAmount = (float)hp / (float)hpMax;

    //        if (hpBar.fillAmount <= 0.25f)
    //            hpBar.color = Color.red;
    //        else if (hpBar.fillAmount <= 0.5f)
    //            hpBar.color = Color.yellow;

    //        if (hp <= 0)
    //        {
    //            //enemyAI.state = EnemyAI.State.DIE;
    //            enemyAI.Die();
    //        }
    //    }
    //}
    #endregion
    private void ShowBloodEffect(Collision col)
    {
        //ContactPoint cp = col.contacts[0];
        ////맞았던 첫번째 위치를 받았고 
        //Quaternion rot = Quaternion.LookRotation(-cp.normal);
        ////회전은 바라보는 반대쪽으로 혈흔이 생성되기 위해
        //var blood = Instantiate<GameObject>(BloodEffect, cp.point, rot);

        //Destroy(blood, 1.0f);
        Vector3 pos = col.transform.position;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, pos);
        GameObject blood = Instantiate(BloodEffect, pos, rot);
        Destroy(blood, Random.Range(0.8f, 1.3f));
    }
    private void ShowBloodEffect(Vector3 pos)
    {
        ////맞았던 첫번째 위치를 받았고 
        Quaternion rot = Quaternion.LookRotation(-pos.normalized);
        ////회전은 바라보는 반대쪽으로 혈흔이 생성되기 위해
        var blood = Instantiate<GameObject>(BloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }

}