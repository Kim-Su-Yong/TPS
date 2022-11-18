using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject Spark;
    [SerializeField]
    private AudioClip hitSound;
    private readonly string bulletTag = "BULLET";
    private readonly string bulletTag2 = "E_BULLET";
    void Start()
    {
        hitSound = (AudioClip)Resources.Load("Sounds/bullet_hit_metal_enemy_4");
        Spark = (GameObject)Resources.Load("FlareMobile");
    }
    

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == bulletTag)
        {
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            //source.PlayOneShot(hitSound, 1.0f);
            SoundManager.soundmanager.PlaySoundFunc(transform.position, hitSound);
            ShowEffect(col);
        }
        
    }
    void OnDamage(object[]_params)
    {
        SoundManager.soundmanager.PlaySoundFunc(transform.position, hitSound);
        ShowEffect((Vector3)_params[0]);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(bulletTag2))
        {
            col.gameObject.SetActive(false);
            SoundManager.soundmanager.PlaySoundFunc(transform.position, hitSound);
            //source.PlayOneShot(hitSound, 1.0f);
            //ShowEffect(col);
        }
    }
    void BulletDeactive(Collision col)
    {
        col.gameObject.SetActive(false);
    }
    private void ShowEffect(Collision col)
    {
        //충돌 지점 좌표를 추출
        ContactPoint contact = col.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        //벡터가 이루는 회전 각도를 추출
        GameObject spk = Instantiate(Spark, contact.point, rot);
        Destroy(spk, 2.0f);
    }
    private void ShowEffect(Vector3 pos)
    {
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, pos.normalized);
        //벡터가 이루는 회전 각도를 추출
        GameObject spk = Instantiate(Spark, pos, rot);
        Destroy(spk, 2.0f);
    }
}
