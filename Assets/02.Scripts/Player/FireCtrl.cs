//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
////1.총알 발사체 2.발사위치 3.연사, 몇초간격으로 발사 할지

//[System.Serializable] //아래의 구조체가 인스펙터 컴퍼넌트에 보여지게 된다.
//public struct PlayerSfx //구조체
//{
//    public AudioClip[] fire;
//    public AudioClip[] reload;
//}
//public class FireCtrl : MonoBehaviour
//{
//    public enum WeaponType
//    {
//        RIFLE = 0,
//        SHOTGUN
//    }
//    public WeaponType currWeapon = WeaponType.RIFLE;
//    public PlayerSfx playersfx;
//    [SerializeField] //발사 위치
//    private Transform firePos;
//    [SerializeField]
//    private AudioSource source;
//    //[SerializeField]
//    //private AudioClip fireSound;
//    [SerializeField]
//    ParticleSystem CartridgeEjectEffect;
//    [SerializeField]
//    ParticleSystem muzzleFlashEffect;
//    [Header("magazineUI")]
//    public Image magazineImg;
//    public Text magazineTxt;
//    //최대 총알수
//    public int maxBullet = 10;
//    //남은 총알수
//    public int remainingBullet = 10;
//    //재장전 시간
//    public float reloadTime = 2.0f;
//    //재장전 여부를 판단할 변수
//    private bool isReloading = false;
//    //[SerializeField]
//    //public AudioClip reloadSound;
//    private readonly string enemyTag = "ENEMY";
//    private readonly string barrelTag = "BARREL";
//    private readonly string wallTag = "WALL";
//    RaycastHit hit; //광선을 쏘았을 때 맞았는지 충돌 포인트 위치를 알아내는 자료형
//    public Sprite[] weaponIcons;
//    public Image weaponImage;
//    public float damage;
//    [Header("Raycast Auto Fire")]
//    [SerializeField]
//    private int enemyLayer;
//    [SerializeField]
//    private bool isFire = false;
//    [SerializeField]
//    private float nextFire; //다음 발사 시간을 저장할 변수
//    [SerializeField]
//    public float fireRate = 0.1f; //총알의 발사 간격
//    void Awake()
//    {
//        source = GetComponent<AudioSource>();
//        //muzzleFlashEffect = GameObject.Find("Player_Rweaponholder").transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
//        //CartridgeEjectEffect = GameObject.Find("Player_Rweaponholder").transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
//        magazineImg = GameObject.Find("Canvas-UI").transform.GetChild(2).GetChild(2).GetComponent<Image>();
//        magazineTxt = GameObject.Find("Canvas-UI").transform.GetChild(2).GetChild(0).GetComponent<Text>();
//        //playersfx.fire[(int)currWeapon] = Resources.Load("Sounds/p_ak_1") as AudioClip;
//        //playersfx.reload[(int)currWeapon] = Resources.Load("Sounds/p_reload 1") as AudioClip;
//        magazineImg.fillAmount = 1.0f;
//        enemyLayer = LayerMask.NameToLayer("ENEMY");
//        //적 캐릭터의 레이어 값을 추출
//        firePos = GameObject.Find("Player_Rweaponholder").transform.GetChild(0).transform.GetChild(0).transform;
//    }
//    private void OnEnable()
//    {
//        GameManager.OnItemChange += UpdateSetup;
//    }
//    void UpdateSetup()
//    {
//        damage = GameManager.gameManager.gameData.damage;
//    }
//    void Update()
//    {
//        Debug.DrawRay(firePos.position, firePos.forward * 50f, Color.green);

//        //현재 마우스 포인터 어떤 UI에 중첩이 되었다면
//        if (MouseHover.instance.isUIHover)
//        return;

//       // if (EventSystem.current.IsPointerOverGameObject()) return;

//        //RaycastHit hit;
//        //if (Physics.Raycast(firePos.position, firePos.forward, out hit, 50f, 1 << enemyLayer))
//        //    isFire = true;
//        //else
//        //    isFire = false;

