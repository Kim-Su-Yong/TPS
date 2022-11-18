using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private readonly string bulletTag = "E_BULLET";
    [SerializeField]
    private GameObject bloodEffect;
    public float hp = 0f;
    public float hpInit = 100;
    [SerializeField]
    public Image bloodscreen;
    [SerializeField]
    private Image hpBar;
    private readonly Color initColor = new Vector4(0.1f, 1.0f, 0.0f, 1.0f);
    private Color currentColor;
    //델리게이트 및 이벤트 선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void Start()
    {     
        hpBar = GameObject.Find("Canvas-UI").transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
        bloodscreen = GameObject.Find("Canvas-UI").transform.GetChild(0).GetComponent<Image>();     
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
        hp = GameManager.gameManager.gameData.hp;
        hpBar.color = initColor;
        currentColor = initColor;
    }
    IEnumerator ShowBloodScreen()
    {
        bloodscreen.enabled = true;
        bloodscreen.color = new Color(1, 0, 0, Random.Range(0.3f, 0.4f));
        yield return new WaitForSeconds(0.1f);
        bloodscreen.color = Color.clear; //모든 색상을 모두 0으로 변경
        bloodscreen.enabled = false;
    }
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetup;
    }
    void UpdateSetup()
    {
        hp = GameManager.gameManager.gameData.hp;
        hpInit = GameManager.gameManager.gameData.hp - hpInit;
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag(bulletTag))
        {
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            StartCoroutine(ShowBloodScreen());
            ShowBloodEffect(col);
            hp -= 15;
            hp = Mathf.Clamp(hp, 0, 100);
            DisPlayHpBar();
        
            if (hp <= 0)
                PlayerDie();
        }
    }
    void PlayerDie()
    {
        bloodscreen.enabled = false;
        GameManager.gameManager.isGameOver = true;
        OnPlayerDie();
        print("GameOver : " + GameManager.gameManager.isGameOver.ToString());
        //Time.timeScale = 0.0f;
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        //foreach(GameObject _enemy in enemies)
        //{
        //    _enemy.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver); //다른 오브젝트의 함수를 호출하게 하는 함수
        //}
        //for(int i = 0; i<enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        //적의 수가 극단적으로 몇백 몇천이 된다. 일일히 하나씩 전달하면 아무래도
        //플레이어 위치의 끝쪽에 있는 적들은 전달을 늦게 받게 된다.
        //그래서 일일히 전달하지 않고 동시에 많은 적들에게 전달하는 사용자 정의 이벤트를 사용
        //해야 한다.
    }
    private void ShowBloodEffect(Collider col)
    {
        Vector3 pos = col.transform.position;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, pos);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, Random.Range(0.8f, 1.3f));
    }
    void DisPlayHpBar()
    {
        hpBar.fillAmount = (float)hp / (float)hpInit;
        if (hpBar.fillAmount <= 0.3f)
            hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow;
    }
}
