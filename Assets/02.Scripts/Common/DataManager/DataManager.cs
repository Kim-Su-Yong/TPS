using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //실시간 게임 했던 데이터의 내용을 파일로 출력하기 위해
//바이너리(이진데이터) 파일 포맷(양식)을 위한 네임스페이스
using System.Runtime.Serialization.Formatters.Binary;
//실시간 게임에서 이루어진 데이터를 이진데이터로 직렬화(바꿈 사람이 알 수 있는 데이터로)
using DataInfo;
public class DataManager : MonoBehaviour
{   //파일이 저장될 물리적인 경로 및 파일명을 저장할 변수
    private string dataPath;
    public void Initialize() //파일 저장 경로와 파일명 지정
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
        //IOS 안드로이드 폰의 경우 공용폴더의 경로에 반환
        //앱이 업그레이드 되어도 삭제되지 않음
    }
    //데이터 저장 및 파일을 생성하는 함수
    public void Save(GameData gameData)
    {
        //바이너리 파일 포맷을 위한 BinaryFormatter 생성
        BinaryFormatter bf = new BinaryFormatter();
        //데이터 저장을 위한 파일 생성
        FileStream file = File.Create(dataPath);

        //파일에 저장할 클래스에 데이터 할당
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;
        //BinaryFormatter를 사용해 파일에 데이터 기록
        bf.Serialize(file, data);
        file.Close();
    }
    public GameData Load() //파일에서 데이터를 추출하는 함수
    {
        if (File.Exists(dataPath))
        {
            //파일이 존재하는 경우 데이터 불러오기
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            //GameData 클래스에 파일로부터 읽은 데이터를 기록
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else //존재하지 않은 경우 기본값을 반환
        {
            GameData data = new GameData();
            return data;
        }
    }
}