//        if(!isReloading && Input.GetMouseButtonDown(0))//)
//        {   
//            if(Time.time > nextFire)
//            {
//                --remainingBullet; //총알 수 하나 감소
//                Fire();
//                if (remainingBullet == 0)
//                {
//                    StartCoroutine(Reloading());
//                }
//                nextFire = Time.time + fireRate;
//            }
//        }
//    }
//    void Fire()
//    {
//        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
//        //what         //where          //how rotation
//        #region 오브젝트 풀링
//        //GameObject _bullet = GameManager.gameManager.GetBullet();
//        //if(_bullet != null)
//        //{
//        //    _bullet.transform.position = firePos.position;
//        //    _bullet.transform.rotation = firePos.rotation;
//        //    _bullet.SetActive(true);
//        //}
//        #endregion
//        #region 레이캐스트
//        RaycastHit hit;
//        if(Physics.Raycast(firePos.position,firePos.forward, out hit, 20f))
//        {
//            if(hit.collider.tag == enemyTag)
//            {
//                object[] _params = new object[2];
//                _params[0] = damage; //배열의 첫번째에 데미지 전달
//                _params[1] = hit.point;
//                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
//            }
//            if(hit.collider.tag == barrelTag || hit.collider.tag == wallTag)
//            {
//                object[] _params = new object[2];
//                _params[0] = hit.point; //맞은위치
//                _params[1] = firePos.position; //발사위치      
//                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
//            }
//        }
//        #endregion
//        var _sfx = playersfx.fire[(int)currWeapon];
//        source.PlayOneShot(_sfx, 1.0f);
//        CartridgeEjectEffect.Play();
//        muzzleFlashEffect.Play();

