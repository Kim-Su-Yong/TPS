using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public int killCount;
    public Text killCountTxt;
    public static UIManager uimanager;
    void Awake()
    {
        if (uimanager == null)
            uimanager = this;
        else if (uimanager != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        LoadGameData();
    }
    void LoadGameData() //게임 초기 데이터 로드
    {
        killCount = PlayerPrefs.GetInt("KILL_COUNT", 0); //예약된 키값을 등록
        killCountTxt.text = "KILL : " + "<color=#ff0000>" + killCount.ToString("0000") + "</color>";
    }
    public void IncKillCount()
    {
        ++killCount;
        killCountTxt.text = "KILL : " + "<color=#ff0000>" + killCount.ToString("0000") + "</color>";
        PlayerPrefs.SetInt("KILL_COUNT", killCount);
        //죽인 횟수를 저장
    }
}