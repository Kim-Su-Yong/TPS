using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public Texture[] textures;
    [SerializeField]
    private MeshRenderer renderers;
    [SerializeField]
    private GameObject ExplosionEffect;
    public int hitCount = 0;
    //private AudioSource _audio;
    [SerializeField]
    private AudioClip expSound;
    [SerializeField]
    //CameraShake shake;
    ShakeCam2D shake;
    public delegate void EnemyDieHandler();
    public static event EnemyDieHandler OnEnemyDie;
    void Start()
    {
        shake = Camera.main.GetComponent<ShakeCam2D>();
        //_audio = GetComponent<AudioSource>();
        renderers = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("BarrelTexture");
        ExplosionEffect = Resources.Load<GameObject>("Effects/BigExplosionEffect");
        expSound = Resources.Load<AudioClip>("Sounds/missile_explosion");
        int idx = Random.Range(0, textures.Length);
        renderers.material.mainTexture = textures[idx];
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag("BULLET"))
        {
            if(++hitCount == 3)
            {
                BarrelExplosion();
            }
        }
    }
    void OnDamage(object[] _params)
    {
        Vector3 firePos = (Vector3)_params[1];
        Vector3 hitPos = (Vector3)_params[0];

        Vector3 incomeVector = hitPos - firePos;
        //입사각 입사 벡터
        incomeVector = incomeVector.normalized;
        //입사 벡터를 정규화 벡터로 변경
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 10000f, hitPos);

        if (++hitCount == 3)
        {
            BarrelExplosion();
        }
    }
    void BarrelExplosion() //배럴 폭파 함수
    {
        GameObject exp = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(exp, 1.0f);
        //_audio.PlayOneShot(expSound, 1.0f);
        SoundManager.soundmanager.PlaySoundFunc(transform.position, expSound);
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f);
                            //20근방에 오브젝트에서 충돌체가 있으면 Cols라는 배열에 대입이 된다.
        foreach(Collider col in Cols)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                if (col.gameObject.tag != "Player")
                {
                    rb.mass = 1.0f; //리지디 바디의 무게를 1로 만듬 높이 날아가기 위해
                    rb.AddExplosionForce(1200f, transform.position, 20, 1000f);
                    //폭파함수        //폭파력, 위치, 반경, 위로 솟구치는 힘
                    OnEnemyDie();
                    //col.gameObject.SendMessage("expDie", SendMessageOptions.DontRequireReceiver);
                    //다른 오브젝트에 있는 함수를 호출 할 수 있는 함수
                    //SendMessageOptions.DontRequireReceiver 함수가 없거나 오타가 있어도
                    //오류를 내지 않게 하는 옵션이다.
                }
            }
        }
        shake.TurnOn();
        Invoke("BarrelNormalMass", 3.0f);
    }
    void BarrelNormalMass()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, 40f);
        //20근방에 오브젝트에서 충돌체가 있으면 Cols라는 배열에 대입이 된다.
        foreach (Collider col in Cols)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (col.gameObject.tag != "Player")
                {
                    rb.mass = 40.0f; //리지디 바디의 무게를 1로 만듬 높이 날아가기 위해

                }
            }
        }
    }
}