//        UpdateBulletText();
//        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
//    }
//    IEnumerator Reloading()
//    {
//        isReloading = true;
//        var _reload = playersfx.reload[(int)currWeapon];
//        source.PlayOneShot(_reload, 1.0f);
//        yield return new WaitForSeconds(_reload.length + 0.3f);
//        isReloading = false;
//        magazineImg.fillAmount = 1.0f;
//        remainingBullet = maxBullet;
//        UpdateBulletText();
//    }
//    void UpdateBulletText()
//    {
//        magazineTxt.text = string.Format("<color=#ff0000>{0}</color>/{1}"
//            , remainingBullet, maxBullet);
//    }
//    public void OnChangeWeapon()
//    {
//        currWeapon = (WeaponType)((int)++currWeapon % 2);
//        weaponImage.sprite = weaponIcons[(int)currWeapon];
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable] //아래의 구조체가 인스펙터 컴퍼넌트에 보여 지게 된다.
public struct PlayerSfx //구조체 
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 0,
        SHOTGUN
    }
    public WeaponType currWeapon = WeaponType.RIFLE;
    public PlayerSfx playersfx;
    [SerializeField]//발사 위치
    private Transform firePos;
    [SerializeField]
    private AudioSource source;
    //[SerializeField]
    //private AudioClip fireSound;
    [SerializeField]
    ParticleSystem CartridgeEjectEffect;
    [SerializeField]
    ParticleSystem MuzzleFlashEffect;
    [Header("magazineUI")]
    public Image magazineImg;
    public Text magazineTxt;
    //최대 총알수 
    public int maxBullet = 10;
    //남은 총알 수 
    public int remainingBullet = 10;
    //재장전 시간
    public float reloadTime = 2.0f;
    //재장전 여부를 판단할 변수 
    private bool isReloading = false;
    //public AudioClip reloadSound;
    private readonly string enemyTag = "ENEMY";
    private readonly string barrelTag = "BARREL";
    private readonly string wallTag = "WALL";
    public Sprite[] weaponIcons;
    public Image weaponImage;
    public float damage;

    [Header("Raycast Auto Fire")]
    [SerializeField]
    private int enemyLayer;
    [SerializeField]
    private int obstacleLayer;
    [SerializeField]
    private int layerMask;
    [SerializeField]
    private bool isFire = false;
    [SerializeField]
    private float nextFire; //다음 발사 시간을 저장 할 변수 
    [SerializeField]
    public float fireRate = 0.1f; //총알 의 발사 간격
    void Awake()
    {

        MuzzleFlashEffect = GameObject.Find("Player_Rweaponholder")
            .transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
         CartridgeEjectEffect = GameObject.Find("Player_Rweaponholder").
        transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
        magazineImg = GameObject.Find("Canvas-UI").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        magazineTxt = GameObject.Find("Canvas-UI").transform.GetChild(2).GetChild(0).GetComponent<Text>();
        source = GetComponent<AudioSource>();
        // playersfx.reload[(int)currWeapon] = Resources.Load("Sounds/p_reload 1") as AudioClip;
        // playersfx.fire[(int)currWeapon] = Resources.Load("Sounds/p_ak_1") as AudioClip;

        magazineImg.fillAmount = 1.0f;
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << obstacleLayer | 1 << enemyLayer;
        //적 캐릭터의 레이어 값을 추출 
        firePos = GameObject.Find("Player_Rweaponholder").transform.GetChild(0).transform.GetChild(0).transform;
    }
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetup;
    }
    void UpdateSetup()
    {
        damage = GameManager.gameManager.gameData.damage;

    }
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.green);


        //현재 마우스 포인터가  어떤 UI에 충돌 감지가  되었다면
        if (MouseHover.instance.isUIHover) return;
        RaycastHit hit;
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 50f, layerMask))
            isFire = hit.collider.CompareTag("ENEMY");
        else
            isFire = false;

        if (!isReloading && isFire)
        {
            if (Time.time > nextFire)
            {
                --remainingBullet;//총알 수 하나 감소 
                Fire();
                if (remainingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }
                nextFire = Time.time + fireRate;
            }
        }
    }
    void Fire()
    {
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        //what        where              how rotation
        #region 오브젝트 풀링
        //GameObject _bullet = GameManager.gameManager.GetBullet();
        //if(_bullet !=null)
        //{
        //    _bullet.transform.position = firePos.position;
        //    _bullet.transform.rotation = firePos.rotation;
        //    _bullet.SetActive(true);
        //}
        #endregion
        #region 레이캐스트
        RaycastHit hit;
        //광선 쏘았을 때 특정 오브젝트에 맞았는 지 충돌 포인트 위치를 알아내는 자료형
        //광선을 발사 했는 데 맞았다면.......
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20f))
        {    //충돌한 충돌체의 태그가 맞다면
            if (hit.collider.tag == enemyTag)
            {
                object[] _params = new object[2];
                _params[0] = damage; //배열의 첫번째에 데미지 전달
                _params[1] = hit.point; //맞은 위치 
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                    SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.tag == barrelTag || hit.collider.tag == wallTag)
            {
                object[] _params = new object[2];

                _params[0] = hit.point; //맞은 위치 
                _params[1] = firePos.position; //발사위치 
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                    SendMessageOptions.DontRequireReceiver);
            }

        }
        #endregion

        var _sfx = playersfx.fire[(int)currWeapon];
        source.PlayOneShot(_sfx, 1.0f);
        CartridgeEjectEffect.Play();
        MuzzleFlashEffect.Play();

        UpdateBulletText();
        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
    }
    IEnumerator Reloading()
    {
        isReloading = true;
        var _reload = playersfx.reload[(int)currWeapon];
        source.PlayOneShot(_reload, 1.0f);
        yield return new WaitForSeconds(_reload.length + 0.3f);
        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        UpdateBulletText();
    }
    void UpdateBulletText()
    {
        magazineTxt.text = string.Format("<color=#ff0000>{0}</color>/{1}",
            remainingBullet, maxBullet);
    }
    public void OnChangeWeapon()
    {
        currWeapon = (WeaponType)((int)++currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }
}
