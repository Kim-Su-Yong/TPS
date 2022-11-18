using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;
//1.enemyPrefab 2. 태어날 위치 좌표들 3.몇 초 간격으로 태어날지를 정해야 함
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    //[SerializeField]
    //private GameObject enemy2;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject e_bulletPrefab;
    [SerializeField]
    private Transform[] points;
    public List<GameObject> enemyPool = new List<GameObject>();
    public List<GameObject> bulletPool = new List<GameObject>();
    public List<GameObject> enemybulletPool = new List<GameObject>();
    //어떤 자료형을 다 담을수 있는 배열 클래스
    public bool isGameOver = false;
    public int maxcount = 10;
    //private float CreateTime = 5.0f;
    public static GameManager gameManager; //싱글턴 패턴 디자인
    public CanvasGroup inventoryCG;
    [Header("GameData")]
    public Text killCountTxt;
    public DataManager dataManager;
    public GameDataObject gameData;
    //public GameData gameData;
    //인벤토리 아이템이 변경되었을 때 발생시킬 이벤트 정의
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    //SlotList 게임 오브젝트에 저장할 변수
    private GameObject slotList;
    //ItemList 하위에 있는 네개의 아이템을 저장할 배열
    public GameObject[] itemObjects;
    private void Awake()
    {
        //gameManager = this;
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

        bulletPrefab = (GameObject)Resources.Load("Bullet");
        e_bulletPrefab = (GameObject)Resources.Load("E_Bullet");
        enemy = Resources.Load<GameObject>("Enemy");
        //enemy2 = Resources.Load<GameObject>("Enemy2");
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        //인벤토리에 추가된 아이템을 검색하기 위해 SlotList게임 오브젝트를 추출
        slotList = inventoryCG.transform.Find("SlotList").gameObject;
        LoadGameData();
    }
    void LoadGameData()
    {
        //GameData data = dataManager.Load();
        //gameData.hp = data.hp;
        //gameData.damage = data.damage;
        //gameData.speed = data.speed;
        //gameData.killCount = data.killCount;
        //gameData.equipItem = data.equipItem;
        //killCountTxt.text = "Kill : " + gameData.killCount.ToString("0000");
        //보유한 아이템이 있는 경우 호출
        if(gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }
    }
    //로드한 데이터를 기준으로 인벤토리 아이템을 추가하는 함수
    void InventorySetup()
    {           //Slot list 하위에 있는 모든 slot의 트랜스폼을 추출
        var slots = slotList.GetComponentsInChildren<Transform>();
        //보유한 아이템의 갯수만큼 반복
        for(int i = 0; i < gameData.equipItem.Count; i++)
        {       //인벤토리 UI에 있는 Slot 개수만큼 반복
            for(int j = 1; j < slots.Length; j++)
            {   //Slot 하위에 다른 아이템이 있으면 다음 인덱스로 넘어감
                if (slots[j].childCount > 0) continue;
                //보유한 아이템의 종류에 따라 인덱스 추출
                int itemIndex = (int)gameData.equipItem[i].itemType;
                //아이템의 부모를 Slot 게임오브젝트로 변경
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                //아이템의 ItemInfo 클래스의 itemData에 로드한 데이터 값을 저장
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                //아이템을 Slot에 추가하면 바깥 for문으로 빠져나감
                break;
            }
        }
    }
    void Start()
    {
        OnInventoryOpen(false);
        CreateEnemyPool();
        CreateBulletPool();
        EnemyBulletPool();

        //if (points.Length > 0)
        //    StartCoroutine(CreateEnemy());

        InvokeRepeating("BulletDeActive", 1.0f, 3.0f);
        //InvokeRepeating("e_BulletDeActive", 1.0f, 3.0f);
        InvokeRepeating("RepeatingEnemy", 2.0f, 3.0f);
    }
    void BulletDeActive()
    {
        bulletPrefab.SetActive(false);
    }
    void e_BulletDeActive()
    {
        e_bulletPrefab.SetActive(false);
    }
    private void CreateBulletPool()
    {
        GameObject bulletPools = new GameObject("BulletPool");
        for (int i = 0; i < maxcount; i++)
        {
            var obj = Instantiate<GameObject>(bulletPrefab, bulletPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }
    private void EnemyBulletPool()
    {
        GameObject enemybulletPools = new GameObject("EnemyBulletPool");
        for (int i = 0; i < 20; i++)
        {
            var obj = Instantiate<GameObject>(e_bulletPrefab, enemybulletPools.transform);
            obj.name = "E_Bullet_" + i.ToString("00");
            obj.SetActive(false);
            enemybulletPool.Add(obj);
        }
    }
    public GameObject GetBullet()
    {
        for(int i = 0; i<bulletPool.Count; i++)
        {
            if(bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }
    public GameObject GetenemyBullet()
    {
        for (int i = 0; i < enemybulletPool.Count; i++)
        {
            if (enemybulletPool[i].activeSelf == false)
            {
                return enemybulletPool[i];
            }
        }
        return null;
    }
    private void CreateEnemyPool()
    {
        GameObject enemyObj = new GameObject("EnemyPool");
        for (int i = 0; i < maxcount; i++)
        {
            GameObject _enemy = Instantiate(enemy, enemyObj.transform);
            //GameObject _enemy2 = Instantiate(enemy2, enemyObj.transform);
            _enemy.name = "Enemy" + i.ToString();
            //_enemy2.name = "Enemy" + i.ToString();
            _enemy.SetActive(false); //액티브를 비활성화
            //_enemy2.SetActive(false);
            enemyPool.Add(_enemy);
            //enemyPool.Add(_enemy2);
            //리스트에 담는다
        }
    }

    //IEnumerator CreateEnemy()
    //{
    //    while (!isGameOver)
    //    {
    //        #region 오브젝트 풀링이 아닌 경우에 적생성 방법
    //        int enemycount = GameObject.FindGameObjectsWithTag("ENEMY").Length;
    //        //if(enemycount < maxcount)
    //        //{
    //        //    yield return new WaitForSeconds(CreateTime);
    //        //    int idx = Random.Range(1, points.Length);
    //        //    Instantiate(enemy, points[idx].position, points[idx].rotation);
    //        //    Instantiate(enemy2, points[idx].position, points[idx].rotation);
    //        //}
    //        //else
    //        //{
    //        //    yield return null; //한프레임 쉬고
    //        //}
    //    #endregion
    //    yield return new WaitForSeconds(CreateTime);
        
    //        foreach (GameObject _enemy2 in enemyPool)
    //        {   //비활성화 된 오브젝트라면
    //            if (_enemy2.activeSelf == false)
    //            {
    //                int idx = Random.Range(1, points.Length);
    //                _enemy2.transform.position = points[idx].position;
    //                _enemy2.transform.rotation = points[idx].rotation;
    //                _enemy2.SetActive(true);
    //                //오브젝트 활성화
    //                break;
    //                //enemy하나를 생성 후 for 루프를 빠져나감
    //            }
    //        }
    //    }
    //}
    void RepeatingEnemy()
    {
        if (isGameOver == true) return;
        //게임오버인 경우 반복문 while문을 빠져나가서 코루틴을 종료
        foreach (GameObject _enemy in enemyPool)
        {   //비활성화 된 오브젝트라면
            if (_enemy.activeSelf == false)
            {
                int idx = Random.Range(1, points.Length);
                _enemy.transform.position = points[idx].position;
                _enemy.transform.rotation = points[idx].rotation;
                _enemy.SetActive(true);
                //오브젝트 활성화
                break;
                //enemy하나를 생성 후 for 루프를 빠져나감
            }
        }
    }
    private bool isPaused;
    public void OnPauseClick()
    {
        isPaused = !isPaused;
        //타임스케일이 0이면 정지 1이면 정상속도
        Time.timeScale = (isPaused) ? 0.0f : 1.0f;
        //플레이어 오브젝트에 달린 스크립트들을 비활성화
        var playerObj = GameObject.FindWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach(var script in scripts)
        {
            script.enabled = !isPaused;
        }
        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }
    //인벤토리를 활성화 비활성화 하는 함수
    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
    }
    public void IncKillCount()
    {
        //++killCount;
        ++gameData.killCount;
        killCountTxt.text = "KILL : " + "<color=#ff0000>" + gameData.killCount.ToString("0000") + "</color>";
        //PlayerPrefs.SetInt("KILL_COUNT", killCount);
        //죽인 횟수를 저장
    }
    //게임 데이터를 저장
    void SaveGameData()
    {
        //dataManager.Save(gameData);
        //a.asset 파일에 데이터 저장
        UnityEditor.EditorUtility.SetDirty(gameData);
    }
    //인벤토리 아이템을 추가했을 때 데이터의 정보를 갱신하는 함수

    public void AddItem(Item item)
    {       //보유 아이템에 같은 아이템이 있다면 추가하지 않고
            //빠져나감
        if (gameData.equipItem.Contains(item)) return;
        //아이템을 GameData.equipItem 배열에 추가
        gameData.equipItem.Add(item);
        //아이템 종류에 따라 분기
        switch(item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp /(1.0f + item.value);

                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage / (1.0f + item.value);

                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed / (1.0f + item.value);

                break;
            case Item.ItemType.GRENADE:


                break;
        }
        UnityEditor.EditorUtility.SetDirty(gameData);
        OnItemChange();
        //아이템이 변경 된 것을 실시간으로 반영하기 위해 이벤트를 발생시킴    
    }
    //인벤토리에서 아이템을 제거했을때 데이터를 갱신하는 함수
    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        switch (item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp / (1.0f + item.value);

                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1.0f + item.value);

                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed / (1.0f + item.value);

                break;
            case Item.ItemType.GRENADE:


                break;
        }
        UnityEditor.EditorUtility.SetDirty(gameData);
        OnItemChange();
    }
    //OnDisable()오브젝트가 비활성화 될때랑은 틀리다.
    private void OnApplicationQuit() //게임이 종료될때 호출되는 함수
    {
        SaveGameData();
    }
}