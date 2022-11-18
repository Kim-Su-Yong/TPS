using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePos;
    //private AudioSource audio;
    [SerializeField]
    private AudioClip fireSound;
    private Animator animator;
    [SerializeField]
    private Transform playerTr;
    private Transform enemyTr;
    public MeshRenderer renderer;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    private float nextFire = 0; //발사 시간 계산용 변수
    private readonly float fireRate = 0.1f; //총알 발사 간격
    private readonly float damping = 10.0f; //주인공을 향해 회전할 속도 계수
    public bool isFire = false;
    //재장전 관련 변수들
    private readonly float reloadTime = 2.0f; //재장전 시간
    private readonly int maxBullet = 10; //탄창의 최대 총알수
    private int CurrentBullet = 10; //초기 총알 수
    private bool isReload = false;
    private WaitForSeconds wsReload; //재장전할동안 기다릴 변수
    [SerializeField]
    private AudioClip reloadSfx;

    void Start()
    {
        //audio = GetComponent<AudioSource>();
        fireSound = (AudioClip)Resources.Load("Sounds/p_ak_1");
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        //bullet = Resources.Load<GameObject>("E_BULLET");
        reloadSfx = Resources.Load<AudioClip>("Sounds/p_reload 1");
        wsReload = new WaitForSeconds(reloadTime);
    }

    void Update()
    {
        if(isFire && !isReload)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.1f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);

        }
    }
    void Fire()
    {
        GameObject e_bullet = GameManager.gameManager.GetenemyBullet();
        if (e_bullet != null)
        {
            e_bullet.transform.position = firePos.position;
            e_bullet.transform.rotation = firePos.rotation;
            e_bullet.SetActive(true);
        }
        animator.SetTrigger(hashFire);
        //audio.PlayOneShot(fireSound, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
        isReload = (--CurrentBullet % maxBullet == 0);
        if(isReload)
        {   //재장전 하기 위한 프레임을 가지고 오기 위해
            StartCoroutine(Reloading());
        }

        //Instantiate(bullet, firePos.position, firePos.rotation);
    }
    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        //audio.PlayOneShot(reloadSfx, 1.0f);
        yield return wsReload;


        CurrentBullet = maxBullet;
        isReload = false;
    }
    IEnumerator ShowMuzzleFlash()
    {
        renderer.enabled = true;
        float _scale = Random.Range(1f, 2f);
        renderer.transform.localScale = Vector3.one * _scale;
        //x,y,z축으로 동일하게
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        renderer.transform.localRotation = rot;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        renderer.enabled = false;
    }
}
